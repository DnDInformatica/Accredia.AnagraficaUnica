namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Implementazione del servizio degli Organismi
    /// </summary>
    public class OrganismiService : IOrganismiService
    {
        private readonly IApiHttpClient _apiHttpClient;
        private readonly ILogger<OrganismiService> _logger;
        private const string BaseUrl = "/api/organismi";

        public OrganismiService(IApiHttpClient apiHttpClient, ILogger<OrganismiService> logger)
        {
            _apiHttpClient = apiHttpClient;
            _logger = logger;
        }

        public async Task<List<object>> GetOrganismiAsync()
        {
            try
            {
                _logger.LogInformation("Recupero degli organismi in corso...");
                var result = await _apiHttpClient.GetAsync<List<object>>(BaseUrl);
                return result ?? new List<object>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel recupero degli organismi: {ex.Message}");
                return new List<object>();
            }
        }

        public async Task<object?> GetOrganismoByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Recupero dell'organismo con ID: {id}");
                return await _apiHttpClient.GetAsync<object>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel recupero dell'organismo: {ex.Message}");
                return null;
            }
        }
    }
}
