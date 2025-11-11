using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;
using System.Net.Http.Json;

namespace Accredia.GestioneAnagrafica.Web.Services;

/// <summary>
/// Implementazione del servizio per la gestione delle email
/// </summary>
public class EmailService : IEmailService
{
    private readonly IApiHttpClient _httpClient;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IApiHttpClient httpClient, ILogger<EmailService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<PageResult<EmailDTO.ListItem>>> GetEmailByEntitaAsync(int entitaAziendaleId, int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Recupero email per EntitaAziendaleId: {EntitaId}, Page: {Page}, PageSize: {PageSize}",
                entitaAziendaleId, page, pageSize);

            var response = await _httpClient.GetFromJsonAsync<PageResult<EmailDTO.ListItem>>(
                $"api/email/entita/{entitaAziendaleId}?page={page}&pageSize={pageSize}");

            if (response != null)
            {
                return new ApiResponse<PageResult<EmailDTO.ListItem>>
                {
                    Success = true,
                    Data = response
                };
            }

            return new ApiResponse<PageResult<EmailDTO.ListItem>>
            {
                Success = false,
                Message = "Nessun dato ricevuto dall'API"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero delle email per EntitaAziendaleId: {EntitaId}", entitaAziendaleId);
            return new ApiResponse<PageResult<EmailDTO.ListItem>>
            {
                Success = false,
                Message = $"Errore durante il recupero delle email: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<EmailDTO.Response>> GetEmailByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Recupero dettaglio email con ID: {EmailId}", id);

            var response = await _httpClient.GetFromJsonAsync<EmailDTO.Response>($"api/email/{id}");

            if (response != null)
            {
                return new ApiResponse<EmailDTO.Response>
                {
                    Success = true,
                    Data = response
                };
            }

            return new ApiResponse<EmailDTO.Response>
            {
                Success = false,
                Message = "Email non trovata"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dell'email con ID: {EmailId}", id);
            return new ApiResponse<EmailDTO.Response>
            {
                Success = false,
                Message = $"Errore durante il recupero dell'email: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<EmailDTO.Response>> CreateEmailAsync(EmailDTO.Create request)
    {
        try
        {
            _logger.LogInformation("Creazione nuova email per EntitaAziendaleId: {EntitaId}", request.EntitaAziendaleId);

            var response = await _httpClient.PostAsJsonAsync("api/email", request);

            if (response.IsSuccessStatusCode)
            {
                var createdEmail = await response.Content.ReadFromJsonAsync<EmailDTO.Response>();
                return new ApiResponse<EmailDTO.Response>
                {
                    Success = true,
                    Data = createdEmail,
                    Message = "Email creata con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nella creazione dell'email. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<EmailDTO.Response>
            {
                Success = false,
                Message = $"Errore nella creazione dell'email: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione dell'email");
            return new ApiResponse<EmailDTO.Response>
            {
                Success = false,
                Message = $"Errore durante la creazione dell'email: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<EmailDTO.Response>> UpdateEmailAsync(int id, EmailDTO.Update request)
    {
        try
        {
            _logger.LogInformation("Aggiornamento email con ID: {EmailId}", id);

            var response = await _httpClient.PutAsJsonAsync($"api/email/{id}", request);

            if (response.IsSuccessStatusCode)
            {
                var updatedEmail = await response.Content.ReadFromJsonAsync<EmailDTO.Response>();
                return new ApiResponse<EmailDTO.Response>
                {
                    Success = true,
                    Data = updatedEmail,
                    Message = "Email aggiornata con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nell'aggiornamento dell'email. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<EmailDTO.Response>
            {
                Success = false,
                Message = $"Errore nell'aggiornamento dell'email: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiornamento dell'email con ID: {EmailId}", id);
            return new ApiResponse<EmailDTO.Response>
            {
                Success = false,
                Message = $"Errore durante l'aggiornamento dell'email: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<bool>> DeleteEmailAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminazione email con ID: {EmailId}", id);

            var response = await _httpClient.DeleteAsync($"api/email/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Email eliminata con successo"
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Errore nell'eliminazione dell'email. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);

            return new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = $"Errore nell'eliminazione dell'email: {errorContent}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione dell'email con ID: {EmailId}", id);
            return new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = $"Errore durante l'eliminazione dell'email: {ex.Message}"
            };
        }
    }

    /// <inheritdoc/>
    public async Task<ApiResponse<bool>> CheckDuplicateEmailAsync(string email, int entitaId, int? excludeId = null)
    {
        try
        {
            _logger.LogInformation("Verifica duplicato email: {Email} per EntitaId: {EntitaId}", email, entitaId);

            var queryString = $"api/email/check-duplicate?email={Uri.EscapeDataString(email)}&entitaId={entitaId}";
            if (excludeId.HasValue)
            {
                queryString += $"&excludeId={excludeId.Value}";
            }

            var response = await _httpClient.GetFromJsonAsync<bool>(queryString);

            return new ApiResponse<bool>
            {
                Success = true,
                Data = response
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la verifica duplicato email: {Email}", email);
            return new ApiResponse<bool>
            {
                Success = false,
                Data = false,
                Message = $"Errore durante la verifica: {ex.Message}"
            };
        }
    }
}