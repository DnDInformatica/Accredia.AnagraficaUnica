namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Interfaccia per il servizio Dashboard
    /// </summary>
    public interface IDashboardService
    {
        Task<object?> GetDashboardDataAsync();
    }
}
