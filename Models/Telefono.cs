using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneOrganismi.Backend.Models
{
    /// <summary>
    /// Entità Telefono - Gestisce i numeri di telefono associati a EntitàAziendali
    /// Schema: Persone
    /// Tabella: Telefono
    /// </summary>
    [Table("Telefono", Schema = "Persone")]
    public class Telefono
    {
        /// <summary>
        /// Identificativo univoco del telefono
        /// </summary>
        [Key]
        [Column("TelefonoId")]
        public int TelefonoId { get; set; }

        /// <summary>
        /// ID dell'entità aziendale associata
        /// </summary>
        [Required]
        [Column("EntitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        /// <summary>
        /// ID del tipo di telefono (es. Fisso, Cellulare, Fax)
        /// </summary>
        [Required]
        [Column("TipoTelefonoId")]
        public int TipoTelefonoId { get; set; }

        /// <summary>
        /// Prefisso internazionale (es. +39, +1, +44)
        /// </summary>
        [StringLength(20)]
        [Column("PrefissoInternazionale")]
        public string? PrefissoInternazionale { get; set; }

        /// <summary>
        /// Numero di telefono
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("Numero")]
        public string Numero { get; set; } = string.Empty;

        /// <summary>
        /// Estensione telefonica (per numeri interni aziendali)
        /// </summary>
        [StringLength(20)]
        [Column("Estensione")]
        public string? Estensione { get; set; }

        /// <summary>
        /// Identificatore univoco globale (GUID)
        /// </summary>
        [Required]
        [Column("RowGuid")]
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        // ==================== AUDITING ====================
        
        /// <summary>
        /// Data di creazione del record
        /// </summary>
        [Required]
        [Column("DataCreazione")]
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Utente che ha creato il record
        /// </summary>
        [Column("CreatoDa")]
        public int? CreatoDa { get; set; }

        /// <summary>
        /// Data dell'ultima modifica
        /// </summary>
        [Column("DataModifica")]
        public DateTime? DataModifica { get; set; }

        /// <summary>
        /// Utente che ha modificato il record
        /// </summary>
        [Column("ModificatoDa")]
        public int? ModificatoDa { get; set; }

        /// <summary>
        /// Data della cancellazione logica (soft delete)
        /// </summary>
        [Column("DataCancellazione")]
        public DateTime? DataCancellazione { get; set; }

        /// <summary>
        /// Utente che ha eseguito la cancellazione logica
        /// </summary>
        [Column("CancellatoDa")]
        public int? CancellatoDa { get; set; }

        // ==================== TEMPORAL VALIDITY ====================

        /// <summary>
        /// Data di inizio validità temporale del record
        /// </summary>
        [Required]
        [Column("DataInizioValidita")]
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data di fine validità temporale del record
        /// </summary>
        [Required]
        [Column("DataFineValidita")]
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;

        // ==================== COMPUTED PROPERTIES ====================

        /// <summary>
        /// Indica se il record è stato cancellato (soft delete)
        /// </summary>
        [NotMapped]
        public bool IsDeleted => DataCancellazione.HasValue;

        /// <summary>
        /// Indica se il record è attualmente valido (non cancellato e validità attiva)
        /// </summary>
        [NotMapped]
        public bool IsActive => !IsDeleted && 
            DataInizioValidita <= DateTime.UtcNow && 
            DataFineValidita > DateTime.UtcNow;

        /// <summary>
        /// Numero completo formattato (prefisso + numero + estensione)
        /// </summary>
        [NotMapped]
        public string NumeroCompleto
        {
            get
            {
                var numero = !string.IsNullOrWhiteSpace(PrefissoInternazionale)
                    ? $"{PrefissoInternazionale} {Numero}"
                    : Numero;

                return !string.IsNullOrWhiteSpace(Estensione)
                    ? $"{numero} ext. {Estensione}"
                    : numero;
            }
        }

        // ==================== NAVIGATION PROPERTIES ====================

        /// <summary>
        /// Navigazione verso l'Entità Aziendale associata
        /// </summary>
        [ForeignKey(nameof(EntitaAziendaleId))]
        public virtual EntitaAziendale? EntitaAziendale { get; set; }

        /// <summary>
        /// Navigazione verso il Tipo Telefono
        /// </summary>
        [ForeignKey(nameof(TipoTelefonoId))]
        public virtual TipoTelefono? TipoTelefono { get; set; }
    }
}
