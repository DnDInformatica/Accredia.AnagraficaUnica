using Accredia.GestioneAnagrafica.API.DTOs;
using Accredia.GestioneAnagrafica.Shared.DTOs;
using Accredia.GestioneAnagrafica.Shared.Models;

namespace Accredia.GestioneAnagrafica.Web.Services;

/// <summary>
/// Interfaccia per il servizio di gestione delle email
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Ottiene la lista delle email per un'entità aziendale
    /// </summary>
    /// <param name="entitaAziendaleId">ID dell'entità aziendale</param>
    /// <param name="page">Numero di pagina</param>
    /// <param name="pageSize">Dimensione della pagina</param>
    /// <returns>Lista paginata di email</returns>
    Task<ApiResponse<PageResult<EmailDTO.ListItem>>> GetEmailByEntitaAsync(int entitaAziendaleId, int page = 1, int pageSize = 10);

    /// <summary>
    /// Ottiene il dettaglio di un'email specifica
    /// </summary>
    /// <param name="id">ID dell'email</param>
    /// <returns>Dettaglio dell'email</returns>
    Task<ApiResponse<EmailDTO.Response>> GetEmailByIdAsync(int id);

    /// <summary>
    /// Crea una nuova email
    /// </summary>
    /// <param name="request">Dati della nuova email</param>
    /// <returns>Email creata</returns>
    Task<ApiResponse<EmailDTO.Response>> CreateEmailAsync(EmailDTO.Create request);

    /// <summary>
    /// Aggiorna un'email esistente
    /// </summary>
    /// <param name="id">ID dell'email</param>
    /// <param name="request">Dati aggiornati</param>
    /// <returns>Email aggiornata</returns>
    Task<ApiResponse<EmailDTO.Response>> UpdateEmailAsync(int id, EmailDTO.Update request);

    /// <summary>
    /// Elimina un'email (soft delete)
    /// </summary>
    /// <param name="id">ID dell'email</param>
    /// <returns>Risultato dell'operazione</returns>
    Task<ApiResponse<bool>> DeleteEmailAsync(int id);

    /// <summary>
    /// Verifica se un'email è duplicata per un'entità
    /// </summary>
    /// <param name="email">Indirizzo email da verificare</param>
    /// <param name="entitaId">ID dell'entità</param>
    /// <param name="excludeId">ID email da escludere (per update)</param>
    /// <returns>True se l'email è duplicata</returns>
    Task<ApiResponse<bool>> CheckDuplicateEmailAsync(string email, int entitaId, int? excludeId = null);
}