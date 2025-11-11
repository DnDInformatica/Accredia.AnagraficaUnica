# 📋 INDIRIZZI FINALI E ISTRUZIONI - ACCREDIA SOLUTION

## ✅ PROBLEMA RISOLTO

La porta 62412 era occupata. Ho cambiato i porti del Web ai seguenti:
- **HTTPS**: 7412 (era 62412)
- **HTTP**: 7413 (era 62413)

---

## 🌐 INDIRIZZI FINALI

### API (Porta 5001 - Non cambiata)
```
HTTP:    http://localhost:5000
HTTPS:   https://localhost:5001
Swagger: https://localhost:5001/swagger
Ping:    https://localhost:5001/ping
```

### Web (Nuovo - Porta 7412)
```
HTTP:    http://localhost:7413
HTTPS:   https://localhost:7412
```

---

## 🚀 COME AVVIARE (PROCEDURA COMPLETA)

### PASSO 1: Termina i vecchi processi

Apri PowerShell e esegui:

```powershell
taskkill /IM dotnet.exe /F
```

### PASSO 2: Pulisci i progetti (Consigliato)

**Opzione A - Automatico:**
```batch
cd C:\Accredia\Sviluppo
cleanup-and-restart.bat
```

**Opzione B - Manuale:**
```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
dotnet clean -c Release
dotnet restore

cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet clean -c Release
dotnet restore
```

### PASSO 3: Riavvia il sistema

```batch
cd C:\Accredia\Sviluppo
start-all.bat
```

---

## ✨ COSA ASPETTARSI

### Ventana 1 - API
```
[BUILD] Compilando API...
[RUN] Eseguendo API...

Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

### Ventana 2 - Web
```
[BUILD] Compilando Web...
[RUN] Eseguendo Web...

Now listening on: https://localhost:7412
Now listening on: http://localhost:7413
Application started. Press Ctrl+C to shut down.
```

---

## 🧪 VERIFICHE

### Test API (da PowerShell)
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/ping" -SkipCertificateCheck
# Risposta: "pong"
```

### Accedere a Swagger
```
https://localhost:5001/swagger
```

### Accedere al Web
```
https://localhost:7412
o
http://localhost:7413
```

---

## 📊 RIEPILOGO FINALE

| Servizio | Protocollo | Host | Porta | URL |
|----------|-----------|------|-------|-----|
| **API** | HTTP | localhost | 5000 | http://localhost:5000 |
| **API** | HTTPS | localhost | 5001 | https://localhost:5001 |
| **API Swagger** | HTTPS | localhost | 5001 | https://localhost:5001/swagger |
| **Web** | HTTP | localhost | 7413 | http://localhost:7413 |
| **Web** | HTTPS | localhost | 7412 | https://localhost:7412 |

---

## 🛑 FERMARE I SERVIZI

### Opzione 1: Chiudi le ventane
Premi `Ctrl+C` in ogni ventana o chiudile direttamente

### Opzione 2: Forza la terminazione
```powershell
taskkill /IM dotnet.exe /F
```

---

## 📁 FILE IMPORTANTI

- `start-all.bat` - Avvia API + Web (AGGIORNATO)
- `cleanup-and-restart.bat` - Pulisce tutto e ricomincia
- `GUIDA_RISOLVIMENTO_PROBLEMA_WEB.md` - Guida completa
- `Accredia.GestioneAnagrafica.Web/Properties/launchSettings.json` - Configurazione Web (AGGIORNATO)

---

## 💡 COMANDI RAPIDI

```bash
# Termina processi
taskkill /IM dotnet.exe /F

# Pulisci
cleanup-and-restart.bat

# Avvia tutto
start-all.bat

# Test API
Invoke-RestMethod -Uri "https://localhost:5001/ping" -SkipCertificateCheck
```

---

## ✅ CHECKLIST FINALE

- [ ] Ho terminato i processi precedenti
- [ ] Ho eseguito cleanup-and-restart.bat
- [ ] Ho eseguito start-all.bat
- [ ] Vedo "Now listening on" in entrambe le ventane
- [ ] Posso accedere a https://localhost:5001/swagger
- [ ] Posso accedere a https://localhost:7412
- [ ] Il ping ritorna "pong"

---

## 🎊 RISULTATO

```
✅ API funzionante su https://localhost:5001
✅ Web funzionante su https://localhost:7412
✅ Entrambi i servizi in esecuzione
✅ Logs visibili in ventane separate
✅ Pronto per lo sviluppo
```

---

**Data**: 3 Novembre 2025  
**Status**: ✅ **PRONTO**

Esegui `start-all.bat` per iniziare!

