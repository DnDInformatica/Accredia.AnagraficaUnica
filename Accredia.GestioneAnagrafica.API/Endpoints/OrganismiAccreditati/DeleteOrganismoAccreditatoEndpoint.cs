using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.OrganismiAccreditati;

public class DeleteOrganismoAccreditatoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/organismi-accreditati/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
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

            // Soft Delete
            organismo.SoftDelete(0);

            await context.SaveChangesAsync();

            return Results.Ok(ApiResponse.SuccessResponse("Organismo Accreditato eliminato con successo."));
        })
            .WithTags("OrganismiAccreditati")
            .WithName("DeleteOrganismoAccreditato")
            .Produces<ApiResponse>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
