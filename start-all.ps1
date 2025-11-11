# Script PowerShell para iniciar API y Web en paralelo

param(
    [string]$Environment = "Development",
    [string]$Configuration = "Release"
)

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║      ACCREDIA SOLUTION - INICIANDO API Y WEB               ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Paths
$apiPath = "C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API"
$webPath = "C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web"

# Verificar rutas
if (-not (Test-Path $apiPath)) {
    Write-Host "[ERROR] Carpeta API no encontrada: $apiPath" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $webPath)) {
    Write-Host "[ERROR] Carpeta Web no encontrada: $webPath" -ForegroundColor Red
    exit 1
}

Write-Host "[INFO] Ambiente: $Environment" -ForegroundColor Yellow
Write-Host "[INFO] Configuracion: $Configuration" -ForegroundColor Yellow
Write-Host ""

# Función para iniciar un proyecto
function Start-Service {
    param(
        [string]$ServicePath,
        [string]$ServiceName,
        [string]$Port
    )
    
    $windowTitle = "Accredia $ServiceName"
    
    Write-Host "[INICIANDO] $ServiceName..." -ForegroundColor Yellow
    Write-Host "   Ruta: $ServicePath" -ForegroundColor Gray
    Write-Host "   Puerto: $Port" -ForegroundColor Gray
    
    # Crear script para ejecutar en PowerShell
    $scriptBlock = {
        param($path, $env, $config, $title)
        
        $host.UI.RawUI.WindowTitle = $title
        
        Set-Location $path
        Write-Host ""
        Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
        Write-Host "║  $title - INICIANDO" -ForegroundColor Green
        Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Green
        Write-Host ""
        
        Write-Host "[BUILD] Compilando..." -ForegroundColor Cyan
        dotnet build -c $config
        
        Write-Host ""
        Write-Host "[RUN] Ejecutando..." -ForegroundColor Cyan
        Write-Host ""
        
        $env:ASPNETCORE_ENVIRONMENT = $env
        
        dotnet run --configuration $config
        
        Write-Host ""
        Write-Host "Presiona una tecla para cerrar esta ventana..." -ForegroundColor Yellow
        $null = $host.UI.RawUI.ReadKey()
    }
    
    # Iniciar en nueva ventana PowerShell
    Start-Process PowerShell.exe -ArgumentList "-NoProfile", "-Command", {
        param($path, $env, $config, $title)
        
        $host.UI.RawUI.WindowTitle = $title
        
        Set-Location $path
        Write-Host ""
        Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
        Write-Host "║  $title - INICIANDO" -ForegroundColor Green
        Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Green
        Write-Host ""
        
        Write-Host "[BUILD] Compilando..." -ForegroundColor Cyan
        dotnet build -c $config
        
        Write-Host ""
        Write-Host "[RUN] Ejecutando..." -ForegroundColor Cyan
        Write-Host ""
        
        $env:ASPNETCORE_ENVIRONMENT = $env
        
        dotnet run --configuration $config
        
        Write-Host ""
        Write-Host "Presiona una tecla para cerrar esta ventana..." -ForegroundColor Yellow
        $null = $host.UI.RawUI.ReadKey()
    } -ArgumentList $ServicePath, $Environment, $Configuration, $windowTitle -NoNewWindow:$false
    
    Write-Host "[OK] $ServiceName iniciado en nueva ventana" -ForegroundColor Green
}

# Iniciar API
Start-Service -ServicePath $apiPath -ServiceName "API" -Port "5001"

# Esperar un poco antes de iniciar Web
Write-Host ""
Write-Host "[ESPERANDO] 2 segundos antes de iniciar Web..." -ForegroundColor Yellow
Start-Sleep -Seconds 2

# Iniciar Web
Start-Service -ServicePath $webPath -ServiceName "Web" -Port "62412"

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║              SERVICIOS INICIADOS                           ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""

Write-Host "PUERTOS Y URLS:" -ForegroundColor Cyan
Write-Host ""
Write-Host "  API:" -ForegroundColor Green
Write-Host "  ├─ HTTP:    http://localhost:5000" -ForegroundColor Gray
Write-Host "  ├─ HTTPS:   https://localhost:5001" -ForegroundColor Gray
Write-Host "  └─ Swagger: https://localhost:5001/swagger" -ForegroundColor Gray
Write-Host ""
Write-Host "  WEB:" -ForegroundColor Green
Write-Host "  ├─ HTTP:    http://localhost:62413" -ForegroundColor Gray
Write-Host "  └─ HTTPS:   https://localhost:62412" -ForegroundColor Gray
Write-Host ""

Write-Host "NOTA: Las ventanas permanecerán abiertas para ver los logs" -ForegroundColor Yellow
Write-Host ""

# Mantener la ventana principal abierta
Read-Host "Presiona ENTER para terminar"
