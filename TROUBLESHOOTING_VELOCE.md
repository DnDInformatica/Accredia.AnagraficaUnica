═══════════════════════════════════════════════════════════════════════════════
                    🐛 TROUBLESHOOTING VELOCE
═══════════════════════════════════════════════════════════════════════════════

## Se il Login NON Funziona

### ❌ Errore: "Username o password non corretti"

1. Verifica le credenziali:
   Username: admin (tutto minuscolo)
   Password: password (tutto minuscolo)

2. Controlla che l'API sia in esecuzione:
   - Apri: https://localhost:7043/swagger
   - Clicca POST /auth/login
   - Prova con: {"username":"admin","password":"password"}
   - Dovrebbe restituire un token

3. Controlla appsettings.json:
   "API": {
     "Url": "https://localhost:7043"
   }

4. Controlla i log del server:
   - Console dovrebbe mostrare: "Login fallito con status code: XXX"
   - Se mostra 401: credenziali sbagliate
   - Se mostra 0: API non raggiungibile

---

### ❌ Errore: "Cannot cast AuthStateProvider"

1. Verifica Program.cs:
   builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
   builder.Services.AddScoped<JwtAuthenticationStateProvider>();

2. Controlla che JwtAuthenticationStateProvider esista:
   C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Server\Auth\JwtAuthenticationStateProvider.cs

---

### ❌ Dashboard dice "Not Authorized"

1. Verifica che i metodi siano stati chiamati:
   - Console dovrebbe mostrare: "Stato di autenticazione aggiornato"
   - Se non lo mostra, il token potrebbe essere null

2. Prova Ctrl+F5 (refresh forcato):
   - Potrebbe essere un problema di cache

3. Verifica che il token non sia scaduto:
   - Token validità: 1 ora
   - Se è passato più di 1 ora dal login, fai un nuovo login

---

### ❌ Nessun output nei log

1. Verifica appsettings.json:
   "Logging": {
     "LogLevel": {
       "Default": "Information"
     }
   }

2. Verifica che Logger sia registrato in Program.cs:
   builder.Services.AddLogging(config =>
   {
       config.AddConsole();
       config.AddDebug();
   });

---

### ❌ Exception durante il login

1. Leggi il messaggio di errore completo
2. Controlla i log per la stack trace
3. Verifica che tutti i using statement siano corretti

---

## Comandi di Debug

### Test API direttamente:

```powershell
# PowerShell
$body = @{
    username = "admin"
    password = "password"
} | ConvertTo-Json

$response = Invoke-RestMethod `
  -Uri https://localhost:7043/auth/login `
  -Method POST `
  -Body $body `
  -ContentType application/json `
  -SkipCertificateCheck

Write-Host $response.token
```

### Verifica che le porte siano in uso:

```powershell
# Server Blazor
netstat -ano | findstr :7413

# API
netstat -ano | findstr :7043
```

### Forza close di processi sulla porta:

```powershell
# Se porta 7413 è occupata
netstat -ano | findstr :7413
taskkill /PID <PID> /F

# Se porta 7043 è occupata
netstat -ano | findstr :7043
taskkill /PID <PID> /F
```

---

## Checklist Finale

Prima di testare, assicurati che:

- [ ] AuthService.cs ha proprietà static SessionToken
- [ ] Login.razor aggiorna JwtAuthenticationStateProvider
- [ ] JwtAuthenticationStateProvider ha metodo MarkUserAsAuthenticated
- [ ] Program.cs registra UserState e AppState nel DI
- [ ] appsettings.json ha "API:Url": "https://localhost:7043"
- [ ] API LoginEndpoint valida admin/password
- [ ] Server Blazor è in ascolto su http://localhost:7413
- [ ] API è in ascolto su https://localhost:7043
- [ ] Non ci sono processi precedenti in esecuzione sulle porte 7413 e 7043

═══════════════════════════════════════════════════════════════════════════════

Se nulla di questo aiuta, contatta me con:
1. Screenshot dell'errore
2. Output completo della console
3. Quale step esatto fallisce

═══════════════════════════════════════════════════════════════════════════════
