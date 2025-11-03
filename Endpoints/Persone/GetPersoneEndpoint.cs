using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Persone;

public class GetPersoneEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/persone", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? entitaAziendaleId = null,
            [FromQuery] string? genere = null) =>
        {
            // Fix per CS0266: Dopo aver usato Include, assegnare la query a IQueryable per evitare problemi di tipo.
            // Fix per CA1862: Non usare StringComparison in query EF Core, perché non è tradotto in SQL. Mantenere ToLower() per compatibilità con EF Core.

            var query = context.Persone
                .Where(p => p.DataCancellazione == null)
                .Include(p => p.EntitaAziendale)
                .AsQueryable(); // <-- Fix CS0266

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(p =>
                    p.Nome.ToLower().Contains(searchLower) ||
                    p.Cognome.ToLower().Contains(searchLower) ||
                    p.CodiceFiscale.ToLower().Contains(searchLower));
                // CA1862: NON usare StringComparison qui, EF Core non lo supporta in SQL.
            }

            if (entitaAziendaleId.HasValue)
                query = query.Where(p => p.EntitaAziendaleId == entitaAziendaleId.Value);

            if (!string.IsNullOrWhiteSpace(genere))
                query = query.Where(p => p.Genere == genere.ToUpper());

            query = query.OrderBy(p => p.Cognome).ThenBy(p => p.Nome);

            var totalCount = await query.CountAsync();

            var persone = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PersonaListItemResponse
                {
                    PersonaId = p.PersonaId,
                    Nome = p.Nome,
                    Cognome = p.Cognome,
                    CodiceFiscale = p.CodiceFiscale,
                    Qualifica = p.Qualifica,
                    EntitaAziendaleName = p.EntitaAziendale != null ? p.EntitaAziendale.Denominazione : null,
                    DataCreazione = p.DataCreazione
                })
                .ToListAsync();

            var pageResult = new PageResult<PersonaListItemResponse>
            {
                Data = persone,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
          .WithTags("Persone")
          .WithName("GetPersone")
          .Produces<PageResult<PersonaListItemResponse>>(200);

        app.MapGet("/api/persone/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var persona = await context.Persone
                .Include(p => p.EntitaAziendale)
                .FirstOrDefaultAsync(p => p.PersonaId == id && p.DataCancellazione == null);

            if (persona == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Persona non trovata" });

            var response = new PersonaResponse
            {
                PersonaId = persona.PersonaId,
                Nome = persona.Nome,
                Cognome = persona.Cognome,
                CodiceFiscale = persona.CodiceFiscale,
                Genere = persona.Genere,
                DataNascita = persona.DataNascita,
                LuogoNascita = persona.LuogoNascita,
                Qualifica = persona.Qualifica,
                Titolo = persona.Titolo,
                EntitaAziendaleId = persona.EntitaAziendaleId,
                EntitaAziendaleName = persona.EntitaAziendale?.Denominazione,
                PrivacyConsent = persona.PrivacyConsent,
                DataConsensoPrivacy = persona.DataConsensoPrivacy,
                Note = persona.Note,
                DataCreazione = persona.DataCreazione,
                DataModifica = persona.DataModifica,
                CreatoDa = persona.CreatoDa,
                ModificatoDa = persona.ModificatoDa,
                RowGuid = persona.RowGuid
            };

            return Results.Ok(response);
        })
          .WithTags("Persone")
          .WithName("GetPersonaById")
          .Produces<PersonaResponse>(200)
          .Produces<ApiResponse>(404);

        app.MapGet("/api/persone/by-cf/{codiceFiscale}", async (
            [FromRoute] string codiceFiscale,
            [FromServices] PersoneDbContext context) =>
        {
            var persona = await context.Persone
                .Include(p => p.EntitaAziendale)
                .FirstOrDefaultAsync(p => p.CodiceFiscale.ToUpper() == codiceFiscale.ToUpper() && p.DataCancellazione == null);

            if (persona == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = $"Persona non trovata: {codiceFiscale}" });

            var response = new PersonaResponse
            {
                PersonaId = persona.PersonaId,
                Nome = persona.Nome,
                Cognome = persona.Cognome,
                CodiceFiscale = persona.CodiceFiscale,
                Genere = persona.Genere,
                DataNascita = persona.DataNascita,
                LuogoNascita = persona.LuogoNascita,
                Qualifica = persona.Qualifica,
                EntitaAziendaleId = persona.EntitaAziendaleId,
                EntitaAziendaleName = persona.EntitaAziendale?.Denominazione,
                PrivacyConsent = persona.PrivacyConsent,
                DataConsensoPrivacy = persona.DataConsensoPrivacy,
                DataCreazione = persona.DataCreazione,
                RowGuid = persona.RowGuid
            };

            return Results.Ok(response);
        })
          .WithTags("Persone")
          .WithName("GetPersonaByCodiceFiscale")
          .Produces<PersonaResponse>(200)
          .Produces<ApiResponse>(404);
    }
}
