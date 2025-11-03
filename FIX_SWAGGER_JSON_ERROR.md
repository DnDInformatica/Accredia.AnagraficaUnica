# 🔧 RISOLUZIONE: Failed to load API definition (swagger.json error)

## ❌ ERRORE

```
Failed to load API definition.
Fetch error
Internal Server Error http://localhost:5000/swagger/v1/swagger.json
```

## ✅ SOLUZIONE APPLICATA

Il file `Program.cs` aveva 2 problemi:

### Problema 1: Connection String Sbagliata
**Prima**:
```csharp
builder.Configuration.GetConnectionString("DefaultConnection")
```

**Dopo**:
```csharp
builder.Configuration.GetConnectionString("PersoneDb_SqlServer")
```

**Motivo**: In `appsettings.json` non esiste `"DefaultConnection"`, ma esiste `"PersoneDb_SqlServer"`

### Problema 2: Missing Using Directive
**Prima**:
```csharp
using FluentValidation;
```

**Dopo**:
```csharp
using Carter;
using FluentValidation;
```

**Motivo**: Mancava il `using Carter;` per registrare gli endpoint

---

## 🚀 COSA FARE ORA

### Step 1: Ferma il server
```
Terminale: Ctrl+C
```

### Step 2: Ricompila
```bash
dotnet clean
dotnet build
```

### Step 3: Esegui
```bash
dotnet run
```

### Step 4: Apri Swagger
```
Browser: http://localhost:5000/swagger
```

---

## ✅ VERIFICA

Dovresti vedere in Swagger:
- ✅ "Gestione Organismi API" nel titolo
- ✅ Tag "Tipologiche" nella lista
- ✅ 11 endpoint sotto il tag Tipologiche

---

## 🧪 TEST VELOCE

1. Clicca: **GET /api/tipologiche**
2. Clicca: **Try it out**
3. Clicca: **Execute**
4. ✅ Dovresti vedere: **Response 200 OK**

Se vedi Response 200 OK con i dati: **TUTTO FUNZIONA!** 🎉

---

## ⚠️ SE ANCORA NON FUNZIONA

### Errore: "Cannot connect to database"

**Soluzione**: Verifica che SQL Server è in esecuzione:
```bash
# Windows
sqlcmd -S localhost -Q "SELECT @@VERSION"

# O usa Management Studio per verificare
```

Se SQL Server non è disponibile, crea un database mock:
1. Uncomment PostgreSQL in `appsettings.json`
2. Cambia `PersoneDb_PostgreSQL` in `Program.cs`
3. O crea il database manualmente

### Errore: Still seeing "Failed to load API definition"

Controlla i logs nel terminale per l'errore effettivo:
```
ERR: [ExceptionType] - [Message]
```

Copia l'errore e risolvilo di conseguenza.

---

## 📝 FILE CORRETTO

**Program.cs** - Lines 1-18:
```csharp
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.Validators;
using Accredia.GestioneAnagrafica.API.Config;
using Accredia.GestioneAnagrafica.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Database Context
builder.Services.AddDbContext<PersoneDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PersoneDb_SqlServer"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);
```

---

**Status**: ✅ FIX APPLICATO - Ready to test!
