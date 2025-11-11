using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.Shared.DTOs
{
    /// <summary>
    /// DTO per la gestione dei Telefoni
    /// </summary>
    public static class TelefonoDTO
    {
        /// <summary>
        /// DTO per la creazione di un nuovo Telefono
        /// </summary>
        public class Create
        {
            [Required(ErrorMessage = "L'EntitaAziendaleId è obbligatorio")]
            [JsonPropertyName("entitaAziendaleId")]
            public int EntitaAziendaleId { get; set; }

            [Required(ErrorMessage = "Il tipo di telefono è obbligatorio")]
            [JsonPropertyName("tipoTelefonoId")]
            public int TipoTelefonoId { get; set; }

            [JsonPropertyName("prefissoInternazionale")]
            [StringLength(20, ErrorMessage = "Il prefisso internazionale non può superare 20 caratteri")]
            public string? PrefissoInternazionale { get; set; }

            [Required(ErrorMessage = "Il numero di telefono è obbligatorio")]
            [StringLength(100, ErrorMessage = "Il numero di telefono non può superare 100 caratteri")]
            [JsonPropertyName("numero")]
            public string Numero { get; set; } = string.Empty;

            [JsonPropertyName("estensione")]
            [StringLength(20, ErrorMessage = "L'estensione non può superare 20 caratteri")]
            public string? Estensione { get; set; }

            [JsonPropertyName("note")]
            [StringLength(500, ErrorMessage = "Le note non possono superare 500 caratteri")]
            public string? Note { get; set; }
        }

        /// <summary>
        /// DTO per l'aggiornamento di un Telefono esistente
        /// </summary>
        public class Update
        {
            [Required(ErrorMessage = "Il tipo di telefono è obbligatorio")]
            [JsonPropertyName("tipoTelefonoId")]
            public int TipoTelefonoId { get; set; }

            [JsonPropertyName("prefissoInternazionale")]
            [StringLength(20, ErrorMessage = "Il prefisso internazionale non può superare 20 caratteri")]
            public string? PrefissoInternazionale { get; set; }

            [Required(ErrorMessage = "Il numero di telefono è obbligatorio")]
            [StringLength(100, ErrorMessage = "Il numero di telefono non può superare 100 caratteri")]
            [JsonPropertyName("numero")]
            public string Numero { get; set; } = string.Empty;

            [JsonPropertyName("estensione")]
            [StringLength(20, ErrorMessage = "L'estensione non può superare 20 caratteri")]
            public string? Estensione { get; set; }

            [JsonPropertyName("note")]
            [StringLength(500, ErrorMessage = "Le note non possono superare 500 caratteri")]
            public string? Note { get; set; }
        }

        /// <summary>
        /// DTO per la risposta completa di un Telefono
        /// </summary>
        public class Response
        {
            [JsonPropertyName("telefonoId")]
            public int TelefonoId { get; set; }

            [JsonPropertyName("entitaAziendaleId")]
            public int EntitaAziendaleId { get; set; }

            [JsonPropertyName("tipoTelefonoId")]
            public int TipoTelefonoId { get; set; }

            [JsonPropertyName("tipoTelefonoDescrizione")]
            public string? TipoTelefonoDescrizione { get; set; }

            [JsonPropertyName("prefissoInternazionale")]
            public string? PrefissoInternazionale { get; set; }

            [JsonPropertyName("numero")]
            public string Numero { get; set; } = string.Empty;

            [JsonPropertyName("estensione")]
            public string? Estensione { get; set; }

            [JsonPropertyName("numeroCompleto")]
            public string NumeroCompleto 
            { 
                get
                {
                    var numero = PrefissoInternazionale != null 
                        ? $"{PrefissoInternazionale} {Numero}" 
                        : Numero;
                    
                    return Estensione != null 
                        ? $"{numero} ext. {Estensione}" 
                        : numero;
                }
            }

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
        /// DTO semplificato per liste di Telefoni
        /// </summary>
        public class ListItem
        {
            [JsonPropertyName("telefonoId")]
            public int TelefonoId { get; set; }

            [JsonPropertyName("tipoTelefonoId")]
            public int TipoTelefonoId { get; set; }

            [JsonPropertyName("tipoTelefonoDescrizione")]
            public string? TipoTelefonoDescrizione { get; set; }

            [JsonPropertyName("numeroCompleto")]
            public string NumeroCompleto { get; set; } = string.Empty;

            [JsonPropertyName("dataCreazione")]
            public DateTime DataCreazione { get; set; }
        }
    }
}
