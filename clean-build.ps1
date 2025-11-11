#!/bin/bash
# Script di pulizia e ricostruzione progetto Accredia

Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "🧹 PULIZIA PROGETTO ACCREDIA" -ForegroundColor Yellow
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan

$projPath = "C:\Accredia\Sviluppo"

Write-Host ""
Write-Host "📁 Cartella progetto: $projPath" -ForegroundColor Green

# Pulizia Web
Write-Host ""
Write-Host "🧹 Pulizia: Accredia.GestioneAnagrafica.Web" -ForegroundColor Yellow
Remove-Item -Recurse -Force "$projPath\Accredia.GestioneAnagrafica.Web\obj" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "$projPath\Accredia.GestioneAnagrafica.Web\bin" -ErrorAction SilentlyContinue
Write-Host "✅ Eliminati obj e bin" -ForegroundColor Green

# Pulizia Server
Write-Host ""
Write-Host "🧹 Pulizia: Accredia.GestioneAnagrafica.Server" -ForegroundColor Yellow
Remove-Item -Recurse -Force "$projPath\Accredia.GestioneAnagrafica.Server\obj" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "$projPath\Accredia.GestioneAnagrafica.Server\bin" -ErrorAction SilentlyContinue
Write-Host "✅ Eliminati obj e bin" -ForegroundColor Green

# Pulizia API
Write-Host ""
Write-Host "🧹 Pulizia: Accredia.GestioneAnagrafica.API" -ForegroundColor Yellow
Remove-Item -Recurse -Force "$projPath\Accredia.GestioneAnagrafica.API\obj" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "$projPath\Accredia.GestioneAnagrafica.API\bin" -ErrorAction SilentlyContinue
Write-Host "✅ Eliminati obj e bin" -ForegroundColor Green

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "✅ PULIZIA COMPLETATA" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan

Write-Host ""
Write-Host "📦 Prossimi step:" -ForegroundColor Yellow
Write-Host "1. cd C:\Accredia\Sviluppo" -ForegroundColor Cyan
Write-Host "2. dotnet build -c Debug" -ForegroundColor Cyan
Write-Host "3. dotnet run --project Accredia.GestioneAnagrafica.Server" -ForegroundColor Cyan
Write-Host ""
