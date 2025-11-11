using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.API.DTOs
{
    /// <summary>
    /// DTOs per le Entità Tipologiche (lookup tables READ-ONLY)
    /// </summary>

    /// <summary>
    /// DTO per TipoEmail
    /// </summary>
    public class TipoEmailDTO
    {
        [JsonPropertyName("tipoEmailId")]
        public int TipoEmailId { get; set; }

        [JsonPropertyName("codice")]
        public string Codice { get; set; } = string.Empty;

        [JsonPropertyName("descrizione")]
        public string Descrizione { get; set; } = string.Empty;

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataInizioValidita")]
        public DateTime DataInizioValidita { get; set; }

        [JsonPropertyName("dataFineValidita")]
        public DateTime DataFineValidita { get; set; }
    }

    /// <summary>
    /// DTO per TipoTelefono
    /// </summary>
    public class TipoTelefonoDTO
    {
        [JsonPropertyName("tipoTelefonoId")]
        public int TipoTelefonoId { get; set; }

        [JsonPropertyName("codice")]
        public string Codice { get; set; } = string.Empty;

        [JsonPropertyName("descrizione")]
        public string Descrizione { get; set; } = string.Empty;

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataInizioValidita")]
        public DateTime DataInizioValidita { get; set; }

        [JsonPropertyName("dataFineValidita")]
        public DateTime DataFineValidita { get; set; }
    }

    /// <summary>
    /// DTO per TipoIndirizzo
    /// </summary>
    public class TipoIndirizzoDTO
    {
        [JsonPropertyName("tipoIndirizzoId")]
        public int TipoIndirizzoId { get; set; }

        [JsonPropertyName("codice")]
        public string Codice { get; set; } = string.Empty;

        [JsonPropertyName("descrizione")]
        public string Descrizione { get; set; } = string.Empty;

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataInizioValidita")]
        public DateTime DataInizioValidita { get; set; }

        [JsonPropertyName("dataFineValidita")]
        public DateTime DataFineValidita { get; set; }
    }

    /// <summary>
    /// DTO per TipoEnteAccreditamento
    /// </summary>
    public class TipoEnteAccreditamentoDTO
    {
        [JsonPropertyName("tipoEnteAccreditamentoId")]
        public int TipoEnteAccreditamentoId { get; set; }

        [JsonPropertyName("codice")]
        public string Codice { get; set; } = string.Empty;

        [JsonPropertyName("descrizione")]
        public string Descrizione { get; set; } = string.Empty;

        [JsonPropertyName("uniqueRowId")]
        public Guid? UniqueRowId { get; set; }

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataInizioValidita")]
        public DateTime DataInizioValidita { get; set; }

        [JsonPropertyName("dataFineValidita")]
        public DateTime DataFineValidita { get; set; }
    }

    /// <summary>
    /// DTO per TitoloOnorifico
    /// </summary>
    public class TitoloOnorificoDTO
    {
        [JsonPropertyName("titoloOnorificoId")]
        public int TitoloOnorificoId { get; set; }

        [JsonPropertyName("codice")]
        public string Codice { get; set; } = string.Empty;

        [JsonPropertyName("descrizione")]
        public string Descrizione { get; set; } = string.Empty;

        [JsonPropertyName("titoloMaschile")]
        public string? TitoloMaschile { get; set; }

        [JsonPropertyName("titoloFemminile")]
        public string? TitoloFemminile { get; set; }

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataInizioValidita")]
        public DateTime DataInizioValidita { get; set; }

        [JsonPropertyName("dataFineValidita")]
        public DateTime DataFineValidita { get; set; }
    }
}
