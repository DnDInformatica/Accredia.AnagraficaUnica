using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Models;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Indirizzi;

public class UpdateIndirizzoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // PUT: Aggiorna indirizzo
        app.MapPut("/api/indirizzi/{id}", async (
            [FromRoute] int id,
            [FromBody] IndirizzoDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<IndirizzoDTO.Create> validator,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());

                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Errori di validazione",
                    Errors = errors
                });
            }

            var indirizzo = await context.Set<Indirizzo>()
                .FirstOrDefaultAsync(i => i.IndirizzoId == id && i.DataCancellazione == null, cancellationToken);

            if (indirizzo == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Indirizzo non trovato"
                });
            }

            try
            {
                indirizzo.Via = request.Via.Trim();
                indirizzo.NumeroCivico = request.NumeroCivico?.Trim();
                indirizzo.CAP = request.CAP.Trim();
                indirizzo.Citta = request.Citta.Trim();
                indirizzo.Provincia = request.Provincia.ToUpperInvariant().Trim();
                indirizzo.Regione = request.Regione?.Trim();
                indirizzo.Stato = request.Stato.Trim();
                indirizzo.ComuneId = request.ComuneId;
                indirizzo.Latitudine = request.Latitudine;
                indirizzo.Longitudine = request.Longitudine;
                indirizzo.Note = request.Note?.Trim();
                indirizzo.DataModifica = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                var response = new IndirizzoDTO.Response
                {
                    IndirizzoId = indirizzo.IndirizzoId,
                    Via = indirizzo.Via,
                    NumeroCivico = indirizzo.NumeroCivico,
                    CAP = indirizzo.CAP,
                    Citta = indirizzo.Citta,
                    Provincia = indirizzo.Provincia,
                    Regione = indirizzo.Regione,
                    Stato = indirizzo.Stato,
                    Latitudine = indirizzo.Latitudine,
                    Longitudine = indirizzo.Longitudine,
                    IndirizzoCompleto = indirizzo.IndirizzoCompleto,
                    DataCreazione = indirizzo.DataCreazione,
                    DataModifica = indirizzo.DataModifica,
                    RowGuid = indirizzo.RowGuid
                };

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Errore durante l'aggiornamento dell'indirizzo",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        })
        .WithTags("Indirizzi")
        .WithName("UpdateIndirizzo")
        .Produces<IndirizzoDTO.Response>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse>(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

    }
}
