# Accredia - Configurazione Avvio Visual Studio

## ✅ Configurazione Completata

### File Modificati
1. **Accredia.GestioneAnagrafica.API/Properties/launchSettings.json**
   - Profile: "Accredia.GestioneAnagrafica.API"
   - URL: https://localhost:5001; http://localhost:5000
   - Swagger UI: https://localhost:5001/swagger
   - launchBrowser: false (non apre automaticamente)

2. **Accredia.GestioneAnagrafica.Web/Properties/launchSettings.json**
   - Profile: "Accredia.GestioneAnagrafica.Web"
   - URL: https://localhost:7412; http://localhost:7413
   - API_URL env var: http://localhost:5000
   - launchBrowser: true (apre il browser)

3. **Accredia.GestioneAnagrafica.sln**
   - Configurato multi-project startup:
     - API: {0EAA1AD2-FAF8-4CB7-2A1F-AAA4BB60EB4B}
     - Web: {6D035ACA-53F1-4038-952B-FF26E01A118D}

4. **File Aggiuntivi Creati**
   - .vscode/launch.json - Config VS Code Debug
   - .vscode/tasks.json - Task build VS Code
   - STARTUP_GUIDE.md - Documentazione completa
   - START_ALL.ps1 - Script PowerShell
   - START_ALL.bat - Script Batch

## 🚀 Come Usare

### In Visual Studio
1. Apri la soluzione
2. Premi F5 (avvierà sia API che Web)

### Endpoint Disponibili
- API HTTP: http://localhost:5000
- API HTTPS: https://localhost:5001
- Swagger: https://localhost:5001/swagger
- Web: https://localhost:7412

## 🔧 Configurazione Avanzata
- launchSettings configurati per Development
- Ambiente ASPNETCORE_ENVIRONMENT=Development
- API_URL passata come variabile di ambiente al Web
- Multi-startup configurato nel .sln
