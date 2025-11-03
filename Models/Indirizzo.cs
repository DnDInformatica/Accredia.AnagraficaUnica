using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.API.Models
{
    /// <summary>
    /// Entità Indirizzo
    /// </summary>
    [Table("Indirizzo", Schema = "Persone")]
    public class Indirizzo
    {
        [Key]
        [Column("IndirizzoId")]
        public int IndirizzoId { get; set; }

        [Required]
        [StringLength(200)]
        [Column("Via")]
        public string Via { get; set; } = string.Empty;

        [StringLength(20)]
        [Column("NumeroCivico")]
        public string? NumeroCivico { get; set; }

        [Required]
        [StringLength(10)]
        [Column("CAP")]
        public string CAP { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("Citta")]
        public string Citta { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        [Column("Provincia")]
        public string Provincia { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("Regione")]
        public string? Regione { get; set; }

        [StringLength(100)]
        [Column("Stato")]
        public string Stato { get; set; } = "Italia";

        [Column("ComuneId")]
        public int? ComuneId { get; set; }

        [Column("Latitudine", TypeName = "decimal(10,7)")]
        public decimal? Latitudine { get; set; }

        [Column("Longitudine", TypeName = "decimal(10,7)")]
        public decimal? Longitudine { get; set; }

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

        [Column("DataCancellazione")]
        public DateTime? DataCancellazione { get; set; }

        [Column("CancellatoDa")]
        [StringLength(256)]
        public string? CancellatoDa { get; set; }

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

        [NotMapped]
        public string IndirizzoCompleto => 
            $"{Via} {NumeroCivico}, {CAP} {Citta} ({Provincia})".Trim();

        // Navigation Properties
        public virtual ICollection<PersonaIndirizzo> PersonaIndirizzi { get; set; } = new List<PersonaIndirizzo>();
    }
}
