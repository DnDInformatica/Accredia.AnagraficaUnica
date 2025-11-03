# Guía de Autenticación JWT

## Descripción General

El endpoint `/auth/login` permite obtener un token JWT para autenticar las siguientes solicitudes a la API.

## Configuración

### 1. Variables de configuración en `appsettings.json`

```json
{
  "Jwt": {
    "Key": "your-super-secret-key-min-32-characters-change-this-in-production!!!",
    "Issuer": "GestioneOrganismi",
    "Audience": "GestioneOrganismiUsers",
    "ExpirationMinutes": 60
  }
}
```

### 2. Credenciales por defecto para Testing

- **Username:** `admin`
- **Password:** `password`

⚠️ **TODO:** Implementar autenticación contra base de datos o Identity

## Flujo de autenticación

### 1. Obtener Token (POST)

```bash
curl -X POST http://localhost:7000/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password"
  }'
```

**Respuesta exitosa (200 OK):**

```json
{
  "success": true,
  "message": "Autenticazione riuscita",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600
}
```

**Respuesta fallida (401 Unauthorized):**

```json
{
  "success": false,
  "message": "Credenziali non valide"
}
```

### 2. Usar Token en Solicitudes Protegidas

Una vez obtenido el token, incluirlo en el header `Authorization`:

```bash
curl -X GET http://localhost:7000/api/enti-accreditamento \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

## Información del Token JWT

El token contiene los siguientes claims:

- **Sub (Subject):** Username del usuario
- **Jti (JWT ID):** ID único del token
- **Name:** Nombre del usuario
- **Role:** Rol del usuario (ej: "Administrator")
- **Exp (Expiration):** Tiempo de expiración (1 hora desde emisión)
- **Iat (Issued At):** Hora de emisión
- **Iss (Issuer):** "GestioneOrganismi"
- **Aud (Audience):** "GestioneOrganismiUsers"

## Integración en Swagger

En el Swagger UI (`/swagger`):

1. Haz clic en el botón **"Authorize"** (arriba a la derecha)
2. Ingresa el token en el formato: `Bearer {tu_token}`
3. Haz clic en **"Authorize"**
4. Ahora todas tus solicitudes incluirán automáticamente el token

## Middleware de Autenticación

El middleware de autenticación:

- Valida la firma del token
- Verifica el issuer
- Verifica la audiencia
- Comprueba la fecha de expiración
- Rechaza tokens inválidos con respuesta 401

## Próximos pasos

1. **Implementar autenticación contra BD:**
   - Crear tabla de usuarios
   - Validar username/password desde DB
   - Agregar hash de contraseñas (bcrypt)

2. **Agregar roles y permisos:**
   - Definir roles (Admin, Editor, Viewer)
   - Implementar autorización por roles
   - Proteger endpoints específicos

3. **Refresh tokens:**
   - Implementar tokens de refresco
   - Permitir renovación sin re-login

4. **Cambio de contraseña:**
   - Endpoint para cambiar contraseña
   - Validar contraseña actual
   - Enviar confirmación por email
