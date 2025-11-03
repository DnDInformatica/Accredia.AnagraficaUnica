using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Persone;

public class DeletePersonaEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/persone/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context,
            CancellationToken cancellationToken) =>
        {
            var persona = await context.Persone
                .FirstOrDefaultAsync(p => p.PersonaId == id && p.DataCancellazione == null, cancellationToken);

            if (persona == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Persona non trovata"
                });
            }

            try
            {
                // Soft delete
                persona.DataCancellazione = DateTime.UtcNow;
                persona.DataFineValidita = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Persona eliminata con successo (soft delete)"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Errore durante l'eliminazione della persona",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        })
          .WithTags("Persone")
          .WithName("DeletePersona")
          .Produces<ApiResponse>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound)
          .Produces(StatusCodes.Status500InternalServerError);
    }
}
