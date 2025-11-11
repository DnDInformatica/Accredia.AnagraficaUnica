# ACCREDIA IDENTITY - ISTRUZIONI FINALI CRITICHE

## 🔴 Situazione Attuale

- appsettings.json: ✅ CORRETTO (URL: https://localhost:5001)
- Server in memoria: ❌ VECCHIA configurazione ancora attiva
- Errore ancora presente: localhost:7001

## ✅ Soluzione

ESEGUIRE MANUALMENTE:

1. Script batch: STOP_AND_CLEAN_ALL.bat
   - Ferma tutti i processi
   - Pulisce progetti
   - Ricompila

2. Apri DUE CONSOLE SEPARATE:

CONSOLE 1 (API):
```bash
cd C:\Accredia\Sviluppo
dotnet run --project Accredia.GestioneAnagrafica.API --no-build
```

Aspetta: https://localhost:5001

CONSOLE 2 (Server):
```bash
cd C:\Accredia\Sviluppo
dotnet run --project Accredia.GestioneAnagrafica.Server --no-build
```

Aspetta: https://localhost:7412

3. Browser:
https://localhost:7412/

4. Login:
admin / password

## 📊 Porte Corrette (Verifica)

- SERVER: 7412 / 7413 ✅
- API: 5001 / 5000 ✅
- Frontend chiama API su: 5001 ✅
- appsettings.json: "URL": "https://localhost:5001" ✅

## ✅ Verifica nei Log

Dovrai vedere:
```
info: AuthService - Tentativo di login
info: AuthService - Login riuscito
```

NO più "localhost:7001"

## ⚠️ Importante

- ✅ Due console SEPARATE
- ✅ API prima, SERVER dopo
- ✅ Lasciarle aperte
- ✅ Controllare log in Console 2

