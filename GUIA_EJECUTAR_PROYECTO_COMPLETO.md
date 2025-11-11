# 🚀 GUÍA COMPLETA - EJECUTAR TODO EL PROYECTO

## 📋 Componentes del Proyecto

```
Accredia.GestioneAnagrafica
├── API (Puerto 5001)
├── Web - Blazor WASM (se publica en Server)
└── Server - Host para Web (Puerto 7412/7413)
```

---

## ✅ OPCIÓN 1: Desde Visual Studio (RECOMENDADO)

### Paso 1: Abrir la Solution
1. Abre Visual Studio
2. File → Open → Project/Solution
3. Navega a: `C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.sln`
4. Abre

### Paso 2: Configurar Startup Projects
1. Solution Explorer → Click derecho en la Solution
2. Set Startup Projects
3. Selecciona "Multiple startup projects"
4. Configura:
   - **Accredia.GestioneAnagrafica.API** → Action: **Start**
   - **Accredia.GestioneAnagrafica.Server** → Action: **Start**

### Paso 3: Recargar Proyecto Server (IMPORTANTE)
1. Click derecho en **Accredia.GestioneAnagrafica.Server**
2. Selecciona "Unload Project"
3. Espera 2 segundos
4. Click derecho → "Reload Project"

### Paso 4: Limpiar y Compilar
1. Build → Clean Solution
2. Build → Build Solution
3. Espera a que termine (no debería haber errores)

### Paso 5: Ejecutar
1. Press **F5** o Debug → Start Debugging
2. Se abrirá Visual Studio y ejecutará ambos:
   - API en: `https://localhost:5001`
   - Server Web en: `https://localhost:7412`

### Paso 6: Acceder
- Web Blazor: **https://localhost:7412**
- Swagger API: **https://localhost:5001/swagger**

---

## ✅ OPCIÓN 2: Desde PowerShell (Scripts Automáticos)

### Paso 1: Abrir 2 PowerShell como Admin

**PowerShell 1 - Compilar todo:**
```powershell
cd C:\Accredia\Sviluppo

# Limpiar todo
dotnet clean -c Release

# Compilar todos los proyectos
dotnet build -c Release

# Ejecutar API en background
Start-Process -NoNewWindow -FilePath dotnet -ArgumentList `
  "run -c Release -p Accredia.GestioneAnagrafica.API"
```

**PowerShell 2 - Ejecutar Server:**
```powershell
cd C:\Accredia\Sviluppo
.\run-server.ps1
```

### Paso 2: Acceder
- Web: **https://localhost:7412**
- API: **https://localhost:5001/swagger**

---

## ✅ OPCIÓN 3: Scripts Batch (Más Simple)

### Ejecutar todo con un click:

**Crear archivo: `start-all.bat` en `C:\Accredia\Sviluppo\`**

```batch
@echo off
echo.
echo ====================================================================
echo  🚀 INICIANDO TODOS LOS PROYECTOS
echo ====================================================================
echo.

cd /d "%~dp0"

echo [1] Limpiando solución...
dotnet clean -c Release 2>nul

echo [2] Compilando...
dotnet build -c Release
if errorlevel 1 (
    echo ❌ Error en compilación
    pause
    exit /b 1
)

echo.
echo [3] Iniciando API...
start "API - https://localhost:5001" cmd /k "cd Accredia.GestioneAnagrafica.API && dotnet run -c Release"

echo [4] Esperando a que inicie la API...
timeout /t 3 /nobreak

echo [5] Iniciando Server...
cd Accredia.GestioneAnagrafica.Server
dotnet run -c Release

echo.
echo ====================================================================
echo Proyectos iniciados:
echo - API: https://localhost:5001
echo - Web: https://localhost:7412
echo - Swagger: https://localhost:5001/swagger
echo ====================================================================
echo.
```

**Ejecución:**
1. Guarda el archivo como `start-all.bat`
2. Doble click para ejecutar
3. Se abrirán automáticamente API y Server

---

## ✅ OPCIÓN 4: PowerShell Script (Profesional)

**Crear archivo: `start-all.ps1` en `C:\Accredia\Sviluppo\`**

```powershell
#!/usr/bin/env pwsh

Write-Host "`n╔════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║   🚀 INICIANDO TODO EL PROYECTO ACCREDIA                ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════╝`n" -ForegroundColor Green

$projectRoot = "C:\Accredia\Sviluppo"
Set-Location $projectRoot

# PASO 1: Limpiar
Write-Host "[1] Limpiando solución..." -ForegroundColor Yellow
dotnet clean -c Release 2>$null

# PASO 2: Compilar
Write-Host "[2] Compilando..." -ForegroundColor Yellow
$buildResult = dotnet build -c Release 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error en compilación" -ForegroundColor Red
    Write-Host ($buildResult | Out-String)
    Read-Host "Presiona Enter para salir"
    exit 1
}
Write-Host "✅ Compilación exitosa`n" -ForegroundColor Green

# PASO 3: Iniciar API
Write-Host "[3] Iniciando API (puerto 5001)..." -ForegroundColor Cyan
$apiProcess = Start-Process -NoNewWindow -PassThru -FilePath "dotnet" `
    -ArgumentList "run -c Release -p Accredia.GestioneAnagrafica.API"
Write-Host "✅ API iniciada (PID: $($apiProcess.Id))`n" -ForegroundColor Green

# PASO 4: Esperar a que API esté lista
Write-Host "[4] Esperando a que API esté lista..." -ForegroundColor Yellow
Start-Sleep -Seconds 3

# PASO 5: Iniciar Server
Write-Host "[5] Iniciando Server (puerto 7412)..." -ForegroundColor Cyan
Set-Location "$projectRoot\Accredia.GestioneAnagrafica.Server"
Write-Host "`n📍 Accesos disponibles:" -ForegroundColor Green
Write-Host "  • Web Blazor: https://localhost:7412" -ForegroundColor Green
Write-Host "  • Swagger API: https://localhost:5001/swagger" -ForegroundColor Green
Write-Host "  • Health Check: https://localhost:7412/health`n" -ForegroundColor Green

dotnet run -c Release
```

**Ejecución:**
```powershell
.\start-all.ps1
```

---

## 📊 VERIFICACIÓN - Todos los Servicios Funcionando

Una vez iniciado, verifica que TODO funcione:

### ✅ API Check
```
URL: https://localhost:5001/swagger
Debería mostrar: Swagger UI con todos los endpoints
```

### ✅ Web Check
```
URL: https://localhost:7412
Debería mostrar: Página Blazor sin errores
```

### ✅ Health Check
```
URL: https://localhost:7412/health
Response: {"status":"OK","timestamp":"..."}
```

### ✅ Network Check
Abre F12 → Network tab y verifica:
- index.html: 200 OK
- .css files: 200 OK
- .js files: 200 OK
- .wasm files: 200 OK

---

## 🎯 URLS DE ACCESO

| Componente | URL | Descripción |
|-----------|-----|-------------|
| Web Blazor | https://localhost:7412 | Aplicación principal |
| API Swagger | https://localhost:5001/swagger | Documentación API |
| Health Check | https://localhost:7412/health | Estado del servidor |
| API Base | https://localhost:5001 | Base URL de API |

---

## 🛑 DETENER TODO

### Opción 1: Visual Studio
- Debug → Stop Debugging (Shift+F5)

### Opción 2: PowerShell
```powershell
# Matar todos los procesos dotnet
taskkill /IM dotnet.exe /F
```

### Opción 3: Task Manager
- Busca "dotnet" en la lista de procesos
- Selecciona y click en "End Task"

---

## ⚠️ PROBLEMAS COMUNES Y SOLUCIONES

### Error: "Port 5001 already in use"
```powershell
# Matar proceso en puerto 5001
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

### Error: "Port 7412 already in use"
```powershell
netstat -ano | findstr :7412
taskkill /PID <PID> /F
```

### Error: CS0117 en Server
1. Visual Studio: Unload/Reload proyecto Server
2. Build → Clean Solution
3. Build → Build Solution

### La Web muestra página en blanco
1. Abre F12 Console
2. Verifica que no haya errores
3. Verifica Network tab (todos deben ser 200)

### API no responde
1. Verifica que el proceso dotnet esté corriendo
2. Verifica puerto 5001: `netstat -ano | findstr :5001`
3. Reconstruye: `dotnet clean && dotnet build -c Release`

---

## 📋 CHECKLIST DE EJECUCIÓN

### Antes de ejecutar:
- [ ] Visual Studio está cerrado (si no es necesario)
- [ ] .NET 9.0 está instalado (`dotnet --version`)
- [ ] No hay procesos dotnet corriendo (`tasklist | findstr dotnet`)

### Compilación:
- [ ] `dotnet build -c Release` sin errores
- [ ] No hay CS0117 u otros errores
- [ ] Proyectos construidos correctamente

### Ejecución:
- [ ] API iniciada en puerto 5001
- [ ] Server iniciado en puerto 7412
- [ ] https://localhost:7412 carga sin errores
- [ ] F12 Console sin errores
- [ ] Network tab: todos 200 OK

### Verificación:
- [ ] Web Blazor funciona
- [ ] Swagger API accesible
- [ ] Health check responde OK
- [ ] CORS funciona correctamente

---

## 🎊 RESULTADO FINAL ESPERADO

```
✅ API ejecutando en https://localhost:5001
✅ Web ejecutando en https://localhost:7412
✅ Swagger UI disponible
✅ Sin errores de compresión
✅ Assets cargan correctamente
✅ Aplicación completamente funcional
```

---

## 🚀 RESUMEN RÁPIDO

**Opción más rápida (Visual Studio):**
1. Abrir solution en Visual Studio
2. Set Multiple Startup Projects (API + Server)
3. Unload/Reload Server
4. Build Solution (Ctrl+Shift+B)
5. Press F5
6. Acceder a https://localhost:7412

**Tiempo total:** ~2 minutos

---

**Status**: ✅ PRONTO PARA EJECUTAR
**Todos los componentes**: ✅ LISTOS
**Documentación**: ✅ COMPLETA
