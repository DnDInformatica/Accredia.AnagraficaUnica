#!/usr/bin/env pwsh

################################################################################
#
# 🚀 SCRIPT DE EJECUCIÓN AUTOMÁTICA COMPLETA
# 
# Proyecto: Accredia.GestioneAnagrafica
# Descripción: Ejecuta automáticamente API + Server
#
################################################################################

$ErrorActionPreference = "Stop"

# Colores
$Green = [System.ConsoleColor]::Green
$Yellow = [System.ConsoleColor]::Yellow
$Red = [System.ConsoleColor]::Red
$Cyan = [System.ConsoleColor]::Cyan
$White = [System.ConsoleColor]::White

Function Write-Status {
    param([string]$Message, [System.ConsoleColor]$Color = $White)
    Write-Host $Message -ForegroundColor $Color
}

Function Write-Header {
    param([string]$Message)
    Write-Host "`n" -NoNewline
    Write-Host ("=" * 80) -ForegroundColor $Cyan
    Write-Host "🚀 $Message" -ForegroundColor $Green
    Write-Host ("=" * 80) -ForegroundColor $Cyan
    Write-Host ""
}

Function Write-Error-Custom {
    param([string]$Message)
    Write-Host "❌ $Message" -ForegroundColor $Red
}

Function Write-Success {
    param([string]$Message)
    Write-Host "✅ $Message" -ForegroundColor $Green
}

################################################################################
# INICIO
################################################################################

Clear-Host

Write-Host "`n`n" -NoNewline
Write-Host "╔" + ("=" * 78) + "╗" -ForegroundColor $Cyan
Write-Host "║" + " " * 20 + "🎉 EJECUCIÓN AUTOMÁTICA COMPLETA" + " " * 24 + "║" -ForegroundColor $Green
Write-Host "║" + " " * 15 + "Accredia.GestioneAnagrafica - API + Server" + " " * 20 + "║" -ForegroundColor $Green
Write-Host "╚" + ("=" * 78) + "╝" -ForegroundColor $Cyan
Write-Host ""

$projectRoot = "C:\Accredia\Sviluppo"

# Verificar que existe el directorio
if (-not (Test-Path $projectRoot)) {
    Write-Error-Custom "No se encontró el directorio del proyecto: $projectRoot"
    Write-Status "Por favor, verifica que la ruta es correcta."
    Read-Host "Presiona Enter para salir"
    exit 1
}

Write-Status "📁 Directorio del proyecto: $projectRoot" $Cyan
Set-Location $projectRoot

################################################################################
# PASO 1: VERIFICAR .NET
################################################################################

Write-Header "PASO 1: Verificando .NET"

$dotnetVersion = dotnet --version
Write-Success "✅ .NET versión encontrada: $dotnetVersion"

if (-not ($dotnetVersion -match "9\.")) {
    Write-Error-Custom "Se requiere .NET 9.0 o superior"
    Write-Status "Versión actual: $dotnetVersion"
    Read-Host "Presiona Enter para salir"
    exit 1
}

################################################################################
# PASO 2: LIMPIAR SOLUCIÓN
################################################################################

Write-Header "PASO 2: Limpiando solución"

Write-Status "🧹 Ejecutando 'dotnet clean -c Release'..." $Yellow

try {
    $cleanOutput = dotnet clean -c Release 2>&1
    Write-Success "✅ Solución limpia"
} catch {
    Write-Error-Custom "Error al limpiar: $_"
    Read-Host "Presiona Enter para salir"
    exit 1
}

################################################################################
# PASO 3: COMPILAR SOLUCIÓN
################################################################################

Write-Header "PASO 3: Compilando solución"

Write-Status "🔨 Ejecutando 'dotnet build -c Release'..." $Yellow
Write-Status "⏳ Esto puede tomar 1-2 minutos..." $Yellow
Write-Host ""

try {
    $buildOutput = dotnet build -c Release 2>&1
    
    # Verificar si hay errores
    if ($LASTEXITCODE -ne 0) {
        Write-Error-Custom "❌ Error en la compilación"
        Write-Host $buildOutput
        Read-Host "Presiona Enter para salir"
        exit 1
    }
    
    Write-Success "✅ Compilación exitosa"
    
    # Mostrar resumen
    if ($buildOutput -match "Build succeeded") {
        Write-Success "✅ Todos los proyectos compilados correctamente"
    }
} catch {
    Write-Error-Custom "Error durante la compilación: $_"
    Read-Host "Presiona Enter para salir"
    exit 1
}

################################################################################
# PASO 4: INICIAR SERVICIOS
################################################################################

Write-Header "PASO 4: Iniciando servicios"

Write-Status "🚀 Iniciando API (puerto 5001)..." $Yellow

try {
    $apiPath = Join-Path $projectRoot "Accredia.GestioneAnagrafica.API"
    
    # Iniciar API en background
    $apiProcess = Start-Process -NoNewWindow -PassThru -FilePath "dotnet" `
        -ArgumentList "run -c Release" `
        -WorkingDirectory $apiPath
    
    Write-Success "✅ API iniciada (PID: $($apiProcess.Id))"
    Write-Status "📍 Swagger: https://localhost:5001/swagger" $Cyan
} catch {
    Write-Error-Custom "Error al iniciar API: $_"
    Read-Host "Presiona Enter para salir"
    exit 1
}

################################################################################
# PASO 5: ESPERAR A QUE API ESTÉ LISTA
################################################################################

Write-Header "PASO 5: Esperando a que API esté lista"

Write-Status "⏳ Esperando 5 segundos..." $Yellow

$maxAttempts = 10
$attempt = 0
$apiReady = $false

for ($i = 0; $i -lt 5; $i++) {
    Write-Status "⏳ Espera: $($i + 1)/5 segundos..." -Color $Yellow
    Start-Sleep -Seconds 1
}

Write-Success "✅ API debería estar lista"

################################################################################
# PASO 6: INICIAR SERVER
################################################################################

Write-Header "PASO 6: Iniciando Server"

Write-Status "🚀 Iniciando Server (puerto 7412)..." $Yellow

try {
    $serverPath = Join-Path $projectRoot "Accredia.GestioneAnagrafica.Server"
    
    Write-Host ""
    Write-Host "╔" + ("=" * 78) + "╗" -ForegroundColor $Green
    Write-Host "║" + " " * 20 + "✅ SERVICIOS INICIADOS" + " " * 36 + "║" -ForegroundColor $Green
    Write-Host "╚" + ("=" * 78) + "╝" -ForegroundColor $Green
    Write-Host ""
    
    Write-Status "📊 ACCESOS DISPONIBLES:" $Cyan
    Write-Status "  🌐 Web Blazor:     https://localhost:7412" $Green
    Write-Status "  📚 Swagger API:    https://localhost:5001/swagger" $Green
    Write-Status "  💚 Health Check:   https://localhost:7412/health" $Green
    Write-Status "  🔌 API Base:       https://localhost:5001" $Green
    Write-Host ""
    
    Write-Status "⏳ El servidor se ejecutará en la ventana actual..." $Yellow
    Write-Status "⏳ Cierra esta ventana para detener todos los servicios" $Yellow
    Write-Host ""
    
    # Ejecutar Server (bloqueante)
    Set-Location $serverPath
    dotnet run -c Release
    
} catch {
    Write-Error-Custom "Error al iniciar Server: $_"
    
    # Intentar matar API
    Write-Status "🛑 Deteniendo API..." $Yellow
    if ($null -ne $apiProcess -and -not $apiProcess.HasExited) {
        $apiProcess.Kill()
        Write-Status "API detenida"
    }
    
    Read-Host "Presiona Enter para salir"
    exit 1
}
