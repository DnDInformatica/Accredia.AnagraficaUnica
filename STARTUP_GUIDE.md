# 🚀 Accredia - Gestione Anagrafica - Guida di Avvio

Guida completa per avviare automaticamente le API e il progetto Web.

## 📋 Prerequisiti

- Visual Studio 2022 (Community Edition o superiore)
- .NET 8.0 SDK installato
- Porta 5000/5001 disponibile (API)
- Porta 7412/7413 disponibile (Web)

## ▶️ Avvio Rapido in Visual Studio

### Opzione 1: Avvio Multi-Progetto (CONSIGLIATO)

Visual Studio è già configurato per avviare **sia API che Web contemporaneamente**.

1. Apri `Accredia.GestioneAnagrafica.sln` in Visual Studio
2. Premi **`F5`** (Start with Debugging) oppure **`Ctrl+F5`** (Start without Debugging)

✅ Questo avvierà automaticamente:
- **API** su `http://localhost:5000` e `https://localhost:5001`
- **Web** su `https://localhost:7412`

### Opzione 2: Avvio Manuale di un Progetto Specifico

**Avviare solo l'API:**
1. Clicca con tasto destro su `Accredia.GestioneAnagrafica.API` in Solution Explorer
2. Seleziona **"Set as Startup Project"**
3. Premi `F5`

**Avviare solo il Web:**
1. Clicca con tasto destro su `Accredia.GestioneAnagrafica.Web` in Solution Explorer
2. Seleziona **"Set as Startup Project"**
3. Premi `F5`

## 🖥️ Avvio da Command Line

### Build e Avvio Completo

```bash
# Posizionati nella cartella della soluzione
cd C:\Accredia\Sviluppo

# Build completo
dotnet build

# Avvia API
cd Accredia.GestioneAnagrafica.API
dotnet run

# In un'altra finestra del terminale
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet run
```

### Script Automatico PowerShell

```bash
# Esegui lo script PowerShell
.\START_ALL.ps1

# Con opzioni
.\START_ALL.ps1 -SkipBuild -SkipBrowser
```

### Script Automatico Batch

```bash
# Esegui lo script Batch
START_ALL.bat
```

## 🔌 Endpoint Disponibili

| Servizio | URL | Descrizione |
|----------|-----|-------------|
| **API - HTTP** | `http://localhost:5000` | API REST |
| **API - HTTPS** | `https://localhost:5001` | API REST Sicura |
| **API - Swagger** | `https://localhost:5001/swagger` | Documentazione API |
| **Web - HTTPS** | `https://localhost:7412` | Applicazione Web |

## 📊 Configurazione Avanzata

### launchSettings.json (API)

File: `Accredia.GestioneAnagrafica.API/Properties/launchSettings.json`

```json
{
  "profiles": {
    "Accredia.GestioneAnagrafica.API": {
      "commandName": "Project",
      "launchBrowser": false,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://localhost:5001;http://localhost:5000"
    }
  }
}
```

### launchSettings.json (Web)

File: `Accredia.GestioneAnagrafica.Web/Properties/launchSettings.json`

```json
{
  "profiles": {
    "Accredia.GestioneAnagrafica.Web": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "API_URL": "http://localhost:5000"
      },
      "applicationUrl": "https://localhost:7412;http://localhost:7413"
    }
  }
}
```

## 🔧 VS Code (Alternativa)

Se usi VS Code, la configurazione è già pronta:

1. Apri il workspace in VS Code
2. Premi **`F5`** e seleziona **"API + Web (Accredia)"** dal menu
3. Entrambi i servizi si avvieranno in debug mode

Configurazioni disponibili:
- `.NET Core Launch (API)` - Debug solo API
- `.NET Core Launch (Web)` - Debug solo Web
- `API + Web (Accredia)` - Debug entrambi (CONSIGLIATO)

## 🧪 Test dell'Applicazione

Dopo l'avvio:

1. **Visita il Web:** `https://localhost:7412`
2. **Test API:** `https://localhost:5001/swagger`
3. **Verifica connessione:** L'API deve essere raggiungibile dal Web

## 🛠️ Troubleshooting

### Porta già in uso

Se ricevi errore "Port 5000 already in use":

```bash
# Trova il processo sulla porta 5000
netstat -ano | findstr :5000

# Termina il processo (sostituisci PID)
taskkill /PID <PID> /F
```

### Certificato HTTPS non attendibile

La prima volta potrebbero apparire avvisi sul certificato SSL:
- Clicca "Continua comunque" o "Accetta rischi"
- Questo è normale in ambiente Development

### Web non trova l'API

Verifica che:
1. API sia avviata su `http://localhost:5000`
2. La variabile `API_URL` sia configurata in launchSettings.json
3. Non ci siano firewall che bloccano la comunicazione

## 📝 File Configurazione

```
Accredia.GestioneAnagrafica.sln          ← Configurazione multi-startup
├── Accredia.GestioneAnagrafica.API/
│   └── Properties/launchSettings.json    ← Config avvio API
├── Accredia.GestioneAnagrafica.Web/
│   └── Properties/launchSettings.json    ← Config avvio Web
├── .vscode/
│   ├── launch.json                        ← Config VS Code Debug
│   └── tasks.json                         ← Task di build VS Code
├── START_ALL.ps1                          ← Script PowerShell
└── START_ALL.bat                          ← Script Batch
```

## ✅ Checklist di Avvio

- [ ] Visual Studio aperto
- [ ] Soluzione caricata correttamente
- [ ] Nessun errore di compilazione
- [ ] Porte 5000/5001 e 7412/7413 disponibili
- [ ] Premi F5 per avviare
- [ ] API online su http://localhost:5000
- [ ] Web online su https://localhost:7412
- [ ] Accedi all'app web e verifica funzionalità

## 🆘 Supporto

Per problemi contatta il team di sviluppo Accredia.

---

**Ultima modifica:** Novembre 2025  
**Versione:** Accredia Gestione Anagrafica v1.0
