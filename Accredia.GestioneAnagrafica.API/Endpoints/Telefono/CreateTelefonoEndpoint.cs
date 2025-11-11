using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;
using TelefonoModel = Accredia.GestioneAnagrafica.Shared.Models.Telefono;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Telefono;

public class CreateTelefonoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/telefono", async (
            [FromBody] TelefonoDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<TelefonoDTO.Create> validator) =>
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

            // Verifica che il TipoTelefono esista
            var tipoTelefonoExists = await context.TipiTelefono
                .AnyAsync(t => t.TipoTelefonoId == request.TipoTelefonoId);

            if (!tipoTelefonoExists)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "Tipo Telefono non trovato."
                });
            }

            var telefono = new TelefonoModel
            {
                EntitaAziendaleId = request.EntitaAziendaleId,
                TipoTelefonoId = request.TipoTelefonoId,
                PrefissoInternazionale = request.PrefissoInternazionale,
                Numero = request.Numero,
                Estensione = request.Estensione,
                RowGuid = Guid.NewGuid(),
                DataCreazione = DateTime.UtcNow,
                DataInizioValidita = DateTime.UtcNow,
                DataFineValidita = DateTime.MaxValue
            };

            context.Telefoni.Add(telefono);
            await context.SaveChangesAsync();

            // Carica il TipoTelefono per la risposta
            var telefonoWithDetails = await context.Telefoni
                .Include(t => t.TipoTelefono)
                .FirstOrDefaultAsync(t => t.TelefonoId == telefono.TelefonoId);

            var response = new TelefonoDTO.Response
            {
                TelefonoId = telefonoWithDetails!.TelefonoId,
                EntitaAziendaleId = telefonoWithDetails.EntitaAziendaleId,
                TipoTelefonoId = telefonoWithDetails.TipoTelefonoId,
                TipoTelefonoDescrizione = telefonoWithDetails.TipoTelefono?.Descrizione,
                PrefissoInternazionale = telefonoWithDetails.PrefissoInternazionale,
                Numero = telefonoWithDetails.Numero,
                Estensione = telefonoWithDetails.Estensione,
                RowGuid = telefonoWithDetails.RowGuid,
                DataCreazione = telefonoWithDetails.DataCreazione,
                DataModifica = telefonoWithDetails.DataModifica,
                CreatoDa = telefonoWithDetails.CreatoDa,
                ModificatoDa = telefonoWithDetails.ModificatoDa,
                DataInizioValidita = telefonoWithDetails.DataInizioValidita,
                DataFineValidita = telefonoWithDetails.DataFineValidita
            };

            return Results.Ok(response);
        }).RequireAuthorization()
          .WithTags("Telefono")
          .WithName("CreateTelefono")
          .Produces<TelefonoDTO.Response>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
