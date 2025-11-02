using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.Persone;

public class UpdatePersonaEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/persone/{id}", async (
            [FromRoute] int id,
            [FromBody] CreatePersonaRequest request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<CreatePersonaRequest> validator,
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

            var persona = await context.Persone
                .FirstOrDefaultAsync(p => p.PersonaId == id && p.DataCancellazione == null, cancellationToken);

            if (persona == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Persona non trovata"
                });
            }

            var cfExists = await context.Persone
                .AnyAsync(p => p.CodiceFiscale.ToUpper() == request.CodiceFiscale.ToUpper()
                    && p.PersonaId != id
                    && p.DataCancellazione == null, cancellationToken);

            if (cfExists)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = $"Esiste già un'altra persona con codice fiscale: {request.CodiceFiscale}"
                });
            }

            try
            {
                persona.Nome = request.Nome.Trim();
                persona.Cognome = request.Cognome.Trim();
                persona.CodiceFiscale = request.CodiceFiscale.ToUpperInvariant().Trim();
                persona.Genere = request.Genere.ToUpperInvariant();
                persona.DataNascita = request.DataNascita;
                persona.LuogoNascita = request.LuogoNascita?.Trim();
                persona.ComuneNascitaId = request.ComuneNascitaId;
                persona.Qualifica = request.Qualifica?.Trim();
                persona.Titolo = request.Titolo?.Trim();
                persona.TitoloOnorificoId = request.TitoloOnorificoId;
                persona.PrivacyConsent = request.PrivacyConsent;
                persona.DataConsensoPrivacy = request.DataConsensoPrivacy;
                persona.Note = request.Note?.Trim();
                persona.DataModifica = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                var personaAggiornata = await context.Persone
                    .Include(p => p.EntitaAziendale)
                    .FirstAsync(p => p.PersonaId == id, cancellationToken);

                var response = new PersonaResponse
                {
                    PersonaId = personaAggiornata.PersonaId,
                    Nome = personaAggiornata.Nome,
                    Cognome = personaAggiornata.Cognome,
                    CodiceFiscale = personaAggiornata.CodiceFiscale,
                    Genere = personaAggiornata.Genere,
                    DataNascita = personaAggiornata.DataNascita,
                    LuogoNascita = personaAggiornata.LuogoNascita,
                    Qualifica = personaAggiornata.Qualifica,
                    EntitaAziendaleId = personaAggiornata.EntitaAziendaleId,
                    EntitaAziendaleName = personaAggiornata.EntitaAziendale?.Denominazione,
                    PrivacyConsent = personaAggiornata.PrivacyConsent,
                    DataConsensoPrivacy = personaAggiornata.DataConsensoPrivacy,
                    DataCreazione = personaAggiornata.DataCreazione,
                    DataModifica = personaAggiornata.DataModifica,
                    RowGuid = personaAggiornata.RowGuid
                };

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Errore durante l'aggiornamento della persona",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        })
          .WithTags("Persone")
          .WithName("UpdatePersona")
          .Produces<PersonaResponse>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound)
          .Produces(StatusCodes.Status500InternalServerError);
    }
}
