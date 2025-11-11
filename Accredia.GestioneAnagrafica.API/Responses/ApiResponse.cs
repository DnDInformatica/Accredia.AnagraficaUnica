// This file is now deprecated. ApiResponse has been moved to Accredia.GestioneAnagrafica.Shared.Responses
// This file is kept as a compatibility shim

namespace Accredia.GestioneAnagrafica.API.Responses
{
    /// <summary>
    /// Alias for ApiResponse in Shared project - for backward compatibility
    /// </summary>
    public class ApiResponse<T> : Accredia.GestioneAnagrafica.Shared.Responses.ApiResponse<T>
    {
    }

    /// <summary>
    /// Alias for ApiResponse in Shared project - for backward compatibility
    /// </summary>
    public class ApiResponse : Accredia.GestioneAnagrafica.Shared.Responses.ApiResponse
    {
    }
}
