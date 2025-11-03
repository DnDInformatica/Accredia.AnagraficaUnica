using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Models;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Indirizzi;

public class PersonaIndirizzoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // GET: Lista indirizzi di una persona
        app.MapGet("/api/persone/{personaId}/indirizzi", async (
            [FromRoute] int personaId,
            [FromServices] PersoneDbContext context) =>
        {
            var personaExists = await context.Set<Persona>()
                .AnyAsync(p => p.PersonaId == personaId && p.DataCancellazione == null);

            if (!personaExists)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Persona non trovata"
                });
            }

            var indirizzi = await context.Set<PersonaIndirizzo>()
                .Where(pi => pi.PersonaId == personaId && pi.DataCancellazione == null)
                .Select(pi => new PersonaIndirizzoDTO.Response
                {
                    PersonaIndirizzoId = pi.PersonaIndirizzoId,
                    PersonaId = pi.PersonaId.Value,
                    IndirizzoId = pi.IndirizzoId.Value,
                    TipoIndirizzoId = pi.TipoIndirizzoId.Value,
                    Attivo = pi.Attivo,
                    DataCreazione = pi.DataCreazione,
                    Indirizzo = context.Set<Indirizzo>()
                        .Where(i => i.IndirizzoId == pi.IndirizzoId && i.DataCancellazione == null)
                        .Select(i => new IndirizzoDTO.Response
                        {
                            IndirizzoId = i.IndirizzoId,
                            Via = i.Via,
                            NumeroCivico = i.NumeroCivico,
                            CAP = i.CAP,
                            Citta = i.Citta,
                            Provincia = i.Provincia,
                            Regione = i.Regione,
                            Stato = i.Stato,
                            IndirizzoCompleto = i.IndirizzoCompleto,
                            Latitudine = i.Latitudine,
                            Longitudine = i.Longitudine,
                            DataCreazione = i.DataCreazione,
                            DataModifica = i.DataModifica,
                            RowGuid = i.RowGuid
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Results.Ok(indirizzi);
        })
        .WithTags("Persone", "Indirizzi")
        .WithName("GetIndirizziPersona")
        .Produces<List<PersonaIndirizzoDTO.Response>>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status404NotFound);

        // POST: Collega indirizzo a persona
        app.MapPost("/api/persone/{personaId}/indirizzi", async (
            [FromRoute] int personaId,
            [FromBody] PersonaIndirizzoDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<PersonaIndirizzoDTO.Create> validator,
            CancellationToken cancellationToken) =>
        {
            // Override personaId dal route
            request.PersonaId = personaId;

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

            // Verifica esistenza persona
            var personaExists = await context.Set<Persona>()
                .AnyAsync(p => p.PersonaId == personaId && p.DataCancellazione == null, cancellationToken);

            if (!personaExists)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Persona non trovata"
                });
            }

            // Verifica esistenza indirizzo
            var indirizzoExists = await context.Set<Indirizzo>()
                .AnyAsync(i => i.IndirizzoId == request.IndirizzoId && i.DataCancellazione == null, cancellationToken);

            if (!indirizzoExists)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Indirizzo non trovato"
                });
            }

            // Verifica se relazione già esiste
            var exists = await context.Set<PersonaIndirizzo>()
                .AnyAsync(pi => pi.PersonaId == personaId 
                    && pi.IndirizzoId == request.IndirizzoId 
                    && pi.DataCancellazione == null, cancellationToken);

            if (exists)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Questo indirizzo è già collegato alla persona"
                });
            }

            try
            {
                var personaIndirizzo = new PersonaIndirizzo
                {
                    PersonaId = personaId,
                    IndirizzoId = request.IndirizzoId,
                    TipoIndirizzoId = request.TipoIndirizzoId,
                    Attivo = request.Attivo,
                    DataCreazione = DateTime.UtcNow,
                    DataInizioValidita = DateTime.UtcNow
                };

                context.Set<PersonaIndirizzo>().Add(personaIndirizzo);
                await context.SaveChangesAsync(cancellationToken);

                // Recupera indirizzo collegato
                var indirizzo = await context.Set<Indirizzo>()
                    .FirstAsync(i => i.IndirizzoId == request.IndirizzoId, cancellationToken);

                var response = new PersonaIndirizzoDTO.Response
                {
                    PersonaIndirizzoId = personaIndirizzo.PersonaIndirizzoId,
                    PersonaId = personaId,
                    IndirizzoId = request.IndirizzoId,
                    TipoIndirizzoId = request.TipoIndirizzoId,
                    Attivo = request.Attivo,
                    DataCreazione = personaIndirizzo.DataCreazione,
                    Indirizzo = new IndirizzoDTO.Response
                    {
                        IndirizzoId = indirizzo.IndirizzoId,
                        Via = indirizzo.Via,
                        NumeroCivico = indirizzo.NumeroCivico,
                        CAP = indirizzo.CAP,
                        Citta = indirizzo.Citta,
                        Provincia = indirizzo.Provincia,
                        Regione = indirizzo.Regione,
                        Stato = indirizzo.Stato,
                        IndirizzoCompleto = indirizzo.IndirizzoCompleto,
                        Latitudine = indirizzo.Latitudine,
                        Longitudine = indirizzo.Longitudine,
                        DataCreazione = indirizzo.DataCreazione,
                        RowGuid = indirizzo.RowGuid
                    }
                };

                return Results.Created($"/api/persone/{personaId}/indirizzi", response);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Errore durante il collegamento dell'indirizzo",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        })
        .WithTags("Persone", "Indirizzi")
        .WithName("CollegaIndirizzoPersona")
        .Produces<PersonaIndirizzoDTO.Response>(StatusCodes.Status201Created)
        .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse>(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);


        // PUT: Aggiorna collegamento indirizzo-persona
        app.MapPut("/api/persone/{personaId}/indirizzi/{personaIndirizzoId}", async (
            [FromRoute] int personaId,
            [FromRoute] int personaIndirizzoId,
            [FromBody] PersonaIndirizzoDTO.Update request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<PersonaIndirizzoDTO.Update> validator,
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

            var personaIndirizzo = await context.Set<PersonaIndirizzo>()
                .FirstOrDefaultAsync(pi => pi.PersonaIndirizzoId == personaIndirizzoId 
                    && pi.PersonaId == personaId 
                    && pi.DataCancellazione == null, cancellationToken);

            if (personaIndirizzo == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Collegamento indirizzo-persona non trovato"
                });
            }

            try
            {
                personaIndirizzo.TipoIndirizzoId = request.TipoIndirizzoId;
                personaIndirizzo.Attivo = request.Attivo;
                personaIndirizzo.DataModifica = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Collegamento aggiornato con successo"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Errore durante l'aggiornamento",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        })
        .WithTags("Persone", "Indirizzi")
        .WithName("AggiornaCollegamentoIndirizzoPersona")
        .Produces<ApiResponse>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse>(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        // DELETE: Scollega indirizzo da persona (soft delete)
        app.MapDelete("/api/persone/{personaId}/indirizzi/{personaIndirizzoId}", async (
            [FromRoute] int personaId,
            [FromRoute] int personaIndirizzoId,
            [FromServices] PersoneDbContext context,
            CancellationToken cancellationToken) =>
        {
            var personaIndirizzo = await context.Set<PersonaIndirizzo>()
                .FirstOrDefaultAsync(pi => pi.PersonaIndirizzoId == personaIndirizzoId 
                    && pi.PersonaId == personaId 
                    && pi.DataCancellazione == null, cancellationToken);

            if (personaIndirizzo == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Collegamento indirizzo-persona non trovato"
                });
            }

            try
            {
                // Soft delete
                personaIndirizzo.DataCancellazione = DateTime.UtcNow;
                personaIndirizzo.DataFineValidita = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Collegamento eliminato con successo"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Errore durante l'eliminazione",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        })
        .WithTags("Persone", "Indirizzi")
        .WithName("ScollegaIndirizzoPersona")
        .Produces<ApiResponse>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
