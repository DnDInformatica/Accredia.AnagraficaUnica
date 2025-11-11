using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Indirizzi
{
    public class CreateIndirizzoEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // POST: Crea indirizzo
            app.MapPost("/api/indirizzi", async (
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

                try
                {
                    var indirizzo = new Indirizzo
                    {
                        Via = request.Via.Trim(),
                        NumeroCivico = request.NumeroCivico?.Trim(),
                        CAP = request.CAP.Trim(),
                        Citta = request.Citta.Trim(),
                        Provincia = request.Provincia.ToUpperInvariant().Trim(),
                        Regione = request.Regione?.Trim(),
                        Stato = request.Stato.Trim(),
                        ComuneId = request.ComuneId,
                        Latitudine = request.Latitudine,
                        Longitudine = request.Longitudine,
                        Note = request.Note?.Trim(),
                        DataCreazione = DateTime.UtcNow,
                        DataInizioValidita = DateTime.UtcNow,
                        RowGuid = Guid.NewGuid()
                    };

                    context.Set<Indirizzo>().Add(indirizzo);
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
                        ComuneId = indirizzo.ComuneId,
                        Latitudine = indirizzo.Latitudine,
                        Longitudine = indirizzo.Longitudine,
                        Note = indirizzo.Note,
                        IndirizzoCompleto = indirizzo.IndirizzoCompleto,
                        DataCreazione = indirizzo.DataCreazione,
                        RowGuid = indirizzo.RowGuid
                    };

                    return Results.Created($"/api/indirizzi/{indirizzo.IndirizzoId}", response);
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        title: "Errore durante la creazione dell'indirizzo",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }
            })
            .WithTags("Indirizzi")
            .WithName("CreateIndirizzo")
            .Produces<IndirizzoDTO.Response>(StatusCodes.Status201Created)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}

