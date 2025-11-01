namespace GestioneOrganismi.Backend.DTOs;

/// <summary>
/// DTOs per Documento
/// </summary>
public static class DocumentoDTO
{
    /// <summary>
    /// DTO per il caricamento di un nuovo documento
    /// </summary>
    public class Upload
    {
        public int? EntitaAziendaleId { get; set; }
        public string NomeFile { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public string? Note { get; set; }
        /// <summary>
        /// Contenuto del file in Base64 per upload
        /// </summary>
        public string? ContenutoBase64 { get; set; }
    }

    /// <summary>
    /// DTO per la risposta di un documento
    /// </summary>
    public class Response
    {
        public int DocumentoId { get; set; }
        public int? EntitaAziendaleId { get; set; }
        public string? NomeFile { get; set; }
        public string? MimeType { get; set; }
        public string? PercorsoArchivio { get; set; }
        public string? Note { get; set; }
        public DateTime DataCaricamento { get; set; }
        public int? CreatoDa { get; set; }
        public Guid UniqueRowId { get; set; }
    }

    /// <summary>
    /// DTO per lista documenti
    /// </summary>
    public class List
    {
        public int DocumentoId { get; set; }
        public string? NomeFile { get; set; }
        public string? MimeType { get; set; }
        public DateTime DataCaricamento { get; set; }
        public string? Note { get; set; }
    }
}
