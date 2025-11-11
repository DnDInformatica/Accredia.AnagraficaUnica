using System.Net.Http.Json;

namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Implementazione del servizio di autenticazione con JWT
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly HttpClient _httpClient;

        public AuthService(
            ILogger<AuthService> logger,
            HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _logger.LogInformation($"[AUTHSERVICE] HttpClient BaseAddress: {httpClient.BaseAddress}");
        }

        /// <summary>
        /// Esegue il login con username e password e ritorna il token
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                _logger.LogInformation($"Tentativo di login per l'utente: {username}");

                var request = new { username, password };

                // Chiama l'API di login
                var response = await _httpClient.PostAsJsonAsync(
                    "/auth/login",
                    request
                );

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Login fallito con status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Risposta errore: {errorContent}");
                    return false;
                }

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (loginResponse?.Success == true && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    // Salva il token per uso successivo
                    _logger.LogInformation($"Login riuscito per {username}, token ricevuto");
                    
                    // Salva il token in SessionStorage o in una proprietà statica
                    // (verrà usato da Login.razor per aggiornare lo stato)
                    SessionToken = loginResponse.Token;
                    
                    return true;
                }

                _logger.LogWarning("Risposta di login non valida");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel login: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Proprietà statica per salvare il token durante il login
        /// </summary>
        public static string? SessionToken { get; set; }

        /// <summary>
        /// Esegue il logout
        /// </summary>
        public async Task LogoutAsync()
        {
            try
            {
                _logger.LogInformation("Logout completato");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel logout: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se l'utente è autenticato
        /// </summary>
        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                // Metodo stub - la vera logica è nel JwtAuthenticationStateProvider lato Server
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nella verifica dell'autenticazione: {ex.Message}");
                return false;
            }
        }
    }

    /// <summary>
    /// Response del login con token JWT
    /// </summary>
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
