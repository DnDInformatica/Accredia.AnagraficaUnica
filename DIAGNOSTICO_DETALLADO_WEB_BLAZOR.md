# 🔍 PROBLEMA IDENTIFICADO - WEB BLAZOR WEBASSEMBLY

## 📋 DIAGNÓSTICO

### El Problema:
El Web no muestra contenido porque falta el **servidor host** de ASP.NET Core.

### Causa Raíz:
El proyecto Web es **Blazor WebAssembly Standalone** (`Microsoft.NET.Sdk.BlazorWebAssembly`)
- No tiene un servidor ASP.NET Core host propio
- Necesita servir archivos estáticos como una SPA

---

## 🔧 SOLUCIONES DISPONIBLES

### SOLUCIÓN 1: Usar Blazor Web App (Recomendado para el futuro)
Convertir a Blazor Web App (servidor + cliente integrados)

### SOLUCIÓN 2: Crear un Servidor Host ASP.NET Core (Actual)
Crear un nuevo proyecto que aloje el Blazor WebAssembly

### SOLUCIÓN 3: Publicar como Static Files
Construir y publicar como sitio estático

---

## 📊 ESTADO ACTUAL

### Archivo .csproj Web:
```xml
<Project Sdk="Microsoft.NET.Sdk.BlazorWebAssembly">
    <!-- ↑ BLAZOR WEBASSEMBLY PURO - NO TIENE SERVIDOR -->
</Project>
```

### Lo que tiene:
- ✅ Componentes Blazor
- ✅ MudBlazor UI
- ✅ wwwroot (archivos estáticos)
- ❌ **FALTA: Servidor ASP.NET Core para servir los archivos**

---

## 🚀 SOLUCIÓN INMEDIATA

### OPCIÓN A: Ejecutar con DevServer de Blazor (Desarrollo)
```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet run
```

Esto debería usar `Microsoft.AspNetCore.Components.WebAssembly.DevServer`
para servir la aplicación en `https://localhost:7412`

**Nota**: Este es el servidor de desarrollo, NO para producción.

---

### OPCIÓN B: Crear un Servidor Host (Recomendado para Producción)

#### Paso 1: Crear un nuevo proyecto ASP.NET Core
```powershell
cd C:\Accredia\Sviluppo
dotnet new web -n Accredia.GestioneAnagrafica.Server
```

#### Paso 2: Configurar como Host para Blazor WebAssembly
En el `Program.cs` del servidor:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseDefaultFiles(); // Sirve index.html por defecto
app.UseStaticFiles();  // Sirve wwwroot
app.UseCors("AllowBlazor");

app.MapFallbackToFile("index.html"); // SPA fallback

app.Run();
```

#### Paso 3: Copiar wwwroot del Web al Servidor
```powershell
Copy-Item "Accredia.GestioneAnagrafica.Web\wwwroot\*" `
          "Accredia.GestioneAnagrafica.Server\wwwroot\" -Recurse
```

---

## 🎯 POR QUÉ NO FUNCIONA ACTUALMENTE

```
Usuario accede a: https://localhost:7412
         ↓
    Browser intenta cargar:
    ├─ GET /index.html ✓ (funciona)
    ├─ GET /css/bootstrap/bootstrap.min.css ❌ (404)
    ├─ GET /css/app.css ✓ (existe)
    ├─ GET /_content/MudBlazor/MudBlazor.min.css ❌ (404)
    ├─ GET /_framework/blazor.web.js ❌ (404)
    └─ GET /_framework/blazor.boot.json ❌ (404)
         ↓
    Falta la infraestructura que sirva estos archivos
```

---

## 📁 ARCHIVOS NECESARIOS FALTANTES

En `wwwroot`:
- ❌ `css/bootstrap/bootstrap.min.css` - Bootstrap no incluido
- ❌ `Accredia.GestioneAnagrafica.Web.styles.css` - CSS de estilos generados
- ❌ `_content/MudBlazor/*` - Archivos de MudBlazor
- ❌ `_framework/*` - Runtime de Blazor

Estos archivos se generan durante la **compilación** y construcción.

---

## ✅ VERIFICACIÓN DEL BUILD

Para que todo funcione, necesitas:

1. **Compilar el Web correctamente:**
   ```powershell
   cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
   dotnet build -c Release
   ```

2. **Verificar que se generaron los archivos:**
   - Revisar `bin/Release/net9.0/wwwroot/`
   - Debería contener:
     - `_framework/` (runtime Blazor)
     - `_content/` (dependencias de MudBlazor, etc.)
     - `css/` (estilos compilados)

3. **Publicar correctamente:**
   ```powershell
   dotnet publish -c Release -o publish
   ```

---

## 🚀 TEST CON PLAYWRIGHT - CONCLUSIÓN

### API: ✅ **COMPLETAMENTE FUNCIONAL**
- Ping responde correctamente
- Swagger documentación completa
- 15+ endpoints operativos

### Web: ⚠️ **REQUIERE INVESTIGACIÓN**
- Blazor WebAssembly sin servidor host
- Falta infraestructura para servir static files
- Necesita configuración adicional

---

## 📋 PRÓXIMOS PASOS

### Opción 1 (Corta): Probar DevServer
```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet run
# Esperar a ver si aparecen mensajes de compilación
```

### Opción 2 (Completa): Crear un servidor host
Ver sección "OPCIÓN B" arriba

### Opción 3 (Larga): Convertir a Blazor Web App
Modernizar el proyecto a .NET 9 Blazor Web App

---

## 🎯 RECOMENDACIÓN INMEDIATA

Ejecuta lo siguiente para ver si el DevServer inicia correctamente:

```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet run
# Deberías ver: "Now listening on: https://localhost:7412"
# Si lo ves, la aplicación está funcionando
```

Si ves mensajes de error, necesitaremos crear un servidor host.

---

**Data**: 3 Novembre 2025  
**Tool**: Playwright + Diagnóstico Manual  
**Status**: 🔍 Investigación Completada

