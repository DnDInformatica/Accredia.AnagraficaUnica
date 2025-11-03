using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.API.Models
{
    /// <summary>
    /// Organismo Accreditato - schema Organismi
    /// </summary>
    [Table("OrganismoAccreditato", Schema = "Organismi")]
    public class OrganismoAccreditato
    {
        [Key]
        [Column("EntitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        [Required]
        [StringLength(510)]
        [Column("RagioneSociale")]
        public required string RagioneSociale { get; set; }

        [StringLength(100)]
        [Column("PartitaIVA")]
        public string? PartitaIVA { get; set; }

        [StringLength(100)]
        [Column("CodiceFiscale")]
        public string? CodiceFiscale { get; set; }

        [Column("TipoOrganismoId")]
        public int? TipoOrganismoId { get; set; }

        [Column("EnteAccreditamentoId")]
        public int? EnteAccreditamentoId { get; set; }

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

        [Column("UniqueRowId")]
        public Guid UniqueRowId { get; set; } = Guid.NewGuid();

        [Column("DataInizioValidita")]
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;

        [Column("DataFineValidita")]
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;

        // Computed
        [NotMapped]
        public bool IsDeleted => DataCancellazione.HasValue;

        // Navigation Properties
        [ForeignKey(nameof(EnteAccreditamentoId))]
        public virtual EnteAccreditamento? EnteAccreditamento { get; set; }

        // Metodi Soft Delete
        public void SoftDelete(int userId)
        {
            DataCancellazione = DateTime.UtcNow;
            CancellatoDa = userId;
        }

        public void Restore(int userId)
        {
            DataCancellazione = null;
            CancellatoDa = null;
            DataModifica = DateTime.UtcNow;
            ModificatoDa = userId;
        }
    }
}
