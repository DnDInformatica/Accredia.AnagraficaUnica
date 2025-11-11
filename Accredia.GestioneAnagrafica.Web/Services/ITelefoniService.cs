using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;

namespace Accredia.GestioneAnagrafica.Web.Services;

/// <summary>
/// Interfaccia per il servizio di gestione dei telefoni
/// </summary>
public interface ITelefoniService
{
    /// <summary>
    /// Ottiene la lista dei telefoni per un'entità aziendale
    /// </summary>
    /// <param name="entitaAziendaleId">ID dell'entità aziendale</param>
    /// <param name="page">Numero di pagina</param>
    /// <param name="pageSize">Dimensione della pagina</param>
    /// <returns>Lista paginata di telefoni</returns>
    Task<ApiResponse<PageResult<TelefonoDTO.ListItem>>> GetTelefoniByEntitaAsync(int entitaAziendaleId, int page = 1, int pageSize = 10);

    /// <summary>
    /// Ottiene il dettaglio di un telefono specifico
    /// </summary>
    /// <param name="id">ID del telefono</param>
    /// <returns>Dettaglio del telefono</returns>
    Task<ApiResponse<TelefonoDTO.Response>> GetTelefonoByIdAsync(int id);

    /// <summary>
    /// Crea un nuovo telefono
    /// </summary>
    /// <param name="request">Dati del nuovo telefono</param>
    /// <returns>Telefono creato</returns>
    Task<ApiResponse<TelefonoDTO.Response>> CreateTelefonoAsync(TelefonoDTO.Create request);

    /// <summary>
    /// Aggiorna un telefono esistente
    /// </summary>
    /// <param name="id">ID del telefono</param>
    /// <param name="request">Dati aggiornati</param>
    /// <returns>Telefono aggiornato</returns>
    Task<ApiResponse<TelefonoDTO.Response>> UpdateTelefonoAsync(int id, TelefonoDTO.Update request);

    /// <summary>
    /// Elimina un telefono (soft delete)
    /// </summary>
    /// <param name="id">ID del telefono</param>
    /// <returns>Risultato dell'operazione</returns>
    Task<ApiResponse<bool>> DeleteTelefonoAsync(int id);
}