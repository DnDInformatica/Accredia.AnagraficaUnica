# ACCREDIA IDENTITY - GUIDA IMPLEMENTAZIONE

## Componenti Chiave

### API Layer
- **LoginEndpoint.cs**: POST /auth/login
  - Input: { username, password }
  - Output: { success, token, expiresIn }
  - Test: admin/password
  - Token: JWT (1 ora)

### Web Layer
- **JwtAuthenticationStateProvider.cs**: Gestisce stato Auth
- **AuthService.cs**: Servizio login/logout
- **IAuthService.cs**: Interfaccia servizio

## Flusso di Autenticazione

1. User → Login.razor (form username/password)
2. AuthService.LoginAsync() → API POST /auth/login
3. API valida credenziali → Restituisce JWT
4. Web salva token in localStorage
5. JwtAuthenticationStateProvider parsa token
6. Stato cambia a Authenticated
7. Navigation verso Dashboard

## Credenziali Test
- Username: admin
- Password: password

## File Creato
- GUIDA_IMPLEMENTAZIONE_IDENTITY.md (in project root)

## Prossimi Step
1. Completare AuthService con logic reale
2. Completare JwtAuthenticationStateProvider per localStorage
3. Creare Login.razor
4. Creare Dashboard.razor con @attribute [Authorize]
5. Implementare logout
6. Aggiungere middleware JWT in API
