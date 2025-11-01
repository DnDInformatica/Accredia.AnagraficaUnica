using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneOrganismi.Backend.Models
{
    /// <summary>
    /// Tipo Email - schema Tipologica
    /// </summary>
    [Table("TipoEmail", Schema = "Tipologica")]
    public class TipoEmail
    {
        [Key]
        [Column("TipoEmailId")]
        public int TipoEmailId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Codice")]
        public required string Codice { get; set; }

        [Required]
        [StringLength(400)]
        [Column("Descrizione")]
        public required string Descrizione { get; set; }

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
    }

    [Table("TipoTelefono", Schema = "Tipologica")]
    public class TipoTelefono
    {
        [Key]
        public int TipoTelefonoId { get; set; }
        [Required, StringLength(100)]
        public required string Codice { get; set; }
        [Required, StringLength(400)]
        public required string Descrizione { get; set; }
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;
    }

    [Table("TipoIndirizzo", Schema = "Tipologica")]
    public class TipoIndirizzo
    {
        [Key]
        public int TipoIndirizzoId { get; set; }
        [Required, StringLength(100)]
        public required string Codice { get; set; }
        [Required, StringLength(400)]
        public required string Descrizione { get; set; }
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;
    }

    [Table("TipoEnteAccreditamento", Schema = "Tipologica")]
    public class TipoEnteAccreditamento
    {
        [Key]
        public int TipoEnteAccreditamentoId { get; set; }
        [Required, StringLength(100)]
        public required string Codice { get; set; }
        [Required, StringLength(510)]
        public required string Descrizione { get; set; }
        public Guid? UniqueRowId { get; set; }
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }
        public DateTime DataInizioValidita { get; set; } = DateTime.UtcNow;
        public DateTime DataFineValidita { get; set; } = DateTime.MaxValue;
    }

    [Table("TitoloOnorifico", Schema = "Tipologica")]
    public class TitoloOnorifico
    {
        [Key]
        public int TitoloOnorificoId { get; set; }
        [Required, StringLength(20)]
        public required string Codice { get; set; }
        [Required, StringLength(100)]
        public required string Descrizione { get; set; }
        [StringLength(100)]
        public string? TitoloMaschile { get; set; }
        [StringLength(100)]
        public string? TitoloFemminile { get; set; }
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
