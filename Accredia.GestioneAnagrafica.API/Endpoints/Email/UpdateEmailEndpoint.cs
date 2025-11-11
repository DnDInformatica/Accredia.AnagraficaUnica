using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Email;

public class UpdateEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/email/{id}", async (
            [FromRoute] int id,
            [FromBody] EmailDTO.Update request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<EmailDTO.Update> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var email = await context.Email
                .FirstOrDefaultAsync(e => e.EmailId == id && e.DataCancellazione == null);

            if (email == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "Email non trovata."
                });
            }

            // Verifica che il TipoEmail esista
            var tipoEmailExists = await context.TipiEmail
                .AnyAsync(t => t.TipoEmailId == request.TipoEmailId);

            if (!tipoEmailExists)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "Tipo Email non trovato."
                });
            }

            // Verifica duplicati (email diversa sulla stessa entità)
            if (email.EmailAddress != request.Email)
            {
                var emailExists = await context.Email
                    .AnyAsync(e => e.EntitaAziendaleId == email.EntitaAziendaleId
                        && e.EmailAddress == request.Email
                        && e.EmailId != id
                        && e.DataCancellazione == null);

                if (emailExists)
                {
                    return Results.BadRequest(new ApiResponse
                    {
                        Message = "Questa email è già associata all'entità aziendale."
                    });
                }
            }

            // Aggiorna proprietà
            email.TipoEmailId = request.TipoEmailId;
            email.EmailAddress = request.Email;
            email.DataModifica = DateTime.UtcNow;

            await context.SaveChangesAsync();

            // Carica dettagli per risposta
            var emailWithDetails = await context.Email
                .Include(e => e.TipoEmail)
                .FirstOrDefaultAsync(e => e.EmailId == id);

            var response = new EmailDTO.Response
            {
                EmailId = emailWithDetails!.EmailId,
                EntitaAziendaleId = emailWithDetails.EntitaAziendaleId,
                TipoEmailId = emailWithDetails.TipoEmailId,
                TipoEmailDescrizione = emailWithDetails.TipoEmail?.Descrizione,
                Email = emailWithDetails.EmailAddress,
                RowGuid = emailWithDetails.RowGuid,
                DataCreazione = emailWithDetails.DataCreazione,
                DataModifica = emailWithDetails.DataModifica,
                CreatoDa = emailWithDetails.CreatoDa,
                ModificatoDa = emailWithDetails.ModificatoDa,
                DataInizioValidita = emailWithDetails.DataInizioValidita,
                DataFineValidita = emailWithDetails.DataFineValidita
            };

            return Results.Ok(response);
        }).RequireAuthorization()
          .WithTags("Email")
          .WithName("UpdateEmail")
          .Produces<EmailDTO.Response>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
