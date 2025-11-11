═══════════════════════════════════════════════════════════════════════════════
                    ⚠️  ISTRUZIONI CRITICHE - LEGGI TUTTO ⚠️
═══════════════════════════════════════════════════════════════════════════════

## ❌ ERRORE ANCORA PRESENTE

Ancora vedi:
```
fail: AuthService - Errore nel login: localhost:7001
```

## 🔴 CAUSA

Il server NON è stato riavviato dopo le correzioni!

appsettings.json È STATO CORRETTO ✅
MA il server è ancora in memoria con la VECCHIA configurazione!

═══════════════════════════════════════════════════════════════════════════════
                    🛑 FERMA TUTTO ADESSO
═══════════════════════════════════════════════════════════════════════════════

### STEP 1: Esegui lo script batch

```
C:\Accredia\Sviluppo\STOP_AND_CLEAN_ALL.bat
```

Questo script:
- ❌ Ferma TUTTI i processi sulle porte 7412, 7413, 5001, 5000
- 🧹 Pulisce i progetti (dotnet clean)
- 📦 Ricompila tutto (dotnet build)

### STEP 2: Attendi che il script finisca

Dovresti vedere:
```
✅ PULIZIA COMPLETATA - ADESSO AVVIA MANUALMENTE IN DUE CONSOLE
```

### STEP 3: Apri DUE CONSOLE SEPARATE

**CONSOLE 1 - API (Backend):**
```bash
cd C:\Accredia\Sviluppo
dotnet run --project Accredia.GestioneAnagrafica.API --no-build
```

Aspetta di vedere:
```
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

**NON CHIUDERE QUESTA CONSOLE!**

### STEP 4: Apri CONSOLE 2 - SERVER (Frontend)

```bash
cd C:\Accredia\Sviluppo
dotnet run --project Accredia.GestioneAnagrafica.Server --no-build
```

Aspetta di vedere:
```
Now listening on: https://localhost:7412
Now listening on: http://localhost:7413
Application started. Press Ctrl+C to shut down.
```

**NON CHIUDERE QUESTA CONSOLE!**

### STEP 5: Apri il browser

```
https://localhost:7412/
```

Ignora l'avvertimento sul certificato e continua.

### STEP 6: Testa il login

1. Clicca "Login"

2. Inserisci:
   ```
   Username: admin
   Password: password
   ```

3. Clicca "Accedi"

### STEP 7: Verifica nei log (CONSOLE 2)

Dovrai vedere:
```
info: AuthService - Tentativo di login per l'utente: admin
info: AuthService - Login riuscito per admin, token ricevuto
info: Login - Stato di autenticazione aggiornato
```

✅ NON dovrà più apparire:
```
fail: AuthService - Errore nel login: localhost:7001
```

### STEP 8: Se vedi il login riuscito

Nel browser dovresti vedere:
- ✅ Messaggio "Login riuscito!"
- ✅ Redirect automatico a /dashboard
- ✅ Dashboard mostra "Benvenuto, admin"
- ✅ Header mostra "admin" e badge "Autenticato"

═══════════════════════════════════════════════════════════════════════════════
                    ⚠️  IMPORTANTE - PROCEDURE CORRETTE ⚠️
═══════════════════════════════════════════════════════════════════════════════

### ❌ NON FARE:

- ❌ Aprire un'unica console e avviare entrambi i progetti
- ❌ Usare F5 del browser per refresh
- ❌ Chiudere le console
- ❌ Usare hot reload
- ❌ Aspettare che il vecchio server si aggiorni da solo

### ✅ DEVI FARE:

- ✅ Usare due console SEPARATE
- ✅ Avviare API prima (CONSOLE 1)
- ✅ Avviare SERVER dopo (CONSOLE 2)
- ✅ Lasciarle entrambe aperte
- ✅ Controllare i log in CONSOLE 2 quando fai login

═══════════════════════════════════════════════════════════════════════════════
                    📊 CONFIGURAZIONE CORRETTA (VERIFICA)
═══════════════════════════════════════════════════════════════════════════════

File: C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Server\appsettings.json

```json
"API": {
  "Url": "https://localhost:5001"  ← ✅ CORRETTO (NON 7043!)
}
```

Se vedi "7043", cambia in "5001" e ripeti da STEP 1.

═══════════════════════════════════════════════════════════════════════════════
                    🆘 SE ANCORA NON FUNZIONA
═══════════════════════════════════════════════════════════════════════════════

1. Scaricare l'ultima versione di .NET:
   ```
   dotnet --version
   ```
   Dovrebbe essere 9.0.0 o superiore

2. Pulire la cache di .NET:
   ```
   dotnet nuget locals all --clear
   ```

3. Eliminare cartelle .vs, obj, bin:
   ```
   rm -r C:\Accredia\Sviluppo\.vs
   rm -r C:\Accredia\Sviluppo\*\obj
   rm -r C:\Accredia\Sviluppo\*\bin
   ```

4. Ripetere da STEP 1

═══════════════════════════════════════════════════════════════════════════════

ESEGUI LO SCRIPT E SEGUI STEP BY STEP!

Fatto? Contami cosa vedi nei log!

═══════════════════════════════════════════════════════════════════════════════
