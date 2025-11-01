using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneOrganismi.Backend.Models
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
    }

    [Table("Dipartimento", Schema = "RisorseUmane")]
    public class Dipartimento
    {
        [Key]
        public int DipartimentoId { get; set; }
        [Required, StringLength(200)]
        public required string Nome { get; set; }
        public int? RepartoId { get; set; }
        public Guid UniqueRowId { get; set; } = Guid.NewGuid();
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;
    }

    [Table("Reparto", Schema = "RisorseUmane")]
    public class Reparto
    {
        [Key]
        public int RepartoId { get; set; }
        [Required, StringLength(100)]
        public required string Nome { get; set; }
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
    }
}
