using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.Shared.Responses
{
    /// <summary>
    /// Risultato paginato standardizzato
    /// </summary>
    public class PageResult<T>
    {
        [JsonPropertyName("data")]
        public List<T> Data { get; set; } = new();

        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages => PageSize > 0 ? (TotalRecords + PageSize - 1) / PageSize : 1;

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage => PageNumber < TotalPages;

        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage => PageNumber > 1;

        public PageResult() { }

        public PageResult(List<T> data, int totalRecords, int pageNumber, int pageSize)
        {
            Data = data;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        /// <summary>
        /// Factory per creazione risultato non paginato (tutti i record)
        /// </summary>
        public static PageResult<T> AllRecords(List<T> data)
            => new(data, data.Count, 1, data.Count);

        /// <summary>
        /// Factory per risultato vuoto
        /// </summary>
        public static PageResult<T> Empty()
            => new(new List<T>(), 0, 1, 20);
    }
}
