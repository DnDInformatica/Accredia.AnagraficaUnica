using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.Models;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.OrganismiAccreditati;

public class CreateOrganismoAccreditatoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/organismi-accreditati", async (
            [FromBody] OrganismoAccreditatoDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<OrganismoAccreditatoDTO.Create> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            // Check for duplicate P.IVA or CF
            if (!string.IsNullOrWhiteSpace(request.PartitaIVA))
            {
                var existingByPIva = await context.OrganismiAccreditati
                    .FirstOrDefaultAsync(o => o.PartitaIVA == request.PartitaIVA);

                if (existingByPIva != null)
                {
                    return Results.BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Un organismo con questa Partita IVA esiste già."
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(request.CodiceFiscale))
            {
                var existingByCF = await context.OrganismiAccreditati
                    .FirstOrDefaultAsync(o => o.CodiceFiscale == request.CodiceFiscale);

                if (existingByCF != null)
                {
                    return Results.BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Un organismo con questo Codice Fiscale esiste già."
                    });
                }
            }

            var organismo = new OrganismoAccreditato
            {
                RagioneSociale = request.RagioneSociale,
                PartitaIVA = request.PartitaIVA,
                CodiceFiscale = request.CodiceFiscale,
                TipoOrganismoId = request.TipoOrganismoId,
                EnteAccreditamentoId = request.EnteAccreditamentoId,
                DataCreazione = DateTime.UtcNow,
                DataInizioValidita = DateTime.UtcNow
            };

            context.OrganismiAccreditati.Add(organismo);
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

            return Results.Created($"/api/organismi-accreditati/{organismo.EntitaAziendaleId}", response);
        })
            .WithTags("OrganismiAccreditati")
            .WithName("CreateOrganismoAccreditato")
            .Produces<OrganismoAccreditatoDTO.Response>(StatusCodes.Status201Created)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization();
    }
}
