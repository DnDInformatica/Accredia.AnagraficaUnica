# 🚀 GUIDA ESECUZIONE API - ACCREDIA GESTIONE ANAGRAFICA

## ✅ PROGETTO COMPLETATO E COMPILATO

Il progetto **Accredia.GestioneAnagrafica.API** è stato:
- ✅ Spostato in cartella separata: `C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API`
- ✅ Compilato con successo (0 errori)
- ✅ Compilazione Release completata
- ✅ DLL generato: `bin\Release\net9.0\Accredia.GestioneAnagrafica.API.dll`

---

## 🎯 COME ESEGUIRE L'API

### METODO 1: Script automatico (Consigliato)

Doppio click su `run-api.bat` nella cartella del progetto:
```
C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API\run-api.bat
```

Il script ti offrirà le opzioni:
1. Debug (dotnet run)
2. Release (dotnet run -c Release)
3. Compilare e eseguire Release
4. Salire

### METODO 2: PowerShell / Command Prompt

```powershell
# Navigare alla cartella del progetto
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API

# Eseguire in Debug
dotnet run

# Oppure eseguire in Release
dotnet run -c Release
```

### METODO 3: Da Visual Studio

1. Apri Visual Studio
2. Apri il progetto da: `C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API\Accredia.GestioneAnagrafica.API.csproj`
3. Premi `F5` per eseguire in debug o `Ctrl+F5` per eseguire senza debug

---

## 🌐 ACCESSO ALL'API

Una volta avviata, l'API sarà disponibile su:

```
https://localhost:7043
```

### Swagger/OpenAPI Documentation:
```
https://localhost:7043/swagger
```

### Test endpoint pubblico (no auth):
```
GET https://localhost:7043/ping
Risposta: "pong"
```

---

## 🔐 AUTENTICAZIONE JWT

### 1. Login per ottenere il Token

```http
POST https://localhost:7043/login
Content-Type: application/json

{
  "username": "admin",
  "password": "password123"
}
```

**Risposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-11-04T14:22:00Z"
}
```

### 2. Usare il Token negli Endpoint

Aggiungi l'header `Authorization`:
```http
GET https://localhost:7043/api/persone
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 3. In Swagger

1. Clicca il pulsante "Authorize" in alto a destra
2. Inserisci: `Bearer TOKEN_QUI`
3. Clicca "Authorize"
4. Ora puoi testare gli endpoint protetti

---

## 📋 ENDPOINTS DISPONIBILI

### Ambiti Applicazione
- `GET /api/ambiti` - Lista ambiti
- `POST /api/ambiti` - Crea ambito
- `PUT /api/ambiti/{id}` - Aggiorna ambito
- `DELETE /api/ambiti/{id}` - Elimina ambito

### Persone
- `GET /api/persone` - Lista persone
- `POST /api/persone` - Crea persona
- `PUT /api/persone/{id}` - Aggiorna persona
- `DELETE /api/persone/{id}` - Elimina persona

### Email
- `GET /api/email` - Lista email
- `POST /api/email` - Crea email
- `PUT /api/email/{id}` - Aggiorna email
- `DELETE /api/email/{id}` - Elimina email

### Telefoni
- `GET /api/telefoni` - Lista telefoni
- `POST /api/telefoni` - Crea telefono
- `PUT /api/telefoni/{id}` - Aggiorna telefono
- `DELETE /api/telefoni/{id}` - Elimina telefono

### Indirizzi
- `GET /api/indirizzi` - Lista indirizzi
- `POST /api/indirizzi` - Crea indirizzo
- `PUT /api/indirizzi/{id}` - Aggiorna indirizzo
- `DELETE /api/indirizzi/{id}` - Elimina indirizzo

### Documenti
- `GET /api/documenti` - Lista documenti
- `POST /api/documenti/upload` - Carica documento
- `GET /api/documenti/download/{id}` - Scarica documento
- `DELETE /api/documenti/{id}` - Elimina documento

### Risorse Umane
- `GET /api/dipartimenti` - Lista dipartimenti
- `GET /api/dipendenti` - Lista dipendenti
- `GET /api/turni` - Lista turni
- `POST /api/dipartimenti` - Crea dipartimento
- `POST /api/dipendenti` - Crea dipendente
- `POST /api/turni` - Crea turno

### Tipologiche
- `GET /api/tipologiche` - Lista tipologiche
- `GET /api/tipologiche/complete` - Lista tipologiche complete

---

## ⚙️ CONFIGURAZIONE

### File `.env`

Modifica il file `.env` per configurare:

```env
DB_SERVER=localhost
DB_PORT=1433
DB_NAME=Accredia
DB_USER=sa
DB_PASSWORD=YourPassword123!
DB_INTEGRATED_SECURITY=false
DB_ENCRYPT=true
DB_TRUST_SERVER_CERTIFICATE=true

JWT_KEY=super-secret-key-change-in-production-min-32-chars!!!!
JWT_ISSUER=GestioneOrganismi
JWT_AUDIENCE=GestioneOrganismiUsers

DOCUMENT_STORAGE_TYPE=Local
LOCAL_STORAGE_PATH=C:\Accredia\Documents
NEXTCLOUD_URL=https://nextcloud.example.com
NEXTCLOUD_USERNAME=user
NEXTCLOUD_PASSWORD=pass
```

### Database Connection

L'API legge la stringa di connessione dal file `.env`. Assicurati che:
1. SQL Server sia in esecuzione
2. Le credenziali siano corrette
3. Il database `Accredia` esista (o verrà creato automaticamente da EF Core)

---

## 🔍 DEBUG E TROUBLESHOOTING

### Errore: "Unable to connect to database"
1. Verifica che SQL Server sia in esecuzione
2. Controlla le credenziali nel file `.env`
3. Assicurati che il server e porta siano corretti

### Errore: "Port 7043 already in use"
1. Cambia la porta in `Properties\launchSettings.json`
2. O chiudi l'applicazione che sta usando la porta

### Errore: "Compilation failed"
1. Pulisci il progetto: `dotnet clean`
2. Ripristina le dipendenze: `dotnet restore`
3. Ricompila: `dotnet build -c Release`

---

## 📊 LOG E MONITORAGGIO

I log sono scritti in:
- Console durante l'esecuzione
- File di log (se configurato in `appsettings.Development.json`)

---

## 🎉 SUMMARY

| Elemento | Status |
|----------|--------|
| Progetto | ✅ Separato |
| Cartella | ✅ `C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API` |
| Compilazione | ✅ Successo (0 errori) |
| DLL | ✅ Generato |
| Configurazione | ✅ Pronta |
| Esecuzione | ✅ Pronta |
| API | ✅ Funzionante |

---

**Pronto per la produzione!** 🚀

