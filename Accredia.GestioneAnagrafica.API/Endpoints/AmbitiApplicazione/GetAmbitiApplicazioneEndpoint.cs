using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.AmbitiApplicazione;

public class GetAmbitiApplicazioneEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // GET: Lista paginata con ricerca e filtri
        app.MapGet("/api/ambiti-applicazione", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] bool? attivo = null,
            [FromQuery] string? orderBy = "Ordine") =>
        {
            var query = context.AmbitoApplicazione
                .Where(a => a.DataCancellazione == null);

            // Filtro ricerca
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a => 
                    a.Codice.Contains(search) || 
                    a.Denominazione.Contains(search) ||
                    (a.Descrizione != null && a.Descrizione.Contains(search)));
            }

            // Filtro attivo
            if (attivo.HasValue)
            {
                query = query.Where(a => a.Attivo == attivo.Value);
            }

            // Ordinamento
            query = orderBy?.ToLower() switch
            {
                "codice" => query.OrderBy(a => a.Codice),
                "denominazione" => query.OrderBy(a => a.Denominazione),
                "datacreazione" => query.OrderByDescending(a => a.DataCreazione),
                "ordine" => query.OrderBy(a => a.Ordine),
                _ => query.OrderBy(a => a.Ordine)
            };

            var totalCount = await query.CountAsync();

            var ambiti = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AmbitoApplicazioneDTO.List
                {
                    AmbitoApplicazioneId = a.AmbitoApplicazioneId,
                    Codice = a.Codice,
                    Denominazione = a.Denominazione,
                    Ordine = a.Ordine,
                    Attivo = a.Attivo
                })
                .ToListAsync();

            var pageResult = new PageResult<AmbitoApplicazioneDTO.List>
            {
                Data = ambiti,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
          .WithTags("AmbitiApplicazione")
          .WithName("GetAmbitiApplicazione")
          .Produces<PageResult<AmbitoApplicazioneDTO.List>>(StatusCodes.Status200OK);

        // GET: Dettaglio singolo ambito per ID
        app.MapGet("/api/ambiti-applicazione/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var ambito = await context.AmbitoApplicazione
                .FirstOrDefaultAsync(a => a.AmbitoApplicazioneId == id && a.DataCancellazione == null);

            if (ambito == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Ambito Applicazione non trovato."
                });
            }

            var response = new AmbitoApplicazioneDTO.Response
            {
                AmbitoApplicazioneId = ambito.AmbitoApplicazioneId,
                Codice = ambito.Codice,
                Denominazione = ambito.Denominazione,
                Descrizione = ambito.Descrizione,
                Ordine = ambito.Ordine,
                Attivo = ambito.Attivo,
                DataCreazione = ambito.DataCreazione,
                DataModifica = ambito.DataModifica,
                DataInizioValidita = ambito.DataInizioValidita,
                DataFineValidita = ambito.DataFineValidita,
                IsDeleted = ambito.IsDeleted
            };

            return Results.Ok(response);
        })
          .WithTags("AmbitiApplicazione")
          .WithName("GetAmbitoApplicazioneById")
          .Produces<AmbitoApplicazioneDTO.Response>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);

        // GET: Lista per dropdown/select (lookup)
        app.MapGet("/api/ambiti-applicazione/lookup", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] bool soloAttivi = true) =>
        {
            var query = context.AmbitoApplicazione
                .Where(a => a.DataCancellazione == null);

            if (soloAttivi)
            {
                query = query.Where(a => a.Attivo);
            }

            var lookupList = await query
                .OrderBy(a => a.Ordine)
                .Select(a => new AmbitoApplicazioneDTO.Lookup
                {
                    AmbitoApplicazioneId = a.AmbitoApplicazioneId,
                    Codice = a.Codice,
                    Denominazione = a.Denominazione
                })
                .ToListAsync();

            return Results.Ok(lookupList);
        })
          .WithTags("AmbitiApplicazione")
          .WithName("GetAmbitiApplicazioneLookup")
          .Produces<List<AmbitoApplicazioneDTO.Lookup>>(StatusCodes.Status200OK);
    }
}
