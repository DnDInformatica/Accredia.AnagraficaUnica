using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Indirizzi;

public class GetIndirizziEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // GET: Lista indirizzi paginata
        app.MapGet("/api/indirizzi", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? cap = null,
            [FromQuery] string? citta = null,
            [FromQuery] string? provincia = null,
            [FromQuery] string? orderBy = "Citta") =>
        {
            var query = context.Set<Indirizzo>()
                .Where(i => i.DataCancellazione == null);

            // Filtro ricerca
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(i =>
                    i.Via.ToLower().Contains(searchLower) ||
                    i.Citta.ToLower().Contains(searchLower) ||
                    i.CAP.Contains(searchLower));
            }

            // Filtro CAP
            if (!string.IsNullOrWhiteSpace(cap))
                query = query.Where(i => i.CAP == cap);

            // Filtro città
            if (!string.IsNullOrWhiteSpace(citta))
                query = query.Where(i => i.Citta.ToLower() == citta.ToLower());

            // Filtro provincia
            if (!string.IsNullOrWhiteSpace(provincia))
                query = query.Where(i => i.Provincia.ToUpper() == provincia.ToUpper());

            // Ordinamento
            query = orderBy?.ToLower() switch
            {
                "via" => query.OrderBy(i => i.Via),
                "cap" => query.OrderBy(i => i.CAP),
                "citta" => query.OrderBy(i => i.Citta),
                "provincia" => query.OrderBy(i => i.Provincia),
                _ => query.OrderBy(i => i.Citta).ThenBy(i => i.Via)
            };

            var totalCount = await query.CountAsync();

            var indirizzi = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new IndirizzoDTO.List
                {
                    IndirizzoId = i.IndirizzoId,
                    IndirizzoCompleto = i.IndirizzoCompleto,
                    Citta = i.Citta,
                    Provincia = i.Provincia,
                    CAP = i.CAP
                })
                .ToListAsync();

            var pageResult = new PageResult<IndirizzoDTO.List>
            {
                Data = indirizzi,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
        .WithTags("Indirizzi")
        .WithName("GetIndirizzi")
        .Produces<PageResult<IndirizzoDTO.List>>(StatusCodes.Status200OK);

        // GET: Dettaglio indirizzo
        app.MapGet("/api/indirizzi/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var indirizzo = await context.Set<Indirizzo>()
                .FirstOrDefaultAsync(i => i.IndirizzoId == id && i.DataCancellazione == null);

            if (indirizzo == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Indirizzo non trovato"
                });
            }

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
                DataModifica = indirizzo.DataModifica,
                RowGuid = indirizzo.RowGuid
            };

            return Results.Ok(response);
        })
        .WithTags("Indirizzi")
        .WithName("GetIndirizzoById")
        .Produces<IndirizzoDTO.Response>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status404NotFound);

        // GET: Ricerca per CAP
        app.MapGet("/api/indirizzi/by-cap/{cap}", async (
            [FromRoute] string cap,
            [FromServices] PersoneDbContext context) =>
        {
            var indirizzi = await context.Set<Indirizzo>()
                .Where(i => i.CAP == cap && i.DataCancellazione == null)
                .Select(i => new IndirizzoDTO.List
                {
                    IndirizzoId = i.IndirizzoId,
                    IndirizzoCompleto = i.IndirizzoCompleto,
                    Citta = i.Citta,
                    Provincia = i.Provincia,
                    CAP = i.CAP
                })
                .ToListAsync();

            return Results.Ok(indirizzi);
        })
        .WithTags("Indirizzi")
        .WithName("GetIndirizziByCAP")
        .Produces<List<IndirizzoDTO.List>>(StatusCodes.Status200OK);

        // GET: Ricerca per città
        app.MapGet("/api/indirizzi/by-citta/{citta}", async (
            [FromRoute] string citta,
            [FromServices] PersoneDbContext context) =>
        {
            var indirizzi = await context.Set<Indirizzo>()
                .Where(i => i.Citta.ToLower() == citta.ToLower() && i.DataCancellazione == null)
                .Select(i => new IndirizzoDTO.List
                {
                    IndirizzoId = i.IndirizzoId,
                    IndirizzoCompleto = i.IndirizzoCompleto,
                    Citta = i.Citta,
                    Provincia = i.Provincia,
                    CAP = i.CAP
                })
                .ToListAsync();

            return Results.Ok(indirizzi);
        })
        .WithTags("Indirizzi")
        .WithName("GetIndirizziByCitta")
        .Produces<List<IndirizzoDTO.List>>(StatusCodes.Status200OK);
    }
}
