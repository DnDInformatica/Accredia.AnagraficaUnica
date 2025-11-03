using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Models;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.RisorseUmane;

public class DipendentiEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dipendenti").WithTags("Dipendenti");

        // GET: Lista dipendenti
        group.MapGet("", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? repartoId = null,
            [FromQuery] int? turnoId = null,
            [FromQuery] string? orderBy = "Matricola") =>
        {
            var query = context.Set<Dipendente>()
                .Include(d => d.Reparto)
                .Include(d => d.Turno)
                .Where(d => d.DataCancellazione == null);

            // Filtri
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(d =>
                    d.Matricola.ToLower().Contains(searchLower) ||
                    d.CodiceFiscale.ToLower().Contains(searchLower) ||
                    d.Mansione.ToLower().Contains(searchLower));
            }

            if (repartoId.HasValue)
                query = query.Where(d => d.RepartoId == repartoId.Value);

            if (turnoId.HasValue)
                query = query.Where(d => d.TurnoId == turnoId.Value);

            // Ordinamento
            query = orderBy?.ToLower() switch
            {
                "matricola" => query.OrderBy(d => d.Matricola),
                "codicefiscale" => query.OrderBy(d => d.CodiceFiscale),
                "mansione" => query.OrderBy(d => d.Mansione),
                _ => query.OrderBy(d => d.Matricola)
            };

            var totalCount = await query.CountAsync();

            var dipendenti = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DipendenteDTO.List
                {
                    DipendenteId = d.DipendenteId,
                    CodiceFiscale = d.CodiceFiscale,
                    Matricola = d.Matricola,
                    Mansione = d.Mansione,
                    RepartoNome = d.Reparto != null ? d.Reparto.Nome : null,
                    TurnoDescrizione = d.Turno != null ? d.Turno.Descrizione : null
                })
                .ToListAsync();

            return Results.Ok(new PageResult<DipendenteDTO.List>
            {
                Data = dipendenti,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            });
        });

        // GET: Dettaglio dipendente
        group.MapGet("{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var dipendente = await context.Set<Dipendente>()
                .Include(d => d.Reparto)
                .Include(d => d.Turno)
                .FirstOrDefaultAsync(d => d.DipendenteId == id && d.DataCancellazione == null);

            if (dipendente == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Dipendente non trovato" });

            var response = new DipendenteDTO.Response
            {
                DipendenteId = dipendente.DipendenteId,
                EntitaAziendaleId = dipendente.EntitaAziendaleId,
                CodiceFiscale = dipendente.CodiceFiscale,
                Matricola = dipendente.Matricola,
                LoginID = dipendente.LoginID,
                Mansione = dipendente.Mansione,
                TitoloOnorificoId = dipendente.TitoloOnorificoId,
                RepartoId = dipendente.RepartoId,
                RepartoNome = dipendente.Reparto?.Nome,
                TurnoId = dipendente.TurnoId,
                TurnoDescrizione = dipendente.Turno?.Descrizione,
                DataCreazione = dipendente.DataCreazione,
                DataModifica = dipendente.DataModifica,
                UniqueRowId = dipendente.UniqueRowId
            };

            return Results.Ok(response);
        });

        // GET: Ricerca per matricola
        group.MapGet("by-matricola/{matricola}", async (
            [FromRoute] string matricola,
            [FromServices] PersoneDbContext context) =>
        {
            var dipendente = await context.Set<Dipendente>()
                .Include(d => d.Reparto)
                .Include(d => d.Turno)
                .FirstOrDefaultAsync(d => d.Matricola == matricola && d.DataCancellazione == null);

            if (dipendente == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Dipendente non trovato" });

            var response = new DipendenteDTO.Response
            {
                DipendenteId = dipendente.DipendenteId,
                EntitaAziendaleId = dipendente.EntitaAziendaleId,
                CodiceFiscale = dipendente.CodiceFiscale,
                Matricola = dipendente.Matricola,
                LoginID = dipendente.LoginID,
                Mansione = dipendente.Mansione,
                TitoloOnorificoId = dipendente.TitoloOnorificoId,
                RepartoId = dipendente.RepartoId,
                RepartoNome = dipendente.Reparto?.Nome,
                TurnoId = dipendente.TurnoId,
                TurnoDescrizione = dipendente.Turno?.Descrizione,
                DataCreazione = dipendente.DataCreazione,
                DataModifica = dipendente.DataModifica,
                UniqueRowId = dipendente.UniqueRowId
            };

            return Results.Ok(response);
        });

        // POST: Crea dipendente
        group.MapPost("", async (
            [FromBody] DipendenteDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<DipendenteDTO.Create> validator,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Errori di validazione",
                    Errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList())
                });
            }

            // Verifica univocità Matricola
            var matricolaExists = await context.Set<Dipendente>()
                .AnyAsync(d => d.Matricola == request.Matricola && d.DataCancellazione == null, cancellationToken);

            if (matricolaExists)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Matricola già esistente"
                });
            }

            // Verifica univocità CF
            var cfExists = await context.Set<Dipendente>()
                .AnyAsync(d => d.CodiceFiscale == request.CodiceFiscale && d.DataCancellazione == null, cancellationToken);

            if (cfExists)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Codice Fiscale già esistente"
                });
            }

            try
            {
                var dipendente = new Dipendente
                {
                    EntitaAziendaleId = request.EntitaAziendaleId,
                    CodiceFiscale = request.CodiceFiscale.Trim().ToUpperInvariant(),
                    Matricola = request.Matricola.Trim().ToUpperInvariant(),
                    LoginID = request.LoginID.Trim(),
                    Mansione = request.Mansione.Trim(),
                    TitoloOnorificoId = request.TitoloOnorificoId,
                    RepartoId = request.RepartoId,
                    TurnoId = request.TurnoId,
                    DataCreazione = DateTime.UtcNow,
                    DataInizioValidita = DateTime.UtcNow,
                    UniqueRowId = Guid.NewGuid()
                };

                context.Set<Dipendente>().Add(dipendente);
                await context.SaveChangesAsync(cancellationToken);

                var response = new DipendenteDTO.Response
                {
                    DipendenteId = dipendente.DipendenteId,
                    EntitaAziendaleId = dipendente.EntitaAziendaleId,
                    CodiceFiscale = dipendente.CodiceFiscale,
                    Matricola = dipendente.Matricola,
                    LoginID = dipendente.LoginID,
                    Mansione = dipendente.Mansione,
                    TitoloOnorificoId = dipendente.TitoloOnorificoId,
                    RepartoId = dipendente.RepartoId,
                    TurnoId = dipendente.TurnoId,
                    DataCreazione = dipendente.DataCreazione,
                    UniqueRowId = dipendente.UniqueRowId
                };

                return Results.Created($"/api/dipendenti/{dipendente.DipendenteId}", response);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        });

        // PUT: Aggiorna dipendente
        group.MapPut("{id}", async (
            [FromRoute] int id,
            [FromBody] DipendenteDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<DipendenteDTO.Create> validator,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Errori di validazione",
                    Errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList())
                });
            }

            var dipendente = await context.Set<Dipendente>()
                .FirstOrDefaultAsync(d => d.DipendenteId == id && d.DataCancellazione == null, cancellationToken);

            if (dipendente == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Dipendente non trovato" });

            // Verifica univocità Matricola (escluso dipendente corrente)
            var matricolaExists = await context.Set<Dipendente>()
                .AnyAsync(d => d.Matricola == request.Matricola && d.DipendenteId != id && d.DataCancellazione == null, cancellationToken);

            if (matricolaExists)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Matricola già esistente"
                });
            }

            try
            {
                dipendente.EntitaAziendaleId = request.EntitaAziendaleId;
                dipendente.CodiceFiscale = request.CodiceFiscale.Trim().ToUpperInvariant();
                dipendente.Matricola = request.Matricola.Trim().ToUpperInvariant();
                dipendente.LoginID = request.LoginID.Trim();
                dipendente.Mansione = request.Mansione.Trim();
                dipendente.TitoloOnorificoId = request.TitoloOnorificoId;
                dipendente.RepartoId = request.RepartoId;
                dipendente.TurnoId = request.TurnoId;
                dipendente.DataModifica = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Dipendente aggiornato con successo"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        });

        // DELETE: Elimina dipendente
        group.MapDelete("{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context,
            CancellationToken cancellationToken) =>
        {
            var dipendente = await context.Set<Dipendente>()
                .FirstOrDefaultAsync(d => d.DipendenteId == id && d.DataCancellazione == null, cancellationToken);

            if (dipendente == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Dipendente non trovato" });

            try
            {
                dipendente.DataCancellazione = DateTime.UtcNow;
                dipendente.DataFineValidita = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Dipendente eliminato con successo"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        });
    }
}
