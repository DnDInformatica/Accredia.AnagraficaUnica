using Accredia.GestioneAnagrafica.Shared.DTOs;

namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Implementazione del servizio delle Persone
    /// </summary>
    public class PersoneService : IPersoneService
    {
        private readonly IApiHttpClient _apiHttpClient;
        private readonly ILogger<PersoneService> _logger;
        private const string BaseUrl = "/api/persone";

        public PersoneService(IApiHttpClient apiHttpClient, ILogger<PersoneService> logger)
        {
            _apiHttpClient = apiHttpClient;
            _logger = logger;
        }

        public async Task<List<PersonaGridItemResponse>?> GetAllPersoneAsync()
        {
            try
            {
                _logger.LogInformation("Recupero di tutte le persone per DataGrid");
                return await _apiHttpClient.GetAsync<List<PersonaGridItemResponse>>($"{BaseUrl}/all");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero di tutte le persone");
                return null;
            }
        }

        public async Task<PersoneListResponse?> GetPersoneAsync(int page = 1, int pageSize = 10, string? search = null)
        {
            try
            {
                _logger.LogInformation("Recupero delle persone - Pagina: {Page}, Dimensione: {PageSize}", page, pageSize);

                var queryParams = new List<string>
                {
                    $"page={page}",
                    $"pageSize={pageSize}"
                };

                if (!string.IsNullOrWhiteSpace(search))
                {
                    queryParams.Add($"search={Uri.EscapeDataString(search)}");
                }

                var url = $"{BaseUrl}?{string.Join("&", queryParams)}";
                var result = await _apiHttpClient.GetAsync<PersoneListResponse>(url);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero delle persone");
                return null;
            }
        }

        public async Task<PersonaResponse?> GetPersonaByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Recupero della persona con ID: {Id}", id);
                return await _apiHttpClient.GetAsync<PersonaResponse>($"{BaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero della persona con ID: {Id}", id);
                return null;
            }
        }

        public async Task<PersonaResponse?> CreatePersonaAsync(CreatePersonaRequest request)
        {
            try
            {
                _logger.LogInformation("Creazione nuova persona: {Nome} {Cognome}", request.Nome, request.Cognome);
                return await _apiHttpClient.PostAsync<PersonaResponse>(BaseUrl, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nella creazione della persona");
                return null;
            }
        }

        public async Task<PersonaResponse?> UpdatePersonaAsync(int id, UpdatePersonaRequest request)
        {
            try
            {
                _logger.LogInformation("Aggiornamento persona ID: {Id}", id);
                return await _apiHttpClient.PutAsync<PersonaResponse>($"{BaseUrl}/{id}", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'aggiornamento della persona con ID: {Id}", id);
                return null;
            }
        }

        public async Task<bool> DeletePersonaAsync(int id)
        {
            try
            {
                _logger.LogInformation("Eliminazione persona ID: {Id}", id);
                await _apiHttpClient.DeleteAsync($"{BaseUrl}/{id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nell'eliminazione della persona con ID: {Id}", id);
                return false;
            }
        }
    }
}
