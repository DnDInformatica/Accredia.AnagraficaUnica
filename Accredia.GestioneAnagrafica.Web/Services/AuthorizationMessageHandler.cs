using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Handler che aggiunge automaticamente il token JWT all'header Authorization
    /// </summary>
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private const string TOKEN_KEY = "accredia_jwt_token";

        public AuthorizationMessageHandler(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Legge il token direttamente da localStorage
                var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);

                // Aggiunge il token all'header Authorization se presente
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch
            {
                // Se c'è un errore nella lettura del token, continua senza autorizzazione
            }

            // Procede con la richiesta
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
