using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.OrganismiAccreditati;

public class GetOrganismiAccreditatiEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Get all (paginated)
        app.MapGet("/api/organismi-accreditati", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? enteAccreditamentoId = null) =>
        {
            var query = context.OrganismiAccreditati
                .Include(o => o.EnteAccreditamento)
                .Where(o => !o.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(o =>
                    o.RagioneSociale.Contains(searchTerm) ||
                    (o.PartitaIVA != null && o.PartitaIVA.Contains(searchTerm)) ||
                    (o.CodiceFiscale != null && o.CodiceFiscale.Contains(searchTerm)));
            }

            if (enteAccreditamentoId.HasValue)
            {
                query = query.Where(o => o.EnteAccreditamentoId == enteAccreditamentoId.Value);
            }

            var totalRecords = await query.CountAsync();

            var organismi = await query
                .OrderBy(o => o.RagioneSociale)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrganismoAccreditatoDTO.List
                {
                    EntitaAziendaleId = o.EntitaAziendaleId,
                    RagioneSociale = o.RagioneSociale,
                    PartitaIVA = o.PartitaIVA,
                    CodiceFiscale = o.CodiceFiscale,
                    NomeEnteAccreditamento = o.EnteAccreditamento != null ? o.EnteAccreditamento.Nome : null,
                    DataCreazione = o.DataCreazione
                })
                .ToListAsync();

            var pageResult = new PageResult<OrganismoAccreditatoDTO.List>
            {
                Data = organismi,
                TotalRecords = totalRecords,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(ApiResponse<PageResult<OrganismoAccreditatoDTO.List>>.SuccessResponse(pageResult));
        }).RequireAuthorization();

        // Get by ID
        app.MapGet("/api/organismi-accreditati/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var organismo = await context.OrganismiAccreditati
                .Include(o => o.EnteAccreditamento)
                .FirstOrDefaultAsync(o => o.EntitaAziendaleId == id && !o.IsDeleted);

            if (organismo == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Organismo Accreditato non trovato."
                });
            }

            var response = new OrganismoAccreditatoDTO.Response
            {
                EntitaAziendaleId = organismo.EntitaAziendaleId,
                RagioneSociale = organismo.RagioneSociale,
                PartitaIVA = organismo.PartitaIVA,
                CodiceFiscale = organismo.CodiceFiscale,
                TipoOrganismoId = organismo.TipoOrganismoId,
                EnteAccreditamentoId = organismo.EnteAccreditamentoId,
                NomeEnteAccreditamento = organismo.EnteAccreditamento?.Nome,
                DataCreazione = organismo.DataCreazione,
                DataModifica = organismo.DataModifica,
                IsDeleted = organismo.IsDeleted
            };

            return Results.Ok(ApiResponse<OrganismoAccreditatoDTO.Response>.SuccessResponse(response));
        }).RequireAuthorization();
    }
}
