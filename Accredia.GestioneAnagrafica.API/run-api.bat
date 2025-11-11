@echo off
REM Script para ejecutar la API Accredia.GestioneAnagrafica

echo.
echo ====================================================
echo   ACCREDIA - GESTIONE ANAGRAFICA API
echo ====================================================
echo.

setlocal enabledelayedexpansion

REM Cambiar a la carpeta del proyecto
cd /d C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API

REM Mostrar opciones
echo Selecciona como ejecutar la API:
echo.
echo 1. Debug (dotnet run)
echo 2. Release (dotnet run -c Release)
echo 3. Compilar y ejecutar Release
echo 4. Salir
echo.

set /p choice="Ingresa tu opcion (1-4): "

if "%choice%"=="1" (
    echo.
    echo Ejecutando en modo DEBUG...
    echo.
    dotnet run
) else if "%choice%"=="2" (
    echo.
    echo Ejecutando en modo RELEASE...
    echo.
    dotnet run -c Release
) else if "%choice%"=="3" (
    echo.
    echo Compilando en modo RELEASE...
    echo.
    dotnet build -c Release
    echo.
    echo Ejecutando API...
    echo.
    dotnet run -c Release
) else if "%choice%"=="4" (
    echo Saliendo...
    exit /b 0
) else (
    echo Opcion invalida. Intenta nuevamente.
    pause
    goto :eof
)

pause
