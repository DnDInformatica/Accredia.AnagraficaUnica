using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.OrganismiAccreditati;

public class DeleteOrganismoAccreditatoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/organismi-accreditati/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var organismo = await context.OrganismiAccreditati
                .FirstOrDefaultAsync(o => o.EntitaAziendaleId == id && !o.IsDeleted);

            if (organismo == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Organismo Accreditato non trovato."
                });
            }

            // Soft Delete
            organismo.SoftDelete(0); // TODO: Get current user ID

            await context.SaveChangesAsync();

            return Results.Ok(ApiResponse.SuccessResponse("Organismo Accreditato eliminato con successo."));
        }).RequireAuthorization();
    }
}
