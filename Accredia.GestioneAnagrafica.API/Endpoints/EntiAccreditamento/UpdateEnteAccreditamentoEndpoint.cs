using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;
using AutoMapper;

namespace Accredia.GestioneAnagrafica.API.Endpoints.EntiAccreditamento;

public class UpdateEnteAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/enti-accreditamento/{id}", async (
            [FromRoute] int id,
            [FromBody] EnteAccreditamentoDTO.Update request,
            [FromServices] PersoneDbContext context,
            [FromServices] IMapper mapper,
            [FromServices] IValidator<EnteAccreditamentoDTO.Update> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

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

            // Update properties
            enteAccreditamento.Denominazione = request.Denominazione;
            enteAccreditamento.Sigla = request.Sigla;
            enteAccreditamento.Note = request.Note;
            enteAccreditamento.DataFondazione = request.DataFondazione;
            enteAccreditamento.DataModifica = DateTime.UtcNow;

            await context.SaveChangesAsync();

            var responseDto = mapper.Map<EnteAccreditamentoDTO.Response>(enteAccreditamento);

            return Results.Ok(responseDto);
        })
            .WithTags("EntiAccreditamento")
            .WithName("UpdateEnteAccreditamento")
            .Produces<EnteAccreditamentoDTO.Response>(StatusCodes.Status200OK)
            .Produces<ApiResponse>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization();
    }
}
