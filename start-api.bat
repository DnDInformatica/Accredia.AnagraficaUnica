@echo off
REM Script para iniciar la API en una nueva ventana

cls
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║          ACCREDIA API - INICIANDO                          ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

cd /d C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API

echo [INFO] Directorio: %CD%
echo [INFO] Iniciando API...
echo.

REM Compilar si es necesario
echo [BUILD] Compilando proyecto...
dotnet build -c Release

echo.
echo [RUN] Ejecutando API...
echo.

REM Ejecutar la API
dotnet run -c Release

pause
