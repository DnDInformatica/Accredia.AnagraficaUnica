# 🔧 RISOLUZIONE: Swagger SchemaId Conflict Error

## ❌ ERRORE

```
Swashbuckle.AspNetCore.SwaggerGen.SwaggerGeneratorException: 
Can't use schemaId "$Create" for type "$Accredia.GestioneAnagrafica.API.DTOs.DipartimentoDTO+Create". 
The same schemaId is already used for type "$Accredia.GestioneAnagrafica.API.DTOs.AmbitoApplicazioneDTO+Create"
```

## 🔍 CAUSA

Il progetto usa **nested DTO classes** come pattern:
```csharp
public static class AmbitoApplicazioneDTO
{
    public class Create { ... }
    public class Update { ... }
}

public static class DipartimentoDTO
{
    public class Create { ... }
    public class Update { ... }
}
```

Swagger non riesce a distinguere i duplicati (`Create`, `Update`, etc.) tra diverse classi DTO perché genera schemaId con lo stesso nome.

## ✅ SOLUZIONE APPLICATA

Aggiunto alla sezione Swagger in `Program.cs`:

```csharp
// Fix per nested DTO classes con lo stesso nome (Create, Update, etc.)
options.CustomSchemaIds(type => 
{
    if (type.DeclaringType == null)
        return type.Name;
    return $"{type.DeclaringType.Name}_{type.Name}";
});
```

**Cosa fa**: Renomina gli schemaId per nested classes da:
- ❌ `Create` → `$Create` (conflitto)
- ✅ `Create` → `AmbitoApplicazioneDTO_Create` (univoco)
- ✅ `Create` → `DipartimentoDTO_Create` (univoco)

## 🚀 PROSSIMO STEP

```bash
dotnet clean
dotnet build
dotnet run
```

---

## ✅ VERIFICA

Nel browser:
```
http://localhost:5000/swagger
```

Dovresti vedere:
- ✅ Nessun errore di schema
- ✅ Tutti i tag e endpoint visibili
- ✅ Riesci a testare gli endpoint

---

## 📝 FILE MODIFICATO

**Program.cs** - Sezione Swagger (dopo riga 42):

```csharp
// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Gestione Organismi API",
        Version = "v1",
        Description = "API per la gestione completa di Enti, Organismi, Documenti, Email, Telefoni e Risorse Umane"
    });
    
    // Fix per nested DTO classes con lo stesso nome
    options.CustomSchemaIds(type => 
    {
        if (type.DeclaringType == null)
            return type.Name;
        return $"{type.DeclaringType.Name}_{type.Name}";
    });
});
```

---

**Status**: ✅ FIX APPLICATO - Ready to rebuild!
