namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Interfaccia per il servizio degli Organismi
    /// </summary>
    public interface IOrganismiService
    {
        Task<List<object>> GetOrganismiAsync();
        Task<object?> GetOrganismoByIdAsync(int id);
    }
}
