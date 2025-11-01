using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Responses;

namespace GestioneOrganismi.Backend.Endpoints.Email;

public class GetEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // GET: Lista email per EntitaAziendale con paginazione
        app.MapGet("/api/email/entita/{entitaAziendaleId}", async (
            [FromRoute] int entitaAziendaleId,
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10) =>
        {
            var query = context.Email
                .Where(e => e.EntitaAziendaleId == entitaAziendaleId 
                    && e.DataCancellazione == null)
                .Include(e => e.TipoEmail);

            var totalCount = await query.CountAsync();

            var emails = await query
                .OrderByDescending(e => e.DataCreazione)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmailDTO.ListItem
                {
                    EmailId = e.EmailId,
                    Email = e.EmailAddress,
                    TipoEmailId = e.TipoEmailId,
                    TipoEmailDescrizione = e.TipoEmail != null ? e.TipoEmail.Descrizione : null,
                    DataCreazione = e.DataCreazione
                })
                .ToListAsync();

            var pageResult = new PageResult<EmailDTO.ListItem>
            {
                Data = emails,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        }).RequireAuthorization()
          .WithTags("Email")
          .WithName("GetEmailsByEntita")
          .Produces<PageResult<EmailDTO.ListItem>>(StatusCodes.Status200OK);

        // GET: Dettaglio singola email per ID
        app.MapGet("/api/email/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var email = await context.Email
                .Include(e => e.TipoEmail)
                .FirstOrDefaultAsync(e => e.EmailId == id && e.DataCancellazione == null);

            if (email == null)
            {
                return Results.NotFound(new ApiResponse
                {
                    Message = "Email non trovata."
                });
            }

            var response = new EmailDTO.Response
            {
                EmailId = email.EmailId,
                EntitaAziendaleId = email.EntitaAziendaleId,
                TipoEmailId = email.TipoEmailId,
                TipoEmailDescrizione = email.TipoEmail?.Descrizione,
                Email = email.EmailAddress,
                RowGuid = email.RowGuid,
                DataCreazione = email.DataCreazione,
                DataModifica = email.DataModifica,
                CreatoDa = email.CreatoDa,
                ModificatoDa = email.ModificatoDa,
                DataInizioValidita = email.DataInizioValidita,
                DataFineValidita = email.DataFineValidita
            };

            return Results.Ok(response);
        }).RequireAuthorization()
          .WithTags("Email")
          .WithName("GetEmailById")
          .Produces<EmailDTO.Response>(StatusCodes.Status200OK)
          .Produces<ApiResponse>(StatusCodes.Status404NotFound);
    }
}
