# ACCREDIA IDENTITY - PORTE E CONFIGURAZIONE CORRETTE ✅

## 🔴 PROBLEMA PRINCIPALE TROVATO!

appsettings.json del SERVER aveva URL SBAGLIATO:

❌ PRIMA: "API:Url": "https://localhost:7043"
✅ DOPO: "API:Url": "https://localhost:5001"

## 📊 Porte Corrette

### SERVER BLAZOR (Frontend)
- HTTPS: https://localhost:7412/
- HTTP: http://localhost:7413/
- Configurazione: launchSettings.json ✅

### API (Backend)
- HTTPS: https://localhost:5001/
- HTTP: http://localhost:5000/
- Configurazione: launchSettings.json ✅

### Frontend API URL
- PRIMA: https://localhost:7043 ❌ (SBAGLIATO)
- DOPO: https://localhost:5001 ✅ (CORRETTO)
- File: Server/appsettings.json ✅ (CORRETTO)

## 🔄 Flusso Corretto

1. Browser: https://localhost:7412/ (SERVER BLAZOR)
2. User login: admin/password
3. Frontend legge: "API:Url": "https://localhost:5001"
4. Frontend POST: https://localhost:5001/auth/login
5. API in ascolto su 5001 ✅
6. Connessione riuscita ✅
7. API valida e genera token ✅
8. Frontend riceve token ✅
9. Redirect a /dashboard ✅

## 🛑 Come Avviare

Console 1 - API:
```bash
cd C:\Accredia\Sviluppo
dotnet run --project Accredia.GestioneAnagrafica.API
```
Aspetta: https://localhost:5001

Console 2 - Server:
```bash
dotnet run --project Accredia.GestioneAnagrafica.Server
```
Aspetta: https://localhost:7412

Browser:
https://localhost:7412/

## ✅ Test

Username: admin
Password: password

Nei log dovrai vedere:
```
info: AuthService - Tentativo di login
info: AuthService - Login riuscito
```

Senza errore "localhost:7001"!

## 📁 File Corretto

✅ Accredia.GestioneAnagrafica.Server/appsettings.json
   "API:Url": "https://localhost:5001"
