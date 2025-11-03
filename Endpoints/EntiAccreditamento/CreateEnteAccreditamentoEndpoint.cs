using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Models;
using Accredia.GestioneAnagrafica.API.Responses;
using AutoMapper;

namespace Accredia.GestioneAnagrafica.API.Endpoints.EntiAccreditamento;

public class CreateEnteAccreditamentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/enti-accreditamento", async (
            [FromBody] EnteAccreditamentoDTO.Create request,
            [FromServices] PersoneDbContext context,
            [FromServices] IMapper mapper,
            [FromServices] IValidator<EnteAccreditamentoDTO.Create> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            // Check if EntitaAziendale exists
            var entitaExists = await context.EntiAziendali
                .AnyAsync(e => e.EntitaAziendaleId == request.EntitaAziendaleId);

            if (!entitaExists)
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "L'Entità Aziendale specificata non esiste."
                });
            }

            var enteAccreditamento = new EnteAccreditamento
            {
                EntitaAziendaleId = request.EntitaAziendaleId,
                Denominazione = request.Denominazione,
                Sigla = request.Sigla,
                Note = request.Note,
                DataFondazione = request.DataFondazione,
                DataCreazione = DateTime.UtcNow,
                UniqueRowId = Guid.NewGuid(),
                DataInizioValidita = DateTime.UtcNow,
                DataFineValidita = DateTime.MaxValue
            };

            context.EntiAccreditamento.Add(enteAccreditamento);
            await context.SaveChangesAsync();

            var responseDto = mapper.Map<EnteAccreditamentoDTO.Response>(enteAccreditamento);

            return Results.Created($"/api/enti-accreditamento/{enteAccreditamento.EntitaAziendaleId}", responseDto);
        })
            .WithTags("EntiAccreditamento")
            .WithName("CreateEnteAccreditamento")
            .Produces<EnteAccreditamentoDTO.Response>(StatusCodes.Status201Created)
            .Produces<ApiResponse>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization();
    }
}
