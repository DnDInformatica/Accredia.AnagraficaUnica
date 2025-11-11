using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Tipologiche;

/// <summary>
/// Endpoint per la gestione delle Tipologiche
/// Tabelle di lookup READ-ONLY: TipoEmail, TipoTelefono, TipoIndirizzo, TipoEnteAccreditamento, TitoloOnorifico
/// </summary>
public class GetTipologicheEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // ==================== TipoEmail ====================
        
        app.MapGet("/api/tipologiche/tipi-email", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50) =>
        {
            var query = context.TipiEmail.AsQueryable();

            var totalCount = await query.CountAsync();

            var tipi = await query
                .OrderBy(t => t.Codice)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TipoEmailDTO
                {
                    TipoEmailId = t.TipoEmailId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            var pageResult = new PageResult<TipoEmailDTO>
            {
                Data = tipi,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
          .WithTags("Tipologiche")
          .WithName("GetTipiEmail")
          .Produces<PageResult<TipoEmailDTO>>(200);

        app.MapGet("/api/tipologiche/tipi-email/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var tipo = await context.TipiEmail
                .FirstOrDefaultAsync(t => t.TipoEmailId == id);

            if (tipo == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Tipo Email non trovato" });

            var response = new TipoEmailDTO
            {
                TipoEmailId = tipo.TipoEmailId,
                Codice = tipo.Codice,
                Descrizione = tipo.Descrizione,
                DataCreazione = tipo.DataCreazione,
                DataInizioValidita = tipo.DataInizioValidita,
                DataFineValidita = tipo.DataFineValidita
            };

            return Results.Ok(response);
        })
          .WithTags("Tipologiche")
          .WithName("GetTipoEmailById")
          .Produces<TipoEmailDTO>(200)
          .Produces<ApiResponse>(404);

        // ==================== TipoTelefono ====================

        app.MapGet("/api/tipologiche/tipi-telefono", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50) =>
        {
            var query = context.TipiTelefono.AsQueryable();

            var totalCount = await query.CountAsync();

            var tipi = await query
                .OrderBy(t => t.Codice)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TipoTelefonoDTO
                {
                    TipoTelefonoId = t.TipoTelefonoId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            var pageResult = new PageResult<TipoTelefonoDTO>
            {
                Data = tipi,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
          .WithTags("Tipologiche")
          .WithName("GetTipiTelefono")
          .Produces<PageResult<TipoTelefonoDTO>>(200);

        app.MapGet("/api/tipologiche/tipi-telefono/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var tipo = await context.TipiTelefono
                .FirstOrDefaultAsync(t => t.TipoTelefonoId == id);

            if (tipo == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Tipo Telefono non trovato" });

            var response = new TipoTelefonoDTO
            {
                TipoTelefonoId = tipo.TipoTelefonoId,
                Codice = tipo.Codice,
                Descrizione = tipo.Descrizione,
                DataCreazione = tipo.DataCreazione,
                DataInizioValidita = tipo.DataInizioValidita,
                DataFineValidita = tipo.DataFineValidita
            };

            return Results.Ok(response);
        })
          .WithTags("Tipologiche")
          .WithName("GetTipoTelefonoById")
          .Produces<TipoTelefonoDTO>(200)
          .Produces<ApiResponse>(404);

        // ==================== TipoIndirizzo ====================

        app.MapGet("/api/tipologiche/tipi-indirizzo", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50) =>
        {
            var query = context.TipiIndirizzo.AsQueryable();

            var totalCount = await query.CountAsync();

            var tipi = await query
                .OrderBy(t => t.Codice)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TipoIndirizzoDTO
                {
                    TipoIndirizzoId = t.TipoIndirizzoId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            var pageResult = new PageResult<TipoIndirizzoDTO>
            {
                Data = tipi,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
          .WithTags("Tipologiche")
          .WithName("GetTipiIndirizzo")
          .Produces<PageResult<TipoIndirizzoDTO>>(200);

        app.MapGet("/api/tipologiche/tipi-indirizzo/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var tipo = await context.TipiIndirizzo
                .FirstOrDefaultAsync(t => t.TipoIndirizzoId == id);

            if (tipo == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Tipo Indirizzo non trovato" });

            var response = new TipoIndirizzoDTO
            {
                TipoIndirizzoId = tipo.TipoIndirizzoId,
                Codice = tipo.Codice,
                Descrizione = tipo.Descrizione,
                DataCreazione = tipo.DataCreazione,
                DataInizioValidita = tipo.DataInizioValidita,
                DataFineValidita = tipo.DataFineValidita
            };

            return Results.Ok(response);
        })
          .WithTags("Tipologiche")
          .WithName("GetTipoIndirizzoById")
          .Produces<TipoIndirizzoDTO>(200)
          .Produces<ApiResponse>(404);

        // ==================== TipoEnteAccreditamento ====================

        app.MapGet("/api/tipologiche/tipi-ente-accreditamento", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50) =>
        {
            var query = context.TipiEnteAccreditamento.AsQueryable();

            var totalCount = await query.CountAsync();

            var tipi = await query
                .OrderBy(t => t.Codice)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TipoEnteAccreditamentoDTO
                {
                    TipoEnteAccreditamentoId = t.TipoEnteAccreditamentoId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    UniqueRowId = t.UniqueRowId,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            var pageResult = new PageResult<TipoEnteAccreditamentoDTO>
            {
                Data = tipi,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
          .WithTags("Tipologiche")
          .WithName("GetTipiEnteAccreditamento")
          .Produces<PageResult<TipoEnteAccreditamentoDTO>>(200);

        app.MapGet("/api/tipologiche/tipi-ente-accreditamento/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var tipo = await context.TipiEnteAccreditamento
                .FirstOrDefaultAsync(t => t.TipoEnteAccreditamentoId == id);

            if (tipo == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Tipo Ente Accreditamento non trovato" });

            var response = new TipoEnteAccreditamentoDTO
            {
                TipoEnteAccreditamentoId = tipo.TipoEnteAccreditamentoId,
                Codice = tipo.Codice,
                Descrizione = tipo.Descrizione,
                UniqueRowId = tipo.UniqueRowId,
                DataCreazione = tipo.DataCreazione,
                DataInizioValidita = tipo.DataInizioValidita,
                DataFineValidita = tipo.DataFineValidita
            };

            return Results.Ok(response);
        })
          .WithTags("Tipologiche")
          .WithName("GetTipoEnteAccreditamentoById")
          .Produces<TipoEnteAccreditamentoDTO>(200)
          .Produces<ApiResponse>(404);

        // ==================== TitoloOnorifico ====================

        app.MapGet("/api/tipologiche/titoli-onorifici", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50) =>
        {
            var query = context.TitoliOnorifici.AsQueryable();

            var totalCount = await query.CountAsync();

            var titoli = await query
                .OrderBy(t => t.Codice)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TitoloOnorificoDTO
                {
                    TitoloOnorificoId = t.TitoloOnorificoId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    TitoloMaschile = t.TitoloMaschile,
                    TitoloFemminile = t.TitoloFemminile,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            var pageResult = new PageResult<TitoloOnorificoDTO>
            {
                Data = titoli,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
          .WithTags("Tipologiche")
          .WithName("GetTitoliOnorifici")
          .Produces<PageResult<TitoloOnorificoDTO>>(200);

        app.MapGet("/api/tipologiche/titoli-onorifici/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var titolo = await context.TitoliOnorifici
                .FirstOrDefaultAsync(t => t.TitoloOnorificoId == id);

            if (titolo == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Titolo Onorifico non trovato" });

            var response = new TitoloOnorificoDTO
            {
                TitoloOnorificoId = titolo.TitoloOnorificoId,
                Codice = titolo.Codice,
                Descrizione = titolo.Descrizione,
                TitoloMaschile = titolo.TitoloMaschile,
                TitoloFemminile = titolo.TitoloFemminile,
                DataCreazione = titolo.DataCreazione,
                DataInizioValidita = titolo.DataInizioValidita,
                DataFineValidita = titolo.DataFineValidita
            };

            return Results.Ok(response);
        })
          .WithTags("Tipologiche")
          .WithName("GetTitoloOnorificoById")
          .Produces<TitoloOnorificoDTO>(200)
          .Produces<ApiResponse>(404);
    }
}
