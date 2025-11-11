namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Implementazione del servizio Dashboard
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly IApiHttpClient _apiHttpClient;
        private readonly ILogger<DashboardService> _logger;
        private const string BaseUrl = "/api/dashboard";

        public DashboardService(IApiHttpClient apiHttpClient, ILogger<DashboardService> logger)
        {
            _apiHttpClient = apiHttpClient;
            _logger = logger;
        }

        public async Task<object?> GetDashboardDataAsync()
        {
            try
            {
                _logger.LogInformation("Recupero dei dati della dashboard in corso...");
                return await _apiHttpClient.GetAsync<object>(BaseUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel recupero dei dati della dashboard: {ex.Message}");
                return null;
            }
        }
    }
}
