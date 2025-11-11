using System;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;
using AutoMapper;

namespace Accredia.GestioneAnagrafica.API.Endpoints.EntiAccreditamento;

public class GetEntiAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/enti-accreditamento", async (
            [FromServices] PersoneDbContext context,
            [FromServices] IMapper mapper,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true) =>
        {

            try
            {
                var query = context.EntiAccreditamento.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(e =>
                        e.Denominazione.Contains(searchTerm) ||
                        (e.Sigla != null && e.Sigla.Contains(searchTerm)) ||
                        (e.Note != null && e.Note.Contains(searchTerm)));
                }

                // Implementazione ordinamento dinamico
                query = sortBy?.ToLower() switch
                {
                    "denominazione" => ascending ? query.OrderBy(e => e.Denominazione) : query.OrderByDescending(e => e.Denominazione),
                    "sigla" => ascending ? query.OrderBy(e => e.Sigla) : query.OrderByDescending(e => e.Sigla),
                    "datafondazione" => ascending ? query.OrderBy(e => e.DataFondazione) : query.OrderByDescending(e => e.DataFondazione),
                    _ => query.OrderBy(e => e.EntitaAziendaleId)
                };

                var totalCount = await query.CountAsync();

                var entiAccreditamento = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var entiDto = mapper.Map<List<EnteAccreditamentoDTO.Response>>(entiAccreditamento);

                var result = new PageResult<EnteAccreditamentoDTO.Response>
                {
                    Data = entiDto,
                    TotalRecords = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new ApiResponse { Success = false, Message = $"Errore nel recupero dei Enti Accreditamento: {ex.Message}" });
            }
        })
            .WithTags("EntiAccreditamento")
            .WithName("GetEntiAccreditamento")
            .Produces<PageResult<EnteAccreditamentoDTO.Response>>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization();

        // GET: Dettaglio singolo Ente Accreditamento per ID
        app.MapGet("/api/enti-accreditamento/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context,
            [FromServices] IMapper mapper) =>
        {
            if (id <= 0)
            {
                return Results.BadRequest(new ApiResponse { Success = false, Message = "L'ID deve essere maggiore di 0" });
            }

            var ente = await context.EntiAccreditamento
                .Where(e => e.EntitaAziendaleId == id)
                .FirstOrDefaultAsync();

            if (ente == null)
            {
                return Results.NotFound(new ApiResponse { Success = false, Message = $"Ente Accreditamento con ID {id} non trovato" });
            }

            var enteDto = mapper.Map<EnteAccreditamentoDTO.Response>(ente);

            return Results.Ok(enteDto);
        })
            .WithTags("EntiAccreditamento")
            .WithName("GetEnteAccreditamentoById")
            .Produces<EnteAccreditamentoDTO.Response>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
}
