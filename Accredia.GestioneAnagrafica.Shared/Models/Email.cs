using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.Shared.Models
{
    /// <summary>
    /// Entità Email - Gestisce gli indirizzi email associati a EntitàAziendali
    /// Schema: Persone
    /// Tabella: Email
    /// </summary>
    [Table("Email", Schema = "Persone")]
    public class Email
    {
        /// <summary>
        /// Identificativo univoco dell'email
        /// </summary>
        [Key]
        [Column("EmailId")]
        public int EmailId { get; set; }

        /// <summary>
        /// ID dell'entità aziendale associata
        /// </summary>
        [Required]
        [Column("EntitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        /// <summary>
        /// ID del tipo di email (es. Personale, Aziendale, PEC)
        /// </summary>
        [Required]
        [Column("TipoEmailId")]
        public int TipoEmailId { get; set; }

        /// <summary>
        /// Indirizzo email
        /// </summary>
        [Required]
        [StringLength(510)]
        [Column("Email")]
        public string EmailAddress { get; set; } = string.Empty;

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

        // ==================== NAVIGATION PROPERTIES ====================

        /// <summary>
        /// Navigazione verso l'Entità Aziendale associata
        /// </summary>
        [ForeignKey(nameof(EntitaAziendaleId))]
        public virtual EntitaAziendale? EntitaAziendale { get; set; }

        /// <summary>
        /// Navigazione verso il Tipo Email
        /// </summary>
        [ForeignKey(nameof(TipoEmailId))]
        public virtual TipoEmail? TipoEmail { get; set; }
    }
}
