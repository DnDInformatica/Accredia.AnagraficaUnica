using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.Responses;
using Accredia.GestioneAnagrafica.API.Services;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Documenti;

public class DeleteDocumentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/documenti/{id}", async (
            [FromRoute] int id,
            [FromServices] PersoneDbContext context,
            [FromServices] IDocumentStorageService storageService,
            [FromServices] ILogger<DeleteDocumentoEndpoint> logger,
            CancellationToken cancellationToken) =>
        {
            var documento = await context.Documenti.FirstOrDefaultAsync(d => d.DocumentoId == id, cancellationToken);

            if (documento == null)
                return Results.NotFound(new ApiResponse { Success = false, Message = "Documento non trovato" });

            try
            {
                if (!string.IsNullOrWhiteSpace(documento.PercorsoArchivio))
                {
                    await storageService.DeleteFileAsync(documento.PercorsoArchivio, cancellationToken);
                }

                context.Documenti.Remove(documento);
                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse { Success = true, Message = "Documento eliminato" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Errore eliminazione documento {Id}", id);
                return Results.Problem(title: "Delete error", detail: ex.Message, statusCode: 500);
            }
        })
          .WithTags("Documenti")
          .WithName("DeleteDocumento")
          .Produces<ApiResponse>(200)
          .Produces<ApiResponse>(404);

        app.MapDelete("/api/documenti/bulk", async (
            [FromBody] int[] documentIds,
            [FromServices] PersoneDbContext context,
            [FromServices] IDocumentStorageService storageService,
            [FromServices] ILogger<DeleteDocumentoEndpoint> logger,
            CancellationToken cancellationToken) =>
        {
            if (documentIds == null || documentIds.Length == 0)
                return Results.BadRequest(new ApiResponse { Success = false, Message = "No IDs provided" });

            try
            {
                var documenti = await context.Documenti
                    .Where(d => documentIds.Contains(d.DocumentoId))
                    .ToListAsync(cancellationToken);

                if (documenti.Count == 0)
                    return Results.NotFound(new ApiResponse { Success = false, Message = "No documents found" });

                var deletedCount = 0;
                foreach (var documento in documenti)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(documento.PercorsoArchivio))
                        {
                            await storageService.DeleteFileAsync(documento.PercorsoArchivio, cancellationToken);
                        }
                        context.Documenti.Remove(documento);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error deleting doc {Id}", documento.DocumentoId);
                    }
                }

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new ApiResponse
                {
                    Success = deletedCount > 0,
                    Message = $"Deleted {deletedCount}/{documentIds.Length} documents"
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Bulk delete error");
                return Results.Problem(title: "Bulk delete error", detail: ex.Message, statusCode: 500);
            }
        })
          .WithTags("Documenti")
          .WithName("DeleteDocumentiBulk")
          .Produces<ApiResponse>(200);
    }
}
