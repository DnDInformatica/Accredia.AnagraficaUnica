@echo off
REM Script para iniciar API y Web en automatico en ventanas separadas

cls
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║      ACCREDIA SOLUTION - INICIANDO API Y WEB               ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

setlocal enabledelayedexpansion

REM Definir rutas
set API_PATH=C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
set WEB_PATH=C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web

REM Verificar que las rutas existan
if not exist "%API_PATH%" (
    echo [ERROR] Carpeta API no encontrada: %API_PATH%
    pause
    exit /b 1
)

if not exist "%WEB_PATH%" (
    echo [ERROR] Carpeta Web no encontrada: %WEB_PATH%
    pause
    exit /b 1
)

echo [INFO] Iniciando servicios...
echo.

REM Iniciar API en una nueva ventana
echo [INICIANDO] Accredia.GestioneAnagrafica.API
echo   Puerto: 5001 (HTTPS) / 5000 (HTTP)
echo   URL: https://localhost:5001
start "Accredia API" cmd /k "cd /d "%API_PATH%" && echo. && echo Compilando API... && dotnet build -c Release && echo. && echo Ejecutando API... && echo. && dotnet run -c Release"

REM Esperar 3 segundos antes de iniciar el Web
echo.
echo [ESPERANDO] 3 segundos antes de iniciar Web...
timeout /t 3 /nobreak

REM Iniciar Web en una nueva ventana
echo.
echo [INICIANDO] Accredia.GestioneAnagrafica.Web
echo   Puerto: 7412 (HTTPS) / 7413 (HTTP)
echo   URL: https://localhost:7412
start "Accredia Web" cmd /k "cd /d "%WEB_PATH%" && echo. && echo Compilando Web... && dotnet build -c Release && echo. && echo Ejecutando Web... && echo. && dotnet run -c Release"

echo.
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║              SERVICIOS INICIADOS                           ║
echo ╚════════════════════════════════════════════════════════════╝
echo.
echo [OK] API iniciado en una nueva ventana
echo [OK] Web iniciado en una nueva ventana
echo.
echo.
echo PUERTOS Y URLS:
echo.
echo   API:
echo   ├─ HTTP:   http://localhost:5000
echo   ├─ HTTPS:  https://localhost:5001
echo   └─ Swagger: https://localhost:5001/swagger
echo.
echo   WEB:
echo   ├─ HTTP:   http://localhost:7413
echo   └─ HTTPS:  https://localhost:7412
echo.
echo.
echo ACCIONES:
echo.
echo   [1] Para ver los logs, mira las ventanas abiertas
echo   [2] Para detener, cierra las ventanas o presiona Ctrl+C
echo   [3] Las ventanas se mantendrán abiertas para ver los logs
echo.
echo.
pause
