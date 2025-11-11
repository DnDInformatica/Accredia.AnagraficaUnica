using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.Shared.Responses
{
    /// <summary>
    /// Wrapper standard per tutte le risposte API.
    /// Compatibile con OpenAPI/Swagger.
    /// </summary>
    /// <typeparam name="T">Tipo del payload restituito.</typeparam>
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>>? Errors { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        [JsonPropertyName("correlationId")]
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString("D");

        /// <summary>
        /// Crea una risposta di successo.
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T? data = default, string? message = "Operazione completata con successo", string? correlationId = null)
            => new()
            {
                Success = true,
                Data = data,
                Message = message,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString("D")
            };

        /// <summary>
        /// Crea una risposta di errore generica.
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, Dictionary<string, List<string>>? errors = null, string? correlationId = null)
            => new()
            {
                Success = false,
                Data = default,
                Message = message,
                Errors = errors,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString("D")
            };

        /// <summary>
        /// Crea una risposta di errore di validazione.
        /// </summary>
        public static ApiResponse<T> ValidationErrorResponse(Dictionary<string, List<string>> errors, string? correlationId = null)
            => new()
            {
                Success = false,
                Data = default,
                Message = "Errori di validazione",
                Errors = errors,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString("D")
            };
    }

    /// <summary>
    /// Wrapper standard per risposte API senza payload (es. POST/DELETE).
    /// </summary>
    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>>? Errors { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        [JsonPropertyName("correlationId")]
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString("D");

        public static ApiResponse SuccessResponse(string? message = "Operazione completata con successo", string? correlationId = null)
            => new()
            {
                Success = true,
                Message = message,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString("D")
            };

        public static ApiResponse ErrorResponse(string message, Dictionary<string, List<string>>? errors = null, string? correlationId = null)
            => new()
            {
                Success = false,
                Message = message,
                Errors = errors,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString("D")
            };
    }
}
