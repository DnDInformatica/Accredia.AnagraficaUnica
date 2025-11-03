using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.RilasciAccreditamento;

public class GetRilasciAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Get all (paginated with filters)
        app.MapGet("/api/rilasci-accreditamento", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? stato = null,
            [FromQuery] bool? soloScaduti = null,
            [FromQuery] bool? soloInScadenza = null,
            [FromQuery] int? enteAccreditamentoId = null,
            [FromQuery] int? enteAccreditatoId = null) =>
        {
            var query = context.RilascioAccreditamento
                .Include(r => r.EnteAccreditamento)
                .Include(r => r.EnteAccreditato)
                .Include(r => r.AmbitoApplicazione)
                .AsQueryable();

            // Filters
            if (!string.IsNullOrWhiteSpace(stato))
            {
                query = query.Where(r => r.Stato == stato);
            }

            if (soloScaduti == true)
            {
                query = query.Where(r => r.DataScadenza.HasValue && r.DataScadenza.Value < DateTime.UtcNow);
            }

            if (soloInScadenza == true)
            {
                var threeMonthsFromNow = DateTime.UtcNow.AddMonths(3);
                query = query.Where(r => r.DataScadenza.HasValue && 
                                        r.DataScadenza.Value > DateTime.UtcNow && 
                                        r.DataScadenza.Value < threeMonthsFromNow);
            }

            if (enteAccreditamentoId.HasValue)
            {
                query = query.Where(r => r.EnteAccreditamentoId == enteAccreditamentoId.Value);
            }

            if (enteAccreditatoId.HasValue)
            {
                query = query.Where(r => r.EnteAccreditatoId == enteAccreditatoId.Value);
            }

            var totalRecords = await query.CountAsync();

            var rilasci = await query
                .OrderByDescending(r => r.DataRilascio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RilascioAccreditamentoDTO.List
                {
                    RilascioId = r.RilascioId,
                    NomeEnteAccreditamento = r.EnteAccreditamento != null ? r.EnteAccreditamento.Denominazione : null,
                    RagioneSocialeEnteAccreditato = r.EnteAccreditato != null ? r.EnteAccreditato.RagioneSociale : null,
                    NumeroAtto = r.NumeroAtto,
                    DataRilascio = r.DataRilascio,
                    DataScadenza = r.DataScadenza,
                    Stato = r.Stato,
                    IsScaduto = r.DataScadenza.HasValue && r.DataScadenza.Value < DateTime.UtcNow,
                    IsInScadenza = r.DataScadenza.HasValue && 
                                   r.DataScadenza.Value > DateTime.UtcNow && 
                                   r.DataScadenza.Value < DateTime.UtcNow.AddMonths(3)
                })
                .ToListAsync();

            var pageResult = new PageResult<RilascioAccreditamentoDTO.List>
            {
                Data = rilasci,
                TotalRecords = totalRecords,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(ApiResponse<PageResult<RilascioAccreditamentoDTO.List>>.SuccessResponse(pageResult));
        })
            .WithTags("RilasciAccreditamento")
            .WithName("GetRilasciAccreditamento")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .RequireAuthorization();

        // Get by ID
        app.MapGet("/api/rilasci-accreditamento/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var rilascio = await context.RilascioAccreditamento
                .Include(r => r.EnteAccreditamento)
                .Include(r => r.EnteAccreditato)
                .Include(r => r.AmbitoApplicazione)
                .FirstOrDefaultAsync(r => r.RilascioId == id);

            if (rilascio == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Rilascio Accreditamento non trovato."
                });
            }

            var response = new RilascioAccreditamentoDTO.Response
            {
                RilascioId = rilascio.RilascioId,
                EnteAccreditamentoId = rilascio.EnteAccreditamentoId,
                NomeEnteAccreditamento = rilascio.EnteAccreditamento?.Denominazione,
                EnteAccreditatoId = rilascio.EnteAccreditatoId,
                RagioneSocialeEnteAccreditato = rilascio.EnteAccreditato?.RagioneSociale,
                AmbitoApplicazioneId = rilascio.AmbitoApplicazioneId,
                DenominazioneAmbitoApplicazione = rilascio.AmbitoApplicazione?.Denominazione,
                NumeroAtto = rilascio.NumeroAtto,
                DataRilascio = rilascio.DataRilascio,
                DataScadenza = rilascio.DataScadenza,
                Stato = rilascio.Stato,
                DocumentoRilascioId = rilascio.DocumentoRilascioId,
                Note = rilascio.Note
            };

            return Results.Ok(ApiResponse<RilascioAccreditamentoDTO.Response>.SuccessResponse(response));
        })
            .WithTags("RilasciAccreditamento")
            .WithName("GetRilascioAccreditamentoById")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
