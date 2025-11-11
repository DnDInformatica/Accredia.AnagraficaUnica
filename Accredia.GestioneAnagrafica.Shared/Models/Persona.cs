using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.Shared.Models
{
    /// <summary>
    /// Entità Persona
    /// Schema: Persone
    /// Tabella: Persona
    /// </summary>
    [Table("Persona", Schema = "Persone")]
    public class Persona
    {
        /// <summary>
        /// Primary Key - corrisponde a EntitaAziendaleId nel database
        /// </summary>
        [Key]
        [Column("EntitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("Cognome")]
        public string Cognome { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column("CodiceFiscale")]
        public string CodiceFiscale { get; set; } = string.Empty;

        [Required]
        [StringLength(1)]
        [Column("Genere")]
        public string Genere { get; set; } = string.Empty; // M, F, O

        [Column("DataNascita")]
        public DateTime? DataNascita { get; set; }

        [StringLength(100)]
        [Column("LuogoNascita")]
        public string? LuogoNascita { get; set; }

        [Column("ComuneNascitaId")]
        public int? ComuneNascitaId { get; set; }

        [StringLength(100)]
        [Column("Qualifica")]
        public string? Qualifica { get; set; }

        [StringLength(50)]
        [Column("Titolo")]
        public string? Titolo { get; set; }

        [Column("TitoloOnorificoId")]
        public int? TitoloOnorificoId { get; set; }

        [Column("PrivacyConsent")]
        public bool PrivacyConsent { get; set; }

        [Column("DataConsensoPrivacy")]
        public DateTime? DataConsensoPrivacy { get; set; }

        [StringLength(500)]
        [Column("Note")]
        public string? Note { get; set; }

        // Audit fields
        [Column("DataCreazione")]
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        [Column("CreatoDa")]
        public int? CreatoDa { get; set; }

        [Column("DataModifica")]
        public DateTime? DataModifica { get; set; }

        [Column("ModificatoDa")]
        public int? ModificatoDa { get; set; }

        [Column("DataCancellazione")]
        public DateTime? DataCancellazione { get; set; }

        [Column("CancellatoDa")]
        public int? CancellatoDa { get; set; }

        [Column("DataInizioValidita")]
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;

        [Column("DataFineValidita")]
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;

        [Column("RowGuid")]
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        // Navigation Properties
        public virtual EntitaAziendale? EntitaAziendale { get; set; }
        public virtual TitoloOnorifico? TitoloOnorifico { get; set; }
    }
}
