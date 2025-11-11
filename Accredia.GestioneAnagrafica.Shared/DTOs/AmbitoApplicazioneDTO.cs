namespace Accredia.GestioneAnagrafica.API.DTOs;

/// <summary>
/// DTOs per Ambito Applicazione
/// </summary>
public static class AmbitoApplicazioneDTO
{
    /// <summary>
    /// DTO per la creazione di un Ambito Applicazione
    /// </summary>
    public class Create
    {
        public string Codice { get; set; } = string.Empty;
        public string Denominazione { get; set; } = string.Empty;
        public string? Descrizione { get; set; }
        public int Ordine { get; set; }
        public bool Attivo { get; set; } = true;
    }

    /// <summary>
    /// DTO per l'aggiornamento di un Ambito Applicazione
    /// </summary>
    public class Update
    {
        public string Codice { get; set; } = string.Empty;
        public string Denominazione { get; set; } = string.Empty;
        public string? Descrizione { get; set; }
        public int Ordine { get; set; }
        public bool Attivo { get; set; }
    }

    /// <summary>
    /// DTO per la risposta di un Ambito Applicazione
    /// </summary>
    public class Response
    {
        public int AmbitoApplicazioneId { get; set; }
        public string Codice { get; set; } = string.Empty;
        public string Denominazione { get; set; } = string.Empty;
        public string? Descrizione { get; set; }
        public int Ordine { get; set; }
        public bool Attivo { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime? DataModifica { get; set; }
        public DateTime DataInizioValidita { get; set; }
        public DateTime DataFineValidita { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// DTO per lista paginata (vista semplificata)
    /// </summary>
    public class List
    {
        public int AmbitoApplicazioneId { get; set; }
        public string Codice { get; set; } = string.Empty;
        public string Denominazione { get; set; } = string.Empty;
        public int Ordine { get; set; }
        public bool Attivo { get; set; }
    }

    /// <summary>
    /// DTO semplificato per dropdown/select
    /// </summary>
    public class Lookup
    {
        public int AmbitoApplicazioneId { get; set; }
        public string Codice { get; set; } = string.Empty;
        public string Denominazione { get; set; } = string.Empty;
        public string Display => $"{Codice} - {Denominazione}";
    }
}
