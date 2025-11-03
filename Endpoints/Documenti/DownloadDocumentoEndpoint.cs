using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.Responses;
using Accredia.GestioneAnagrafica.API.Services;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Documenti;

public class DownloadDocumentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/documenti/{id}/download", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context,
            [FromServices] IDocumentStorageService storageService,
            CancellationToken cancellationToken) =>
        {
            var documento = await context.Documenti.FirstOrDefaultAsync(d => d.DocumentoId == id, cancellationToken);

            if (documento == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Documento non trovato" });

            if (string.IsNullOrWhiteSpace(documento.PercorsoArchivio))
                return Results.BadRequest(new ApiResponse { Success = false, Message = "Percorso non disponibile" });

            var fileExists = await storageService.FileExistsAsync(documento.PercorsoArchivio, cancellationToken);
            if (!fileExists)
                return Results.NotFound(new ApiResponse { Success = false, Message = "File non trovato" });

            try
            {
                var fileStream = await storageService.GetFileStreamAsync(documento.PercorsoArchivio, cancellationToken);
                
                if (fileStream == null)
                    return Results.NotFound(new ApiResponse { Success = false, Message = "Errore apertura file" });

                return Results.File(fileStream, documento.MimeType ?? "application/octet-stream", documento.NomeFile, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return Results.Problem(title: "Download error", detail: ex.Message, statusCode: 500);
            }
        })
          .WithTags("Documenti")
          .WithName("DownloadDocumento")
          .Produces(200);

        app.MapGet("/api/documenti/{id}/preview", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context,
            [FromServices] IDocumentStorageService storageService,
            CancellationToken cancellationToken) =>
        {
            var documento = await context.Documenti.FirstOrDefaultAsync(d => d.DocumentoId == id, cancellationToken);

            if (documento == null || string.IsNullOrWhiteSpace(documento.PercorsoArchivio))
                return Results.NotFound();

            var fileExists = await storageService.FileExistsAsync(documento.PercorsoArchivio, cancellationToken);
            if (!fileExists)
                return Results.NotFound();

            try
            {
                var fileStream = await storageService.GetFileStreamAsync(documento.PercorsoArchivio, cancellationToken);
                
                if (fileStream == null)
                    return Results.NotFound();

                return Results.File(fileStream, documento.MimeType ?? "application/octet-stream", enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return Results.Problem(title: "Preview error", detail: ex.Message, statusCode: 500);
            }
        })
          .WithTags("Documenti")
          .WithName("PreviewDocumento")
          .Produces(200);
    }
}
