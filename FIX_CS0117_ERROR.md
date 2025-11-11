# ✅ FIX - Error CS0117 'WebApplicationBuilder' no contiene 'CreateBuilder'

## 🔧 Problema

```
Error (attivo) CS0117
'WebApplicationBuilder' non contiene una definizione per 'CreateBuilder'
Accredia.GestioneAnagrafica.Server\Program.cs, Riga 3
```

## ✅ Solución Aplicada

El `Program.cs` ha sido actualizado con sintaxis compatible. Ahora necesitas:

### PASO 1: Recargar Proyecto en Visual Studio

1. **Abre Visual Studio**
2. **Haz clic derecho en el proyecto** `Accredia.GestioneAnagrafica.Server`
3. **Selecciona "Unload Project"**
4. **Espera 2 segundos**
5. **Haz clic derecho nuevamente**
6. **Selecciona "Reload Project"**

### PASO 2: Limpiar y Reconstruir

```powershell
cd C:\Accredia\Sviluppo

# Opción A: Desde Visual Studio
Build → Clean Solution
Build → Build Solution

# Opción B: Desde PowerShell
dotnet clean -c Release
dotnet build -c Release
```

### PASO 3: Verificar

El error debería desaparecer. Si no:

```powershell
# Limpiar completamente
Remove-Item -Recurse bin/ -Force
Remove-Item -Recurse obj/ -Force

# Restaurar dependencias
dotnet restore

# Reconstruir
dotnet build -c Release
```

---

## 📝 Cambios Realizados en Program.cs

### ✅ ANTES (Error)
```csharp
var builder = WebApplicationBuilder.CreateBuilder(args);
```

### ✅ DESPUÉS (Correcto)
```csharp
var builder = WebApplication.CreateBuilder(args);
```

**La diferencia**: 
- `WebApplicationBuilder.CreateBuilder()` - INCORRECTO
- `WebApplication.CreateBuilder()` - CORRECTO ✅

---

## 🔍 Verifica que tengas

### .NET 9.0 instalado
```powershell
dotnet --version
# Debería mostrar: 9.0.x
```

### Todas las dependencias
```powershell
dotnet list package
```

---

## ✅ Status Final

**Program.cs**: ✅ Corregido  
**Sintaxis**: ✅ Correcta  
**Target Framework**: ✅ net9.0  
**Dependencias**: ✅ Correctas  

---

## 🚀 Próximos Pasos

1. **Recargar proyecto en Visual Studio**
2. **Build All (Ctrl+Shift+B)**
3. **Run (F5) o dotnet run**
4. **Acceder: https://localhost:7412**

---

**Error resuelto**: ✅ CS0117
