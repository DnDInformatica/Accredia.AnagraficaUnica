# ACCREDIA - SOLUZIONE PROBLEMA WEB PORTS

## Problema Identificato
Il Web non rispondeva perché la porta 62412 era già occupata.
Errore: `Failed to bind to address https://127.0.0.1:62412: address already in use`

## Soluzione Implementata

### Cambiamenti Porti Web
- Vecchi porti: HTTPS 62412, HTTP 62413 ❌
- Nuovi porti: HTTPS 7412, HTTP 7413 ✅

### File Modificati
- `Accredia.GestioneAnagrafica.Web/Properties/launchSettings.json`
  - applicationUrl: `https://localhost:7412;http://localhost:7413`

### File Aggiornati
- `start-all.bat` - Script con nuovi porti
- `cleanup-and-restart.bat` - Nuovo script per pulizia

### Documentazione Creata
- `GUIDA_RISOLVIMENTO_PROBLEMA_WEB.md`
- `DIAGNOSTICO_PROBLEMA_WEB.md`

## Procedure per Risolvere

1. Termina processi: `taskkill /IM dotnet.exe /F`
2. Pulisci: `cleanup-and-restart.bat`
3. Riavvia: `start-all.bat`

## Nuovi Indirizzi
- API: https://localhost:5001
- API Swagger: https://localhost:5001/swagger
- Web: https://localhost:7412 (o http://localhost:7413)

## Status
✅ Problema risolto
✅ Nuovo script creato
✅ Documentazione completa
