using System.Text.Json.Serialization;

namespace GestioneOrganismi.Backend.DTOs;

// ==================== INDIRIZZO ====================
public class IndirizzoDTO
{
    public class Create
    {
        [JsonPropertyName("via")]
        public string Via { get; set; } = string.Empty;

        [JsonPropertyName("numeroCivico")]
        public string? NumeroCivico { get; set; }

        [JsonPropertyName("cap")]
        public string CAP { get; set; } = string.Empty;

        [JsonPropertyName("citta")]
        public string Citta { get; set; } = string.Empty;

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; } = string.Empty;

        [JsonPropertyName("regione")]
        public string? Regione { get; set; }

        [JsonPropertyName("stato")]
        public string Stato { get; set; } = "Italia";

        [JsonPropertyName("comuneId")]
        public int? ComuneId { get; set; }

        [JsonPropertyName("latitudine")]
        public decimal? Latitudine { get; set; }

        [JsonPropertyName("longitudine")]
        public decimal? Longitudine { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }
    }

    public class Update : Create
    {
        [JsonPropertyName("indirizzoId")]
        public int IndirizzoId { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("indirizzoId")]
        public int IndirizzoId { get; set; }

        [JsonPropertyName("via")]
        public string Via { get; set; } = string.Empty;

        [JsonPropertyName("numeroCivico")]
        public string? NumeroCivico { get; set; }

        [JsonPropertyName("cap")]
        public string CAP { get; set; } = string.Empty;

        [JsonPropertyName("citta")]
        public string Citta { get; set; } = string.Empty;

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; } = string.Empty;

        [JsonPropertyName("regione")]
        public string? Regione { get; set; }

        [JsonPropertyName("stato")]
        public string Stato { get; set; } = "Italia";

        [JsonPropertyName("comuneId")]
        public int? ComuneId { get; set; }

        [JsonPropertyName("latitudine")]
        public decimal? Latitudine { get; set; }

        [JsonPropertyName("longitudine")]
        public decimal? Longitudine { get; set; }

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("indirizzoCompleto")]
        public string IndirizzoCompleto { get; set; } = string.Empty;

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataModifica")]
        public DateTime? DataModifica { get; set; }

        [JsonPropertyName("rowGuid")]
        public Guid RowGuid { get; set; }
    }

    public class List
    {
        [JsonPropertyName("indirizzoId")]
        public int IndirizzoId { get; set; }

        [JsonPropertyName("indirizzoCompleto")]
        public string IndirizzoCompleto { get; set; } = string.Empty;

        [JsonPropertyName("citta")]
        public string Citta { get; set; } = string.Empty;

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; } = string.Empty;

        [JsonPropertyName("cap")]
        public string CAP { get; set; } = string.Empty;
    }
}

// ==================== PERSONA INDIRIZZO ====================
public class PersonaIndirizzoDTO
{
    public class Create
    {
        [JsonPropertyName("personaId")]
        public int PersonaId { get; set; }

        [JsonPropertyName("indirizzoId")]
        public int IndirizzoId { get; set; }

        [JsonPropertyName("tipoIndirizzoId")]
        public int TipoIndirizzoId { get; set; }

        [JsonPropertyName("attivo")]
        public bool Attivo { get; set; } = true;
    }

    public class Update
    {
        [JsonPropertyName("tipoIndirizzoId")]
        public int TipoIndirizzoId { get; set; }

        [JsonPropertyName("attivo")]
        public bool Attivo { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("personaIndirizzoId")]
        public int PersonaIndirizzoId { get; set; }

        [JsonPropertyName("personaId")]
        public int PersonaId { get; set; }

        [JsonPropertyName("indirizzoId")]
        public int IndirizzoId { get; set; }

        [JsonPropertyName("tipoIndirizzoId")]
        public int TipoIndirizzoId { get; set; }

        [JsonPropertyName("attivo")]
        public bool Attivo { get; set; }

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("indirizzo")]
        public IndirizzoDTO.Response? Indirizzo { get; set; }
    }
}
