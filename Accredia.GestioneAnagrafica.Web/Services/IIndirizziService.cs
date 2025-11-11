using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;

namespace Accredia.GestioneAnagrafica.Web.Services;

/// <summary>
/// Interfaccia per il servizio di gestione degli indirizzi
/// </summary>
public interface IIndirizziService
{
    /// <summary>
    /// Ottiene la lista degli indirizzi per un'entità aziendale
    /// </summary>
    /// <param name="entitaAziendaleId">ID dell'entità aziendale</param>
    /// <param name="page">Numero di pagina</param>
    /// <param name="pageSize">Dimensione della pagina</param>
    /// <returns>Lista paginata di indirizzi</returns>
    Task<ApiResponse<PageResult<IndirizzoDTO.List>>> GetIndirizziByEntitaAsync(int entitaAziendaleId, int page = 1, int pageSize = 10);

    /// <summary>
    /// Ottiene il dettaglio di un indirizzo specifico
    /// </summary>
    /// <param name="id">ID dell'indirizzo</param>
    /// <returns>Dettaglio dell'indirizzo</returns>
    Task<ApiResponse<IndirizzoDTO.Response>> GetIndirizzoByIdAsync(int id);

    /// <summary>
    /// Crea un nuovo indirizzo
    /// </summary>
    /// <param name="request">Dati del nuovo indirizzo</param>
    /// <returns>Indirizzo creato</returns>
    Task<ApiResponse<IndirizzoDTO.Response>> CreateIndirizzoAsync(IndirizzoDTO.Create request);

    /// <summary>
    /// Aggiorna un indirizzo esistente
    /// </summary>
    /// <param name="id">ID dell'indirizzo</param>
    /// <param name="request">Dati aggiornati</param>
    /// <returns>Indirizzo aggiornato</returns>
    Task<ApiResponse<IndirizzoDTO.Response>> UpdateIndirizzoAsync(int id, IndirizzoDTO.Update request);

    /// <summary>
    /// Elimina un indirizzo (soft delete)
    /// </summary>
    /// <param name="id">ID dell'indirizzo</param>
    /// <returns>Risultato dell'operazione</returns>
    Task<ApiResponse<bool>> DeleteIndirizzoAsync(int id);

    /// <summary>
    /// Cerca indirizzi per autocomplete
    /// </summary>
    /// <param name="query">Stringa di ricerca</param>
    /// <returns>Lista di indirizzi trovati</returns>
    Task<ApiResponse<List<IndirizzoDTO.List>>> SearchIndirizziAsync(string query);
}
