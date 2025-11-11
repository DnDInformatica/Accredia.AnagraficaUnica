using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Telefono;

public class GetTelefonoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // GET: Lista telefoni per EntitaAziendale con paginazione
        app.MapGet("/api/telefono/entita/{entitaAziendaleId}", async (
            [FromRoute] int entitaAziendaleId,
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10) =>
        {
            var query = context.Telefoni
                .Where(t => t.EntitaAziendaleId == entitaAziendaleId 
                    && t.DataCancellazione == null)
                .Include(t => t.TipoTelefono);

            var totalCount = await query.CountAsync();

            var telefoni = await query
                .OrderByDescending(t => t.DataCreazione)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TelefonoDTO.ListItem
                {
                    TelefonoId = t.TelefonoId,
                    TipoTelefonoId = t.TipoTelefonoId,
                    TipoTelefonoDescrizione = t.TipoTelefono != null ? t.TipoTelefono.Descrizione : null,
                    NumeroCompleto = (t.PrefissoInternazionale != null ? t.PrefissoInternazionale + " " : "") +
                                     t.Numero +
                                     (t.Estensione != null ? " ext. " + t.Estensione : ""),
                    DataCreazione = t.DataCreazione
                })
                .ToListAsync();

            var pageResult = new PageResult<TelefonoDTO.ListItem>
            {
                Data = telefoni,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        }).RequireAuthorization()
          .WithTags("Telefono")
          .WithName("GetTelefoniByEntita")
          .Produces<PageResult<TelefonoDTO.ListItem>>(StatusCodes.Status200OK);

        // GET: Dettaglio singolo telefono per ID
        app.MapGet("/api/telefono/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var telefono = await context.Telefoni
                .Include(t => t.TipoTelefono)
                .FirstOrDefaultAsync(t => t.TelefonoId == id && t.DataCancellazione == null);

            if (telefono == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "Telefono non trovato."
                });
            }

            var response = new TelefonoDTO.Response
            {
                TelefonoId = telefono.TelefonoId,
                EntitaAziendaleId = telefono.EntitaAziendaleId,
                TipoTelefonoId = telefono.TipoTelefonoId,
                TipoTelefonoDescrizione = telefono.TipoTelefono?.Descrizione,
                PrefissoInternazionale = telefono.PrefissoInternazionale,
                Numero = telefono.Numero,
                Estensione = telefono.Estensione,
                RowGuid = telefono.RowGuid,
                DataCreazione = telefono.DataCreazione,
                DataModifica = telefono.DataModifica,
                CreatoDa = telefono.CreatoDa,
                ModificatoDa = telefono.ModificatoDa,
                DataInizioValidita = telefono.DataInizioValidita,
                DataFineValidita = telefono.DataFineValidita
            };

            return Results.Ok(response);
        }).RequireAuthorization()
          .WithTags("Telefono")
          .WithName("GetTelefonoById")
          .Produces<TelefonoDTO.Response>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
