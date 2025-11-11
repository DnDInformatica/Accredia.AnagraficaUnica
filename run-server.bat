@echo off
REM Script para compilar y ejecutar el servidor host Blazor WASM

echo.
echo ╔══════════════════════════════════════════════════════════╗
echo ║   🚀 COMPILANDO Y EJECUTANDO SERVIDOR HOST              ║
echo ╚══════════════════════════════════════════════════════════╝
echo.

cd /d "%~dp0"

echo [PASO 1] Limpiando proyectos...
echo.
cd Accredia.GestioneAnagrafica.Web
call dotnet clean -c Release 2>nul
cd ..

cd Accredia.GestioneAnagrafica.Server
call dotnet clean -c Release 2>nul
cd ..

echo ✅ Proyectos limpiados
echo.

echo [PASO 2] Publicando Web Blazor...
echo.
cd Accredia.GestioneAnagrafica.Web
call dotnet publish -c Release -o ..\Accredia.GestioneAnagrafica.Server\wwwroot
if errorlevel 1 (
    echo ❌ ERROR: Fallo en publicación del Web
    exit /b 1
)
cd ..

echo ✅ Web publicado
echo.

echo [PASO 3] Compilando servidor host...
echo.
cd Accredia.GestioneAnagrafica.Server
call dotnet build -c Release
if errorlevel 1 (
    echo ❌ ERROR: Fallo en compilación
    exit /b 1
)
cd ..

echo ✅ Servidor compilado
echo.

echo ╔══════════════════════════════════════════════════════════╗
echo ║   ✅ TODO LISTO - EJECUTANDO SERVIDOR                   ║
echo ╚══════════════════════════════════════════════════════════╝
echo.

cd Accredia.GestioneAnagrafica.Server
dotnet run -c Release

pause
