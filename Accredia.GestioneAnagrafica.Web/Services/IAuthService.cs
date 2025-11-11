namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Interfaccia per il servizio di autenticazione
    /// </summary>
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
    }
}
