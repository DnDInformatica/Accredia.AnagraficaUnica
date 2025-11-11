using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Persone;

public class CreatePersonaEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/persone", async (
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

            var entitaExists = await context.EntiAziendali
                .AnyAsync(e => e.EntitaAziendaleId == request.EntitaAziendaleId && e.DataCancellazione == null, cancellationToken);

            if (!entitaExists)
                return Results.BadRequest(new ApiResponse { Success = false, Message = $"EntitàAziendale {request.EntitaAziendaleId} non trovata" });

            var codiceFiscaleExists = await context.Persone
                .AnyAsync(p => p.CodiceFiscale.ToUpper() == request.CodiceFiscale.ToUpper() && p.DataCancellazione == null, cancellationToken);

            if (codiceFiscaleExists)
                return Results.BadRequest(new ApiResponse { Success = false, Message = $"Codice fiscale già esistente: {request.CodiceFiscale}" });

            try
            {
                var persona = new Persona
                {
                    EntitaAziendaleId = request.EntitaAziendaleId, // PK mappa a EntitaAziendaleId nel DB
                    Nome = request.Nome.Trim(),
                    Cognome = request.Cognome.Trim(),
                    CodiceFiscale = request.CodiceFiscale.ToUpperInvariant().Trim(),
                    Genere = request.Genere.ToUpperInvariant(),
                    DataNascita = request.DataNascita,
                    LuogoNascita = request.LuogoNascita?.Trim(),
                    ComuneNascitaId = request.ComuneNascitaId,
                    Qualifica = request.Qualifica?.Trim(),
                    Titolo = request.Titolo?.Trim(),
                    TitoloOnorificoId = request.TitoloOnorificoId,
                    PrivacyConsent = request.PrivacyConsent,
                    DataConsensoPrivacy = request.DataConsensoPrivacy ?? (request.PrivacyConsent ? DateTime.UtcNow : null),
                    Note = request.Note?.Trim(),
                    DataCreazione = DateTime.UtcNow,
                    DataInizioValidita = DateTime.UtcNow,
                    RowGuid = Guid.NewGuid()
                };

                context.Persone.Add(persona);
                await context.SaveChangesAsync(cancellationToken);

                var personaCreata = await context.Persone
                    .Include(p => p.EntitaAziendale)
                    .FirstAsync(p => p.EntitaAziendaleId == persona.EntitaAziendaleId, cancellationToken);

                var response = new PersonaResponse
                {
                    EntitaAziendaleId = personaCreata.EntitaAziendaleId,
                    Nome = personaCreata.Nome,
                    Cognome = personaCreata.Cognome,
                    CodiceFiscale = personaCreata.CodiceFiscale,
                    Genere = personaCreata.Genere,
                    DataNascita = personaCreata.DataNascita,
                    LuogoNascita = personaCreata.LuogoNascita,
                    Qualifica = personaCreata.Qualifica,
                    PrivacyConsent = personaCreata.PrivacyConsent,
                    DataConsensoPrivacy = personaCreata.DataConsensoPrivacy,
                    DataCreazione = personaCreata.DataCreazione,
                    RowGuid = personaCreata.RowGuid
                };

                return Results.Created($"/api/persone/{persona.EntitaAziendaleId}", response);
            }
            catch (Exception ex)
            {
                return Results.Problem(title: "Errore creazione persona", detail: ex.Message, statusCode: 500);
            }
        })
          .WithTags("Persone")
          .WithName("CreatePersona")
          .Produces<PersonaResponse>(201)
          .Produces<ApiResponse>(400);
    }
}
