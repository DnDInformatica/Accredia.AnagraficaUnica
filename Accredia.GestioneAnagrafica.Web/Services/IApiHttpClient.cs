namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Interfaccia per il client HTTP personalizzato con gestione JWT
    /// </summary>
    public interface IApiHttpClient
    {
        Task<T?> GetAsync<T>(string url);
        Task<T?> PostAsync<T>(string url, object data);
        Task<T?> PutAsync<T>(string url, object data);
        Task DeleteAsync(string url);
    }
}
