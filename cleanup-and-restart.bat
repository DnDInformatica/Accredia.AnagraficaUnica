@echo off
REM Script per terminare i processi dotnet e pulire

cls
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║           PULIZIA E RESET - ACCREDIA SOLUTION              ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

echo [STEP 1] Terminando tutti i processi dotnet...
taskkill /IM dotnet.exe /F 2>nul
if errorlevel 0 (
    echo [OK] Processi terminati
) else (
    echo [INFO] Nessun processo dotnet in esecuzione
)

echo.
echo [STEP 2] Pulendo il progetto API...
cd /d C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
dotnet clean -c Release
dotnet restore

echo.
echo [STEP 3] Pulendo il progetto Web...
cd /d C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet clean -c Release
dotnet restore

echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║                    PULIZIA COMPLETATA                      ║
echo ╚════════════════════════════════════════════════════════════╝
echo.
echo Adesso puoi eseguire: start-all.bat
echo.
pause
