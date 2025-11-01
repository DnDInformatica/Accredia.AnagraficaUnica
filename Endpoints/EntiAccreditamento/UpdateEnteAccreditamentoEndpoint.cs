using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.Models;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.EntiAccreditamento;

public class UpdateEnteAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/enti-accreditamento/{id}", async (
            [FromRoute] int id,
            [FromBody] EnteAccreditamentoDTO.Update request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<EnteAccreditamentoDTO.Update> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

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

            // Check for duplicate code if changed
            if (enteAccreditamento.CodiceIdentificativo != request.Codice)
            {
                var existingEnte = await context.EntiAccreditamento
                    .FirstOrDefaultAsync(e => e.CodiceIdentificativo == request.Codice && !e.IsDeleted);

                if (existingEnte != null)
                {
                    return Results.BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Un Ente Accreditamento con questo codice esiste già."
                    });
                }
            }

            // Update properties
            enteAccreditamento.Nome = request.Nome;
            enteAccreditamento.CodiceIdentificativo = request.Codice;
            enteAccreditamento.Descrizione = request.Descrizione;
            enteAccreditamento.SettoreMerceologico = request.SettoreMerceologico;
            enteAccreditamento.DataAccreditamento = request.DataAccreditamento;
            enteAccreditamento.Stato = (EnteAccreditamento.StatoAccreditamento)request.Stato;
            enteAccreditamento.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            var response = new EnteAccreditamentoDTO.Response
            {
                Id = enteAccreditamento.Id,
                Nome = enteAccreditamento.Nome,
                Codice = enteAccreditamento.CodiceIdentificativo,
                Descrizione = enteAccreditamento.Descrizione,
                SettoreMerceologico = enteAccreditamento.SettoreMerceologico,
                DataAccreditamento = enteAccreditamento.DataAccreditamento,
                Stato = enteAccreditamento.Stato.ToString(),
                DataCreazione = enteAccreditamento.CreatedAt,
                DataUltimaModifica = enteAccreditamento.UpdatedAt
            };

            return Results.Ok(response);
        }).RequireAuthorization();
    }
}
