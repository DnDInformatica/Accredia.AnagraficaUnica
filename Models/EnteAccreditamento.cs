using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.API.Models
{
    public class EnteAccreditamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntitaAziendaleId { get; set; }

        [Required]
        [StringLength(250)]
        public required string Denominazione { get; set; }

        [StringLength(50)]
        public string? Sigla { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }

        public DateTime? DataFondazione { get; set; }

        // Auditing
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        
        // Soft Delete
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }

        public Guid UniqueRowId { get; set; } = Guid.NewGuid();
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;

        // Computed property per soft delete
        [NotMapped]
        public bool IsDeleted => DataCancellazione.HasValue;

        // Metodi per soft delete
        public void SoftDelete(int userId)
        {
            DataCancellazione = DateTime.UtcNow;
            CancellatoDa = userId;
        }

        public void Restore()
        {
            DataCancellazione = null;
            CancellatoDa = null;
        }
    }
}