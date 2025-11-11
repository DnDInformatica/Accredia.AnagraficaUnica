#!/usr/bin/env pwsh

# Script PowerShell per eseguire Playwright tests - Accredia Corporate Skill
# ==========================================================================

param(
    [switch]$Install = $false,
    [switch]$UI = $false,
    [switch]$Debug = $false,
    [switch]$Headed = $false,
    [switch]$Report = $false
)

Write-Host "`n" -ForegroundColor Cyan
Write-Host "╔════════════════════════════════════════════════════════╗" -ForegroundColor Magenta
Write-Host "║  Accredia - Playwright Tests                          ║" -ForegroundColor Magenta
Write-Host "║  Corporate Skill Verification                          ║" -ForegroundColor Magenta
Write-Host "╚════════════════════════════════════════════════════════╝" -ForegroundColor Magenta
Write-Host "`n"

# Verifica Node.js
Write-Host "[INFO] Verifica Node.js..." -ForegroundColor Cyan
$nodeVersion = node --version 2>$null
if ($null -eq $nodeVersion) {
    Write-Host "[ERROR] Node.js non trovato. Installalo da https://nodejs.org/" -ForegroundColor Red
    exit 1
}
Write-Host "[OK] Node.js $nodeVersion" -ForegroundColor Green

# Installa dipendenze se richiesto
if ($Install) {
    Write-Host "`n[INFO] Installazione dipendenze..." -ForegroundColor Cyan
    npm install
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Errore durante installazione" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "[INFO] Installazione browser Playwright..." -ForegroundColor Cyan
    npx playwright install
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Errore durante installazione browser" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "[OK] Dipendenze installate" -ForegroundColor Green
}

# Verifica che dipendenze siano installate
if (-not (Test-Path "node_modules")) {
    Write-Host "[WARN] node_modules non trovato. Installo..." -ForegroundColor Yellow
    npm install
}

# Verifica connessione Web app
Write-Host "`n[INFO] Verifica connessione Web app..." -ForegroundColor Cyan
try {
    $response = Invoke-WebRequest -Uri "https://localhost:7412" -SkipCertificateCheck -ErrorAction SilentlyContinue -TimeoutSec 5
    Write-Host "[OK] Web app disponibile (Status: $($response.StatusCode))" -ForegroundColor Green
} catch {
    Write-Host "[WARN] Web app potrebbe non essere disponibile su https://localhost:7412" -ForegroundColor Yellow
    Write-Host "       Assicurati che l'app Web sia avviata con F5 in Visual Studio" -ForegroundColor Yellow
}

# Esegui test
Write-Host "`n╔════════════════════════════════════════════════════════╗" -ForegroundColor Blue
Write-Host "║  ESECUZIONE TEST                                      ║" -ForegroundColor Blue
Write-Host "╚════════════════════════════════════════════════════════╝" -ForegroundColor Blue
Write-Host "`n"

$testCommand = "npx playwright test"

if ($UI) {
    Write-Host "[INFO] Modalità UI (interattiva)..." -ForegroundColor Cyan
    $testCommand = "npx playwright test --ui"
} elseif ($Debug) {
    Write-Host "[INFO] Modalità Debug..." -ForegroundColor Cyan
    $testCommand = "npx playwright test --debug"
} elseif ($Headed) {
    Write-Host "[INFO] Modalità Headed (browser visibile)..." -ForegroundColor Cyan
    $testCommand = "npx playwright test --headed"
} else {
    Write-Host "[INFO] Modalità Headless..." -ForegroundColor Cyan
}

Write-Host "Comando: $testCommand`n" -ForegroundColor Gray

& pwsh -NoProfile -Command $testCommand

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n[OK] Test completati con successo!" -ForegroundColor Green
} else {
    Write-Host "`n[ERROR] Alcuni test sono falliti (Exit Code: $LASTEXITCODE)" -ForegroundColor Red
}

# Mostra report se richiesto
if ($Report -or $LASTEXITCODE -ne 0) {
    Write-Host "`n[INFO] Apertura report HTML..." -ForegroundColor Cyan
    npx playwright show-report
}

Write-Host "`n"
