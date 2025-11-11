# SCRIPTS AUTOMÁTICOS CREADOS - EJECUCIÓN LISTA

## ✅ 2 SCRIPTS CREADOS

### 1. RUN_APLICACION_AUTOMATICO.ps1 (PowerShell)
- Verificación automática de .NET
- Limpieza automática
- Compilación automática
- Inicia API automáticamente
- Inicia Server en ventana actual
- Muestra URLs

Ejecución:
```powershell
cd C:\Accredia\Sviluppo
.\RUN_APLICACION_AUTOMATICO.ps1
```

### 2. RUN_APLICACION_AUTOMATICO.bat (Batch)
- Mismo que PowerShell
- Más simple (doble-click)
- API en ventana separada
- Server en ventana actual

Ejecución:
```
Double-click: RUN_APLICACION_AUTOMATICO.bat
```

---

## ⏱️ FLUJO DE EJECUCIÓN

1. [5s] Verifica .NET
2. [10s] Limpia solución
3. [1-2m] Compila todo
4. [3s] Inicia API (puerto 5001)
5. [5s] Espera a que API esté lista
6. [3s] Inicia Server (puerto 7412)
7. ✅ LISTO

**Tiempo total:** 2-3 minutos

---

## 🎯 URLS DE ACCESO

- Web: https://localhost:7412
- Swagger: https://localhost:5001/swagger
- Health: https://localhost:7412/health
- API Base: https://localhost:5001

---

## ✅ VERIFICACIÓN

1. Abre navegador: https://localhost:7412
   ✅ ¿Ves página Blazor?

2. Abre: https://localhost:5001/swagger
   ✅ ¿Ves Swagger UI?

3. Abre F12 Console
   ✅ ¿Sin errores rojos?

---

## ⚠️ IMPORTANTE

❌ NO CIERRES la ventana del script
✅ Si la cierras, se detienen los servicios
✅ Para detener: Cierra la ventana o Ctrl+C

---

## Status

✅ Scripts creados
✅ Listos para ejecutar
✅ Totalmente automatizados
✅ Instrucciones incluidas
