using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.Shared.DTOs;

public class PersonaListItemResponse
{
    [JsonPropertyName("entitaAziendaleId")]
    public int EntitaAziendaleId { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("cognome")]
    public string Cognome { get; set; } = string.Empty;

    [JsonPropertyName("nomeCompleto")]
    public string NomeCompleto { get; set; } = string.Empty;

    [JsonPropertyName("codiceFiscale")]
    public string CodiceFiscale { get; set; } = string.Empty;

    [JsonPropertyName("qualifica")]
    public string? Qualifica { get; set; }

    [JsonPropertyName("entitaAziendaleName")]
    public string? EntitaAziendaleName { get; set; }

    [JsonPropertyName("emailPrincipale")]
    public string? EmailPrincipale { get; set; }

    [JsonPropertyName("telefonoPrincipale")]
    public string? TelefonoPrincipale { get; set; }

    [JsonPropertyName("dataCreazione")]
    public DateTime DataCreazione { get; set; }
}

/// <summary>
/// DTO per MudDataGrid - include tutti i campi necessari per visualizzazione e filtri
/// </summary>
public class PersonaGridItemResponse
{
    [JsonPropertyName("entitaAziendaleId")]
    public int EntitaAziendaleId { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("cognome")]
    public string Cognome { get; set; } = string.Empty;

    [JsonPropertyName("nomeCompleto")]
    public string NomeCompleto => $"{Nome} {Cognome}";

    [JsonPropertyName("codiceFiscale")]
    public string CodiceFiscale { get; set; } = string.Empty;

    [JsonPropertyName("genere")]
    public string Genere { get; set; } = string.Empty;

    [JsonPropertyName("dataNascita")]
    public DateTime? DataNascita { get; set; }

    [JsonPropertyName("luogoNascita")]
    public string? LuogoNascita { get; set; }

    [JsonPropertyName("qualifica")]
    public string? Qualifica { get; set; }

    [JsonPropertyName("titolo")]
    public string? Titolo { get; set; }

    [JsonPropertyName("privacyConsent")]
    public bool PrivacyConsent { get; set; }

    [JsonPropertyName("dataConsensoPrivacy")]
    public DateTime? DataConsensoPrivacy { get; set; }

    [JsonPropertyName("dataCreazione")]
    public DateTime DataCreazione { get; set; }

    [JsonPropertyName("dataCancellazione")]
    public DateTime? DataCancellazione { get; set; }

    /// <summary>
    /// Stato calcolato in base a DataCancellazione
    /// </summary>
    [JsonPropertyName("stato")]
    public string Stato => DataCancellazione.HasValue ? "Eliminato" : "Attivo";
}

public class ContattiPrincipaliResponse
{
    public string? EmailPrincipale { get; set; }
    public string? TelefonoPrincipale { get; set; }
    public string? IndirizzoPrincipale { get; set; }
}
