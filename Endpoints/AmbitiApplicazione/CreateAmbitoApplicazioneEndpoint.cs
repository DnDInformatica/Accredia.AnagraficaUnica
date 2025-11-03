using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.AmbitiApplicazione;

public class CreateAmbitoApplicazioneEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/ambiti-applicazione", async (
            [FromBody] AmbitoApplicazioneDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<AmbitoApplicazioneDTO.Create> validator) =>
        {
            // Validazione
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToList()
                    );

                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Errori di validazione",
                    Errors = errors
                });
            }

            // Verifica univocità codice
            var codiceEsistente = await context.AmbitoApplicazione
                .AnyAsync(a => a.Codice == request.Codice && a.DataCancellazione == null);

            if (codiceEsistente)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = $"Esiste già un Ambito Applicazione con codice '{request.Codice}'"
                });
            }

            // Creazione entità
            var nuovoAmbito = new Models.AmbitoApplicazione
            {
                Codice = request.Codice,
                Denominazione = request.Denominazione,
                Descrizione = request.Descrizione,
                Ordine = request.Ordine,
                Attivo = request.Attivo,
                RowGuid = Guid.NewGuid(),
                DataCreazione = DateTime.UtcNow,
                DataInizioValidita = DateTime.UtcNow,
                DataFineValidita = DateTime.MaxValue
            };

            context.AmbitoApplicazione.Add(nuovoAmbito);
            await context.SaveChangesAsync();

            var response = new AmbitoApplicazioneDTO.Response
            {
                AmbitoApplicazioneId = nuovoAmbito.AmbitoApplicazioneId,
                Codice = nuovoAmbito.Codice,
                Denominazione = nuovoAmbito.Denominazione,
                Descrizione = nuovoAmbito.Descrizione,
                Ordine = nuovoAmbito.Ordine,
                Attivo = nuovoAmbito.Attivo,
                DataCreazione = nuovoAmbito.DataCreazione,
                DataInizioValidita = nuovoAmbito.DataInizioValidita,
                DataFineValidita = nuovoAmbito.DataFineValidita,
                IsDeleted = false
            };

            return Results.Created($"/api/ambiti-applicazione/{nuovoAmbito.AmbitoApplicazioneId}", response);
        })
          .WithTags("AmbitiApplicazione")
          .WithName("CreateAmbitoApplicazione")
          .Produces<AmbitoApplicazioneDTO.Response>(StatusCodes.Status201Created)
          .Produces<ApiResponse>(StatusCodes.Status400BadRequest);
    }
}
