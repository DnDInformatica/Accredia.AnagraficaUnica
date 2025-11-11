using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.OrganismiAccreditati;

public class UpdateOrganismoAccreditatoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/organismi-accreditati/{id}", async (
            [FromRoute] int id,
            [FromBody] OrganismoAccreditatoDTO.Update request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<OrganismoAccreditatoDTO.Update> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var organismo = await context.OrganismiAccreditati
                .FirstOrDefaultAsync(o => o.EntitaAziendaleId == id);

            if (organismo == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Organismo Accreditato non trovato."
                });
            }

            // Check duplicates if changed
            if (!string.IsNullOrWhiteSpace(request.PartitaIVA) && organismo.PartitaIVA != request.PartitaIVA)
            {
                var existingByPIva = await context.OrganismiAccreditati
                    .FirstOrDefaultAsync(o => o.PartitaIVA == request.PartitaIVA && 
                                              o.EntitaAziendaleId != id);

                if (existingByPIva != null)
                {
                    return Results.BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Un organismo con questa Partita IVA esiste già."
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(request.CodiceFiscale) && organismo.CodiceFiscale != request.CodiceFiscale)
            {
                var existingByCF = await context.OrganismiAccreditati
                    .FirstOrDefaultAsync(o => o.CodiceFiscale == request.CodiceFiscale && 
                                              o.EntitaAziendaleId != id);

                if (existingByCF != null)
                {
                    return Results.BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Un organismo con questo Codice Fiscale esiste già."
                    });
                }
            }

            // Update
            organismo.RagioneSociale = request.RagioneSociale;
            organismo.PartitaIVA = request.PartitaIVA;
            organismo.CodiceFiscale = request.CodiceFiscale;
            organismo.TipoOrganismoId = request.TipoOrganismoId;
            organismo.EnteAccreditamentoId = request.EnteAccreditamentoId;
            organismo.DataModifica = DateTime.UtcNow;

            await context.SaveChangesAsync();

            var response = new OrganismoAccreditatoDTO.Response
            {
                EntitaAziendaleId = organismo.EntitaAziendaleId,
                RagioneSociale = organismo.RagioneSociale,
                PartitaIVA = organismo.PartitaIVA,
                CodiceFiscale = organismo.CodiceFiscale,
                TipoOrganismoId = organismo.TipoOrganismoId,
                EnteAccreditamentoId = organismo.EnteAccreditamentoId,
                DataCreazione = organismo.DataCreazione,
                DataModifica = organismo.DataModifica
            };

            return Results.Ok(ApiResponse<OrganismoAccreditatoDTO.Response>.SuccessResponse(response));
        })
            .WithTags("OrganismiAccreditati")
            .WithName("UpdateOrganismoAccreditato")
            .Produces<OrganismoAccreditatoDTO.Response>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization();
    }
}
