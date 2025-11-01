namespace GestioneOrganismi.Backend.DTOs;

/// <summary>
/// DTO per Ente Accreditamento con classi nested
/// </summary>
public static class EnteAccreditamentoDTO
{
    /// <summary>
    /// DTO per la creazione di un Ente Accreditamento
    /// </summary>
    public class Create
    {
        public string Nome { get; set; } = string.Empty;
        public string Codice { get; set; } = string.Empty;
        public string? Descrizione { get; set; }
        public string? SettoreMerceologico { get; set; }
        public DateTime DataAccreditamento { get; set; }
    }

    /// <summary>
    /// DTO per l'aggiornamento di un Ente Accreditamento
    /// </summary>
    public class Update
    {
        public string Nome { get; set; } = string.Empty;
        public string Codice { get; set; } = string.Empty;
        public string? Descrizione { get; set; }
        public string? SettoreMerceologico { get; set; }
        public DateTime DataAccreditamento { get; set; }
        public int Stato { get; set; }
    }

    /// <summary>
    /// DTO per la risposta di un Ente Accreditamento
    /// </summary>
    public class Response
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codice { get; set; } = string.Empty;
        public string? Descrizione { get; set; }
        public string? SettoreMerceologico { get; set; }
        public DateTime DataAccreditamento { get; set; }
        public string Stato { get; set; } = string.Empty;
        public DateTime DataCreazione { get; set; }
        public DateTime? DataUltimaModifica { get; set; }
    }
}
