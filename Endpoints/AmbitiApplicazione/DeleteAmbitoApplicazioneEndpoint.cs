using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.AmbitiApplicazione;

public class DeleteAmbitoApplicazioneEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/ambiti-applicazione/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            // Recupera entità esistente
            var ambito = await context.AmbitoApplicazione
                .FirstOrDefaultAsync(a => a.AmbitoApplicazioneId == id && a.DataCancellazione == null);

            if (ambito == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Ambito Applicazione non trovato."
                });
            }

            // Verifica se l'ambito è utilizzato in RilasciAccreditamento
            var inUso = await context.RilascioAccreditamento
                .AnyAsync(r => r.AmbitoApplicazioneId == id);

            if (inUso)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Impossibile eliminare l'Ambito Applicazione perché è utilizzato in uno o più Rilasci di Accreditamento."
                });
            }

            // Cancellazione logica (soft delete)
            ambito.DataCancellazione = DateTime.UtcNow;
            ambito.Attivo = false;

            await context.SaveChangesAsync();

            return Results.Ok(new ApiResponse
            {
                Success = true,
                Message = "Ambito Applicazione eliminato con successo."
            });
        })
          .WithTags("AmbitiApplicazione")
          .WithName("DeleteAmbitoApplicazione")
          .Produces<ApiResponse>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
