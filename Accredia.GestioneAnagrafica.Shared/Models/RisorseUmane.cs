using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accredia.GestioneAnagrafica.Shared.Models
{
    [Table("Dipendente", Schema = "RisorseUmane")]
    public class Dipendente
    {
        [Key]
        public int DipendenteId { get; set; }
        
        [Required]
        public int EntitaAziendaleId { get; set; }
        
        [Required, StringLength(32)]
        public required string CodiceFiscale { get; set; }
        
        [Required, StringLength(32)]
        public required string Matricola { get; set; }
        
        [Required, StringLength(512)]
        public required string LoginID { get; set; }
        
        public int? TitoloOnorificoId { get; set; }
        
        [Required, StringLength(100)]
        public required string Mansione { get; set; }
        
        public int? RepartoId { get; set; }
        public int? TurnoId { get; set; }
        
        public Guid UniqueRowId { get; set; } = Guid.NewGuid();
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

        // Navigation Properties
        [ForeignKey(nameof(RepartoId))]
        public virtual Reparto? Reparto { get; set; }

        [ForeignKey(nameof(TurnoId))]
        public virtual Turno? Turno { get; set; }
    }

    [Table("Dipartimento", Schema = "RisorseUmane")]
    public class Dipartimento
    {
        [Key]
        public int DipartimentoId { get; set; }
        
        [Required, StringLength(200)]
        public required string Nome { get; set; }
        
        public int? DipartimentoPadreId { get; set; }
        
        public Guid UniqueRowId { get; set; } = Guid.NewGuid();
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

        // Navigation Properties
        [ForeignKey(nameof(DipartimentoPadreId))]
        public virtual Dipartimento? DipartimentoPadre { get; set; }
        
        public virtual ICollection<Reparto> Reparti { get; set; } = new List<Reparto>();
        public virtual ICollection<Dipartimento> DipartimentiFiliali { get; set; } = new List<Dipartimento>();
    }

    [Table("Reparto", Schema = "RisorseUmane")]
    public class Reparto
    {
        [Key]
        public int RepartoId { get; set; }
        
        [Required, StringLength(100)]
        public required string Nome { get; set; }
        
        public int? DipartimentoId { get; set; }
        public int? ManagerDipendenteId { get; set; }
        
        public Guid UniqueRowId { get; set; } = Guid.NewGuid();
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

        // Navigation Properties
        [ForeignKey(nameof(DipartimentoId))]
        public virtual Dipartimento? Dipartimento { get; set; }

        [ForeignKey(nameof(ManagerDipendenteId))]
        public virtual Dipendente? Manager { get; set; }

        public virtual ICollection<Dipendente> Dipendenti { get; set; } = new List<Dipendente>();
    }

    [Table("Turno", Schema = "RisorseUmane")]
    public class Turno
    {
        [Key]
        public int TurnoId { get; set; }
        
        [Required, StringLength(100)]
        public required string Descrizione { get; set; }
        
        [Required]
        public TimeSpan OraInizio { get; set; }
        
        [Required]
        public TimeSpan OraFine { get; set; }
        
        public Guid UniqueRowId { get; set; } = Guid.NewGuid();
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

        [NotMapped]
        public TimeSpan Durata => OraFine - OraInizio;

        // Navigation Properties
        public virtual ICollection<Dipendente> Dipendenti { get; set; } = new List<Dipendente>();
    }
}
