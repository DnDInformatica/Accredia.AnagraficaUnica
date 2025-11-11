═══════════════════════════════════════════════════════════════════════════════
                  📚 GUIDA IMPLEMENTAZIONE ACCREDIA IDENTITY
═══════════════════════════════════════════════════════════════════════════════

## 📋 SOMMARIO

Questa guida spiega come implementare il sistema di autenticazione JWT in Accredia
usando i servizi Identity già presenti nel progetto.

═══════════════════════════════════════════════════════════════════════════════
                        1. ARCHITETTURA CURRENT
═══════════════════════════════════════════════════════════════════════════════

### API LAYER (Backend)
Ubicazione: Accredia.GestioneAnagrafica.API

✅ LoginEndpoint.cs
   - Endpoint: POST /auth/login
   - Input: { username, password }
   - Output: { success, message, token, expiresIn }
   - Credenziali test: admin/password
   - Token: JWT (1 ora di validità)

### WEB LAYER (Frontend)
Ubicazione: Accredia.GestioneAnagrafica.Web

✅ JwtAuthenticationStateProvider.cs
   - Gestisce lo stato di autenticazione
   - Parsa i token JWT
   - Notifica i cambamenti di stato
   - Recupera token da localStorage (TODO)

✅ IAuthService.cs / AuthService.cs
   - LoginAsync(username, password) - TODO
   - LogoutAsync() - TODO
   - IsAuthenticatedAsync() - TODO

═══════════════════════════════════════════════════════════════════════════════
                    2. FLUSSO DI AUTENTICAZIONE COMPLETO
═══════════════════════════════════════════════════════════════════════════════

┌─────────────────────────────────────────────────────────────────┐
│ FASE 1: UTENTE ACCEDE ALLA PAGINA /login                       │
├─────────────────────────────────────────────────────────────────┤
│ - Blazor carica Login.razor                                     │
│ - Mostra form username/password                                 │
│ - Form non è autenticato (AuthenticationState = Anonymous)      │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ FASE 2: UTENTE CLICCA "ACCEDI"                                  │
├─────────────────────────────────────────────────────────────────┤
│ - AuthService.LoginAsync(username, password) viene chiamato    │
│ - Invia POST a /auth/login                                      │
│ - API controlla username/password nel DB                        │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ FASE 3: API RESTITUISCE JWT TOKEN                               │
├─────────────────────────────────────────────────────────────────┤
│ Response: {                                                     │
│   "success": true,                                              │
│   "message": "Autenticazione riuscita",                         │
│   "token": "eyJhbGciOiJIUzI1NiIs...",                           │
│   "expiresIn": 3600                                             │
│ }                                                               │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ FASE 4: WEB SALVA TOKEN E AGGIORNA STATO                        │
├─────────────────────────────────────────────────────────────────┤
│ 1. Salva token in localStorage                                  │
│ 2. Chiama JwtAuthenticationStateProvider.MarkUserAsAuthenticated│
│ 3. Parsa il JWT e estrae i claims                               │
│ 4. NotifyAuthenticationStateChanged()                           │
│ 5. AuthorizeView rileva lo stato Authorized                    │
└─────────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│ FASE 5: NAVIGAZIONE AUTOMATICA AL DASHBOARD                     │
├─────────────────────────────────────────────────────────────────┤
│ - Navigation.NavigateTo("/dashboard")                           │
│ - Blazor carica Dashboard.razor                                 │
│ - Dashboard è protetto con @attribute [Authorize]              │
│ - Solo utenti autenticati possono accedere                      │
└─────────────────────────────────────────────────────────────────┘

═══════════════════════════════════════════════════════════════════════════════
                    3. CREDENZIALI DI TEST
═══════════════════════════════════════════════════════════════════════════════

Username: admin
Password: password

⚠️  NOTA: Questi sono hardcoded nell'endpoint per testing!
    In produzione usare database o ASP.NET Identity.

═══════════════════════════════════════════════════════════════════════════════
                    4. IMPLEMENTAZIONE CONSIGLIATA
═══════════════════════════════════════════════════════════════════════════════

### STEP 1: Completare AuthService.cs
───────────────────────────────────────

public async Task<bool> LoginAsync(string username, string password)
{
    try
    {
        var request = new { username, password };
        var response = await _apiHttpClient.PostAsync<LoginResponse>(
            "/auth/login", 
            request
        );

        if (response?.Success == true && !string.IsNullOrEmpty(response.Token))
        {
            // Salva token in localStorage
            await localStorage.SetItemAsync("authToken", response.Token);
            
            // Aggiorna autenticazione
            await _authStateProvider.MarkUserAsAuthenticated(response.Token);
            
            _logger.LogInformation("Login riuscito");
            return true;
        }
        
        return false;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Errore nel login: {ex.Message}");
        return false;
    }
}

### STEP 2: Completare JwtAuthenticationStateProvider.cs
────────────────────────────────────────────────────────

private async Task<string?> GetTokenAsync()
{
    try
    {
        return await _localStorage.GetItemAsync("authToken");
    }
    catch
    {
        return null;
    }
}

### STEP 3: Creare Pagina Login.razor
──────────────────────────────────────

@page "/login"
@using Accredia.GestioneAnagrafica.Web.Services
@inject IAuthService AuthService
@inject NavigationManager Navigation

<div class="container mt-5">
    <div class="row">
        <div class="col-md-6 offset-md-3">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h3 class="mb-0">Login - Accredia</h3>
                </div>
                <div class="card-body">
                    <form @onsubmit="HandleLogin">
                        <div class="mb-3">
                            <label class="form-label">Username</label>
                            <input type="text" 
                                   class="form-control" 
                                   @bind="_username" 
                                   required />
                        </div>
                        
                        <div class="mb-3">
                            <label class="form-label">Password</label>
                            <input type="password" 
                                   class="form-control" 
                                   @bind="_password" 
                                   required />
                        </div>
                        
                        @if (!string.IsNullOrEmpty(_errorMessage))
                        {
                            <div class="alert alert-danger">@_errorMessage</div>
                        }
                        
                        <button type="submit" 
                                class="btn btn-primary w-100" 
                                disabled="@_isLoading">
                            @if (_isLoading)
                            {
                                <span class="spinner-border spinner-border-sm me-2"></span>
                                <span>Accesso in corso...</span>
                            }
                            else
                            {
                                <span>Accedi</span>
                            }
                        </button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@code {
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading = false;

    private async Task HandleLogin()
    {
        _isLoading = true;
        _errorMessage = string.Empty;

        try
        {
            var result = await AuthService.LoginAsync(_username, _password);
            
            if (result)
            {
                Navigation.NavigateTo("/dashboard");
            }
            else
            {
                _errorMessage = "Username o password non corretti";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Errore: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }
}

### STEP 4: Proteggere Pagine con @attribute [Authorize]
──────────────────────────────────────────────────────

@page "/dashboard"
@attribute [Authorize]
@using Microsoft.AspNetCore.Components.Authorization

<div class="container mt-5">
    <h2>Dashboard - Benvenuto!</h2>
    
    <AuthorizeView>
        <Authorized>
            <p>Utente autenticato: @context.User.Identity?.Name</p>
        </Authorized>
        <NotAuthorized>
            <p>Non sei autenticato. Vai a <a href="/login">Login</a></p>
        </NotAuthorized>
    </AuthorizeView>
</div>

═══════════════════════════════════════════════════════════════════════════════
                    5. CONFIGURAZIONE APPSETTINGS
═══════════════════════════════════════════════════════════════════════════════

Nel appsettings.json (API):

{
  "Jwt": {
    "Key": "super-secret-key-change-in-production",
    "Issuer": "GestioneOrganismi",
    "Audience": "GestioneOrganismiUsers"
  },
  "API": {
    "Url": "https://localhost:7043"
  }
}

═══════════════════════════════════════════════════════════════════════════════
                    6. NUGET PACKAGES RICHIESTI
═══════════════════════════════════════════════════════════════════════════════

✅ Blazor.LocalStorage (per localStorage)
   - Salvare il token nel browser

✅ System.IdentityModel.Tokens.Jwt (già presente)
   - Parsing dei JWT token

═══════════════════════════════════════════════════════════════════════════════
                    7. URL IMPORTANTI
═══════════════════════════════════════════════════════════════════════════════

🔗 API Login:     POST https://localhost:7043/auth/login
🔗 Web Login:     http://localhost:7413/login
🔗 Web Dashboard: http://localhost:7413/dashboard
🔗 API Swagger:   https://localhost:7043/swagger

═══════════════════════════════════════════════════════════════════════════════
                    8. SECURITY BEST PRACTICES
═══════════════════════════════════════════════════════════════════════════════

✅ DO:
   - Usare HTTPS in produzione
   - Salvare token con secure flag
   - Impostare expiration time
   - Validare token lato server
   - Usare refresh tokens

❌ DON'T:
   - Hardcodare credenziali
   - Salvare token in URL
   - Usare localStorage per dati sensibili
   - Esporre JWT payload client-side
   - Ignorare scadenza del token

═══════════════════════════════════════════════════════════════════════════════
                    9. PROSSIMI STEP
═══════════════════════════════════════════════════════════════════════════════

1. ✅ Completare AuthService.cs con logica di login reale
2. ✅ Completare JwtAuthenticationStateProvider.cs per localStorage
3. ✅ Creare pagina Login.razor
4. ✅ Creare pagina Dashboard.razor protetta
5. ✅ Implementare logout
6. ✅ Aggiungere middleware JWT nel API
7. ✅ Integrare ASP.NET Identity DB
8. ✅ Aggiungere refresh token
9. ✅ Implementare 2FA

═══════════════════════════════════════════════════════════════════════════════
