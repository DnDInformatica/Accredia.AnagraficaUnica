using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Accredia.GestioneAnagrafica.Shared.Auth
{
    /// <summary>
    /// Provider di autenticazione che gestisce JWT Token per Blazor WebAssembly
    /// </summary>
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILogger<JwtAuthenticationStateProvider> _logger;
        private readonly IJSRuntime _jsRuntime;
        private const string TOKEN_KEY = "authToken";

        public JwtAuthenticationStateProvider(
            ILogger<JwtAuthenticationStateProvider> logger,
            IJSRuntime jsRuntime)
        {
            _logger = logger;
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Recupera lo stato di autenticazione corrente
        /// </summary>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Recuperare il token
                var token = await GetTokenAsync();

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogInformation("Nessun token trovato");
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                // Verificare se il token è scaduto
                if (IsTokenExpired(token))
                {
                    _logger.LogWarning("Token scaduto");
                    await ClearTokenAsync();
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var principal = ParseToken(token);
                return new AuthenticationState(principal);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel recupero dello stato di autenticazione: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        /// <summary>
        /// Marca l'utente come autenticato
        /// </summary>
        public async Task MarkUserAsAuthenticated(string token)
        {
            await SetTokenAsync(token);
            var principal = ParseToken(token);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        /// <summary>
        /// Marca l'utente come non autenticato
        /// </summary>
        public async Task MarkUserAsLoggedOut()
        {
            await ClearTokenAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }

        /// <summary>
        /// Salva il token in localStorage
        /// </summary>
        public async Task SetTokenAsync(string token)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
                _logger.LogInformation("Token salvato in localStorage");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel salvataggio del token: {ex.Message}");
            }
        }

        /// <summary>
        /// Recupera il token da localStorage
        /// </summary>
        public async Task<string?> GetTokenAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel recupero del token: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Rimuove il token da localStorage
        /// </summary>
        public async Task ClearTokenAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
                _logger.LogInformation("Token rimosso da localStorage");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nella rimozione del token: {ex.Message}");
            }
        }

        /// <summary>
        /// Parsa il token JWT e estrae i claims
        /// </summary>
        private ClaimsPrincipal ParseToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToList();

                _logger.LogInformation($"Token parsato con {claims.Count} claims");

                var identity = new ClaimsIdentity(claims, "jwt");
                return new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel parsing del token JWT: {ex.Message}");
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
        }

        /// <summary>
        /// Verifica se il token è scaduto
        /// </summary>
        private bool IsTokenExpired(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                
                return jwtToken.ValidTo < DateTime.UtcNow;
            }
            catch
            {
                return true;
            }
        }
    }
}

