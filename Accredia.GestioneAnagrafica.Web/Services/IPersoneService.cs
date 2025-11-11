using Accredia.GestioneAnagrafica.Shared.DTOs;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.Web.Services
{
    /// <summary>
    /// Interfaccia per il servizio delle Persone
    /// </summary>
    public interface IPersoneService
    {
        /// <summary>
        /// Recupera TUTTE le persone (per MudDataGrid client-side)
        /// </summary>
        Task<List<PersonaGridItemResponse>?> GetAllPersoneAsync();

        /// <summary>
        /// Recupera l'elenco delle persone con paginazione
        /// </summary>
        Task<PersoneListResponse?> GetPersoneAsync(int page = 1, int pageSize = 10, string? search = null);

        /// <summary>
        /// Recupera una persona per ID
        /// </summary>
        Task<PersonaResponse?> GetPersonaByIdAsync(int id);

        /// <summary>
        /// Crea una nuova persona
        /// </summary>
        Task<PersonaResponse?> CreatePersonaAsync(CreatePersonaRequest request);

        /// <summary>
        /// Aggiorna una persona esistente
        /// </summary>
        Task<PersonaResponse?> UpdatePersonaAsync(int id, UpdatePersonaRequest request);

        /// <summary>
        /// Elimina una persona
        /// </summary>
        Task<bool> DeletePersonaAsync(int id);
    }

    /// <summary>
    /// Response paginata per la lista persone
    /// </summary>
    public class PersoneListResponse
    {
        [JsonPropertyName("data")]
        public List<PersonaListItemResponse> Data { get; set; } = new();

        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonPropertyName("pageNumber")]
        public int Page { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
    }
}
