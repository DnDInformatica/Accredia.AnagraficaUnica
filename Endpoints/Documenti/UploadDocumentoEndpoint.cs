using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.DTOs;
using GestioneOrganismi.Backend.Models;
using GestioneOrganismi.Backend.Responses;
using GestioneOrganismi.Backend.Services;

namespace GestioneOrganismi.Backend.Endpoints.Documenti;

public class UploadDocumentoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/documenti/upload", async (
            [FromBody] DocumentoDTO.Upload request,
            [FromServices] PersoneDbContext context,
            [FromServices] IValidator<DocumentoDTO.Upload> validator,
            [FromServices] IDocumentStorageService storageService,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());

                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Errori di validazione",
                    Errors = errors
                });
            }

            if (!storageService.IsFileAllowed(request.NomeFile, request.MimeType))
            {
                return Results.BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Tipo di file non permesso"
                });
            }

            try
            {
                byte[] fileContent = Convert.FromBase64String(request.ContenutoBase64!);
                var percorsoArchivio = await storageService.SaveFileAsync(request.NomeFile, fileContent, cancellationToken);

                var documento = new Documento
                {
                    EntitaAziendaleId = request.EntitaAziendaleId,
                    NomeFile = request.NomeFile,
                    MimeType = request.MimeType,
                    PercorsoArchivio = percorsoArchivio,
                    Note = request.Note,
                    DataCaricamento = DateTime.UtcNow,
                    UniqueRowId = Guid.NewGuid()
                };

                context.Documenti.Add(documento);
                await context.SaveChangesAsync(cancellationToken);

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

                return Results.Created($"/api/documenti/{documento.DocumentoId}", response);
            }
            catch (Exception ex)
            {
                return Results.Problem(title: "Errore upload", detail: ex.Message, statusCode: 500);
            }
        })
          .WithTags("Documenti")
          .WithName("UploadDocumento")
          .Produces<DocumentoDTO.Response>(201)
          .Produces<ApiResponse>(400)
          .DisableAntiforgery();

        app.MapPost("/api/documenti/upload-multipart", async (
            HttpContext httpContext,
            [FromServices] PersoneDbContext context,
            [FromServices] IDocumentStorageService storageService,
            CancellationToken cancellationToken) =>
        {
            if (!httpContext.Request.HasFormContentType)
                return Results.BadRequest(new ApiResponse { Success = false, Message = "Invalid content type" });

            var form = await httpContext.Request.ReadFormAsync(cancellationToken);
            var file = form.Files.GetFile("file");

            if (file == null || file.Length == 0)
                return Results.BadRequest(new ApiResponse { Success = false, Message = "No file" });

            var entitaAziendaleId = form.TryGetValue("entitaAziendaleId", out var entitaId) ? int.Parse(entitaId!) : (int?)null;
            var note = form.TryGetValue("note", out var noteValue) ? noteValue.ToString() : null;

            if (!storageService.IsFileAllowed(file.FileName, file.ContentType))
                return Results.BadRequest(new ApiResponse { Success = false, Message = "File type not allowed" });

            try
            {
                string percorsoArchivio;
                using (var stream = file.OpenReadStream())
                {
                    percorsoArchivio = await storageService.SaveFileAsync(file.FileName, stream, cancellationToken);
                }

                var documento = new Documento
                {
                    EntitaAziendaleId = entitaAziendaleId,
                    NomeFile = file.FileName,
                    MimeType = file.ContentType,
                    PercorsoArchivio = percorsoArchivio,
                    Note = note,
                    DataCaricamento = DateTime.UtcNow,
                    UniqueRowId = Guid.NewGuid()
                };

                context.Documenti.Add(documento);
                await context.SaveChangesAsync(cancellationToken);

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

                return Results.Created($"/api/documenti/{documento.DocumentoId}", response);
            }
            catch (Exception ex)
            {
                return Results.Problem(title: "Error", detail: ex.Message, statusCode: 500);
            }
        })
          .WithTags("Documenti")
          .WithName("UploadDocumentoMultipart")
          .Produces<DocumentoDTO.Response>(201)
          .DisableAntiforgery();
    }
}
