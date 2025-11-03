# Guía Rápida - Obtener Token y Probar Endpoints

## 1. Obtener Token JWT

### Opción A: Con PowerShell

```powershell
$body = @{
    username = "admin"
    password = "password"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://localhost:5001/auth/login" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body `
    -SkipCertificateCheck

$token = $response.token
Write-Host "Token obtenido: $token"
```

### Opción B: Con curl (Git Bash / WSL)

```bash
TOKEN=$(curl -s -X POST https://localhost:5001/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}' \
  --insecure \
  | jq -r '.token')

echo "Token: $TOKEN"
```

### Opción C: Con Postman

1. **Crea una nueva solicitud POST**
2. **URL:** `https://localhost:5001/auth/login`
3. **Body (JSON):**
```json
{
  "username": "admin",
  "password": "password"
}
```
4. **Envía y copia el token de la respuesta**

---

## 2. Usar el Token en Requests

### Opción A: Con PowerShell

```powershell
# Después de obtener el token
$headers = @{
    Authorization = "Bearer $token"
}

# Obtener Enti Accreditamento
Invoke-RestMethod -Uri "https://localhost:5001/api/enti-accreditamento?page=1&pageSize=10" `
    -Headers $headers `
    -SkipCertificateCheck | ConvertTo-Json
```

### Opción B: Con curl

```bash
# Usa el TOKEN obtenido anteriormente
curl -X GET "https://localhost:5001/api/enti-accreditamento?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN" \
  --insecure
```

### Opción C: Con Postman

1. **Abre una nueva pestaña**
2. **Método:** GET
3. **URL:** `https://localhost:5001/api/enti-accreditamento?page=1&pageSize=10`
4. **Headers:**
   - **Key:** `Authorization`
   - **Value:** `Bearer {tu_token}`
5. **Envía**

---

## 3. Usar Swagger UI

### Opción más fácil:

1. **Accede a:** `https://localhost:5001/swagger`
2. **Busca el endpoint:** `POST /auth/login`
3. **Haz clic en "Try it out"**
4. **Ingresa las credenciales:**
   ```json
   {
     "username": "admin",
     "password": "password"
   }
   ```
5. **Haz clic en "Execute"**
6. **Copia el token de la respuesta**
7. **Haz clic en el botón "Authorize"** (arriba a la derecha)
8. **Ingresa:** `Bearer {tu_token}`
9. **Haz clic en "Authorize"**
10. **Ahora prueba cualquier endpoint protegido** ✅

---

## 4. Endpoints Disponibles

### Authentication
- `POST /auth/login` - Obtener token JWT ✅ (Sin autenticación)

### Enti Accreditamento
- `GET /api/enti-accreditamento` - Listar todos (requiere token)
- `GET /api/enti-accreditamento/{id}` - Obtener por ID (requiere token)
- `POST /api/enti-accreditamento` - Crear nuevo (requiere token)
- `PUT /api/enti-accreditamento/{id}` - Actualizar (requiere token)
- `DELETE /api/enti-accreditamento/{id}` - Eliminar (requiere token)

### Organismi Accreditati
- `GET /api/organismi-accreditati` - Listar todos (requiere token)
- `GET /api/organismi-accreditati/{id}` - Obtener por ID (requiere token)
- `POST /api/organismi-accreditati` - Crear nuevo (requiere token)
- `PUT /api/organismi-accreditati/{id}` - Actualizar (requiere token)
- `DELETE /api/organismi-accreditati/{id}` - Eliminar (requiere token)

### Rilasci Accreditamento
- `GET /api/rilasci-accreditamento` - Listar todos (requiere token)
- `GET /api/rilasci-accreditamento/{id}` - Obtener por ID (requiere token)
- `POST /api/rilasci-accreditamento` - Crear nuevo (requiere token)
- `PUT /api/rilasci-accreditamento/{id}` - Actualizar (requiere token)

---

## 5. Ejemplo Completo con PowerShell

```powershell
# 1. Obtener token
$loginBody = @{
    username = "admin"
    password = "password"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "https://localhost:5001/auth/login" `
    -Method Post `
    -ContentType "application/json" `
    -Body $loginBody `
    -SkipCertificateCheck

$token = $loginResponse.token
Write-Host "✅ Token obtenido: $($token.Substring(0,50))..."

# 2. Crear headers con token
$headers = @{
    Authorization = "Bearer $token"
}

# 3. Obtener Enti Accreditamento
Write-Host "`n📋 Obteniendo Enti Accreditamento..."
$entiResponse = Invoke-RestMethod -Uri "https://localhost:5001/api/enti-accreditamento?page=1&pageSize=10" `
    -Headers $headers `
    -SkipCertificateCheck

Write-Host "✅ Respuesta recibida:"
$entiResponse | ConvertTo-Json

# 4. Obtener Organismi Accreditati
Write-Host "`n📋 Obteniendo Organismi Accreditati..."
$organismiResponse = Invoke-RestMethod -Uri "https://localhost:5001/api/organismi-accreditati?page=1&pageSize=10" `
    -Headers $headers `
    -SkipCertificateCall

Write-Host "✅ Respuesta recibida:"
$organismiResponse | ConvertTo-Json
```

---

## 6. Solución de Problemas

### Error: "Invalid token"
- Verifica que el token sea válido
- Verifica que no haya caracteres extra
- Vuelve a generar un nuevo token

### Error: "Token expired"
- El token expira después de 1 hora
- Ejecuta el login nuevamente para obtener uno nuevo

### Error: "CORS error"
- La configuración CORS está habilitada
- Asegúrate de estar usando HTTPS

### Error: "Certificate error"
- En PowerShell: usa `-SkipCertificateCheck`
- En curl: usa `--insecure`
- En Postman: desactiva SSL verification

---

## ✅ Estado Actual

- ✅ Autenticación JWT implementada
- ✅ Token Bearer en Swagger UI
- ✅ Todos los endpoints protegidos con `RequireAuthorization()`
- ✅ CORS configurado
- ✅ Credenciales de prueba: admin/password

**¡La API está lista para usar!** 🚀
