@echo off
echo ========================================
echo   AVVIO APPLICAZIONE ACCREDIA
echo ========================================
echo.
echo Avvio API su https://localhost:5001...
start "Accredia API" cmd /k "cd /d %~dp0 && dotnet run --project Accredia.GestioneAnagrafica.API"

timeout /t 5 /nobreak >nul

echo.
echo Avvio Server su https://localhost:7412...
start "Accredia Server" cmd /k "cd /d %~dp0 && dotnet run --project Accredia.GestioneAnagrafica.Server"

echo.
echo ========================================
echo   APPLICAZIONE IN AVVIO
echo ========================================
echo.
echo API:    https://localhost:5001
echo WebApp: https://localhost:7412
echo.
echo Apri il browser su: https://localhost:7412/login
echo.
echo Credenziali:
echo   Username: admin
echo   Password: password
echo.
pause
