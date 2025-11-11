using System;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.Shared.DTOs
{
    /// <summary>
    /// DTO per le operazioni sulle Email
    /// </summary>
    public static class EmailDTO
    {
        /// <summary>
        /// DTO per la creazione di una nuova Email
        /// </summary>
        public class Create
        {
            [JsonPropertyName("entitaAziendaleId")]
            public int EntitaAziendaleId { get; set; }

            [JsonPropertyName("tipoEmailId")]
            public int TipoEmailId { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [JsonPropertyName("note")]
            public string? Note { get; set; }
        }

        /// <summary>
        /// DTO per l'aggiornamento di un'Email
        /// </summary>
        public class Update
        {
            [JsonPropertyName("tipoEmailId")]
            public int TipoEmailId { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;

            [JsonPropertyName("note")]
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
        /// DTO per elemento lista Email
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
