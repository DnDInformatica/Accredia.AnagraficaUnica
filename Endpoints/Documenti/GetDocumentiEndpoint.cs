using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.API.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Documenti;

public class GetDocumentiEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/documenti", async (
            [FromServices] PersoneDbContext context,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? entitaAziendaleId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? mimeType = null) =>
        {
            var query = context.Documenti.AsQueryable();

            if (entitaAziendaleId.HasValue)
                query = query.Where(d => d.EntitaAziendaleId == entitaAziendaleId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(d => d.NomeFile != null && d.NomeFile.Contains(search) || d.Note != null && d.Note.Contains(search));

            if (!string.IsNullOrWhiteSpace(mimeType))
                query = query.Where(d => d.MimeType == mimeType);

            query = query.OrderByDescending(d => d.DataCaricamento);

            var totalCount = await query.CountAsync();

            var documenti = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DocumentoDTO.List
                {
                    DocumentoId = d.DocumentoId,
                    NomeFile = d.NomeFile,
                    MimeType = d.MimeType,
                    DataCaricamento = d.DataCaricamento,
                    Note = d.Note
                })
                .ToListAsync();

            var pageResult = new PageResult<DocumentoDTO.List>
            {
                Data = documenti,
                TotalRecords = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Results.Ok(pageResult);
        })
          .WithTags("Documenti")
          .WithName("GetDocumenti")
          .Produces<PageResult<DocumentoDTO.List>>(200);

        app.MapGet("/api/documenti/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context) =>
        {
            var documento = await context.Documenti.FirstOrDefaultAsync(d => d.DocumentoId == id);

            if (documento == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Documento non trovato" });

            var response = new DocumentoDTO.Response
            {
                DocumentoId = documento.DocumentoId,
                EntitaAziendaleId = documento.EntitaAziendaleId,
                NomeFile = documento.NomeFile,
                MimeType = documento.MimeType,
                PercorsoArchivio = documento.PercorsoArchivio,
                Note = documento.Note,
                DataCaricamento = documento.DataCaricamento,
                CreatoDa = documento.CreatoDa,
                UniqueRowId = documento.UniqueRowId
            };

            return Results.Ok(response);
        })
          .WithTags("Documenti")
          .WithName("GetDocumentoById")
          .Produces<DocumentoDTO.Response>(200)
          .Produces<ApiResponse>(404);

        app.MapGet("/api/documenti/mime-types", async ([FromServices] PersoneDbContext context) =>
        {
            var mimeTypes = await context.Documenti
                .Where(d => d.MimeType != null)
                .Select(d => d.MimeType!)
                .Distinct()
                .OrderBy(m => m)
                .ToListAsync();

            return Results.Ok(mimeTypes);
        })
          .WithTags("Documenti")
          .WithName("GetMimeTypes")
          .Produces<List<string>>(200);
    }
}
