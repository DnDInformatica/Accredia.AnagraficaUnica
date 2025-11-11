using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Email;

public class DeleteEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/email/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var email = await context.Email
                .FirstOrDefaultAsync(e => e.EmailId == id && e.DataCancellazione == null);

            if (email == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "Email non trovata."
                });
            }

            // Soft delete
            email.DataCancellazione = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return Results.Ok(new ApiResponse
            {
                Message = "Email eliminata con successo."
            });
        }).RequireAuthorization()
          .WithTags("Email")
          .WithName("DeleteEmail")
          .Produces<ApiResponse>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
