namespace Accredia.GestioneAnagrafica.Server.Middleware
{
    /// <summary>
    /// Middleware per il logging delle richieste
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation(
                $"Richiesta HTTP: {context.Request.Method} {context.Request.Path}");

            await _next(context);

            _logger.LogInformation(
                $"Risposta HTTP: {context.Response.StatusCode}");
        }
    }
}
