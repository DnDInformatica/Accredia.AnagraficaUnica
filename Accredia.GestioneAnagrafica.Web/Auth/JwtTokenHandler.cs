namespace Accredia.GestioneAnagrafica.Web.Auth
{
    /// <summary>
    /// Gestore personalizzato dei token JWT per le richieste HTTP
    /// </summary>
    public class JwtTokenHandler : DelegatingHandler
    {
        private readonly ILogger<JwtTokenHandler> _logger;

        public JwtTokenHandler(ILogger<JwtTokenHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: Recuperare il token e aggiungerlo all'header Authorization
                // var token = await GetTokenAsync();
                // if (!string.IsNullOrEmpty(token))
                // {
                //     request.Headers.Authorization = 
                //         new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                // }

                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel gestore del token JWT: {ex.Message}");
                throw;
            }
        }

        // private async Task<string?> GetTokenAsync()
        // {
        //     // TODO: Implementare il recupero del token
        //     return null;
        // }
    }
}
