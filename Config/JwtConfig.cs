using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Accredia.GestioneAnagrafica.API.Config
{
    /// <summary>
    /// Configurazione JWT da appsettings.json
    /// </summary>
    public class JwtConfig
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
    }

    /// <summary>
    /// DTO per le credenziali di login
    /// </summary>
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// DTO per la risposta di login
    /// </summary>
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("utente")]
        public UtenteJwtResponse? Utente { get; set; }
    }

    /// <summary>
    /// DTO per le informazioni utente nel JWT
    /// </summary>
    public class UtenteJwtResponse
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("nomeCompleto")]
        public string? NomeCompleto { get; set; }

        [JsonPropertyName("ruoli")]
        public List<string> Ruoli { get; set; } = new();

        [JsonPropertyName("permissions")]
        public List<string> Permissions { get; set; } = new();

        [JsonPropertyName("entitaAziendaleId")]
        public int? EntitaAziendaleId { get; set; }
    }

    /// <summary>
    /// Claims personalizzati per JWT
    /// </summary>
    public static class CustomClaimTypes
    {
        /// <summary>
        /// Identificativo univoco dell'utente
        /// </summary>
        public const string UserId = "userId";

        /// <summary>
        /// Nome completo dell'utente
        /// </summary>
        public const string FullName = "fullName";

        /// <summary>
        /// Identificativo della EntitaAziendale
        /// </summary>
        public const string EntitaAziendaleId = "entitaAziendaleId";

        /// <summary>
        /// Ruoli dell'utente (multi-value claim)
        /// </summary>
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        /// <summary>
        /// Permessi specifici dell'utente (multi-value claim)
        /// </summary>
        public const string Permission = "permission";

        /// <summary>
        /// Timestamp di emissione del token
        /// </summary>
        public const string IssuedAt = "iat";

        /// <summary>
        /// Timestamp di scadenza del token
        /// </summary>
        public const string ExpiresAt = "exp";
    }

    /// <summary>
    /// Definizione dei ruoli disponibili nel sistema
    /// </summary>
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Guest = "Guest";
        public const string Ispettore = "Ispettore";
        
        // Da aggiungere altri ruoli secondo i requisiti

        public static List<string> AllRoles => new()
        {
            Admin,
            User,
            Guest,
            Ispettore
        };
    }

    /// <summary>
    /// Definizione dei permessi disponibili nel sistema
    /// </summary>
    public static class Permissions
    {
        // Persone
        public const string CreatePersona = "CreatePersona";
        public const string ReadPersona = "ReadPersona";
        public const string UpdatePersona = "UpdatePersona";
        public const string DeletePersona = "DeletePersona";
        public const string ExportPersone = "ExportPersone";
        public const string ImportPersone = "ImportPersone";

        // EntiAccreditamento
        public const string CreateEnteAccreditamento = "CreateEnteAccreditamento";
        public const string ReadEnteAccreditamento = "ReadEnteAccreditamento";
        public const string UpdateEnteAccreditamento = "UpdateEnteAccreditamento";
        public const string DeleteEnteAccreditamento = "DeleteEnteAccreditamento";

        // Users Management
        public const string ManageUsers = "ManageUsers";
        public const string ManageRoles = "ManageRoles";

        public static List<string> AllPermissions => new()
        {
            // Persone
            CreatePersona,
            ReadPersona,
            UpdatePersona,
            DeletePersona,
            ExportPersone,
            ImportPersone,

            // EntiAccreditamento
            CreateEnteAccreditamento,
            ReadEnteAccreditamento,
            UpdateEnteAccreditamento,
            DeleteEnteAccreditamento,

            // Users
            ManageUsers,
            ManageRoles
        };

        /// <summary>
        /// Mappa dei permessi per ruolo
        /// </summary>
        public static Dictionary<string, List<string>> PermissionsByRole => new()
        {
            {
                UserRoles.Admin,
                new List<string>
                {
                    CreatePersona, ReadPersona, UpdatePersona, DeletePersona, ExportPersone, ImportPersone,
                    CreateEnteAccreditamento, ReadEnteAccreditamento, UpdateEnteAccreditamento, DeleteEnteAccreditamento,
                    ManageUsers, ManageRoles
                }
            },
            {
                UserRoles.User,
                new List<string>
                {
                    ReadPersona, UpdatePersona, ExportPersone,
                    ReadEnteAccreditamento
                }
            },
            {
                UserRoles.Ispettore,
                new List<string>
                {
                    ReadPersona, ReadEnteAccreditamento
                }
            },
            {
                UserRoles.Guest,
                new List<string>
                {
                    ReadPersona
                }
            }
        };
    }
}
