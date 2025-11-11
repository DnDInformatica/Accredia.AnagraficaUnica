using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;
using System.Net.Http.Json;

namespace Accredia.GestioneAnagrafica.Web.Services;

/// <summary>
/// Implementazione del servizio per la gestione dei telefoni
/// </summary>
public class TelefoniService : ITelefoniService
{
    private readonly IApiHttpClient _httpClient;
    private readonly ILogger<TelefoniService> _logger;

    public TelefoniService(IApiHttpClient httpClient, ILogger<TelefoniService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<PageResult<TelefonoDTO.ListItem>>> GetTelefoniByEntitaAsync(int entitaAziendaleId, int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Recupero telefoni per EntitaAziendaleId: {EntitaId}, Page: {Page}, PageSize: {PageSize}",
                entitaAziendaleId, page, pageSize);

            var response = await _httpClient.GetFromJsonAsync<PageResult<TelefonoDTO.ListItem>>(
                $"api/telefoni/entita/{entitaAziendaleId}?page={page}&pageSize={pageSize}");

            if (response != null)
            {
                return new ApiResponse<PageResult<TelefonoDTO.ListItem>>
                {
                    Success = true,
                    Data = response
                };
            }

            return new ApiResponse<PageResult<TelefonoDTO.ListItem>>
            {
                Success = false,
                Message = "Nessun dato ricevuto dall'API"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei telefoni per EntitaAziendaleId: {EntitaId}", entitaAziendaleId);
            return new ApiResponse<PageResult<TelefonoDTO.ListItem>>
            {
                Success = false,
                Message = $"Errore durante il recupero dei telefoni: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<TelefonoDTO.Response>> GetTelefonoByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Recupero dettaglio telefono con ID: {TelefonoId}", id);

            var response = await _httpClient.GetFromJsonAsync<TelefonoDTO.Response>($"api/telefoni/{id}");

            if (response != null)
            {
                return new ApiResponse<TelefonoDTO.Response>
                {
                    Success = true,
                    Data = response
                };
            }

            return new ApiResponse<TelefonoDTO.Response>
            {
                Success = false,
                Message = "Telefono non trovato"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del telefono con ID: {TelefonoId}", id);
            return new ApiResponse<TelefonoDTO.Response>
            {
                Success = false,
                Message = $"Errore durante il recupero del telefono: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<TelefonoDTO.Response>> CreateTelefonoAsync(TelefonoDTO.Create request)
    {
        try
        {
            _logger.LogInformation("Creazione nuovo telefono per EntitaAziendaleId: {EntitaId}", request.EntitaAziendaleId);

            var response = await _httpClient.PostAsJsonAsync("api/telefoni", request);

            if (response.IsSuccessStatusCode)
            {
                var createdTelefono = await response.Content.ReadFromJsonAsync<TelefonoDTO.Response>();
                return new ApiResponse<TelefonoDTO.Response>
                {
                    Success = true,
                    Data = createdTelefono,
                    Message = "Telefono creato con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nella creazione del telefono. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<TelefonoDTO.Response>
            {
                Success = false,
                Message = $"Errore nella creazione del telefono: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione del telefono");
            return new ApiResponse<TelefonoDTO.Response>
            {
                Success = false,
                Message = $"Errore durante la creazione del telefono: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<TelefonoDTO.Response>> UpdateTelefonoAsync(int id, TelefonoDTO.Update request)
    {
        try
        {
            _logger.LogInformation("Aggiornamento telefono con ID: {TelefonoId}", id);

            var response = await _httpClient.PutAsJsonAsync($"api/telefoni/{id}", request);

            if (response.IsSuccessStatusCode)
            {
                var updatedTelefono = await response.Content.ReadFromJsonAsync<TelefonoDTO.Response>();
                return new ApiResponse<TelefonoDTO.Response>
                {
                    Success = true,
                    Data = updatedTelefono,
                    Message = "Telefono aggiornato con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nell'aggiornamento del telefono. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<TelefonoDTO.Response>
            {
                Success = false,
                Message = $"Errore nell'aggiornamento del telefono: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiornamento del telefono con ID: {TelefonoId}", id);
            return new ApiResponse<TelefonoDTO.Response>
            {
                Success = false,
                Message = $"Errore durante l'aggiornamento del telefono: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<bool>> DeleteTelefonoAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminazione telefono con ID: {TelefonoId}", id);

            var response = await _httpClient.DeleteAsync($"api/telefoni/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Telefono eliminato con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nell'eliminazione del telefono. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = $"Errore nell'eliminazione del telefono: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione del telefono con ID: {TelefonoId}", id);
            return new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = $"Errore durante l'eliminazione del telefono: {ex.Message}"
            };
        }
    }
}