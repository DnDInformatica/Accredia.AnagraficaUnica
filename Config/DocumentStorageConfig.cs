namespace Accredia.GestioneAnagrafica.API.Config;

/// <summary>
/// Configurazione per lo storage dei documenti
/// </summary>
public class DocumentStorageConfig
{
    public string StorageType { get; set; } = "FileSystem";
    public string LocalBasePath { get; set; } = "C:\\Accredia\\Documenti";
    public string LocalTempPath { get; set; } = "C:\\Accredia\\Documenti\\Temp";
    public int MaxFileSizeMB { get; set; } = 500;
    public int ChunkSizeMB { get; set; } = 10;
    public List<string> AllowedMimeTypes { get; set; } = new();
    public List<string> AllowedExtensions { get; set; } = new();
    public NextcloudConfig Nextcloud { get; set; } = new();
}

/// <summary>
/// Configurazione specifica per Nextcloud
/// </summary>
public class NextcloudConfig
{
    public bool Enabled { get; set; } = false;
    public string ServerUrl { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string BasePath { get; set; } = "/Accredia/Documenti";
    public bool SyncOnUpload { get; set; } = true;
    public bool SyncOnDelete { get; set; } = true;
    public bool UseWebDAV { get; set; } = true;
    public string WebDAVPath { get; set; } = "/remote.php/dav/files";

    public string GetWebDAVUrl()
    {
        if (string.IsNullOrWhiteSpace(ServerUrl))
            return string.Empty;

        var baseUrl = ServerUrl.TrimEnd('/');
        var webdavPath = WebDAVPath.TrimStart('/');
        var username = Username;
        
        return $"{baseUrl}/{webdavPath}/{username}";
    }
}
