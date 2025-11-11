# Accredia.GestioneAnagrafica.Web

Aplicación Blazor WebAssembly con MudBlazor para la interfaz frontend de Accredia Gestione Anagrafica.

## Características

- **Blazor WebAssembly** - Aplicación Web interactiva del lado del cliente
- **MudBlazor** - Componentes Material Design profesionales
- **Responsive Design** - Funciona en desktop, tablet y móvil
- **Integración API** - Comunica con la API REST backend

## Estructura

```
wwwroot/          - Archivos estáticos (HTML, CSS, JS)
Pages/             - Páginas Blazor (.razor)
Components/        - Componentes Blazor reutilizables
Layouts/           - Layouts de página
css/               - Estilos personalizados
```

## Ejecución

```bash
cd Accredia.GestioneAnagrafica.Web
dotnet run
```

La aplicación estará disponible en `https://localhost:5001`

## Dependencias

- MudBlazor 6.20.0
- Accredia.GestioneAnagrafica.Shared

## Arquitectura Vertical Slice (VSA)

En una arquitectura VSA, cada "slice" contiene:
- Páginas específicas del dominio
- Componentes relacionados
- Servicios locales
- Referencias a Shared cuando sea necesario
