using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestioneOrganismi.Backend.Models
{
    public class EnteAccreditamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Nome { get; set; }

        [StringLength(500)]
        public string? Descrizione { get; set; }

        [Required]
        [StringLength(50)]
        public string CodiceIdentificativo { get; set; }

        [StringLength(100)]
        public string? SettoreMerceologico { get; set; }

        public DateTime DataAccreditamento { get; set; }

        // Soft Delete
        public bool IsDeleted { get; set; } = false;

        // Auditing
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Stato dell'Ente di Accreditamento
        public enum StatoAccreditamento
        {
            Attivo = 1,
            Sospeso = 2,
            Revocato = 3,
            InIstruttoria = 4
        }

        public StatoAccreditamento Stato { get; set; } = StatoAccreditamento.InIstruttoria;

        // Metodi per soft delete
        public void SoftDelete(string userId)
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = userId;
        }

        public void Restore(string userId)
        {
            IsDeleted = false;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = userId;
        }
    }
}