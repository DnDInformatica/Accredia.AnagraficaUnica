@echo off
REM Script para avviare API e Web contemporaneamente

cls
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║     ACCREDIA - AVVIO API E WEB CONTEMPORANEAMENTE         ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

setlocal enabledelayedexpansion

REM Percorsi dei progetti
set API_PATH=C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
set WEB_PATH=C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web

REM Verifica se i percorsi esistono
if not exist "%API_PATH%" (
    echo [ERRORE] Cartella API non trovata: %API_PATH%
    pause
    exit /b 1
)

if not exist "%WEB_PATH%" (
    echo [ERRORE] Cartella Web non trovata: %WEB_PATH%
    pause
    exit /b 1
)

echo [INFO] Avvio API e Web...
echo.

REM Avvia API in una nuova finestra
echo [STARTING] Accredia.GestioneAnagrafica.API...
echo   Porta: 5001 (HTTPS) / 5000 (HTTP)
cd /d "%API_PATH%"
start "Accredia.GestioneAnagrafica.API" cmd /k "set ASPNETCORE_ENVIRONMENT=Development && dotnet run"
echo [OK] API avviata in nuova finestra
echo.

REM Attendi 2 secondi prima di avviare il Web
timeout /t 2 /nobreak

REM Avvia Web in una nuova finestra
echo [STARTING] Accredia.GestioneAnagrafica.Web...
echo   Porta: 62412 (HTTPS) / 62413 (HTTP)
cd /d "%WEB_PATH%"
start "Accredia.GestioneAnagrafica.Web" cmd /k "set ASPNETCORE_ENVIRONMENT=Development && dotnet run"
echo [OK] Web avviato in nuova finestra
echo.

echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║              ENTRAMBI I PROGETTI SONO AVVIATI              ║
echo ╚════════════════════════════════════════════════════════════╝
echo.
echo API:      https://localhost:5001
echo Web:      https://localhost:62412
echo Swagger:  https://localhost:5001/swagger
echo.
echo [OPZIONE 1] Usa Visual Studio: F5
echo [OPZIONE 2] Scorri le finestre aperte
echo [OPZIONE 3] Chiudi questo prompt
echo.
pause
