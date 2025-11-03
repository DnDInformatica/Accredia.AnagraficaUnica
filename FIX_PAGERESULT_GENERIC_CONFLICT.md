# 🔧 RISOLUZIONE: PageResult Generic Type Conflict

## ❌ ERRORE

```
Can't use schemaId "$PageResult`1" for type 
"PageResult`1[DocumentoDTO+List]". 
The same schemaId is already used for type 
"PageResult`1[AmbitoApplicazioneDTO+List]"
```

## 🔍 CAUSA

Il fix precedente (CustomSchemaIds) non trattava correttamente i **generic types**.

Swagger non riusciva a distinguere tra:
- `PageResult<AmbitoApplicazioneDTO.List>`
- `PageResult<DocumentoDTO.List>`

Perché entrambi generavano lo stesso schemaId: `$PageResult`1`

## ✅ SOLUZIONE APPLICATA

Migliorato il CustomSchemaIds in `Program.cs` per gestire:

1. **Generic Types** (es: `PageResult<T>`)
   - Estrae gli argomenti generici
   - Gestisce anche nested types dentro i generici
   - Crea schemaId univoco: `PageResult_AmbitoApplicazioneDTO_List`

2. **Nested Types** (es: `AmbitoApplicazioneDTO.Create`)
   - Mantiene il fix precedente: `AmbitoApplicazioneDTO_Create`

3. **Simple Types** (es: `Persona`)
   - Usa il nome semplice: `Persona`

## 📝 CODICE APPLICATO

```csharp
options.CustomSchemaIds(type => 
{
    // Handle generic types
    if (type.IsGenericType)
    {
        var genericArgs = type.GetGenericArguments();
        var genericArgNames = string.Join("_", genericArgs.Select(t => 
        {
            if (t.DeclaringType != null)
                return $"{t.DeclaringType.Name}_{t.Name}";
            return t.Name;
        }));
        var baseName = type.Name.Substring(0, type.Name.IndexOf('`'));
        return $"{baseName}_{genericArgNames}";
    }
    
    // Handle nested types
    if (type.DeclaringType != null)
        return $"{type.DeclaringType.Name}_{type.Name}";
    
    return type.Name;
});
```

## 🚀 PROSSIMO STEP

```bash
dotnet clean
dotnet build
dotnet run
```

---

## ✅ VERIFICA

Nel terminale:
```
Now listening on: http://localhost:5000
```

Nel browser:
```
http://localhost:5000/swagger
```

Dovresti vedere:
- ✅ Nessun errore di schema
- ✅ Tutti gli endpoint visibili
- ✅ Riesci a testare gli endpoint

---

## 🧪 TEST

1. Espandi qualsiasi endpoint con PageResult (es: GET /api/documenti)
2. Click **Try it out**
3. Click **Execute**
4. ✅ Dovresti vedere **Response 200 OK**

---

**Status**: ✅ FIX APPLICATO - Ready to rebuild!
