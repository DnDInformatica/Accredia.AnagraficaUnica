using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.Shared.Models
{
    /// <summary>
    /// Ambito Applicazione - schema Accreditamento
    /// </summary>
    [Table("AmbitoApplicazione", Schema = "Accreditamento")]
    public class AmbitoApplicazione
    {
        [Key]
        [Column("AmbitoApplicazioneId")]
        public int AmbitoApplicazioneId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Codice")]
        public required string Codice { get; set; }

        [Required]
        [StringLength(200)]
        [Column("Denominazione")]
        public required string Denominazione { get; set; }

        [StringLength(1000)]
        [Column("Descrizione")]
        public string? Descrizione { get; set; }

        [Required]
        [Column("Ordine")]
        public int Ordine { get; set; }

        [Column("Attivo")]
        public bool Attivo { get; set; } = true;

        [Column("rowguid")]
        public Guid RowGuid { get; set; } = Guid.NewGuid();

        // Auditing
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

        [NotMapped]
        public bool IsDeleted => DataCancellazione.HasValue;
    }

    /// <summary>
    /// Rilascio Accreditamento - schema Accreditamento
    /// </summary>
    [Table("RilascioAccreditamento", Schema = "Accreditamento")]
    public class RilascioAccreditamento
    {
        [Key]
        [Column("RilascioId")]
        public int RilascioId { get; set; }

        [Required]
        [Column("EnteAccreditamentoId")]
        public int EnteAccreditamentoId { get; set; }

        [Required]
        [Column("EnteAccreditatoId")]
        public int EnteAccreditatoId { get; set; }

        [Column("AmbitoApplicazioneId")]
        public int? AmbitoApplicazioneId { get; set; }

        [StringLength(200)]
        [Column("NumeroAtto")]
        public string? NumeroAtto { get; set; }

        [Required]
        [Column("DataRilascio")]
        public DateTime DataRilascio { get; set; }

        [Column("DataScadenza")]
        public DateTime? DataScadenza { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Stato")]
        public required string Stato { get; set; }

        [Column("DocumentoRilascioId")]
        public int? DocumentoRilascioId { get; set; }

        [Column("Note")]
        public string? Note { get; set; }

        [Column("DataInizioValidita")]
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;

        [Column("DataFineValidita")]
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;

        // Navigation Properties
        [ForeignKey(nameof(EnteAccreditamentoId))]
        public virtual EnteAccreditamento? EnteAccreditamento { get; set; }

        [ForeignKey(nameof(EnteAccreditatoId))]
        public virtual OrganismoAccreditato? EnteAccreditato { get; set; }

        [ForeignKey(nameof(AmbitoApplicazioneId))]
        public virtual AmbitoApplicazione? AmbitoApplicazione { get; set; }
    }

    /// <summary>
    /// Documento - schema Accreditamento
    /// </summary>
    [Table("Documento", Schema = "Accreditamento")]
    public class Documento
    {
        [Key]
        [Column("DocumentoId")]
        public int DocumentoId { get; set; }

        [Column("EntitaAziendaleId")]
        public int? EntitaAziendaleId { get; set; }

        [StringLength(510)]
        [Column("NomeFile")]
        public string? NomeFile { get; set; }

        [StringLength(200)]
        [Column("MimeType")]
        public string? MimeType { get; set; }

        [StringLength(2000)]
        [Column("PercorsoArchivio")]
        public string? PercorsoArchivio { get; set; }

        [Column("Note")]
        public string? Note { get; set; }

        [Column("DataCaricamento")]
        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;

        [Column("CreatoDa")]
        public int? CreatoDa { get; set; }

        [Column("UniqueRowId")]
        public Guid UniqueRowId { get; set; } = Guid.NewGuid();
    }
}
