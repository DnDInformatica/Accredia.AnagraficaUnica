using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.Shared.Models
{
    /// <summary>
    /// Entità Aziendale - Tabella base per tutte le entità
    /// Contiene solo metadati e audit trail
    /// </summary>
    [Table("EntitaAziendale", Schema = "Persone")]
    public class EntitaAziendale
    {
        [Key]
        [Column("EntitaAziendaleId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntitaAziendaleId { get; set; }

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
        public virtual ICollection<Persona> Persone { get; set; } = new List<Persona>();
    }
}
