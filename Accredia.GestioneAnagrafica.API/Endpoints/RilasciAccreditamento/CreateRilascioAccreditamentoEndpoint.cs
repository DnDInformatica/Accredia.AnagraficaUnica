using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.Models;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.RilasciAccreditamento;

public class CreateRilascioAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rilasci-accreditamento", async (
            [FromBody] RilascioAccreditamentoDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<RilascioAccreditamentoDTO.Create> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            // Verify entities exist
            var enteAccreditamento = await context.EntiAccreditamento
                .FirstOrDefaultAsync(e => e.EntitaAziendaleId == request.EnteAccreditamentoId);
            
            if (enteAccreditamento == null)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Ente Accreditamento non trovato."
                });
            }

            var enteAccreditato = await context.OrganismiAccreditati
                .FirstOrDefaultAsync(o => o.EntitaAziendaleId == request.EnteAccreditatoId);
            
            if (enteAccreditato == null)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Ente Accreditato non trovato."
                });
            }

            var rilascio = new RilascioAccreditamento
            {
                EnteAccreditamentoId = request.EnteAccreditamentoId,
                EnteAccreditatoId = request.EnteAccreditatoId,
                AmbitoApplicazioneId = request.AmbitoApplicazioneId,
                NumeroAtto = request.NumeroAtto,
                DataRilascio = request.DataRilascio,
                DataScadenza = request.DataScadenza,
                Stato = request.Stato,
                DocumentoRilascioId = request.DocumentoRilascioId,
                Note = request.Note,
                DataInizioValidita = DateTime.UtcNow
            };

            context.RilascioAccreditamento.Add(rilascio);
            await context.SaveChangesAsync();

            var response = new RilascioAccreditamentoDTO.Response
            {
                RilascioId = rilascio.RilascioId,
                EnteAccreditamentoId = rilascio.EnteAccreditamentoId,
                NomeEnteAccreditamento = enteAccreditamento.Denominazione,
                EnteAccreditatoId = rilascio.EnteAccreditatoId,
                RagioneSocialeEnteAccreditato = enteAccreditato.RagioneSociale,
                AmbitoApplicazioneId = rilascio.AmbitoApplicazioneId,
                NumeroAtto = rilascio.NumeroAtto,
                DataRilascio = rilascio.DataRilascio,
                DataScadenza = rilascio.DataScadenza,
                Stato = rilascio.Stato,
                DocumentoRilascioId = rilascio.DocumentoRilascioId,
                Note = rilascio.Note
            };

            return Results.Created($"/api/rilasci-accreditamento/{rilascio.RilascioId}", response);
        })
            .WithTags("RilasciAccreditamento")
            .WithName("CreateRilascioAccreditamento")
            .Produces<RilascioAccreditamentoDTO.Response>(StatusCodes.Status201Created)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization();
    }
}
