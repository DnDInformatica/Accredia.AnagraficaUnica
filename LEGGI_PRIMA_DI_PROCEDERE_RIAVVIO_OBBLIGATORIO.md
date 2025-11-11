═══════════════════════════════════════════════════════════════════════════════
                    ⚠️  LEGGI ATTENTAMENTE - IMPORTANTE! ⚠️
═══════════════════════════════════════════════════════════════════════════════

## ❌ ERRORE ANCORA PRESENTE

Nei log vedi ancora:
```
fail: AuthService - Errore nel login: localhost:7001
```

## 🔴 CAUSA

Il server è ancora in esecuzione con la **VECCHIA configurazione**!

Ho modificato Program.cs, ma tu **NON hai riavviato il server**.

Il server è ancora in memoria con il vecchio HttpClient configurato per la porta SBAGLIATA.

## 🛑 COSA DEVI FARE ADESSO

### STEP 1: FERMI il server

Nella console dove il server è in esecuzione, premi:

```
Ctrl + C
```

Aspetta che si fermi completamente. Dovresti vedere:

```
Hosting environment: Development
Press Ctrl+C to shut down.
```

Se non si ferma, usa lo script PowerShell per terminare il processo.

### STEP 2: Usa lo script PowerShell per riavviare COMPLETAMENTE

```powershell
cd C:\Accredia\Sviluppo
.\restart-server-complete.ps1
```

Oppure, manualmente:

```bash
# Pulisci
cd C:\Accredia\Sviluppo
dotnet clean
dotnet build -c Debug

# Riavvia
dotnet run --project Accredia.GestioneAnagrafica.Server --no-build
```

### STEP 3: Aspetta l'output finale

Dovresti vedere:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7412
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:7413
```

### STEP 4: Apri il browser e prova il login

```
https://localhost:7412/login
Username: admin
Password: password
```

### STEP 5: Verifica nei log

Nei log dovrebbe comparire:

```
info: AuthService - Tentativo di login per l'utente: admin
info: AuthService - Login riuscito per admin, token ricevuto
info: Login - Stato di autenticazione aggiornato
```

❌ **NON** dovrebbe più dire:
```
fail: AuthService - Errore nel login: localhost:7001
```

═══════════════════════════════════════════════════════════════════════════════
                    ⚠️  RIAVVIO OBBLIGATORIO ⚠️
═══════════════════════════════════════════════════════════════════════════════

Il server DEVE essere fermato e riavviato COMPLETAMENTE per caricare la nuova 
configurazione di Program.cs.

Non basta un F5 del browser o un hot reload.

Devi:
1. ❌ FERMARE il server (Ctrl+C)
2. ✅ RIAVVIARE il server (dotnet run)

═══════════════════════════════════════════════════════════════════════════════

Fatto? Contami quando vedi nei log:

```
info: AuthService - Tentativo di login per l'utente: admin
```

(Senza il messaggio di errore "localhost:7001")

═══════════════════════════════════════════════════════════════════════════════
