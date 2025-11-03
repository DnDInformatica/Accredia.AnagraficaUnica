# Script PowerShell para probar la API con autenticacion JWT
# Version mejorada con mejor manejo de errores

# Configuracion
$apiUrl = "https://localhost:5001"
$loginEndpoint = "$apiUrl/auth/login"

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Autenticacion JWT - Accredia API" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan

# Ignorar errores de certificado SSL
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

# ==================== PASO 1: LOGIN ====================
Write-Host "`nPASO 1: Obteniendo token JWT..." -ForegroundColor Yellow

try {
    $loginBody = @{
        username = "admin"
        password = "password"
    } | ConvertTo-Json
    
    Write-Host "Enviando request a: $loginEndpoint" -ForegroundColor Gray
    Write-Host "Body: $loginBody" -ForegroundColor Gray

    $loginResponse = Invoke-RestMethod `
        -Uri $loginEndpoint `
        -Method Post `
        -ContentType "application/json" `
        -Body $loginBody `
        -ErrorAction Stop

    Write-Host "Respuesta recibida:" -ForegroundColor Gray
    Write-Host ($loginResponse | ConvertTo-Json) -ForegroundColor Gray

    $token = $loginResponse.token
    $expiresIn = $loginResponse.expiresIn

    if ([string]::IsNullOrEmpty($token)) {
        Write-Host "Error: No se obtuvo token de la respuesta" -ForegroundColor Red
        Write-Host "Respuesta completa: $($loginResponse | ConvertTo-Json)" -ForegroundColor Red
        exit
    }

    Write-Host "Token obtenido exitosamente!" -ForegroundColor Green
    Write-Host "Expira en: $expiresIn segundos" -ForegroundColor Green
    Write-Host ("Token: " + $token.Substring(0,50) + "...") -ForegroundColor Green
}
catch {
    Write-Host "Error en login:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host $_.Exception.Response -ForegroundColor Red
    exit
}

# ==================== PASO 2: PROBAR ENDPOINTS ====================
Write-Host "`nPASO 2: Probando endpoints con token..." -ForegroundColor Yellow

$headers = @{
    Authorization = "Bearer $token"
    "Content-Type" = "application/json"
}

Write-Host "`nHeaders enviados:" -ForegroundColor Gray
Write-Host ($headers | ConvertTo-Json) -ForegroundColor Gray

# Enti Accreditamento
Write-Host "`nGET /api/enti-accreditamento" -ForegroundColor Cyan
try {
    $url = "$apiUrl/api/enti-accreditamento?page=1&pageSize=10"
    Write-Host "URL: $url" -ForegroundColor Gray
    
    $response = Invoke-RestMethod `
        -Uri $url `
        -Method Get `
        -Headers $headers `
        -ContentType "application/json" `
        -ErrorAction Stop

    Write-Host "Exito (200)" -ForegroundColor Green
    Write-Host "Respuesta: $($response | ConvertTo-Json)" -ForegroundColor Gray
}
catch {
    Write-Host "Error:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host "Status Code: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

# Organismi Accreditati
Write-Host "`nGET /api/organismi-accreditati" -ForegroundColor Cyan
try {
    $url = "$apiUrl/api/organismi-accreditati?page=1&pageSize=10"
    Write-Host "URL: $url" -ForegroundColor Gray
    
    $response = Invoke-RestMethod `
        -Uri $url `
        -Method Get `
        -Headers $headers `
        -ContentType "application/json" `
        -ErrorAction Stop

    Write-Host "Exito (200)" -ForegroundColor Green
    Write-Host "Respuesta: $($response | ConvertTo-Json)" -ForegroundColor Gray
}
catch {
    Write-Host "Error:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}

# Rilasci Accreditamento
Write-Host "`nGET /api/rilasci-accreditamento" -ForegroundColor Cyan
try {
    $url = "$apiUrl/api/rilasci-accreditamento?page=1&pageSize=10"
    Write-Host "URL: $url" -ForegroundColor Gray
    
    $response = Invoke-RestMethod `
        -Uri $url `
        -Method Get `
        -Headers $headers `
        -ContentType "application/json" `
        -ErrorAction Stop

    Write-Host "Exito (200)" -ForegroundColor Green
    Write-Host "Respuesta: $($response | ConvertTo-Json)" -ForegroundColor Gray
}
catch {
    Write-Host "Error:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}

# ==================== RESUMEN ====================
Write-Host "`n============================================" -ForegroundColor Cyan
Write-Host "PRUEBA COMPLETADA" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Cyan

Write-Host "`nRESUMEN:" -ForegroundColor Yellow
Write-Host "   - Autenticacion JWT: OK" -ForegroundColor Green
Write-Host "   - Token generado: $($token.Substring(0,50))..." -ForegroundColor Green
Write-Host "   - Valido por: $expiresIn segundos" -ForegroundColor Green

Write-Host "`nPROXIMOS PASOS:" -ForegroundColor Yellow
Write-Host "   1. Accede a https://localhost:5001/swagger" -ForegroundColor Cyan
Write-Host "   2. Haz clic en 'Authorize'" -ForegroundColor Cyan
Write-Host ("   3. Pega: Bearer " + $token.Substring(0,50) + "...") -ForegroundColor Cyan
Write-Host "   4. Prueba los endpoints" -ForegroundColor Cyan

Write-Host "`n============================================`n" -ForegroundColor Cyan
