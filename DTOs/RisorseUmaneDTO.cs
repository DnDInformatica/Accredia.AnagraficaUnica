using System.Text.Json.Serialization;

namespace GestioneOrganismi.Backend.DTOs;

// ==================== DIPENDENTE ====================
public class DipendenteDTO
{
    public class Create
    {
        [JsonPropertyName("entitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        [JsonPropertyName("codiceFiscale")]
        public string CodiceFiscale { get; set; } = string.Empty;

        [JsonPropertyName("matricola")]
        public string Matricola { get; set; } = string.Empty;

        [JsonPropertyName("loginID")]
        public string LoginID { get; set; } = string.Empty;

        [JsonPropertyName("mansione")]
        public string Mansione { get; set; } = string.Empty;

        [JsonPropertyName("titoloOnorificoId")]
        public int? TitoloOnorificoId { get; set; }

        [JsonPropertyName("repartoId")]
        public int? RepartoId { get; set; }

        [JsonPropertyName("turnoId")]
        public int? TurnoId { get; set; }
    }

    public class Update : Create
    {
        [JsonPropertyName("dipendenteId")]
        public int DipendenteId { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("dipendenteId")]
        public int DipendenteId { get; set; }

        [JsonPropertyName("entitaAziendaleId")]
        public int EntitaAziendaleId { get; set; }

        [JsonPropertyName("codiceFiscale")]
        public string CodiceFiscale { get; set; } = string.Empty;

        [JsonPropertyName("matricola")]
        public string Matricola { get; set; } = string.Empty;

        [JsonPropertyName("loginID")]
        public string LoginID { get; set; } = string.Empty;

        [JsonPropertyName("mansione")]
        public string Mansione { get; set; } = string.Empty;

        [JsonPropertyName("titoloOnorificoId")]
        public int? TitoloOnorificoId { get; set; }

        [JsonPropertyName("repartoId")]
        public int? RepartoId { get; set; }

        [JsonPropertyName("repartoNome")]
        public string? RepartoNome { get; set; }

        [JsonPropertyName("turnoId")]
        public int? TurnoId { get; set; }

        [JsonPropertyName("turnoDescrizione")]
        public string? TurnoDescrizione { get; set; }

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataModifica")]
        public DateTime? DataModifica { get; set; }

        [JsonPropertyName("uniqueRowId")]
        public Guid UniqueRowId { get; set; }
    }

    public class List
    {
        [JsonPropertyName("dipendenteId")]
        public int DipendenteId { get; set; }

        [JsonPropertyName("codiceFiscale")]
        public string CodiceFiscale { get; set; } = string.Empty;

        [JsonPropertyName("matricola")]
        public string Matricola { get; set; } = string.Empty;

        [JsonPropertyName("mansione")]
        public string Mansione { get; set; } = string.Empty;

        [JsonPropertyName("repartoNome")]
        public string? RepartoNome { get; set; }

        [JsonPropertyName("turnoDescrizione")]
        public string? TurnoDescrizione { get; set; }
    }
}

// ==================== DIPARTIMENTO ====================
public class DipartimentoDTO
{
    public class Create
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("dipartimentoPadreId")]
        public int? DipartimentoPadreId { get; set; }
    }

    public class Update : Create
    {
        [JsonPropertyName("dipartimentoId")]
        public int DipartimentoId { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("dipartimentoId")]
        public int DipartimentoId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("dipartimentoPadreId")]
        public int? DipartimentoPadreId { get; set; }

        [JsonPropertyName("dipartimentoPadreNome")]
        public string? DipartimentoPadreNome { get; set; }

        [JsonPropertyName("numeroReparti")]
        public int NumeroReparti { get; set; }

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataModifica")]
        public DateTime? DataModifica { get; set; }

        [JsonPropertyName("uniqueRowId")]
        public Guid UniqueRowId { get; set; }
    }

    public class List
    {
        [JsonPropertyName("dipartimentoId")]
        public int DipartimentoId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("numeroReparti")]
        public int NumeroReparti { get; set; }
    }
}

// ==================== REPARTO ====================
public class RepartoDTO
{
    public class Create
    {
        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("dipartimentoId")]
        public int? DipartimentoId { get; set; }

        [JsonPropertyName("managerDipendenteId")]
        public int? ManagerDipendenteId { get; set; }
    }

    public class Update : Create
    {
        [JsonPropertyName("repartoId")]
        public int RepartoId { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("repartoId")]
        public int RepartoId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("dipartimentoId")]
        public int? DipartimentoId { get; set; }

        [JsonPropertyName("dipartimentoNome")]
        public string? DipartimentoNome { get; set; }

        [JsonPropertyName("managerDipendenteId")]
        public int? ManagerDipendenteId { get; set; }

        [JsonPropertyName("managerMatricola")]
        public string? ManagerMatricola { get; set; }

        [JsonPropertyName("numeroDipendenti")]
        public int NumeroDipendenti { get; set; }

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataModifica")]
        public DateTime? DataModifica { get; set; }

        [JsonPropertyName("uniqueRowId")]
        public Guid UniqueRowId { get; set; }
    }

    public class List
    {
        [JsonPropertyName("repartoId")]
        public int RepartoId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("dipartimentoNome")]
        public string? DipartimentoNome { get; set; }

        [JsonPropertyName("numeroDipendenti")]
        public int NumeroDipendenti { get; set; }
    }
}

// ==================== TURNO ====================
public class TurnoDTO
{
    public class Create
    {
        [JsonPropertyName("descrizione")]
        public string Descrizione { get; set; } = string.Empty;

        [JsonPropertyName("oraInizio")]
        public TimeSpan OraInizio { get; set; }

        [JsonPropertyName("oraFine")]
        public TimeSpan OraFine { get; set; }
    }

    public class Update : Create
    {
        [JsonPropertyName("turnoId")]
        public int TurnoId { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("turnoId")]
        public int TurnoId { get; set; }

        [JsonPropertyName("descrizione")]
        public string Descrizione { get; set; } = string.Empty;

        [JsonPropertyName("oraInizio")]
        public TimeSpan OraInizio { get; set; }

        [JsonPropertyName("oraFine")]
        public TimeSpan OraFine { get; set; }

        [JsonPropertyName("durata")]
        public TimeSpan Durata { get; set; }

        [JsonPropertyName("numeroDipendenti")]
        public int NumeroDipendenti { get; set; }

        [JsonPropertyName("dataCreazione")]
        public DateTime DataCreazione { get; set; }

        [JsonPropertyName("dataModifica")]
        public DateTime? DataModifica { get; set; }

        [JsonPropertyName("uniqueRowId")]
        public Guid UniqueRowId { get; set; }
    }

    public class List
    {
        [JsonPropertyName("turnoId")]
        public int TurnoId { get; set; }

        [JsonPropertyName("descrizione")]
        public string Descrizione { get; set; } = string.Empty;

        [JsonPropertyName("oraInizio")]
        public string OraInizio { get; set; } = string.Empty;

        [JsonPropertyName("oraFine")]
        public string OraFine { get; set; } = string.Empty;

        [JsonPropertyName("durata")]
        public string Durata { get; set; } = string.Empty;

        [JsonPropertyName("numeroDipendenti")]
        public int NumeroDipendenti { get; set; }
    }
}
