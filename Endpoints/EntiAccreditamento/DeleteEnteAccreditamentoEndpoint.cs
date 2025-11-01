using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.EntiAccreditamento;

public class DeleteEnteAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/enti-accreditamento/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var enteAccreditamento = await context.EntiAccreditamento
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enteAccreditamento == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Ente Accreditamento non trovato."
                });
            }

            // Soft Delete
            enteAccreditamento.SoftDelete("Sistema");

            await context.SaveChangesAsync();

            return Results.Ok(new ApiResponse
            {
                Success = true,
                Message = "Ente Accreditamento eliminato con successo."
            });
        }).RequireAuthorization();
    }
}
