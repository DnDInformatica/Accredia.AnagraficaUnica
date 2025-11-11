using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.Shared.Models
{
    /// <summary>
    /// Entità Anagrafica Contatto - Relazione N:N tra Entità e Persone
    /// </summary>
    [Table("EntitaAnagraficaContatto", Schema = "Persone")]
    public class EntitaAnagraficaContatto
    {
        [Key]
        public int EntitaAnagraficaContattoId { get; set; }
        
        [Required]
        public int EntitaAnagraficaId { get; set; }
        
        [Required]
        public int EntitaAziendaleId { get; set; }
        
        [Required]
        public int TipologiaContattoId { get; set; }
        
        public string? Note { get; set; }
        public Guid rowguid { get; set; } = Guid.NewGuid();
        public bool Attivo { get; set; } = true;
        
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;
        
        [NotMapped]
        public bool IsDeleted => DataCancellazione.HasValue;
    }
}
