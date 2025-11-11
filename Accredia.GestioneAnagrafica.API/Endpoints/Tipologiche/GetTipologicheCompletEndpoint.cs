using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Tipologiche;

/// <summary>
/// DTO aggregato per tutte le tipologiche
/// Utile per il frontend per caricare tutti i dati di lookup in una singola richiesta
/// </summary>
public class TipologicheCompletDTO
{
    [JsonPropertyName("tipiEmail")]
    public List<TipoEmailDTO> TipiEmail { get; set; } = new();

    [JsonPropertyName("tipiTelefono")]
    public List<TipoTelefonoDTO> TipiTelefono { get; set; } = new();

    [JsonPropertyName("tipiIndirizzo")]
    public List<TipoIndirizzoDTO> TipiIndirizzo { get; set; } = new();

    [JsonPropertyName("tipiEnteAccreditamento")]
    public List<TipoEnteAccreditamentoDTO> TipiEnteAccreditamento { get; set; } = new();

    [JsonPropertyName("titoliOnorifici")]
    public List<TitoloOnorificoDTO> TitoliOnorifici { get; set; } = new();
}

/// <summary>
/// Endpoint aggregato per recuperare tutte le tipologiche in una singola richiesta
/// </summary>
public class GetTipologicheCompletEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tipologiche", async (
            [FromServices] PersoneDbContext context) =>
        {
            var response = new TipologicheCompletDTO();

            // Carica tutti i TipiEmail
            response.TipiEmail = await context.TipiEmail
                .OrderBy(t => t.Codice)
                .Select(t => new TipoEmailDTO
                {
                    TipoEmailId = t.TipoEmailId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            // Carica tutti i TipiTelefono
            response.TipiTelefono = await context.TipiTelefono
                .OrderBy(t => t.Codice)
                .Select(t => new TipoTelefonoDTO
                {
                    TipoTelefonoId = t.TipoTelefonoId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            // Carica tutti i TipiIndirizzo
            response.TipiIndirizzo = await context.TipiIndirizzo
                .OrderBy(t => t.Codice)
                .Select(t => new TipoIndirizzoDTO
                {
                    TipoIndirizzoId = t.TipoIndirizzoId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            // Carica tutti i TipiEnteAccreditamento
            response.TipiEnteAccreditamento = await context.TipiEnteAccreditamento
                .OrderBy(t => t.Codice)
                .Select(t => new TipoEnteAccreditamentoDTO
                {
                    TipoEnteAccreditamentoId = t.TipoEnteAccreditamentoId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    UniqueRowId = t.UniqueRowId,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            // Carica tutti i TitoliOnorifici
            response.TitoliOnorifici = await context.TitoliOnorifici
                .OrderBy(t => t.Codice)
                .Select(t => new TitoloOnorificoDTO
                {
                    TitoloOnorificoId = t.TitoloOnorificoId,
                    Codice = t.Codice,
                    Descrizione = t.Descrizione,
                    TitoloMaschile = t.TitoloMaschile,
                    TitoloFemminile = t.TitoloFemminile,
                    DataCreazione = t.DataCreazione,
                    DataInizioValidita = t.DataInizioValidita,
                    DataFineValidita = t.DataFineValidita
                })
                .ToListAsync();

            return Results.Ok(response);
        })
          .WithTags("Tipologiche")
          .WithName("GetTuTipologiche")
          .Produces<TipologicheCompletDTO>(200)
          .WithDescription("Recupera tutte le tipologiche (lookup tables) in una singola richiesta. Utile per il frontend.");
    }
}
