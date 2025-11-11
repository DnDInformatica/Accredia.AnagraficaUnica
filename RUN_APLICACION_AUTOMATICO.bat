@echo off
REM ============================================================================
REM  SCRIPT DE EJECUCIÓN AUTOMÁTICA COMPLETA
REM  
REM  Proyecto: Accredia.GestioneAnagrafica
REM  Descripción: Ejecuta automáticamente API + Server
REM ============================================================================

setlocal enabledelayedexpansion

echo.
echo ╔════════════════════════════════════════════════════════════════════════╗
echo ║                                                                        ║
echo ║          🚀 EJECUCIÓN AUTOMÁTICA COMPLETA                             ║
echo ║                                                                        ║
echo ║     Accredia.GestioneAnagrafica - API + Server                        ║
echo ║                                                                        ║
echo ╚════════════════════════════════════════════════════════════════════════╝
echo.

REM Cambiar a directorio del proyecto
cd /d "%~dp0"

if not exist "Accredia.GestioneAnagrafica.sln" (
    echo ❌ No se encontró la solución en: %cd%
    pause
    exit /b 1
)

REM PASO 1: Verificar .NET
echo.
echo ════════════════════════════════════════════════════════════════════════
echo PASO 1: Verificando .NET
echo ════════════════════════════════════════════════════════════════════════
echo.

dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ❌ .NET no está instalado o no está en PATH
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo ✅ .NET versión: %DOTNET_VERSION%
echo.

REM PASO 2: Limpiar
echo ════════════════════════════════════════════════════════════════════════
echo PASO 2: Limpiando solución
echo ════════════════════════════════════════════════════════════════════════
echo.

echo 🧹 Ejecutando 'dotnet clean -c Release'...
dotnet clean -c Release >nul 2>&1
if errorlevel 1 (
    echo ⚠️  Advertencia: No se pudo limpiar completamente
) else (
    echo ✅ Solución limpia
)
echo.

REM PASO 3: Compilar
echo ════════════════════════════════════════════════════════════════════════
echo PASO 3: Compilando solución
echo ════════════════════════════════════════════════════════════════════════
echo.

echo 🔨 Ejecutando 'dotnet build -c Release'...
echo ⏳ Esto puede tomar 1-2 minutos...
echo.

dotnet build -c Release
if errorlevel 1 (
    echo.
    echo ❌ Error en la compilación
    pause
    exit /b 1
)

echo.
echo ✅ Compilación exitosa
echo.

REM PASO 4: Iniciar API en ventana separada
echo ════════════════════════════════════════════════════════════════════════
echo PASO 4: Iniciando servicios
echo ════════════════════════════════════════════════════════════════════════
echo.

echo 🚀 Iniciando API (puerto 5001)...
start "API - https://localhost:5001" cmd /k "cd Accredia.GestioneAnagrafica.API && dotnet run -c Release"

echo ✅ API iniciada en ventana separada
echo.

REM PASO 5: Esperar
echo ⏳ Esperando a que API esté lista (5 segundos)...
timeout /t 5 /nobreak

REM PASO 6: Iniciar Server
echo.
echo ════════════════════════════════════════════════════════════════════════
echo PASO 5: Iniciando Server
echo ════════════════════════════════════════════════════════════════════════
echo.

echo ╔════════════════════════════════════════════════════════════════════════╗
echo ║                   ✅ SERVICIOS INICIADOS                             ║
echo ╚════════════════════════════════════════════════════════════════════════╝
echo.

echo 📊 ACCESOS DISPONIBLES:
echo.
echo   🌐 Web Blazor:     https://localhost:7412
echo   📚 Swagger API:    https://localhost:5001/swagger
echo   💚 Health Check:   https://localhost:7412/health
echo   🔌 API Base:       https://localhost:5001
echo.

echo 🚀 Iniciando Server (puerto 7412)...
echo.
echo ⏳ El servidor se ejecutará en la ventana actual...
echo ⏳ Cierra esta ventana para detener todos los servicios
echo.

cd /d "%~dp0\Accredia.GestioneAnagrafica.Server"
dotnet run -c Release

REM Si llegamos aquí, el usuario cerró la ventana
echo.
echo 🛑 Deteniendo servicios...
taskkill /IM dotnet.exe /F 2>nul
echo ✅ Servicios detenidos
pause
