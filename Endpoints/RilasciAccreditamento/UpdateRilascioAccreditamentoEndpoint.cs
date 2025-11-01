using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.RilasciAccreditamento;

public class UpdateRilascioAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/rilasci-accreditamento/{id}", async (
            [FromRoute] int id,
            [FromBody] RilascioAccreditamentoDTO.Update request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<RilascioAccreditamentoDTO.Update> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var rilascio = await context.RilascioAccreditamento
                .Include(r => r.EnteAccreditamento)
                .Include(r => r.EnteAccreditato)
                .FirstOrDefaultAsync(r => r.RilascioId == id);

            if (rilascio == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Rilascio Accreditamento non trovato."
                });
            }

            // Update
            rilascio.AmbitoApplicazioneId = request.AmbitoApplicazioneId;
            rilascio.NumeroAtto = request.NumeroAtto;
            rilascio.DataRilascio = request.DataRilascio;
            rilascio.DataScadenza = request.DataScadenza;
            rilascio.Stato = request.Stato;
            rilascio.DocumentoRilascioId = request.DocumentoRilascioId;
            rilascio.Note = request.Note;

            await context.SaveChangesAsync();

            var response = new RilascioAccreditamentoDTO.Response
            {
                RilascioId = rilascio.RilascioId,
                EnteAccreditamentoId = rilascio.EnteAccreditamentoId,
                NomeEnteAccreditamento = rilascio.EnteAccreditamento?.Nome,
                EnteAccreditatoId = rilascio.EnteAccreditatoId,
                RagioneSocialeEnteAccreditato = rilascio.EnteAccreditato?.RagioneSociale,
                AmbitoApplicazioneId = rilascio.AmbitoApplicazioneId,
                NumeroAtto = rilascio.NumeroAtto,
                DataRilascio = rilascio.DataRilascio,
                DataScadenza = rilascio.DataScadenza,
                Stato = rilascio.Stato,
                DocumentoRilascioId = rilascio.DocumentoRilascioId,
                Note = rilascio.Note
            };

            return Results.Ok(ApiResponse<RilascioAccreditamentoDTO.Response>.SuccessResponse(response));
        }).RequireAuthorization();
    }
}
