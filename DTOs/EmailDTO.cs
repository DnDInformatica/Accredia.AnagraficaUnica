using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.API.DTOs
{
    /// <summary>
    /// DTO per la gestione delle Email
    /// </summary>
    public static class EmailDTO
    {
        /// <summary>
        /// DTO per la creazione di una nuova Email
        /// </summary>
        public class Create
        {
            [Required(ErrorMessage = "L'EntitaAziendaleId è obbligatorio")]
            [JsonPropertyName("entitaAziendaleId")]
            public int EntitaAziendaleId { get; set; }

            [Required(ErrorMessage = "Il tipo di email è obbligatorio")]
            [JsonPropertyName("tipoEmailId")]
            public int TipoEmailId { get; set; }

            [Required(ErrorMessage = "L'indirizzo email è obbligatorio")]
            [EmailAddress(ErrorMessage = "L'indirizzo email non è valido")]
            [StringLength(510, ErrorMessage = "L'indirizzo email non può superare 510 caratteri")]
            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [JsonPropertyName("note")]
            [StringLength(500, ErrorMessage = "Le note non possono superare 500 caratteri")]
            public string? Note { get; set; }
        }

        /// <summary>
        /// DTO per l'aggiornamento di un'Email esistente
        /// </summary>
        public class Update
        {
            [Required(ErrorMessage = "Il tipo di email è obbligatorio")]
            [JsonPropertyName("tipoEmailId")]
            public int TipoEmailId { get; set; }

            [Required(ErrorMessage = "L'indirizzo email è obbligatorio")]
            [EmailAddress(ErrorMessage = "L'indirizzo email non è valido")]
            [StringLength(510, ErrorMessage = "L'indirizzo email non può superare 510 caratteri")]
            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [JsonPropertyName("note")]
            [StringLength(500, ErrorMessage = "Le note non possono superare 500 caratteri")]
            public string? Note { get; set; }
        }

        /// <summary>
        /// DTO per la risposta completa di un'Email
        /// </summary>
        public class Response
        {
            [JsonPropertyName("emailId")]
            public int EmailId { get; set; }

            [JsonPropertyName("entitaAziendaleId")]
            public int EntitaAziendaleId { get; set; }

            [JsonPropertyName("tipoEmailId")]
            public int TipoEmailId { get; set; }

            [JsonPropertyName("tipoEmailDescrizione")]
            public string? TipoEmailDescrizione { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [JsonPropertyName("note")]
            public string? Note { get; set; }

            [JsonPropertyName("rowGuid")]
            public Guid RowGuid { get; set; }

            [JsonPropertyName("dataCreazione")]
            public DateTime DataCreazione { get; set; }

            [JsonPropertyName("dataModifica")]
            public DateTime? DataModifica { get; set; }

            [JsonPropertyName("creatoDa")]
            public int? CreatoDa { get; set; }

            [JsonPropertyName("modificatoDa")]
            public int? ModificatoDa { get; set; }

            [JsonPropertyName("dataInizioValidita")]
            public DateTime DataInizioValidita { get; set; }

            [JsonPropertyName("dataFineValidita")]
            public DateTime DataFineValidita { get; set; }
        }

        /// <summary>
        /// DTO semplificato per liste di Email
        /// </summary>
        public class ListItem
        {
            [JsonPropertyName("emailId")]
            public int EmailId { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [JsonPropertyName("tipoEmailId")]
            public int TipoEmailId { get; set; }

            [JsonPropertyName("tipoEmailDescrizione")]
            public string? TipoEmailDescrizione { get; set; }

            [JsonPropertyName("dataCreazione")]
            public DateTime DataCreazione { get; set; }
        }
    }
}
