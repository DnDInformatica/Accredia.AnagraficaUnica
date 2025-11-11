# Script per avviare API e Web contemporaneamente

param(
    [string]$Environment = "Development",
    [string]$Configuration = "Debug"
)

Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║     ACCREDIA - AVVIO API E WEB CONTEMPORANEAMENTE         ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Percorsi dei progetti
$apiPath = "C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API"
$webPath = "C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web"

# Verifica se i percorsi esistono
if (-not (Test-Path $apiPath)) {
    Write-Host "[ERRORE] Cartella API non trovata: $apiPath" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $webPath)) {
    Write-Host "[ERRORE] Cartella Web non trovata: $webPath" -ForegroundColor Red
    exit 1
}

Write-Host "[INFO] Ambiente: $Environment" -ForegroundColor Yellow
Write-Host "[INFO] Configurazione: $Configuration" -ForegroundColor Yellow
Write-Host ""

# Funzione per avviare un progetto
function Start-Project {
    param(
        [string]$ProjectPath,
        [string]$ProjectName,
        [string]$Port
    )
    
    Write-Host "[STARTING] $ProjectName..." -ForegroundColor Yellow
    Write-Host "   Percorso: $ProjectPath" -ForegroundColor Gray
    Write-Host "   Porta: $Port" -ForegroundColor Gray
    
    # Apri una nuova finestra PowerShell e avvia il progetto
    Start-Process powershell.exe -ArgumentList "-NoExit", "-Command", "cd '$ProjectPath'; `$env:ASPNETCORE_ENVIRONMENT='$Environment'; dotnet run --configuration $Configuration" -WindowStyle Normal
    
    Write-Host "[OK] $ProjectName avviato in nuova finestra" -ForegroundColor Green
    Start-Sleep -Seconds 2
}

# Avvia API
Start-Project -ProjectPath $apiPath -ProjectName "Accredia.GestioneAnagrafica.API" -Port "5001"

# Avvia Web
Start-Project -ProjectPath $webPath -ProjectName "Accredia.GestioneAnagrafica.Web" -Port "62412"

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║              ENTRAMBI I PROGETTI SONO AVVIATI              ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""
Write-Host "API:      https://localhost:5001" -ForegroundColor Cyan
Write-Host "Web:      https://localhost:62412" -ForegroundColor Cyan
Write-Host "Swagger:  https://localhost:5001/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "Per fermare: chiudi le finestre PowerShell" -ForegroundColor Yellow
Write-Host ""

# Mantiene il prompt aperto
Read-Host "Premi ENTER per terminare"
