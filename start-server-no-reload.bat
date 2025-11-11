@echo off
REM Script per avviare Accredia Gestione Anagrafica Server senza Hot Reload

echo.
echo ════════════════════════════════════════════════════════════════
echo   🚀 AVVIO SERVER ACCREDIA - SENZA HOT RELOAD
echo ════════════════════════════════════════════════════════════════
echo.

cd C:\Accredia\Sviluppo

echo 🧹 Pulizia progetti...
dotnet clean

echo.
echo 📦 Build del progetto...
dotnet build -c Debug

echo.
echo 🔐 Avvio del server...
echo.
echo ℹ️  URL: http://localhost:7413
echo 🔑 Credenziali: admin / password
echo.
echo ════════════════════════════════════════════════════════════════
echo.

REM Avvia il server senza Hot Reload
dotnet run --project Accredia.GestioneAnagrafica.Server --no-build

pause
