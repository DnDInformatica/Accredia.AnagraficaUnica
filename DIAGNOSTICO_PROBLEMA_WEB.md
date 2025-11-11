# 🔍 DIAGNÓSTICO - PROBLEMA CON WEB

## ⚠️ PROBLEMA IDENTIFICATO

Il Web non risponde su nessuno dei due porti:
- ❌ http://localhost:62413
- ❌ https://localhost:62412
- ❌ http://localhost:7413 (nuovo)
- ❌ https://localhost:7412 (nuovo)

L'errore nel log mostra:
```
System.IO.IOException: Failed to bind to address https://127.0.0.1:62412: address already in use.
SocketException (10048): Di norma è consentito un solo utilizzo di ogni indirizzo di socket
```

---

## 🔧 SOLUZIONE IMPLEMENTATA

Ho cambiato i purti del Web da:
- **Vecchi**: 62412 (HTTPS), 62413 (HTTP)
- **Nuovi**: 7412 (HTTPS), 7413 (HTTP)

Il file `launchSettings.json` è stato aggiornato con i nuovi porti.

---

## 📋 PROSSIMI STEP

1. **Terminare tutti i processi dotnet**
   ```powershell
   taskkill /IM dotnet.exe /F
   ```

2. **Pulire il progetto Web**
   ```powershell
   cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
   dotnet clean
   dotnet restore
   ```

3. **Ricompilare**
   ```powershell
   dotnet build -c Release
   ```

4. **Riavviare con il nuovo script**
   ```batch
   cd C:\Accredia\Sviluppo
   start-all.bat
   ```

5. **Verificare i nuovi porti**
   - API: https://localhost:5001/ping
   - Web: https://localhost:7412

---

## 🌐 NUOVI PORTI

```
API:
├─ HTTP:    http://localhost:5000
├─ HTTPS:   https://localhost:5001
└─ Swagger: https://localhost:5001/swagger

WEB (NUOVO):
├─ HTTP:    http://localhost:7413
└─ HTTPS:   https://localhost:7412
```

