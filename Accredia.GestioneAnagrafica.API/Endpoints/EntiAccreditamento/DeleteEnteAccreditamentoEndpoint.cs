using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.EntiAccreditamento;

public class DeleteEnteAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/enti-accreditamento/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var enteAccreditamento = await context.EntiAccreditamento
                .FirstOrDefaultAsync(e => e.EntitaAziendaleId == id);

            if (enteAccreditamento == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Ente Accreditamento non trovato."
                });
            }

            // Soft Delete
            enteAccreditamento.SoftDelete(0);

            await context.SaveChangesAsync();

            return Results.Ok(new ApiResponse
            {
                Success = true,
                Message = "Ente Accreditamento eliminato con successo."
            });
        })
            .WithTags("EntiAccreditamento")
            .WithName("DeleteEnteAccreditamento")
            .Produces<ApiResponse>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
