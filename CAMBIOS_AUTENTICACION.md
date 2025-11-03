# Resumen de Cambios - Autenticación JWT

## Archivos Creados

### 1. `Endpoints/Auth/LoginEndpoint.cs`
- Endpoint POST `/auth/login` para obtener token JWT
- Implementa ICarterModule siguiendo la arquitectura del proyecto
- Genera tokens con claims: Subject, JWT ID, Name, Role
- Respuesta estructurada con LoginResponse
- Documentado con XML comments
- Visible en Swagger con AllowAnonymous

### 2. `appsettings.Development.json`
- Configuración específica para desarrollo
- Credenciales JWT para desarrollo
- CORS relajado para localhost
- Logging en Debug level

### 3. `AUTHENTICATION.md`
- Guía completa de autenticación
- Ejemplos de uso con curl
- Instrucciones para Swagger UI
- TODO list para futuras mejoras

## Archivos Modificados

### 1. `Program.cs`
- Importado `Microsoft.IdentityModel.Tokens`
- Configuración de JWT Bearer authentication
- Validación de issuer, audience y lifetime
- Configuración en Swagger UI para incluir Bearer token

### 2. `appsettings.json`
- Actualizada clave "SecretKey" a "Key"
- Actualizada "Audience" para ser consistente

## Características Implementadas

✅ **Endpoint de Login**
- POST `/auth/login`
- Credenciales: admin/password
- Retorna token JWT con expiration

✅ **Validación de Token**
- Firma del token
- Issuer validation
- Audience validation
- Lifetime validation
- Clock skew = 0 (tiempo sincronizado)

✅ **Integración con Swagger**
- Definición de seguridad Bearer
- Campo de entrada de token en Swagger UI
- Documentación del endpoint

✅ **Seguimiento de Arquitectura**
- Estructura de carpetas consistente
- Implementación de ICarterModule
- DTOs separados (LoginRequest, LoginResponse)
- Nombres de métodos claros
- Documentación con XML comments

## Cómo Usar

### 1. Compilar y ejecutar
```bash
dotnet clean
dotnet build
dotnet run
```

### 2. Obtener token
```bash
curl -X POST http://localhost:7000/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'
```

### 3. Usar token en solicitudes
```bash
curl -X GET http://localhost:7000/api/enti-accreditamento \
  -H "Authorization: Bearer {token}"
```

## Próximos Pasos Recomendados

1. **Migrar autenticación a BD**
   - Crear tabla Users
   - Hash de contraseñas con bcrypt
   - Verificar credenciales contra BD

2. **Implementar Roles y Permissions**
   - Definir roles del sistema
   - Atributos de autorización por rol
   - Proteger endpoints sensibles

3. **Refresh Tokens**
   - Token corto plazo (15 min)
   - Refresh token largo plazo (7 días)
   - Endpoint para renovar token

4. **Auditoría y Logging**
   - Registrar intentos de login
   - Logging de errores de autenticación
   - Tracking de cambios por usuario
