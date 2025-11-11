using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;
using System.Net.Http.Json;

namespace Accredia.GestioneAnagrafica.Web.Services;

/// <summary>
/// Implementazione del servizio per la gestione degli indirizzi
/// </summary>
public class IndirizziService : IIndirizziService
{
    private readonly IApiHttpClient _httpClient;
    private readonly ILogger<IndirizziService> _logger;

    public IndirizziService(IApiHttpClient httpClient, ILogger<IndirizziService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<PageResult<IndirizzoDTO.List>>> GetIndirizziByEntitaAsync(int entitaAziendaleId, int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Recupero indirizzi per EntitaAziendaleId: {EntitaId}, Page: {Page}, PageSize: {PageSize}",
                entitaAziendaleId, page, pageSize);

            var response = await _httpClient.GetFromJsonAsync<PageResult<IndirizzoDTO.List>>(
                $"api/indirizzi/entita/{entitaAziendaleId}?page={page}&pageSize={pageSize}");

            if (response != null)
            {
                return new ApiResponse<PageResult<IndirizzoDTO.List>>
                {
                    Success = true,
                    Data = response
                };
            }

            return new ApiResponse<PageResult<IndirizzoDTO.List>>
            {
                Success = false,
                Message = "Nessun dato ricevuto dall'API"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero degli indirizzi per EntitaAziendaleId: {EntitaId}", entitaAziendaleId);
            return new ApiResponse<PageResult<IndirizzoDTO.List>>
            {
                Success = false,
                Message = $"Errore durante il recupero degli indirizzi: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<IndirizzoDTO.Response>> GetIndirizzoByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Recupero dettaglio indirizzo con ID: {IndirizzoId}", id);

            var response = await _httpClient.GetFromJsonAsync<IndirizzoDTO.Response>($"api/indirizzi/{id}");

            if (response != null)
            {
                return new ApiResponse<IndirizzoDTO.Response>
                {
                    Success = true,
                    Data = response
                };
            }

            return new ApiResponse<IndirizzoDTO.Response>
            {
                Success = false,
                Message = "Indirizzo non trovato"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dell'indirizzo con ID: {IndirizzoId}", id);
            return new ApiResponse<IndirizzoDTO.Response>
            {
                Success = false,
                Message = $"Errore durante il recupero dell'indirizzo: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<IndirizzoDTO.Response>> CreateIndirizzoAsync(IndirizzoDTO.Create request)
    {
        try
        {
            _logger.LogInformation("Creazione nuovo indirizzo per EntitaAziendaleId: {EntitaId}", request.ComuneId);

            var response = await _httpClient.PostAsJsonAsync("api/indirizzi", request);

            if (response.IsSuccessStatusCode)
            {
                var createdIndirizzo = await response.Content.ReadFromJsonAsync<IndirizzoDTO.Response>();
                return new ApiResponse<IndirizzoDTO.Response>
                {
                    Success = true,
                    Data = createdIndirizzo,
                    Message = "Indirizzo creato con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nella creazione dell'indirizzo. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<IndirizzoDTO.Response>
            {
                Success = false,
                Message = $"Errore nella creazione dell'indirizzo: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione dell'indirizzo");
            return new ApiResponse<IndirizzoDTO.Response>
            {
                Success = false,
                Message = $"Errore durante la creazione dell'indirizzo: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<IndirizzoDTO.Response>> UpdateIndirizzoAsync(int id, IndirizzoDTO.Update request)
    {
        try
        {
            _logger.LogInformation("Aggiornamento indirizzo con ID: {IndirizzoId}", id);

            var response = await _httpClient.PutAsJsonAsync($"api/indirizzi/{id}", request);

            if (response.IsSuccessStatusCode)
            {
                var updatedIndirizzo = await response.Content.ReadFromJsonAsync<IndirizzoDTO.Response>();
                return new ApiResponse<IndirizzoDTO.Response>
                {
                    Success = true,
                    Data = updatedIndirizzo,
                    Message = "Indirizzo aggiornato con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nell'aggiornamento dell'indirizzo. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<IndirizzoDTO.Response>
            {
                Success = false,
                Message = $"Errore nell'aggiornamento dell'indirizzo: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiornamento dell'indirizzo con ID: {IndirizzoId}", id);
            return new ApiResponse<IndirizzoDTO.Response>
            {
                Success = false,
                Message = $"Errore durante l'aggiornamento dell'indirizzo: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<bool>> DeleteIndirizzoAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminazione indirizzo con ID: {IndirizzoId}", id);

            var response = await _httpClient.DeleteAsync($"api/indirizzi/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Indirizzo eliminato con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nell'eliminazione dell'indirizzo. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = $"Errore nell'eliminazione dell'indirizzo: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione dell'indirizzo con ID: {IndirizzoId}", id);
            return new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = $"Errore durante l'eliminazione dell'indirizzo: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<List<IndirizzoDTO.List>>> SearchIndirizziAsync(string query)
    {
        try
        {
            _logger.LogInformation("Ricerca indirizzi con query: {Query}", query);

            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            {
                return new ApiResponse<List<IndirizzoDTO.List>>
                {
                    Success = true,
                    Data = new List<IndirizzoDTO.List>(),
                    Message = "Query troppo corta, minimo 3 caratteri"
                };
            }

            var response = await _httpClient.GetFromJsonAsync<List<IndirizzoDTO.List>>(
                $"api/indirizzi/search?q={Uri.EscapeDataString(query)}");

            return new ApiResponse<List<IndirizzoDTO.List>>
            {
                Success = true,
                Data = response ?? new List<IndirizzoDTO.List>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la ricerca indirizzi con query: {Query}", query);
            return new ApiResponse<List<IndirizzoDTO.List>>
            {
                Success = false,
                Data = new List<IndirizzoDTO.List>(),
                Message = $"Errore durante la ricerca: {ex.Message}"
            };
        }
    }
}