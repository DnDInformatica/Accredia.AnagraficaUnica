namespace GestioneOrganismi.Backend.DTOs;

/// <summary>
/// DTOs per Organismo Accreditato
/// </summary>
public static class OrganismoAccreditatoDTO
{
    /// <summary>
    /// DTO per la creazione di un Organismo Accreditato
    /// </summary>
    public class Create
    {
        public string RagioneSociale { get; set; } = string.Empty;
        public string? PartitaIVA { get; set; }
        public string? CodiceFiscale { get; set; }
        public int? TipoOrganismoId { get; set; }
        public int? EnteAccreditamentoId { get; set; }
    }

    /// <summary>
    /// DTO per l'aggiornamento di un Organismo Accreditato
    /// </summary>
    public class Update
    {
        public string RagioneSociale { get; set; } = string.Empty;
        public string? PartitaIVA { get; set; }
        public string? CodiceFiscale { get; set; }
        public int? TipoOrganismoId { get; set; }
        public int? EnteAccreditamentoId { get; set; }
    }

    /// <summary>
    /// DTO per la risposta di un Organismo Accreditato
    /// </summary>
    public class Response
    {
        public int EntitaAziendaleId { get; set; }
        public string RagioneSociale { get; set; } = string.Empty;
        public string? PartitaIVA { get; set; }
        public string? CodiceFiscale { get; set; }
        public int? TipoOrganismoId { get; set; }
        public int? EnteAccreditamentoId { get; set; }
        public string? NomeEnteAccreditamento { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataModifica { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// DTO per lista paginata (vista semplificata)
    /// </summary>
    public class List
    {
        public int EntitaAziendaleId { get; set; }
        public string RagioneSociale { get; set; } = string.Empty;
        public string? PartitaIVA { get; set; }
        public string? CodiceFiscale { get; set; }
        public string? NomeEnteAccreditamento { get; set; }
        public DateTime DataCreazione { get; set; }
    }
}
