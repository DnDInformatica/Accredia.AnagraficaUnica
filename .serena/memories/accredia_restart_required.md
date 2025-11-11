# ACCREDIA IDENTITY - RIAVVIO OBBLIGATORIO NECESSARIO

## ❌ Situazione Attuale

Ancora vedi nei log:
```
fail: AuthService - Errore nel login: localhost:7001
```

## 🔴 Causa

Program.cs è stato modificato CORRETTAMENTE ✅
Ma il server NON è stato riavviato ❌

Il server è ancora in memoria con la VECCHIA configurazione!

## 🛑 Cosa Fare

1. FERMI il server (Ctrl+C nella console)
2. PULISCI: `dotnet clean`
3. BUILD: `dotnet build -c Debug`
4. RIAVVIA: `dotnet run --project Accredia.GestioneAnagrafica.Server --no-build`

## ⚠️ Importante

Non è sufficiente:
- F5 del browser
- Hot reload
- Riavviare Visual Studio

Devi FERMARE e RIAVVIARE il processo del server completamente!

## 🚀 Dopo il Riavvio

Nei log dovrai vedere:

✅ (NON più localhost:7001)
```
info: AuthService - Tentativo di login per l'utente: admin
info: AuthService - Login riuscito per admin, token ricevuto
```

## 📁 Script Disponibile

Usa: `.\restart-server-complete.ps1`

Questo script:
1. Termina i processi sulle porte 7412, 7413
2. Pulisce il progetto
3. Ricompila
4. Riavvia il server
