namespace Accredia.GestioneAnagrafica.API.DTOs;

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
        public int EntitaAziendaleId { get; set; }
        public string Denominazione { get; set; } = string.Empty;
        public string? Sigla { get; set; }
        public string? Note { get; set; }
        public DateTime? DataFondazione { get; set; }
    }

    /// <summary>
    /// DTO per l'aggiornamento di un Ente Accreditamento
    /// </summary>
    public class Update
    {
        public string Denominazione { get; set; } = string.Empty;
        public string? Sigla { get; set; }
        public string? Note { get; set; }
        public DateTime? DataFondazione { get; set; }
    }

    /// <summary>
    /// DTO per la risposta di un Ente Accreditamento
    /// </summary>
    public class Response
    {
        public int Id { get; set; }
        public int EntitaAziendaleId { get; set; }
        public string Denominazione { get; set; } = string.Empty;
        public string? Sigla { get; set; }
        public string? Note { get; set; }
        public DateTime? DataFondazione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataModifica { get; set; }
        public Guid UniqueRowId { get; set; }
        public DateTime DataInizioValidita { get; set; }
        public DateTime DataFineValidita { get; set; }
    }
}
