using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.Shared.DTOs
{
    /// <summary>
    /// DTO per la creazione di una nuova Persona
    /// </summary>
    public class CreatePersonaRequest
    {
        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nome non può superare 100 caratteri")]
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il cognome non può superare 100 caratteri")]
        [JsonPropertyName("cognome")]
        public string Cognome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Il codice fiscale è obbligatorio (o N/D, ESTERO per soggetti esteri)")]
        [StringLength(20, ErrorMessage = "Il codice fiscale non può superare 20 caratteri")]
        [JsonPropertyName("codiceFiscale")]
        public string CodiceFiscale { get; set; } = string.Empty;

        [Required(ErrorMessage = "Il genere è obbligatorio")]
        [RegularExpression("^[MFO]$", ErrorMessage = "Genere deve essere M, F o O")]
        [JsonPropertyName("genere")]
        public string Genere { get; set; } = string.Empty; // M = Maschio, F = Femmina, O = Altro

        [Required(ErrorMessage = "L'EntitaAziendaleId è obbligatorio")]
        [JsonPropertyName("entitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        [JsonPropertyName("dataNascita")]
        public DateTime? DataNascita { get; set; }

        [JsonPropertyName("luogoNascita")]
        [StringLength(100)]
        public string? LuogoNascita { get; set; }

        [JsonPropertyName("comuneNascitaId")]
        public int? ComuneNascitaId { get; set; }

        [JsonPropertyName("qualifica")]
        [StringLength(100)]
        public string? Qualifica { get; set; }

        [JsonPropertyName("titolo")]
        [StringLength(50)]
        public string? Titolo { get; set; }

        [JsonPropertyName("titoloOnorificoId")]
        public int? TitoloOnorificoId { get; set; }

        [JsonPropertyName("privacyConsent")]
        public bool PrivacyConsent { get; set; }

        [JsonPropertyName("dataConsensoPrivacy")]
        public DateTime? DataConsensoPrivacy { get; set; }

        [JsonPropertyName("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        /// <summary>
        /// Lista degli indirizzi associati alla persona (in fase di creazione)
        /// </summary>
        [JsonPropertyName("indirizzi")]
        public List<PersonaIndirizzoDTO.Create>? Indirizzi { get; set; }
    }

    /// <summary>
    /// DTO per l'aggiornamento di una Persona
    /// </summary>
    public class UpdatePersonaRequest : CreatePersonaRequest
    {
        [Required(ErrorMessage = "L'ID della persona è obbligatorio")]
        [JsonPropertyName("entitaAziendaleId")]
        public new int EntitaAziendaleId { get; set; }

        /// <summary>
        /// Indirizzi aggiornabili collegati alla persona
        /// </summary>
        [JsonPropertyName("indirizzi")]
        public new List<PersonaIndirizzoDTO.Update>? Indirizzi { get; set; }
    }

    /// <summary>
    /// DTO per la risposta Persona
    /// </summary>
    public class PersonaResponse
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

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataModifica")]
        public DateTime? DataModifica { get; set; }

        [JsonPropertyName("creatoDa")]
        public string? CreatoDa { get; set; }

        [JsonPropertyName("modificatoDa")]
        public string? ModificatoDa { get; set; }

        [JsonPropertyName("rowGuid")]
        public Guid RowGuid { get; set; }

        /// <summary>
        /// Contatto principale (email o telefono)
        /// </summary>
        [JsonPropertyName("contattiPrincipali")]
        public ContattiPrincipaliResponse? ContattiPrincipali { get; set; }

        /// <summary>
        /// Elenco degli indirizzi associati alla persona
        /// </summary>
        [JsonPropertyName("indirizzi")]
        public List<PersonaIndirizzoDTO.Response>? Indirizzi { get; set; }
    }
}
