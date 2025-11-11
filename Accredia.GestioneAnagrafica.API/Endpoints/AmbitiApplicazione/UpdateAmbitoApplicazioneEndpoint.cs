using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.AmbitiApplicazione;

public class UpdateAmbitoApplicazioneEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/ambiti-applicazione/{id}", async (
            [FromRoute] int id,
            [FromBody] AmbitoApplicazioneDTO.Update request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<AmbitoApplicazioneDTO.Update> validator) =>
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

            // Recupera entità esistente
            var ambito = await context.AmbitoApplicazione
                .FirstOrDefaultAsync(a => a.AmbitoApplicazioneId == id && a.DataCancellazione == null);

            if (ambito == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Ambito Applicazione non trovato."
                });
            }

            // Verifica univocità codice (escluso l'ambito corrente)
            var codiceEsistente = await context.AmbitoApplicazione
                .AnyAsync(a => a.Codice == request.Codice 
                    && a.AmbitoApplicazioneId != id 
                    && a.DataCancellazione == null);

            if (codiceEsistente)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = $"Esiste già un altro Ambito Applicazione con codice '{request.Codice}'"
                });
            }

            // Aggiornamento campi
            ambito.Codice = request.Codice;
            ambito.Denominazione = request.Denominazione;
            ambito.Descrizione = request.Descrizione;
            ambito.Ordine = request.Ordine;
            ambito.Attivo = request.Attivo;
            ambito.DataModifica = DateTime.UtcNow;

            await context.SaveChangesAsync();

            var response = new AmbitoApplicazioneDTO.Response
            {
                AmbitoApplicazioneId = ambito.AmbitoApplicazioneId,
                Codice = ambito.Codice,
                Denominazione = ambito.Denominazione,
                Descrizione = ambito.Descrizione,
                Ordine = ambito.Ordine,
                Attivo = ambito.Attivo,
                DataCreazione = ambito.DataCreazione,
                DataModifica = ambito.DataModifica,
                DataInizioValidita = ambito.DataInizioValidita,
                DataFineValidita = ambito.DataFineValidita,
                IsDeleted = ambito.IsDeleted
            };

            return Results.Ok(response);
        })
          .WithTags("AmbitiApplicazione")
          .WithName("UpdateAmbitoApplicazione")
          .Produces<AmbitoApplicazioneDTO.Response>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
