# Accredia.GestioneAnagrafica.Shared

Librería compartida (ClassLib) que contiene modelos, DTOs e interfaces comunes utilizadas por la API y la aplicación Web.

## Contenido

### Models/
- `ApiResponse.cs` - Clase base para respuestas estándar de API

### DTOs/
- Clases de transferencia de datos compartidas

## Uso

Esta librería es referenciada por:
- `Accredia.GestioneAnagrafica.API`
- `Accredia.GestioneAnagrafica.Web`

## Arquitectura Vertical Slice (VSA)

En una arquitectura VSA, el `Shared` contiene:
- DTOs comunes
- Modelos de dominio compartidos
- Interfaces comunes
- Utilities compartidas
