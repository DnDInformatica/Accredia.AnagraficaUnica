using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Models;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.Indirizzi;

public class DeleteIndirizzoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // DELETE: Elimina indirizzo (soft delete)
        app.MapDelete("/api/indirizzi/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context,
            CancellationToken cancellationToken) =>
        {
            var indirizzo = await context.Set<Indirizzo>()
                .FirstOrDefaultAsync(i => i.IndirizzoId == id && i.DataCancellazione == null, cancellationToken);

            if (indirizzo == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Indirizzo non trovato"
                });
            }

            try
            {
                // Soft delete
                indirizzo.DataCancellazione = DateTime.UtcNow;
                indirizzo.DataFineValidita = DateTime.UtcNow;

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Indirizzo eliminato con successo (soft delete)"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    title: "Errore durante l'eliminazione dell'indirizzo",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        })
        .WithTags("Indirizzi")
        .WithName("DeleteIndirizzo")
        .Produces<ApiResponse>(StatusCodes.Status200OK)
        .Produces<ApiResponse>(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}
