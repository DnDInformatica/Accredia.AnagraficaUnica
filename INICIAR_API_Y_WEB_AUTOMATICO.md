# 🚀 INICIAR API Y WEB AUTOMÁTICAMENTE

## ⚡ INICIO RÁPIDO (3 OPCIONES)

### OPCIÓN 1: Batch Script (Más Fácil) ⭐ RECOMENDADO

```batch
cd C:\Accredia\Sviluppo
start-all.bat
```

**Qué hace:**
1. Abre una nueva ventana para la API
2. Compila y ejecuta la API
3. Espera 3 segundos
4. Abre otra ventana para el Web
5. Compila y ejecuta el Web

**Resultado:**
- 2 ventanas separadas (API y Web)
- Cada una con sus logs
- Ambas ejecutándose simultáneamente

---

### OPCIÓN 2: PowerShell Script

```powershell
cd C:\Accredia\Sviluppo
.\start-all.ps1
```

**Ventajas:**
- Más control
- Colores en los logs
- Mejor visualización de errores

---

### OPCIÓN 3: Scripts Individuales

Si prefieres iniciar por separado:

```batch
REM Ventana 1 - API
start-api.bat

REM Ventana 2 - Web (después de 3 segundos)
start-web.bat
```

---

## 📊 PUERTOS Y URLS

Una vez iniciado:

```
API (Ventana 1)
├─ HTTP:    http://localhost:5000
├─ HTTPS:   https://localhost:5001
├─ Swagger: https://localhost:5001/swagger
└─ Test:    https://localhost:5001/ping

WEB (Ventana 2)
├─ HTTP:    http://localhost:62413
└─ HTTPS:   https://localhost:62412
```

---

## ✅ VERIFICACIÓN

### Ventana 1 (API)
Deberías ver:
```
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

### Ventana 2 (Web)
Deberías ver:
```
Now listening on: https://localhost:62412
Now listening on: http://localhost:62413
Application started. Press Ctrl+C to shut down.
```

---

## 🌐 ACCEDER A LAS APLICACIONES

### API Swagger
Abre en navegador:
```
https://localhost:5001/swagger
```

### Web App
Abre en navegador:
```
https://localhost:62412
```

---

## 🔄 FLUJO DE INICIO

```
Ejecuta start-all.bat
        ↓
┌─────────────────────────────────┐
│  Ventana 1: API                 │
├─────────────────────────────────┤
│ • Compilando...                 │
│ • Ejecutando...                 │
│ • Listening on 5001 ✓           │
└─────────────────────────────────┘
        ↓ (después 3 segundos)
┌─────────────────────────────────┐
│  Ventana 2: Web                 │
├─────────────────────────────────┤
│ • Compilando...                 │
│ • Ejecutando...                 │
│ • Listening on 62412 ✓          │
└─────────────────────────────────┘
        ↓
✅ AMBAS APLICACIONES CORRIENDO
```

---

## 🛑 DETENER LOS SERVICIOS

### Opción 1: Cierra las ventanas
- Cierra la ventana de API: `Ctrl+C` o cierra la ventana
- Cierra la ventana de Web: `Ctrl+C` o cierra la ventana

### Opción 2: Desde PowerShell (si necesitas detener forzosamente)
```powershell
# Encontrar procesos dotnet
Get-Process dotnet

# Terminar todos los procesos dotnet
Get-Process dotnet | Stop-Process -Force

# O terminar específicamente
taskkill /IM dotnet.exe /F
```

---

## 🐛 TROUBLESHOOTING

### Problema: "Port already in use"
```
Error: Address already in use :::5001
```

**Solución:**
```powershell
# Encuentra qué usa el puerto
netstat -ano | findstr :5001

# Termina el proceso (reemplaza PID)
taskkill /PID <PID> /F

# Reintenta
start-all.bat
```

### Problema: "Build failed"
```
Error: MSB3030: Could not copy the file
```

**Solución:**
```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.API
dotnet clean
dotnet restore
dotnet build -c Release
```

### Problema: Web no ve la API
```
Error: Connection refused on localhost:5001
```

**Solución:**
1. Verifica que la API esté corriendo en ventana 1
2. Usa `https://localhost:5001/ping` para verificar
3. Verifica los logs de la API

### Problema: HTTPS Certificate Error
```
Error: SSL certificate problem
```

**Solución:**
- Usa `http://localhost:5000` o `http://localhost:62413` (HTTP sin SSL)
- En navegador, acepta el riesgo de seguridad
- En PowerShell, usa `-SkipCertificateCheck`

---

## 📋 CHECKLIST DE INICIO

- [ ] ¿Estás en la carpeta `C:\Accredia\Sviluppo`?
- [ ] ¿Ejecutaste `start-all.bat` o `.\start-all.ps1`?
- [ ] ¿Se abrieron 2 ventanas?
- [ ] ¿Ves "Now listening on" en ambas ventanas?
- [ ] ¿Puedes acceder a `https://localhost:5001/ping`?
- [ ] ¿Puedes acceder a `https://localhost:62412`?

---

## 🎯 ARCHIVOS DISPONIBLES

| Script | Uso | Resultado |
|--------|-----|-----------|
| `start-all.bat` | Inicia API + Web (batch) | 2 ventanas |
| `start-all.ps1` | Inicia API + Web (PowerShell) | 2 ventanas |
| `start-api.bat` | Inicia solo API | 1 ventana |
| `start-web.bat` | Inicia solo Web | 1 ventana |

---

## 💡 CONSEJOS

✅ **Usa `start-all.bat`** para inicio rápido sin escribir comandos  
✅ **Mantén las ventanas minimizadas** pero visibles para ver logs  
✅ **En caso de error, mira los logs** en la ventana correspondiente  
✅ **Para debugging**, usa Visual Studio F5  
✅ **Para desarrollo**, ten ambos servicios corriendo en background  

---

## 🎊 RESULTADO FINAL

Cuando todo funciona correctamente:

```
┌──────────────────────────────────────────┐
│  Accredia API                            │
├──────────────────────────────────────────┤
│  ✓ Listening on https://localhost:5001  │
│  ✓ Swagger disponible                    │
│  ✓ Health check: /ping ✓                 │
└──────────────────────────────────────────┘

┌──────────────────────────────────────────┐
│  Accredia Web                            │
├──────────────────────────────────────────┤
│  ✓ Listening on https://localhost:62412 │
│  ✓ Conectado a la API                    │
│  ✓ Listo para usar                       │
└──────────────────────────────────────────┘

🎉 SISTEMA COMPLETO FUNCIONAL
```

---

**Ahora ejecuta:**
```batch
start-all.bat
```

**¡Y ambas aplicaciones se iniciarán automáticamente!** 🚀

