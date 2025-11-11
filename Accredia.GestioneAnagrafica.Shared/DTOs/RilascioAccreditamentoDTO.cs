namespace Accredia.GestioneAnagrafica.API.DTOs;

/// <summary>
/// DTOs per Rilascio Accreditamento
/// </summary>
public static class RilascioAccreditamentoDTO
{
    /// <summary>
    /// DTO per la creazione di un Rilascio Accreditamento
    /// </summary>
    public class Create
    {
        public int EnteAccreditamentoId { get; set; }
        public int EnteAccreditatoId { get; set; }
        public int? AmbitoApplicazioneId { get; set; }
        public string? NumeroAtto { get; set; }
        public DateTime DataRilascio { get; set; }
        public DateTime? DataScadenza { get; set; }
        public string Stato { get; set; } = "InLavorazione";
        public int? DocumentoRilascioId { get; set; }
        public string? Note { get; set; }
    }

    /// <summary>
    /// DTO per l'aggiornamento di un Rilascio Accreditamento
    /// </summary>
    public class Update
    {
        public int? AmbitoApplicazioneId { get; set; }
        public string? NumeroAtto { get; set; }
        public DateTime DataRilascio { get; set; }
        public DateTime? DataScadenza { get; set; }
        public string Stato { get; set; } = "InLavorazione";
        public int? DocumentoRilascioId { get; set; }
        public string? Note { get; set; }
    }

    /// <summary>
    /// DTO per la risposta di un Rilascio Accreditamento
    /// </summary>
    public class Response
    {
        public int RilascioId { get; set; }
        public int EnteAccreditamentoId { get; set; }
        public string? NomeEnteAccreditamento { get; set; }
        public int EnteAccreditatoId { get; set; }
        public string? RagioneSocialeEnteAccreditato { get; set; }
        public int? AmbitoApplicazioneId { get; set; }
        public string? DenominazioneAmbitoApplicazione { get; set; }
        public string? NumeroAtto { get; set; }
        public DateTime DataRilascio { get; set; }
        public DateTime? DataScadenza { get; set; }
        public string Stato { get; set; } = string.Empty;
        public int? DocumentoRilascioId { get; set; }
        public string? Note { get; set; }
        public bool IsScaduto => DataScadenza.HasValue && DataScadenza.Value < DateTime.UtcNow;
        public bool IsInScadenza => DataScadenza.HasValue && 
                                     DataScadenza.Value > DateTime.UtcNow && 
                                     DataScadenza.Value < DateTime.UtcNow.AddMonths(3);
    }

    /// <summary>
    /// DTO per lista paginata
    /// </summary>
    public class List
    {
        public int RilascioId { get; set; }
        public string? NomeEnteAccreditamento { get; set; }
        public string? RagioneSocialeEnteAccreditato { get; set; }
        public string? NumeroAtto { get; set; }
        public DateTime DataRilascio { get; set; }
        public DateTime? DataScadenza { get; set; }
        public string Stato { get; set; } = string.Empty;
        public bool IsScaduto { get; set; }
        public bool IsInScadenza { get; set; }
    }
}
