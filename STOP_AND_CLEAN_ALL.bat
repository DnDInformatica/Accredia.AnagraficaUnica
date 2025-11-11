@echo off
REM ============================================================================
REM Script per FERMARE TUTTI I PROCESSI e RIAVVIARE IL SISTEMA CORRETTAMENTE
REM ============================================================================

echo.
echo ============================================================================
echo  🛑 FERMANDO TUTTI I PROCESSI SULLE PORTE
echo ============================================================================
echo.

REM Ferma processi sulla porta 7412, 7413 (Server)
echo 🔍 Cercando processi sulla porta 7412, 7413...
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :7412') do taskkill /pid %%a /f 2>nul
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :7413') do taskkill /pid %%a /f 2>nul
echo ✅ Processi terminati

REM Ferma processi sulla porta 5001, 5000 (API)
echo.
echo 🔍 Cercando processi sulla porta 5001, 5000...
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5001') do taskkill /pid %%a /f 2>nul
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5000') do taskkill /pid %%a /f 2>nul
echo ✅ Processi terminati

REM Attendi 2 secondi
echo.
echo ⏳ Attendendo 2 secondi...
timeout /t 2 /nobreak

REM Pulisci
echo.
echo ============================================================================
echo  🧹 PULIZIA PROGETTI
echo ============================================================================
echo.

cd C:\Accredia\Sviluppo
dotnet clean --nologo -q

REM Build
echo.
echo ============================================================================
echo  📦 BUILD PROGETTI
echo ============================================================================
echo.

dotnet build -c Debug --nologo -q

REM Mostra le porte corrette
echo.
echo ============================================================================
echo  📊 CONFIGURAZIONE PORTE CORRETTE
echo ============================================================================
echo.

echo ✅ BLAZOR SERVER (Frontend):
echo    HTTPS: https://localhost:7412/
echo    HTTP:  http://localhost:7413/
echo.

echo ✅ API (Backend):
echo    HTTPS: https://localhost:5001/
echo    HTTP:  http://localhost:5000/
echo.

echo ✅ Frontend chiama API su:
echo    https://localhost:5001/ (da appsettings.json)
echo.

echo ============================================================================
echo  🚀 AVVIARE IN DUE CONSOLE SEPARATE
echo ============================================================================
echo.

echo Console 1 - API:
echo   cd C:\Accredia\Sviluppo
echo   dotnet run --project Accredia.GestioneAnagrafica.API --no-build
echo.

echo Console 2 - SERVER:
echo   cd C:\Accredia\Sviluppo
echo   dotnet run --project Accredia.GestioneAnagrafica.Server --no-build
echo.

echo ============================================================================
echo  ✅ PULIZIA COMPLETATA - ADESSO AVVIA MANUALMENTE IN DUE CONSOLE
echo ============================================================================
echo.

pause
