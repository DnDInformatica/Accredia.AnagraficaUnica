using Microsoft.Extensions.Options;
using Accredia.GestioneAnagrafica.API.Config;
using System.Net.Http.Headers;
using System.Text;

namespace Accredia.GestioneAnagrafica.API.Services;

/// <summary>
/// Servizio per la gestione dello storage documenti con supporto Nextcloud WebDAV
/// </summary>
public class DocumentStorageService : IDocumentStorageService
{
    private readonly DocumentStorageConfig _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DocumentStorageService> _logger;

    public DocumentStorageService(
        IOptions<DocumentStorageConfig> config,
        IHttpClientFactory httpClientFactory,
        ILogger<DocumentStorageService> logger)
    {
        _config = config.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;

        // Crea directory se non esistono
        EnsureDirectoriesExist();
    }

    private void EnsureDirectoriesExist()
    {
        try
        {
            if (!Directory.Exists(_config.LocalBasePath))
            {
                Directory.CreateDirectory(_config.LocalBasePath);
                _logger.LogInformation("Creata directory base: {Path}", _config.LocalBasePath);
            }

            if (!Directory.Exists(_config.LocalTempPath))
            {
                Directory.CreateDirectory(_config.LocalTempPath);
                _logger.LogInformation("Creata directory temp: {Path}", _config.LocalTempPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione delle directory");
        }
    }

    /// <summary>
    /// Genera un percorso file univoco organizzato per anno/mese
    /// </summary>
    private string GenerateFilePath(string originalFileName)
    {
        var now = DateTime.UtcNow;
        var yearMonth = $"{now.Year}\\{now.Month:D2}";
        var directory = Path.Combine(_config.LocalBasePath, yearMonth);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var extension = Path.GetExtension(originalFileName);
        var guid = Guid.NewGuid().ToString("N");
        var safeFileName = $"{guid}{extension}";

        return Path.Combine(yearMonth, safeFileName);
    }

    public async Task<string> SaveFileAsync(string fileName, byte[] fileContent, CancellationToken cancellationToken = default)
    {
        try
        {
            // Genera percorso relativo
            var relativePath = GenerateFilePath(fileName);
            var fullPath = Path.Combine(_config.LocalBasePath, relativePath);

            // Salva su filesystem locale
            await File.WriteAllBytesAsync(fullPath, fileContent, cancellationToken);
            _logger.LogInformation("File salvato localmente: {Path}", fullPath);

            // Sincronizza su Nextcloud se abilitato
            if (_config.Nextcloud.Enabled && _config.Nextcloud.SyncOnUpload)
            {
                await SyncToNextcloudAsync(relativePath, cancellationToken);
            }

            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il salvataggio del file {FileName}", fileName);
            throw;
        }
    }

    public async Task<string> SaveFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default)
    {
        try
        {
            // Genera percorso relativo
            var relativePath = GenerateFilePath(fileName);
            var fullPath = Path.Combine(_config.LocalBasePath, relativePath);

            // Salva su filesystem locale
            using (var fileStreamOut = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true))
            {
                await fileStream.CopyToAsync(fileStreamOut, cancellationToken);
            }

            _logger.LogInformation("File salvato localmente (stream): {Path}", fullPath);

            // Sincronizza su Nextcloud se abilitato
            if (_config.Nextcloud.Enabled && _config.Nextcloud.SyncOnUpload)
            {
                await SyncToNextcloudAsync(relativePath, cancellationToken);
            }

            return relativePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il salvataggio del file (stream) {FileName}", fileName);
            throw;
        }
    }

    public async Task<byte[]?> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = Path.Combine(_config.LocalBasePath, filePath);

            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("File non trovato: {Path}", fullPath);
                return null;
            }

            return await File.ReadAllBytesAsync(fullPath, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la lettura del file {FilePath}", filePath);
            throw;
        }
    }

    public async Task<Stream?> GetFileStreamAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = Path.Combine(_config.LocalBasePath, filePath);

            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("File non trovato: {Path}", fullPath);
                return null;
            }

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'apertura stream del file {FilePath}", filePath);
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            var fullPath = Path.Combine(_config.LocalBasePath, filePath);

            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("File da eliminare non trovato: {Path}", fullPath);
                return false;
            }

            // Elimina da Nextcloud se abilitato
            if (_config.Nextcloud.Enabled && _config.Nextcloud.SyncOnDelete)
            {
                await DeleteFromNextcloudAsync(filePath, cancellationToken);
            }

            // Elimina file locale
            File.Delete(fullPath);
            _logger.LogInformation("File eliminato: {Path}", fullPath);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione del file {FilePath}", filePath);
            throw;
        }
    }

    public async Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        var fullPath = Path.Combine(_config.LocalBasePath, filePath);
        return File.Exists(fullPath);
    }

    public async Task<long> GetFileSizeAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        var fullPath = Path.Combine(_config.LocalBasePath, filePath);

        if (!File.Exists(fullPath))
            return 0;

        var fileInfo = new FileInfo(fullPath);
        return fileInfo.Length;
    }

    public bool IsFileAllowed(string fileName, string mimeType)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!_config.AllowedExtensions.Contains(extension))
        {
            _logger.LogWarning("Estensione file non permessa: {Extension}", extension);
            return false;
        }

        if (!_config.AllowedMimeTypes.Contains(mimeType.ToLowerInvariant()))
        {
            _logger.LogWarning("MIME type non permesso: {MimeType}", mimeType);
            return false;
        }

        return true;
    }

    public async Task<bool> SyncToNextcloudAsync(string localPath, CancellationToken cancellationToken = default)
    {
        if (!_config.Nextcloud.Enabled)
            return false;

        try
        {
            var fullLocalPath = Path.Combine(_config.LocalBasePath, localPath);
            if (!File.Exists(fullLocalPath))
                return false;

            var fileContent = await File.ReadAllBytesAsync(fullLocalPath, cancellationToken);
            var httpClient = _httpClientFactory.CreateClient("Nextcloud");
            
            var credentials = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{_config.Nextcloud.Username}:{_config.Nextcloud.Password}")
            );
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var webdavUrl = _config.Nextcloud.GetWebDAVUrl();
            var remotePath = $"{_config.Nextcloud.BasePath}/{localPath}".Replace("\\", "/");
            var uploadUrl = $"{webdavUrl}{remotePath}";

            await EnsureRemoteDirectoryExistsAsync(httpClient, Path.GetDirectoryName(remotePath)!, cancellationToken);

            using var content = new ByteArrayContent(fileContent);
            var response = await httpClient.PutAsync(uploadUrl, content, cancellationToken);

            if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                _logger.LogInformation("File sincronizzato su Nextcloud: {Path}", remotePath);
                return true;
            }

            _logger.LogError("Errore sync Nextcloud. Status: {Status}", response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la sincronizzazione su Nextcloud");
            return false;
        }
    }

    private async Task<bool> DeleteFromNextcloudAsync(string localPath, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("Nextcloud");
            
            var credentials = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{_config.Nextcloud.Username}:{_config.Nextcloud.Password}")
            );
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var webdavUrl = _config.Nextcloud.GetWebDAVUrl();
            var remotePath = $"{_config.Nextcloud.BasePath}/{localPath}".Replace("\\", "/");
            var deleteUrl = $"{webdavUrl}{remotePath}";

            var response = await httpClient.DeleteAsync(deleteUrl, cancellationToken);

            if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                _logger.LogInformation("File eliminato da Nextcloud: {Path}", remotePath);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione da Nextcloud");
            return false;
        }
    }

    private async Task EnsureRemoteDirectoryExistsAsync(HttpClient httpClient, string remotePath, CancellationToken cancellationToken)
    {
        try
        {
            var webdavUrl = _config.Nextcloud.GetWebDAVUrl();
            var dirPath = remotePath.Replace("\\", "/");
            var mkcolUrl = $"{webdavUrl}{dirPath}";

            var request = new HttpRequestMessage(new HttpMethod("MKCOL"), mkcolUrl);
            var response = await httpClient.SendAsync(request, cancellationToken);

            if (response.StatusCode != System.Net.HttpStatusCode.MethodNotAllowed)
            {
                _logger.LogDebug("Directory remota creata: {Path}", dirPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Errore durante la creazione directory remota");
        }
    }
}
