using Accredia.GestioneAnagrafica.Shared.Responses;
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

            var response = await _httpClient.GetAsync<PageResult<TelefonoDTO.ListItem>>(
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

            var response = await _httpClient.GetAsync<TelefonoDTO.Response>($"api/telefoni/{id}");

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

            var response = await _httpClient.PostAsync<TelefonoDTO.Response>("api/telefoni", request);

            if (response != null)
            {
                return new ApiResponse<TelefonoDTO.Response>
                {
                    Success = true,
                    Data = response,
                    Message = "Telefono creato con successo"
                };
            }

            return new ApiResponse<TelefonoDTO.Response>
            {
                Success = false,
                Message = "Nessun dato ricevuto dall'API"
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

            var response = await _httpClient.PutAsync<TelefonoDTO.Response>($"api/telefoni/{id}", request);

            if (response != null)
            {
                return new ApiResponse<TelefonoDTO.Response>
                {
                    Success = true,
                    Data = response,
                    Message = "Telefono aggiornato con successo"
                };
            }

            return new ApiResponse<TelefonoDTO.Response>
            {
                Success = false,
                Message = "Nessun dato ricevuto dall'API"
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

            await _httpClient.DeleteAsync($"api/telefoni/{id}");

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "Telefono eliminato con successo"
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