using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.Telefono;

public class DeleteTelefonoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/telefono/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var telefono = await context.Telefoni
                .FirstOrDefaultAsync(t => t.TelefonoId == id && t.DataCancellazione == null);

            if (telefono == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "Telefono non trovato."
                });
            }

            // Soft delete
            telefono.DataCancellazione = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return Results.Ok(new ApiResponse
            {
                Message = "Telefono eliminato con successo."
            });
        }).RequireAuthorization()
          .WithTags("Telefono")
          .WithName("DeleteTelefono")
          .Produces<ApiResponse>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
