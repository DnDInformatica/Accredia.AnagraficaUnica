namespace GestioneOrganismi.Backend.Services;

/// <summary>
/// Interfaccia per il servizio di storage documenti
/// </summary>
public interface IDocumentStorageService
{
    /// <summary>
    /// Salva un file su storage locale e opzionalmente su Nextcloud
    /// </summary>
    Task<string> SaveFileAsync(string fileName, byte[] fileContent, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva un file da stream
    /// </summary>
    Task<string> SaveFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Recupera un file dallo storage
    /// </summary>
    Task<byte[]?> GetFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Recupera un file come stream
    /// </summary>
    Task<Stream?> GetFileStreamAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina un file dallo storage locale e opzionalmente da Nextcloud
    /// </summary>
    Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se un file esiste
    /// </summary>
    Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ottiene la dimensione del file
    /// </summary>
    Task<long> GetFileSizeAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sincronizza un file locale su Nextcloud
    /// </summary>
    Task<bool> SyncToNextcloudAsync(string localPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se il file è permesso (MIME type ed estensione)
    /// </summary>
    bool IsFileAllowed(string fileName, string mimeType);
}
