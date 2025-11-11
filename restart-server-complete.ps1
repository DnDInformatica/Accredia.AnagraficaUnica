#!/usr/bin/env pwsh
# Script PowerShell per FERMERE il server e riavviarlo completamente

Write-Host "════════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  🛑 FERMANDO IL SERVER - RIAVVIO COMPLETO" -ForegroundColor Yellow
Write-Host "════════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

# Fermazione server sulla porta 7413 e 7412
Write-Host "🔍 Cercando processi sulla porta 7413 e 7412..." -ForegroundColor Yellow
$process_7413 = Get-NetTCPConnection -LocalPort 7413 -ErrorAction SilentlyContinue
$process_7412 = Get-NetTCPConnection -LocalPort 7412 -ErrorAction SilentlyContinue

if ($process_7413) {
    Write-Host "   ⚠️  Processo trovato sulla porta 7413" -ForegroundColor Red
    $pid_7413 = $process_7413.OwningProcess
    Write-Host "   🛑 Terminando processo PID: $pid_7413" -ForegroundColor Yellow
    Stop-Process -Id $pid_7413 -Force -ErrorAction SilentlyContinue
    Write-Host "   ✅ Processo terminato" -ForegroundColor Green
} else {
    Write-Host "   ✅ Nessun processo sulla porta 7413" -ForegroundColor Green
}

if ($process_7412) {
    Write-Host "   ⚠️  Processo trovato sulla porta 7412" -ForegroundColor Red
    $pid_7412 = $process_7412.OwningProcess
    Write-Host "   🛑 Terminando processo PID: $pid_7412" -ForegroundColor Yellow
    Stop-Process -Id $pid_7412 -Force -ErrorAction SilentlyContinue
    Write-Host "   ✅ Processo terminato" -ForegroundColor Green
} else {
    Write-Host "   ✅ Nessun processo sulla porta 7412" -ForegroundColor Green
}

# Attendi 2 secondi
Write-Host ""
Write-Host "⏳ Attendendo 2 secondi..." -ForegroundColor Yellow
Start-Sleep -Seconds 2

# Pulisci
Write-Host ""
Write-Host "🧹 Pulizia progetti..." -ForegroundColor Yellow
Set-Location "C:\Accredia\Sviluppo"
dotnet clean --nologo -q

# Build
Write-Host ""
Write-Host "📦 Build del progetto..." -ForegroundColor Yellow
dotnet build -c Debug --nologo -q

# Avvio
Write-Host ""
Write-Host "════════════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host "🚀 AVVIANDO IL SERVER - CON HTTPCLIENT CORRETTO" -ForegroundColor Green
Write-Host "════════════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 URL: https://localhost:7412/" -ForegroundColor Cyan
Write-Host "🔐 Login: admin / password" -ForegroundColor Cyan
Write-Host "📍 API: https://localhost:7043/" -ForegroundColor Cyan
Write-Host ""
Write-Host "ℹ️  Il server dovrebbe connettersi all'API su PORTA 7043 (non 7001!)" -ForegroundColor Yellow
Write-Host ""
Write-Host "════════════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host ""

# Avvia il server
dotnet run --project Accredia.GestioneAnagrafica.Server --no-build

Write-Host ""
Write-Host "✅ Server terminato" -ForegroundColor Green
