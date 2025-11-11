using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.RisorseUmane;

public class TurniEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/turni").WithTags("Turni");

        // GET: Lista turni
        group.MapGet("", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null) =>
        {
            var query = context.Set<Turno>()
                .Where(t => t.DataCancellazione == null);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t => t.Descrizione.ToLower().Contains(searchLower));
            }

            var totalCount = await query.CountAsync();

            var turni = await query
                .OrderBy(t => t.OraInizio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TurnoDTO.List
                {
                    TurnoId = t.TurnoId,
                    Descrizione = t.Descrizione,
                    OraInizio = t.OraInizio.ToString(@"hh\:mm"),
                    OraFine = t.OraFine.ToString(@"hh\:mm"),
                    Durata = (t.OraFine - t.OraInizio).ToString(@"hh\:mm"),
                    NumeroDipendenti = context.Set<Dipendente>()
                        .Count(d => d.TurnoId == t.TurnoId && d.DataCancellazione == null)
                })
                .ToListAsync();

            return Results.Ok(new PageResult<TurnoDTO.List>
            {
                Data = turni,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            });
        });

        // GET: Dettaglio turno
        group.MapGet("{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var turno = await context.Set<Turno>()
                .FirstOrDefaultAsync(t => t.TurnoId == id && t.DataCancellazione == null);

            if (turno == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Turno non trovato" });

            var numeroDipendenti = await context.Set<Dipendente>()
                .CountAsync(d => d.TurnoId == id && d.DataCancellazione == null);

            var response = new TurnoDTO.Response
            {
                TurnoId = turno.TurnoId,
                Descrizione = turno.Descrizione,
                OraInizio = turno.OraInizio,
                OraFine = turno.OraFine,
                Durata = turno.Durata,
                NumeroDipendenti = numeroDipendenti,
                DataCreazione = turno.DataCreazione,
                DataModifica = turno.DataModifica,
                UniqueRowId = turno.UniqueRowId
            };

            return Results.Ok(response);
        });

        // POST: Crea turno
        group.MapPost("", async (
            [FromBody] TurnoDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<TurnoDTO.Create> validator,
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

            try
            {
                var turno = new Turno
                {
                    Descrizione = request.Descrizione.Trim(),
                    OraInizio = request.OraInizio,
                    OraFine = request.OraFine,
                    DataCreazione = DateTime.UtcNow,
                    DataInizioValidita = DateTime.UtcNow,
                    UniqueRowId = Guid.NewGuid()
                };

                context.Set<Turno>().Add(turno);
                await context.SaveChangesAsync(cancellationToken);

                var response = new TurnoDTO.Response
                {
                    TurnoId = turno.TurnoId,
                    Descrizione = turno.Descrizione,
                    OraInizio = turno.OraInizio,
                    OraFine = turno.OraFine,
                    Durata = turno.Durata,
                    NumeroDipendenti = 0,
                    DataCreazione = turno.DataCreazione,
                    UniqueRowId = turno.UniqueRowId
                };

                return Results.Created($"/api/turni/{turno.TurnoId}", response);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        });

        // PUT: Aggiorna turno
        group.MapPut("{id}", async (
            [FromRoute] int id,
            [FromBody] TurnoDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<TurnoDTO.Create> validator,
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

            var turno = await context.Set<Turno>()
                .FirstOrDefaultAsync(t => t.TurnoId == id && t.DataCancellazione == null, cancellationToken);

            if (turno == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Turno non trovato" });

            try
            {
                turno.Descrizione = request.Descrizione.Trim();
                turno.OraInizio = request.OraInizio;
                turno.OraFine = request.OraFine;
                turno.DataModifica = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                var numeroDipendenti = await context.Set<Dipendente>()
                    .CountAsync(d => d.TurnoId == id && d.DataCancellazione == null, cancellationToken);

                var response = new TurnoDTO.Response
                {
                    TurnoId = turno.TurnoId,
                    Descrizione = turno.Descrizione,
                    OraInizio = turno.OraInizio,
                    OraFine = turno.OraFine,
                    Durata = turno.Durata,
                    NumeroDipendenti = numeroDipendenti,
                    DataCreazione = turno.DataCreazione,
                    DataModifica = turno.DataModifica,
                    UniqueRowId = turno.UniqueRowId
                };

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        });

        // DELETE: Elimina turno
        group.MapDelete("{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context,
            CancellationToken cancellationToken) =>
        {
            var turno = await context.Set<Turno>()
                .FirstOrDefaultAsync(t => t.TurnoId == id && t.DataCancellazione == null, cancellationToken);

            if (turno == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Turno non trovato" });

            // Verifica se ci sono dipendenti collegati
            var hasDipendenti = await context.Set<Dipendente>()
                .AnyAsync(d => d.TurnoId == id && d.DataCancellazione == null, cancellationToken);

            if (hasDipendenti)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Impossibile eliminare il turno: ci sono dipendenti collegati"
                });
            }

            try
            {
                turno.DataCancellazione = DateTime.UtcNow;
                turno.DataFineValidita = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Turno eliminato con successo"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: 500);
            }
        });
    }
}
