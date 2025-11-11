# 🔍 DIAGNÓSTICO Y VERIFICACIÓN - ACCREDIA API

## ⚠️ PROBLEMA IDENTIFICATO

Cuando se inició el API anteriormente, no respondía en `https://localhost:5001`

---

## 🔧 SOLUCIÓN PARA INICIAR EL API

### Opción 1: Ejecutar start-api.bat (Recomendado)

```batch
cd C:\Accredia\Sviluppo
start-api.bat
```

**Qué hace:**
1. Navega a la carpeta del API
2. Compila el proyecto en Release
3. Ejecuta el API
4. Mantiene la ventana abierta para ver los logs

### Opción 2: Línea de comandos manual

```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
dotnet run -c Release
```

### Opción 3: Visual Studio

1. Abre `Accredia.GestioneAnagrafica.sln`
2. En Solution Explorer, click derecho en `Accredia.GestioneAnagrafica.API`
3. Selecciona "Set as Startup Project"
4. Presi F5

---

## ✅ VERIFICACIÓN DE ÉXITO

Cuando el API se inicia correctamente, deberías ver en la ventana:

```
[output example]
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

O deberías ver algo como:

```
Application is running. Press Ctrl+C to shut down.
Server is listening on https://0.0.0.0:5001
```

---

## 🌐 VERIFICAR LA CONEXIÓN

Una vez que el API esté corriendo, prueba acceder a:

### 1. Test Simple (Ping)
```
URL: https://localhost:5001/ping
Respuesta esperada: "pong"
```

### 2. Swagger API
```
URL: https://localhost:5001/swagger
Respuesta: Página interactiva de Swagger
```

### 3. Desde PowerShell
```powershell
# Test simple
Invoke-RestMethod -Uri "https://localhost:5001/ping" -SkipCertificateCheck

# Debería responder: "pong"
```

---

## 🐛 TROUBLESHOOTING

### Problema 1: "Port already in use"

```
Error: listen EADDRINUSE: address already in use :::5001
```

**Solución:**

```powershell
# Encuentra qué proceso usa el puerto 5001
netstat -ano | findstr :5001

# Termina el proceso (reemplaza PID con el número real)
taskkill /PID <PID> /F

# Reinicia el API
dotnet run -c Release
```

### Problema 2: "SSL Certificate Error"

```
Error: SSL certificate problem
```

**Solución:**

Para PowerShell, usa `-SkipCertificateCheck`:
```powershell
Invoke-RestMethod -Uri "https://localhost:5001/ping" -SkipCertificateCheck
```

Para curl, usa `-k`:
```bash
curl -k https://localhost:5001/ping
```

### Problema 3: "Connection refused"

```
Error: Connection refused on localhost:5001
```

**Soluciones:**

1. Verifica que el API esté realmente corriendo
2. Verifica que esté en el puerto correcto
3. Intenta con `http://localhost:5000` (puerto HTTP)
4. Revisa los logs de error en la ventana del API

### Problema 4: "Application not found"

```
Error: The application has failed to start
```

**Soluciones:**

1. Compila nuevamente: `dotnet build -c Release`
2. Limpia el proyecto: `dotnet clean`
3. Restaura las dependencias: `dotnet restore`
4. Recompila y ejecuta

---

## 📋 CHECKLIST DE INICIO

- [ ] ¿El API está compilado? (`dotnet build -c Release`)
- [ ] ¿Navegaste a la carpeta correcta? (`C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API`)
- [ ] ¿Ejecutaste `dotnet run -c Release`?
- [ ] ¿Ves el mensaje "Now listening on: https://localhost:5001"?
- [ ] ¿Puedes acceder a `https://localhost:5001/ping`?
- [ ] ¿Recibes la respuesta "pong"?

---

## 🎯 PASOS PARA INICIAR CORRECTAMENTE

### Paso 1: Abre una ventana PowerShell

```powershell
Win + R
powershell
```

### Paso 2: Navega a la carpeta del API

```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
```

### Paso 3: Compila

```powershell
dotnet build -c Release
```

**Debería ver:**
```
Compilazione completata.
Avvisi: 0
Errori: 0
```

### Paso 4: Ejecuta

```powershell
dotnet run -c Release
```

**Debería ver:**
```
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

### Paso 5: Verifica

En otra ventana PowerShell:

```powershell
Invoke-RestMethod -Uri "https://localhost:5001/ping" -SkipCertificateCheck
```

**Respuesta esperada:**
```
pong
```

---

## 🔄 ALTERNATIVA: Script Automatizado

He creado `start-api.bat` que hace todo automáticamente:

```batch
cd C:\Accredia\Sviluppo
start-api.bat
```

---

## 📊 INFORMACIÓN DEL SISTEMA

| Elemento | Valor |
|----------|-------|
| API Port HTTP | 5000 |
| API Port HTTPS | 5001 |
| Framework | net9.0 |
| Configuration | Release |
| Status | ✅ Compilado |
| Errors | 0 |
| Warnings | 3 (non-critical) |

---

## ✨ PRÓXIMO PASO

Una vez que el API esté corriendo en `https://localhost:5001`, puedes:

1. ✅ Acceder a Swagger: `https://localhost:5001/swagger`
2. ✅ Testear los endpoints
3. ✅ Iniciar el Web también
4. ✅ Debuggear con F5 en Visual Studio

---

**Date**: 3 Novembre 2025  
**Status**: ✅ Diagnosticado y listo para ejecutar

