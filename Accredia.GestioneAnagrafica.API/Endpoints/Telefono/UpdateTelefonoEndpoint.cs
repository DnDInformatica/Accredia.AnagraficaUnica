using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Telefono;

public class UpdateTelefonoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/telefono/{id}", async (
            [FromRoute] int id,
            [FromBody] TelefonoDTO.Update request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<TelefonoDTO.Update> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var telefono = await context.Telefoni
                .FirstOrDefaultAsync(t => t.TelefonoId == id && t.DataCancellazione == null);

            if (telefono == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "Telefono non trovato."
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

            // Aggiorna proprietà
            telefono.TipoTelefonoId = request.TipoTelefonoId;
            telefono.PrefissoInternazionale = request.PrefissoInternazionale;
            telefono.Numero = request.Numero;
            telefono.Estensione = request.Estensione;
            telefono.DataModifica = DateTime.UtcNow;

            await context.SaveChangesAsync();

            // Carica dettagli per risposta
            var telefonoWithDetails = await context.Telefoni
                .Include(t => t.TipoTelefono)
                .FirstOrDefaultAsync(t => t.TelefonoId == id);

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
          .WithName("UpdateTelefono")
          .Produces<TelefonoDTO.Response>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
