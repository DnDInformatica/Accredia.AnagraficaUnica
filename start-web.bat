@echo off
REM Script para iniciar el Web en una nueva ventana

cls
echo.
echo ╔════════════════════════════════════════════════════════════╗
echo ║          ACCREDIA WEB - INICIANDO                          ║
echo ╚════════════════════════════════════════════════════════════╝
echo.

cd /d C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web

echo [INFO] Directorio: %CD%
echo [INFO] Iniciando Web...
echo.

REM Compilar si es necesario
echo [BUILD] Compilando proyecto...
dotnet build -c Release

echo.
echo [RUN] Ejecutando Web...
echo.

REM Ejecutar el Web
dotnet run -c Release

pause
