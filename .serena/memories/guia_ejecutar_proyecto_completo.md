# GUÍA COMPLETA PARA EJECUTAR TODO EL PROYECTO

## 🏗️ Arquitectura

```
API (puerto 5001)
├── 40+ endpoints REST
├── JWT Authentication
├── Swagger UI
└── Database access

Web - Blazor WASM
├── Cliente interactivo
├── Publicado en Server
└── Comunicación con API

Server - Host (puerto 7412/7413)
├── Sirve Web Blazor
├── CORS habilitado
├── Response Compression: OFF
└── Health Check: /health
```

## ✅ OPCIÓN 1: Visual Studio (Recomendado - 2 min)

1. Abrir solution: `C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.sln`
2. Set Multiple Startup Projects: API + Server (Start)
3. Unload/Reload proyecto Server
4. Build → Clean Solution
5. Build → Build Solution
6. F5 (ejecutar)
7. Acceder: https://localhost:7412

## ✅ OPCIÓN 2: PowerShell Script (Automático - 1 min)

```powershell
cd C:\Accredia\Sviluppo
.\start-all.ps1
```

Script incluido en: GUIA_EJECUTAR_PROYECTO_COMPLETO.md

## ✅ OPCIÓN 3: Batch Script (Más simple - 1 min)

Doble click en: start-all.bat

Script incluido en: GUIA_EJECUTAR_PROYECTO_COMPLETO.md

## 🎯 URLs de Acceso

- Web Blazor: https://localhost:7412
- Swagger API: https://localhost:5001/swagger
- Health Check: https://localhost:7412/health
- API Base: https://localhost:5001

## ✅ Verificación

- [ ] API funciona (Swagger visible)
- [ ] Web carga (sin errores)
- [ ] F12 Console limpia
- [ ] Network tab: todos 200 OK
- [ ] Health check responde OK

## ⚠️ Problemas Comunes

Puerto en uso:
```powershell
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

Error CS0117:
- Unload/Reload Server
- Build Clean + Build

## 📚 Documentación Completa

GUIA_EJECUTAR_PROYECTO_COMPLETO.md

Contiene: Scripts, troubleshooting, checklist completo

## Status

✅ Proyecto completamente listo
✅ Todos los componentes funcionales
✅ Documentación completa
✅ Scripts automáticos listos
