using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.RisorseUmane;

public class DipartimentiRepartiEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // ==================== DIPARTIMENTI ====================
        var dipartimenti = app.MapGroup("/api/dipartimenti").WithTags("Dipartimenti");

        dipartimenti.MapGet("", async ([FromServices] PersoneDbContext context, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
        {
            var query = context.Set<Dipartimento>().Where(d => d.DataCancellazione == null);
            var totalCount = await query.CountAsync();

            var items = await query.OrderBy(d => d.Nome).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(d => new DipartimentoDTO.List
                {
                    DipartimentoId = d.DipartimentoId,
                    Nome = d.Nome,
                    NumeroReparti = d.Reparti.Count(r => r.DataCancellazione == null)
                })
                .ToListAsync();

            return Results.Ok(new PageResult<DipartimentoDTO.List> { Data = items, TotalRecords = totalCount, PageNumber = page, PageSize = pageSize });
        });

        dipartimenti.MapGet("{id}", async ([FromRoute] int id, [FromServices] PersoneDbContext context) =>
        {
            var dip = await context.Set<Dipartimento>().FirstOrDefaultAsync(d => d.DipartimentoId == id && d.DataCancellazione == null);
            if (dip == null) return Results.NotFound(new ApiResponse { Success = false, Message = "Dipartimento non trovato" });

            var response = new DipartimentoDTO.Response
            {
                DipartimentoId = dip.DipartimentoId,
                Nome = dip.Nome,
                DipartimentoPadreId = dip.DipartimentoPadreId,
                NumeroReparti = await context.Set<Reparto>().CountAsync(r => r.DipartimentoId == id && r.DataCancellazione == null),
                DataCreazione = dip.DataCreazione,
                DataModifica = dip.DataModifica,
                UniqueRowId = dip.UniqueRowId
            };

            return Results.Ok(response);
        });

        dipartimenti.MapPost("", async ([FromBody] DipartimentoDTO.Create request, [FromServices] PersoneDbContext context,
            [FromServices] IValidator<DipartimentoDTO.Create> validator, CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid) return Results.BadRequest(new ApiResponse { Success = false, Message = "Errori validazione", Errors = validation.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList()) });

            var dip = new Dipartimento { Nome = request.Nome.Trim(), DipartimentoPadreId = request.DipartimentoPadreId, DataCreazione = DateTime.UtcNow, DataInizioValidita = DateTime.UtcNow, UniqueRowId = Guid.NewGuid() };
            context.Set<Dipartimento>().Add(dip);
            await context.SaveChangesAsync(ct);

            return Results.Created($"/api/dipartimenti/{dip.DipartimentoId}", new DipartimentoDTO.Response
            {
                DipartimentoId = dip.DipartimentoId,
                Nome = dip.Nome,
                DipartimentoPadreId = dip.DipartimentoPadreId,
                NumeroReparti = 0,
                DataCreazione = dip.DataCreazione,
                UniqueRowId = dip.UniqueRowId
            });
        });

        dipartimenti.MapPut("{id}", async ([FromRoute] int id, [FromBody] DipartimentoDTO.Create request, [FromServices] PersoneDbContext context,
            [FromServices] IValidator<DipartimentoDTO.Create> validator, CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid) return Results.BadRequest(new ApiResponse { Success = false, Message = "Errori validazione" });

            var dip = await context.Set<Dipartimento>().FirstOrDefaultAsync(d => d.DipartimentoId == id && d.DataCancellazione == null, ct);
            if (dip == null) return Results.NotFound(new ApiResponse { Success = false, Message = "Dipartimento non trovato" });

            dip.Nome = request.Nome.Trim();
            dip.DipartimentoPadreId = request.DipartimentoPadreId;
            dip.DataModifica = DateTime.UtcNow;
            await context.SaveChangesAsync(ct);

            return Results.Ok(new ApiResponse { Success = true, Message = "Dipartimento aggiornato" });
        });

        dipartimenti.MapDelete("{id}", async ([FromRoute] int id, [FromServices] PersoneDbContext context, CancellationToken ct) =>
        {
            var dip = await context.Set<Dipartimento>().FirstOrDefaultAsync(d => d.DipartimentoId == id && d.DataCancellazione == null, ct);
            if (dip == null) return Results.NotFound(new ApiResponse { Success = false, Message = "Non trovato" });

            var hasReparti = await context.Set<Reparto>().AnyAsync(r => r.DipartimentoId == id && r.DataCancellazione == null, ct);
            if (hasReparti) return Results.BadRequest(new ApiResponse { Success = false, Message = "Impossibile eliminare: ci sono reparti collegati" });

            dip.DataCancellazione = DateTime.UtcNow;
            dip.DataFineValidita = DateTime.UtcNow;
            await context.SaveChangesAsync(ct);

            return Results.Ok(new ApiResponse { Success = true, Message = "Eliminato" });
        });

        // ==================== REPARTI ====================
        var reparti = app.MapGroup("/api/reparti").WithTags("Reparti");

        reparti.MapGet("", async ([FromServices] PersoneDbContext context, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? dipartimentoId = null) =>
        {
            var query = context.Set<Reparto>().Where(r => r.DataCancellazione == null);
            if (dipartimentoId.HasValue) query = query.Where(r => r.DipartimentoId == dipartimentoId.Value);

            var totalCount = await query.CountAsync();

            var items = await query.OrderBy(r => r.Nome).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(r => new RepartoDTO.List
                {
                    RepartoId = r.RepartoId,
                    Nome = r.Nome,
                    DipartimentoNome = r.Dipartimento != null ? r.Dipartimento.Nome : null,
                    NumeroDipendenti = r.Dipendenti.Count(d => d.DataCancellazione == null)
                })
                .ToListAsync();

            return Results.Ok(new PageResult<RepartoDTO.List> { Data = items, TotalRecords = totalCount, PageNumber = page, PageSize = pageSize });
        });

        reparti.MapGet("{id}", async ([FromRoute] int id, [FromServices] PersoneDbContext context) =>
        {
            var rep = await context.Set<Reparto>().Include(r => r.Dipartimento).Include(r => r.Manager)
                .FirstOrDefaultAsync(r => r.RepartoId == id && r.DataCancellazione == null);
            if (rep == null) return Results.NotFound(new ApiResponse { Success = false, Message = "Reparto non trovato" });

            var response = new RepartoDTO.Response
            {
                RepartoId = rep.RepartoId,
                Nome = rep.Nome,
                DipartimentoId = rep.DipartimentoId,
                DipartimentoNome = rep.Dipartimento?.Nome,
                ManagerDipendenteId = rep.ManagerDipendenteId,
                ManagerMatricola = rep.Manager?.Matricola,
                NumeroDipendenti = await context.Set<Dipendente>().CountAsync(d => d.RepartoId == id && d.DataCancellazione == null),
                DataCreazione = rep.DataCreazione,
                DataModifica = rep.DataModifica,
                UniqueRowId = rep.UniqueRowId
            };

            return Results.Ok(response);
        });

        reparti.MapPost("", async ([FromBody] RepartoDTO.Create request, [FromServices] PersoneDbContext context,
            [FromServices] IValidator<RepartoDTO.Create> validator, CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid) return Results.BadRequest(new ApiResponse { Success = false, Message = "Errori validazione" });

            var rep = new Reparto { Nome = request.Nome.Trim(), DipartimentoId = request.DipartimentoId, ManagerDipendenteId = request.ManagerDipendenteId, DataCreazione = DateTime.UtcNow, DataInizioValidita = DateTime.UtcNow, UniqueRowId = Guid.NewGuid() };
            context.Set<Reparto>().Add(rep);
            await context.SaveChangesAsync(ct);

            return Results.Created($"/api/reparti/{rep.RepartoId}", new RepartoDTO.Response { RepartoId = rep.RepartoId, Nome = rep.Nome, DipartimentoId = rep.DipartimentoId, ManagerDipendenteId = rep.ManagerDipendenteId, NumeroDipendenti = 0, DataCreazione = rep.DataCreazione, UniqueRowId = rep.UniqueRowId });
        });

        reparti.MapPut("{id}", async ([FromRoute] int id, [FromBody] RepartoDTO.Create request, [FromServices] PersoneDbContext context,
            [FromServices] IValidator<RepartoDTO.Create> validator, CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid) return Results.BadRequest(new ApiResponse { Success = false, Message = "Errori validazione" });

            var rep = await context.Set<Reparto>().FirstOrDefaultAsync(r => r.RepartoId == id && r.DataCancellazione == null, ct);
            if (rep == null) return Results.NotFound(new ApiResponse { Success = false, Message = "Reparto non trovato" });

            rep.Nome = request.Nome.Trim();
            rep.DipartimentoId = request.DipartimentoId;
            rep.ManagerDipendenteId = request.ManagerDipendenteId;
            rep.DataModifica = DateTime.UtcNow;
            await context.SaveChangesAsync(ct);

            return Results.Ok(new ApiResponse { Success = true, Message = "Reparto aggiornato" });
        });

        reparti.MapDelete("{id}", async ([FromRoute] int id, [FromServices] PersoneDbContext context, CancellationToken ct) =>
        {
            var rep = await context.Set<Reparto>().FirstOrDefaultAsync(r => r.RepartoId == id && r.DataCancellazione == null, ct);
            if (rep == null) return Results.NotFound(new ApiResponse { Success = false, Message = "Non trovato" });

            var hasDipendenti = await context.Set<Dipendente>().AnyAsync(d => d.RepartoId == id && d.DataCancellazione == null, ct);
            if (hasDipendenti) return Results.BadRequest(new ApiResponse { Success = false, Message = "Impossibile eliminare: ci sono dipendenti collegati" });

            rep.DataCancellazione = DateTime.UtcNow;
            rep.DataFineValidita = DateTime.UtcNow;
            await context.SaveChangesAsync(ct);

            return Results.Ok(new ApiResponse { Success = true, Message = "Eliminato" });
        });
    }
}
