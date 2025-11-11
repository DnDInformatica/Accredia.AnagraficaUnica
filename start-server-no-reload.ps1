#!/usr/bin/env pwsh
# Script PowerShell per avviare Accredia Server senza Hot Reload

Write-Host "════════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  🚀 AVVIO SERVER ACCREDIA - SENZA HOT RELOAD" -ForegroundColor Yellow
Write-Host "════════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

$projPath = "C:\Accredia\Sviluppo"
Set-Location $projPath

Write-Host "🧹 Pulizia progetti..." -ForegroundColor Yellow
dotnet clean

Write-Host ""
Write-Host "📦 Build del progetto..." -ForegroundColor Yellow
dotnet build -c Debug

Write-Host ""
Write-Host "🔐 Avvio del server..." -ForegroundColor Green
Write-Host ""
Write-Host "ℹ️  URL: http://localhost:7413" -ForegroundColor Cyan
Write-Host "🔑 Credenziali: admin / password" -ForegroundColor Cyan
Write-Host ""
Write-Host "════════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

# Avvia il server senza Hot Reload
dotnet run --project Accredia.GestioneAnagrafica.Server --no-build --no-restore

Write-Host ""
Write-Host "✅ Server terminato" -ForegroundColor Green
