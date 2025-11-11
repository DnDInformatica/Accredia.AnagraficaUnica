using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.Shared.Models
{
    /// <summary>
    /// Entità di associazione tra Persona e Indirizzo
    /// Schema: Persone
    /// Tabella: PersonaIndirizzo
    /// </summary>
    [Table("PersonaIndirizzo", Schema = "Persone")]
    public class PersonaIndirizzo
    {
        /// <summary>
        /// Identificativo univoco dell'associazione
        /// </summary>
        [Key]
        [Column("PersonaIndirizzoId")]
        public int PersonaIndirizzoId { get; set; }

        /// <summary>
        /// Identificativo della Persona
        /// </summary>
        [Required]
        [Column("EntitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        /// <summary>
        /// Identificativo dell'Indirizzo
        /// </summary>
        [Required]
        [Column("IndirizzoId")]
        public int IndirizzoId { get; set; }

        /// <summary>
        /// Tipo di indirizzo
        /// </summary>
        [Column("TipoIndirizzoId")]
        public int? TipoIndirizzoId { get; set; }

        /// <summary>
        /// Indica se l'associazione è attiva
        /// </summary>
        [Column("Attivo")]
        public bool Attivo { get; set; } = true;

        /// <summary>
        /// Data di creazione
        /// </summary>
        [Column("DataCreazione")]
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Utente che ha creato il record
        /// </summary>
        [Column("CreatoDa")]
        public int? CreatoDa { get; set; }

        /// <summary>
        /// Data di modifica
        /// </summary>
        [Column("DataModifica")]
        public DateTime? DataModifica { get; set; }

        /// <summary>
        /// Utente che ha modificato il record
        /// </summary>
        [Column("ModificatoDa")]
        public int? ModificatoDa { get; set; }

        /// <summary>
        /// Data di cancellazione
        /// </summary>
        [Column("DataCancellazione")]
        public DateTime? DataCancellazione { get; set; }

        /// <summary>
        /// Utente che ha cancellato il record
        /// </summary>
        [Column("CancellatoDa")]
        public int? CancellatoDa { get; set; }

        /// <summary>
        /// Data inizio validità
        /// </summary>
        [Column("DataInizioValidita")]
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Data fine validità
        /// </summary>
        [Column("DataFineValidita")]
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// GUID univoco per il record
        /// </summary>
        [Column("RowGuid")]
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        // Navigation Properties
        public virtual Persona? Persona { get; set; }
        public virtual Indirizzo? Indirizzo { get; set; }
        public virtual TipoIndirizzo? TipoIndirizzo { get; set; }
    }
}
