using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;
using EmailModel = Accredia.GestioneAnagrafica.Shared.Models.Email;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Email;

public class CreateEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/email", async (
            [FromBody] EmailDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<EmailDTO.Create> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            // Verifica che l'EntitaAziendale esista
            var entitaExists = await context.EntiAziendali
                .AnyAsync(e => e.EntitaAziendaleId == request.EntitaAziendaleId);

            if (!entitaExists)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "EntitaAziendale non trovata."
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

            // Verifica che l'email non sia già associata alla stessa entità
            var emailExists = await context.Email
                .AnyAsync(e => e.EntitaAziendaleId == request.EntitaAziendaleId 
                    && e.EmailAddress == request.Email
                    && e.DataCancellazione == null);

            if (emailExists)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Message = "Questa email è già associata all'entità aziendale."
                });
            }

            var email = new EmailModel
            {
                EntitaAziendaleId = request.EntitaAziendaleId,
                TipoEmailId = request.TipoEmailId,
                EmailAddress = request.Email,
                RowGuid = Guid.NewGuid(),
                DataCreazione = DateTime.UtcNow,
                DataInizioValidita = DateTime.UtcNow,
                DataFineValidita = DateTime.MaxValue
            };

            context.Email.Add(email);
            await context.SaveChangesAsync();

            // Carica il TipoEmail per la risposta
            var emailWithDetails = await context.Email
                .Include(e => e.TipoEmail)
                .FirstOrDefaultAsync(e => e.EmailId == email.EmailId);

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
          .WithName("CreateEmail")
          .Produces<EmailDTO.Response>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
