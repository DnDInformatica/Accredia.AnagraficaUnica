using Microsoft.AspNetCore.Http;
using System.Net;

namespace Accredia.GestioneAnagrafica.Server.Middleware
{
    /// <summary>
    /// Middleware per la gestione globale delle eccezioni
    /// </summary>
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eccezione non gestita");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                error = exception.Message,
                statusCode = context.Response.StatusCode
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
