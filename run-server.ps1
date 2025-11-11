#!/usr/bin/env pwsh
# Script para compilar y ejecutar el servidor host Blazor WASM

Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║   🚀 COMPILANDO Y EJECUTANDO SERVIDOR HOST              ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝`n" -ForegroundColor Green

$webDir = "Accredia.GestioneAnagrafica.Web"
$serverDir = "Accredia.GestioneAnagrafica.Server"

# PASO 1: Limpiar proyectos
Write-Host "[PASO 1] Limpiando proyectos..." -ForegroundColor Yellow

Push-Location $webDir
dotnet clean -c Release 2>$null
Pop-Location

Push-Location $serverDir
dotnet clean -c Release 2>$null
Pop-Location

Write-Host "✅ Proyectos limpiados`n" -ForegroundColor Green

# PASO 2: Publicar Web Blazor
Write-Host "[PASO 2] Publicando Web Blazor..." -ForegroundColor Yellow

Push-Location $webDir
$publishResult = dotnet publish -c Release -o "..\$serverDir\wwwroot" 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ ERROR: Fallo en publicación del Web" -ForegroundColor Red
    Write-Host ($publishResult | Out-String)
    exit 1
}
Pop-Location

Write-Host "✅ Web publicado`n" -ForegroundColor Green

# PASO 3: Compilar servidor host
Write-Host "[PASO 3] Compilando servidor host..." -ForegroundColor Yellow

Push-Location $serverDir
$buildResult = dotnet build -c Release 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ ERROR: Fallo en compilación" -ForegroundColor Red
    Write-Host ($buildResult | Out-String)
    exit 1
}
Pop-Location

Write-Host "✅ Servidor compilado`n" -ForegroundColor Green

# PASO 4: Ejecutar servidor
Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║   ✅ TODO LISTO - EJECUTANDO SERVIDOR                   ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝`n" -ForegroundColor Green

Push-Location $serverDir
Write-Host "Ejecutando: dotnet run -c Release" -ForegroundColor Cyan
Write-Host "Acceder a: https://localhost:7412`n" -ForegroundColor Cyan

dotnet run -c Release

Pop-Location
