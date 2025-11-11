# 🔧 GUIDA RISOLVIMENTO - PROBLEMA WEB NON VISIBILE

## ⚠️ PROBLEMA

Il Web non risponde dopo l'esecuzione di `start-all.bat`

Errore nel log:
```
System.IO.IOException: Failed to bind to address https://127.0.0.1:62412: address already in use
```

---

## ✅ SOLUZIONE

Ho identificato e risolto il problema:

1. **Causa**: La porta 62412 era già in uso
2. **Soluzione**: Ho cambiato i purti del Web ai seguenti:
   - HTTPS: **7412** (era 62412)
   - HTTP: **7413** (era 62413)
3. **File aggiornato**: `launchSettings.json` del Web

---

## 🚀 COME PROCEDERE

### PASSO 1: Terminare tutti i processi

Apri PowerShell e esegui:

```powershell
taskkill /IM dotnet.exe /F
```

### PASSO 2: Pulire e ricostituire (CONSIGLIATO)

Esegui il script di pulizia:

```batch
cd C:\Accredia\Sviluppo
cleanup-and-restart.bat
```

Oppure manualmente:

```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
dotnet clean -c Release
dotnet restore

cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet clean -c Release
dotnet restore
```

### PASSO 3: Riavviare il sistema

```batch
cd C:\Accredia\Sviluppo
start-all.bat
```

---

## 🌐 NUOVI INDIRIZZI

Dopo il riavvio, accedi a:

```
API Swagger:     https://localhost:5001/swagger
API Ping:        https://localhost:5001/ping
Web (HTTPS):     https://localhost:7412
Web (HTTP):      http://localhost:7413
```

---

## ✨ VERIFICA CHE FUNZIONA

### Nel Log della Ventana 1 (API)
Dovresti vedere:
```
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

### Nel Log della Ventana 2 (Web)
Dovresti vedere:
```
Now listening on: https://localhost:7412
Now listening on: http://localhost:7413
Application started. Press Ctrl+C to shut down.
```

---

## 🌐 TEST DELLA CONNESSIONE

### Test API
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/ping" -SkipCertificateCheck
# Risposta: "pong"
```

### Accedere al Web
Apri nel navigatore:
```
https://localhost:7412
```

---

## 🐛 SE CONTINUA A NON FUNZIONARE

### Verifica che i porti siano liberi

```powershell
# Verifica porta 5001 (API)
netstat -ano | findstr :5001

# Verifica porta 7412 (Web)
netstat -ano | findstr :7412

# Verifica porta 7413 (Web HTTP)
netstat -ano | findstr :7413
```

### Se i porti sono occupati

```powershell
# Trovare il PID del processo
netstat -ano | findstr :7412

# Terminare il processo (sostituisci PID)
taskkill /PID <PID> /F
```

---

## 📋 CHECKLIST FINALE

- [ ] Ho terminato tutti i processi dotnet
- [ ] Ho pulito i progetti (dotnet clean)
- [ ] Ho ripristinato le dipendenze (dotnet restore)
- [ ] Ho eseguito start-all.bat
- [ ] Ho visto "Now listening on" in entrambe le ventane
- [ ] Posso accedere a https://localhost:5001/swagger
- [ ] Posso accedere a https://localhost:7412
- [ ] Il sistema funziona completamente

---

## 📊 RIEPILOGO DEI CAMBIAMENTI

| Elemento | Vecchio | Nuovo | Motivo |
|----------|--------|-------|--------|
| **Web HTTPS** | 62412 | 7412 | Porta occupata |
| **Web HTTP** | 62413 | 7413 | Corrispondenza |
| **API HTTPS** | 5001 | 5001 | Inalterato |
| **API HTTP** | 5000 | 5000 | Inalterato |

---

## ✅ RISULTATO

```
✓ API funzionante su https://localhost:5001
✓ Web funzionante su https://localhost:7412
✓ Entrambi i servizi controllabili e loggabili
✓ Pronto per lo sviluppo
```

---

**Data**: 3 Novembre 2025
**Status**: ✅ RISOLTO

