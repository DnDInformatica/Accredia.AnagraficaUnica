using Carter;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Accredia.GestioneAnagrafica.Shared.Responses;

namespace Accredia.GestioneAnagrafica.API.Endpoints.Auth;

/// <summary>
/// Endpoint per l'autenticazione e emissione di token JWT
/// </summary>
public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", (
            LoginRequest request,
            IConfiguration config) =>
        {
            // ✅ TODO: In un caso reale verificheresti username/password dal DB o Identity
            // Per ora usiamo credenziali hardcoded per testing
            if (request.Username != "admin" || request.Password != "password")
            {
                return Results.Unauthorized();
            }

            var token = GenerateJwtToken(request.Username, config);

            return Results.Ok(new LoginResponse
            {
                Success = true,
                Message = "Autenticazione riuscita",
                Token = token,
                ExpiresIn = 3600 // 1 ora in secondi
            });
        })
            .AllowAnonymous()
            .WithTags("Auth")
            .WithName("Login")
            .WithOpenApi()
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    /// Genera un token JWT valido
    /// </summary>
    private static string GenerateJwtToken(string username, IConfiguration config)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Administrator")
        };

        var keyString = config["Jwt:Key"] ?? "super-secret-key-change-in-production";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"] ?? "GestioneOrganismi",
            audience: config["Jwt:Audience"] ?? "GestioneOrganismiUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

/// <summary>
/// Richiesta di login
/// </summary>
public record LoginRequest(string Username, string Password);

/// <summary>
/// Risposta di login con token JWT
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Indica se l'autenticazione è riuscita
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Messaggio di risposta
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Token JWT per le richieste successive
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Tempo di scadenza del token in secondi
    /// </summary>
    public int ExpiresIn { get; set; }
}
