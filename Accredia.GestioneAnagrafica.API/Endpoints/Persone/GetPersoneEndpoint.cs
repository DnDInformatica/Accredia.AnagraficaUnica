using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Persone;

public class GetPersoneEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Endpoint per caricare TUTTE le persone (per MudDataGrid client-side)
        app.MapGet("/api/persone/all", async (
            [FromServices] PersoneDbContext context) =>
        {
            var persone = await context.Persone
                .Include(p => p.EntitaAziendale)
                .OrderBy(p => p.Cognome).ThenBy(p => p.Nome)
                .Select(p => new PersonaGridItemResponse
                {
                    EntitaAziendaleId = p.EntitaAziendaleId,
                    Nome = p.Nome,
                    Cognome = p.Cognome,
                    CodiceFiscale = p.CodiceFiscale,
                    Genere = p.Genere,
                    DataNascita = p.DataNascita,
                    LuogoNascita = p.LuogoNascita,
                    Qualifica = p.Qualifica,
                    Titolo = p.Titolo,
                    PrivacyConsent = p.PrivacyConsent,
                    DataConsensoPrivacy = p.DataConsensoPrivacy,
                    DataCreazione = p.DataCreazione,
                    DataCancellazione = p.DataCancellazione
                })
                .ToListAsync();

            return Results.Ok(persone);
        })
          .WithTags("Persone")
          .WithName("GetAllPersone")
          .Produces<List<PersonaGridItemResponse>>(200);

        app.MapGet("/api/persone", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? entitaAziendaleId = null,
            [FromQuery] string? genere = null) =>
        {
            // Fix per CS0266: Dopo aver usato Include, assegnare la query a IQueryable per evitare problemi di tipo.
            // Fix per CA1862: Non usare StringComparison in query EF Core, perch� non � tradotto in SQL. Mantenere ToLower() per compatibilit� con EF Core.

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
                    EntitaAziendaleId = p.EntitaAziendaleId,
                    Nome = p.Nome,
                    Cognome = p.Cognome,
                    CodiceFiscale = p.CodiceFiscale,
                    Qualifica = p.Qualifica,
                    EntitaAziendaleName = p.EntitaAziendaleId.ToString(), // EntitaAziendaleId == EntitaAziendaleId
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
                .FirstOrDefaultAsync(p => p.EntitaAziendaleId == id && p.DataCancellazione == null);

            if (persona == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Persona non trovata" });

            var response = new PersonaResponse
            {
                EntitaAziendaleId = persona.EntitaAziendaleId,
                Nome = persona.Nome,
                Cognome = persona.Cognome,
                CodiceFiscale = persona.CodiceFiscale,
                Genere = persona.Genere,
                DataNascita = persona.DataNascita,
                LuogoNascita = persona.LuogoNascita,
                Qualifica = persona.Qualifica,
                Titolo = persona.Titolo,
                PrivacyConsent = persona.PrivacyConsent,
                DataConsensoPrivacy = persona.DataConsensoPrivacy,
                Note = persona.Note,
                DataCreazione = persona.DataCreazione,
                DataModifica = persona.DataModifica,
                CreatoDa = persona.CreatoDa?.ToString(),
                ModificatoDa = persona.ModificatoDa?.ToString(),
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
                EntitaAziendaleId = persona.EntitaAziendaleId,
                Nome = persona.Nome,
                Cognome = persona.Cognome,
                CodiceFiscale = persona.CodiceFiscale,
                Genere = persona.Genere,
                DataNascita = persona.DataNascita,
                LuogoNascita = persona.LuogoNascita,
                Qualifica = persona.Qualifica,
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
