using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneOrganismi.Backend.Models
{
    /// <summary>
    /// Entità Persona con supporto Soft Delete e Auditing
    /// </summary>
    [Table("Persona", Schema = "Persone")]
    public class Persona
    {
        [Key]
        [Column("PersonaId")]
        public int PersonaId { get; set; }

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
        public string Genere { get; set; } = "O"; // M, F, O (Altro)

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

        [Required]
        [Column("EntitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        [Column("PrivacyConsent")]
        public bool PrivacyConsent { get; set; }

        [Column("DataConsensoPrivacy")]
        public DateTime? DataConsensoPrivacy { get; set; }

        [StringLength(500)]
        [Column("Note")]
        public string? Note { get; set; }

        // Auditing
        [Column("DataCreazione")]
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        [Column("CreatoDa")]
        [StringLength(256)]
        public string? CreatoDa { get; set; }

        [Column("DataModifica")]
        public DateTime? DataModifica { get; set; }

        [Column("ModificatoDa")]
        [StringLength(256)]
        public string? ModificatoDa { get; set; }

        // Soft Delete
        [Column("DataCancellazione")]
        public DateTime? DataCancellazione { get; set; }

        [Column("CancellatoDa")]
        [StringLength(256)]
        public string? CancellatoDa { get; set; }

        // Temporal Validity
        [Column("DataInizioValidita")]
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;

        [Column("DataFineValidita")]
        public DateTime? DataFineValidita { get; set; }

        // GUID
        [Column("rowguid")]
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        // Computed Property
        [NotMapped]
        public bool IsDeleted => DataCancellazione.HasValue;

        [NotMapped]
        public bool IsActive => !IsDeleted && (DataFineValidita == null || DataFineValidita > DateTime.UtcNow);

        // Navigation Properties
        [ForeignKey(nameof(EntitaAziendaleId))]
        public virtual EntitaAziendale? EntitaAziendale { get; set; }
    }

    /// <summary>
    /// Entità EntitaAziendale
    /// </summary>
    [Table("EntitaAziendale", Schema = "Persone")]
    public class EntitaAziendale
    {
        [Key]
        [Column("EntitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        [StringLength(500)]
        [Column("Denominazione")]
        public string? Denominazione { get; set; }

        [StringLength(20)]
        [Column("CodiceFiscale")]
        public string? CodiceFiscale { get; set; }

        [StringLength(20)]
        [Column("PartitaIVA")]
        public string? PartitaIVA { get; set; }

        // Auditing
        [Column("DataCreazione")]
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        [Column("CreatoDa")]
        [StringLength(256)]
        public string? CreatoDa { get; set; }

        [Column("DataModifica")]
        public DateTime? DataModifica { get; set; }

        [Column("ModificatoDa")]
        [StringLength(256)]
        public string? ModificatoDa { get; set; }

        // Soft Delete
        [Column("DataCancellazione")]
        public DateTime? DataCancellazione { get; set; }

        [Column("CancellatoDa")]
        [StringLength(256)]
        public string? CancellatoDa { get; set; }

        // Temporal Validity
        [Column("DataInizioValidita")]
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;

        [Column("DataFineValidita")]
        public DateTime? DataFineValidita { get; set; }

        [Column("rowguid")]
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        [NotMapped]
        public bool IsDeleted => DataCancellazione.HasValue;

        [NotMapped]
        public bool IsActive => !IsDeleted && (DataFineValidita == null || DataFineValidita > DateTime.UtcNow);

        // Navigation Properties
        public virtual ICollection<Persona> Persone { get; set; } = new List<Persona>();
    }

    /// <summary>
    /// Entità Indirizzo per Persona
    /// </summary>
    [Table("PersonaIndirizzo", Schema = "Persone")]
    public class PersonaIndirizzo
    {
        [Key]
        [Column("PersonaIndirizzoId")]
        public int PersonaIndirizzoId { get; set; }

        [Column("PersonaId")]
        public int? PersonaId { get; set; }

        [Column("IndirizzoId")]
        public int? IndirizzoId { get; set; }

        [Column("TipoIndirizzoId")]
        public int? TipoIndirizzoId { get; set; }

        [Column("EntitaAziendaleId")]
        public int? EntitaAziendaleId { get; set; }

        [Column("Attivo")]
        public bool Attivo { get; set; } = true;

        // Auditing
        [Column("DataCreazione")]
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        [Column("CreatoDa")]
        [StringLength(256)]
        public string? CreatoDa { get; set; }

        [Column("DataModifica")]
        public DateTime? DataModifica { get; set; }

        [Column("ModificatoDa")]
        [StringLength(256)]
        public string? ModificatoDa { get; set; }

        [Column("DataCancellazione")]
        public DateTime? DataCancellazione { get; set; }

        [Column("CancellatoDa")]
        [StringLength(256)]
        public string? CancellatoDa { get; set; }

        [Column("DataInizioValidita")]
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;

        [Column("DataFineValidita")]
        public DateTime? DataFineValidita { get; set; }

        [NotMapped]
        public bool IsDeleted => DataCancellazione.HasValue;
    }
}
