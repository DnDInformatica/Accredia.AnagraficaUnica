═══════════════════════════════════════════════════════════════════════════════
            ✅ VERIFICA PORTE E CONFIGURAZIONE - PROBLEMA IDENTIFICATO
═══════════════════════════════════════════════════════════════════════════════

## 🔴 PROBLEMA PRINCIPALE TROVATO!

Le porte sono **SBAGLIATE** nella configurazione!

═══════════════════════════════════════════════════════════════════════════════
                    📊 TABELLA CONFIGURAZIONE PORTE
═══════════════════════════════════════════════════════════════════════════════

### SERVER BLAZOR (Frontend)
- launchSettings.json:
  ✅ HTTPS: https://localhost:7412
  ✅ HTTP: http://localhost:7413
- Stato: ✅ CORRETTO

### API (Backend)
- launchSettings.json:
  ✅ HTTPS: https://localhost:5001
  ✅ HTTP: http://localhost:5000
- appsettings.json URL: https://localhost:5001
- Stato: ✅ CORRETTO

### SERVER BLAZOR appsettings.json (CONFIGURAZIONE DEL FRONTEND)
- "API:Url": "https://localhost:7043"
- Stato: ❌ **SBAGLIATO!**

═══════════════════════════════════════════════════════════════════════════════
                    🔴 IL PROBLEMA
═══════════════════════════════════════════════════════════════════════════════

FRONTEND dice all'API:

```
Vai su: https://localhost:7043
```

MA l'API è su:

```
https://localhost:5001
```

ECCO PERCHE' HttpClient cerca localhost:7001!

(Probabilmente è un typo: 7043 dovrebbe essere 5001)

═══════════════════════════════════════════════════════════════════════════════
                    ✅ SOLUZIONE: CORRIGGI appsettings.json
═══════════════════════════════════════════════════════════════════════════════

Cambia in Accredia.GestioneAnagrafica.Server/appsettings.json:

```json
PRIMA (❌ SBAGLIATO):
"API": {
  "Url": "https://localhost:7043"
}

DOPO (✅ CORRETTO):
"API": {
  "Url": "https://localhost:5001"
}
```

═══════════════════════════════════════════════════════════════════════════════
                    📋 VERIFICA COMPLETA DELLE PORTE
═══════════════════════════════════════════════════════════════════════════════

### 1️⃣ BLAZOR SERVER (Frontend - quello che vedi nel browser)

File: Accredia.GestioneAnagrafica.Server/Properties/launchSettings.json
```json
"applicationUrl": "https://localhost:7412;http://localhost:7413"
```

Significato:
- ✅ HTTPS: https://localhost:7412/
- ✅ HTTP: http://localhost:7413/

Accesso dal browser:
- https://localhost:7412/ (HTTPS - certificato autodefinito)
- http://localhost:7413/ (HTTP - non sicuro)

### 2️⃣ API (Backend - dove vanno le richieste di login)

File: Accredia.GestioneAnagrafica.API/Properties/launchSettings.json
```json
"applicationUrl": "https://localhost:5001;http://localhost:5000"
```

Significato:
- ✅ HTTPS: https://localhost:5001/
- ✅ HTTP: http://localhost:5000/

Quando il frontend fa login:
- POST https://localhost:5001/auth/login
- Body: { "username": "admin", "password": "password" }

### 3️⃣ CONFIGURAZIONE DEL FRONTEND

File: Accredia.GestioneAnagrafica.Server/appsettings.json
```json
"API": {
  "Url": "https://localhost:7043"  ❌ SBAGLIATO!
}
```

✅ DOVREBBE ESSERE:
```json
"API": {
  "Url": "https://localhost:5001"
}
```

═══════════════════════════════════════════════════════════════════════════════
                    🔄 FLUSSO DELLE RICHIESTE
═══════════════════════════════════════════════════════════════════════════════

1. Browser apre:
   https://localhost:7412/ ← SERVER BLAZOR
   
2. User digita username/password e clicca "Accedi"

3. Frontend (Blazor) legge appsettings.json:
   "API:Url": "https://localhost:7043" ❌ SBAGLIATO
   
4. Frontend crea HttpClient con BaseAddress:
   https://localhost:7043/ ❌ SBAGLIATO!
   
5. Frontend fa richiesta:
   POST https://localhost:7043/auth/login ❌ SBAGLIATO!
   
6. Sistema operativo tenta connessione:
   Nessuno in ascolto su localhost:7043 ❌
   Fallisce: "Rifiuto persistente" (Connection refused)

✅ COSA DOVREBBE SUCCEDERE:

1. Browser apre:
   https://localhost:7412/ ← SERVER BLAZOR
   
2. User digita username/password e clicca "Accedi"

3. Frontend legge appsettings.json:
   "API:Url": "https://localhost:5001" ✅ CORRETTO
   
4. Frontend crea HttpClient con BaseAddress:
   https://localhost:5001/ ✅ CORRETTO
   
5. Frontend fa richiesta:
   POST https://localhost:5001/auth/login ✅ CORRETTO
   
6. Sistema operativo tenta connessione:
   API in ascolto su localhost:5001 ✅
   Connessione riuscita! ✅
   
7. API valida admin/password ✅

8. API restituisce JWT token ✅

9. Frontend riceve token e autentica utente ✅

10. Redirect a /dashboard ✅

═══════════════════════════════════════════════════════════════════════════════
                    🛠️ COME AVVIARE I SERVIZI
═══════════════════════════════════════════════════════════════════════════════

### CONSOLE 1 - API (Backend)

```bash
cd C:\Accredia\Sviluppo
dotnet run --project Accredia.GestioneAnagrafica.API

# Dovresti vedere:
# Now listening on: https://localhost:5001
# Now listening on: http://localhost:5000
```

### CONSOLE 2 - SERVER BLAZOR (Frontend)

```bash
cd C:\Accredia\Sviluppo
dotnet run --project Accredia.GestioneAnagrafica.Server

# Dovresti vedere:
# Now listening on: https://localhost:7412
# Now listening on: http://localhost:7413
```

### Browser

```
https://localhost:7412/
```

═══════════════════════════════════════════════════════════════════════════════
                    ✅ STEP PER STEP - SOLUZIONE
═══════════════════════════════════════════════════════════════════════════════

1. FERMAMI entrambi i server (Ctrl+C nelle console)

2. CORREGGI appsettings.json del SERVER:
   
   File: C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Server\appsettings.json
   
   Cambia:
   ```json
   "API": {
     "Url": "https://localhost:7043"
   }
   ```
   
   In:
   ```json
   "API": {
     "Url": "https://localhost:5001"
   }
   ```

3. PULISCI E RICOMPILA:
   ```bash
   cd C:\Accredia\Sviluppo
   dotnet clean
   dotnet build -c Debug
   ```

4. AVVIA l'API (CONSOLE 1):
   ```bash
   dotnet run --project Accredia.GestioneAnagrafica.API
   ```
   
   Aspetta: "Now listening on: https://localhost:5001"

5. AVVIA il SERVER (CONSOLE 2):
   ```bash
   dotnet run --project Accredia.GestioneAnagrafica.Server
   ```
   
   Aspetta: "Now listening on: https://localhost:7412"

6. APRI BROWSER:
   ```
   https://localhost:7412/
   ```

7. CLICCA "Login"

8. INSERISCI:
   ```
   Username: admin
   Password: password
   ```

9. CLICCA "Accedi"

10. VERIFICA NEI LOG (CONSOLE 2):
    ```
    info: AuthService - Tentativo di login per l'utente: admin
    info: AuthService - Login riuscito per admin, token ricevuto
    info: Login - Stato di autenticazione aggiornato
    ```
    
    ✅ NON dovrai più vedere: "localhost:7001"

═══════════════════════════════════════════════════════════════════════════════
                    📊 TABELLA FINALE DI VERIFICA
═══════════════════════════════════════════════════════════════════════════════

| Componente | HTTPS | HTTP | Configurazione | Status |
|-----------|-------|------|-----------------|--------|
| Blazor Server | 7412 | 7413 | launchSettings.json | ✅ OK |
| API | 5001 | 5000 | launchSettings.json | ✅ OK |
| Frontend API URL | - | - | appsettings.json Server | ❌ SBAGLIATO |
| Frontend chiama API su | 7043 | - | HttpClient BaseAddress | ❌ SBAGLIATO |

SOLUZIONE: Cambia appsettings.json Server da 7043 a 5001

═══════════════════════════════════════════════════════════════════════════════
                    🎉 QUESTO È IL VERO PROBLEMA! 🎉
═══════════════════════════════════════════════════════════════════════════════

La porta sbagliata (7043) in appsettings.json era il BUG nascosto!

Cambia da 7043 a 5001 e tutto funzionerà perfettamente!

═══════════════════════════════════════════════════════════════════════════════
