# 🔧 CONFIGURAZIONE FILE .env PER SQL SERVER

## 📋 FILE CREATI

### 1. `.env.example` (Template)
Questo è il file template che mostra tutte le variabili disponibili.
**Non modificare questo file** - è per la documentazione.

### 2. `.env` (Configurazione Locale)
Questo è il file con le tue configurazioni personali.
**Modifica questo file** con i tuoi dati.
**Non committare su GitHub** - aggiungi a `.gitignore`

---

## 🔑 VARIABILI SQL SERVER

### Connection String con SQL Authentication
```env
DB_SERVER=localhost
DB_PORT=1433
DB_NAME=GestioneOrganismi.Persone
DB_USER=sa
DB_PASSWORD=your_sql_password_here
DB_INTEGRATED_SECURITY=false
DB_ENCRYPT=true
DB_TRUST_SERVER_CERTIFICATE=true
```

### Connection String con Windows Authentication (Consigliato)
```env
DB_SERVER=localhost
DB_PORT=1433
DB_NAME=GestioneOrganismi.Persone
DB_INTEGRATED_SECURITY=true
DB_ENCRYPT=true
DB_TRUST_SERVER_CERTIFICATE=true
```

---

## 🚀 STEP-BY-STEP CONFIGURAZIONE

### Step 1: Installa il Package
Aggiungi il package NuGet per leggere i file `.env`:

```bash
dotnet add package DotNetEnv
```

### Step 2: Modifica il `.env` con i tuoi dati

**Opzione A: Windows Authentication (CONSIGLIATO)**
```env
DB_SERVER=localhost
DB_PORT=1433
DB_NAME=GestioneOrganismi.Persone
DB_INTEGRATED_SECURITY=true
DB_ENCRYPT=true
DB_TRUST_SERVER_CERTIFICATE=true
```

**Opzione B: SQL Authentication**
```env
DB_SERVER=localhost
DB_PORT=1433
DB_NAME=GestioneOrganismi.Persone
DB_USER=sa
DB_PASSWORD=your_actual_password
DB_INTEGRATED_SECURITY=false
DB_ENCRYPT=true
DB_TRUST_SERVER_CERTIFICATE=true
```

### Step 3: Aggiorna il `Program.cs`

Sostituisci questa linea:
```csharp
builder.Configuration.GetConnectionString("PersoneDb_SqlServer")
```

Con:
```csharp
// Carica le variabili dal file .env
DotNetEnv.Env.Load();

// Costruisci la connection string dalle variabili d'ambiente
var connectionString = BuildConnectionString();

// Nel metodo DbContext:
builder.Services.AddDbContext<PersoneDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure())
);

// ===== HELPER METHOD =====
static string BuildConnectionString()
{
    var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
    var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "GestioneOrganismi.Persone";
    var integratedSecurity = Environment.GetEnvironmentVariable("DB_INTEGRATED_SECURITY") == "true";
    var encrypt = Environment.GetEnvironmentVariable("DB_ENCRYPT") ?? "true";
    var trustCert = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE") ?? "true";

    if (integratedSecurity)
    {
        return $"Server={server},{port};Database={database};Integrated Security=true;Encrypt={encrypt};TrustServerCertificate={trustCert};";
    }
    else
    {
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        return $"Server={server},{port};Database={database};User Id={user};Password={password};Encrypt={encrypt};TrustServerCertificate={trustCert};";
    }
}
```

### Step 4: Aggiungi `.env` a `.gitignore`

Crea o aggiorna il file `.gitignore`:
```
# Environment files
.env
.env.local
.env.*.local
```

### Step 5: Test della Connection

```bash
dotnet clean
dotnet build
dotnet run
```

Dovresti vedere nel terminale:
```
Now listening on: http://localhost:5000
```

Se vedi errori di connessione, verifica:
1. SQL Server è in esecuzione
2. Le credenziali sono corrette
3. Il database esiste

---

## 🔍 VERIFICA CONNECTION STRING

### Opzione 1: Via Code
Aggiungi un endpoint di test (SOLO per DEBUG):
```csharp
app.MapGet("/api/health/db", async (PersoneDbContext db) =>
{
    try
    {
        await db.Database.OpenConnectionAsync();
        await db.Database.CloseConnectionAsync();
        return Results.Ok("Database connection OK");
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Database error: {ex.Message}");
    }
});
```

Poi testa: `http://localhost:5000/api/health/db`

### Opzione 2: Via SQL Management Studio
Connetti manualmente al server con le stesse credenziali.

---

## 📝 VARIABILI DISPONIBILI NEL `.env`

| Variabile | Descrizione | Valore Default |
|-----------|-------------|-----------------|
| `DB_SERVER` | Host SQL Server | localhost |
| `DB_PORT` | Porta SQL Server | 1433 |
| `DB_NAME` | Nome database | GestioneOrganismi.Persone |
| `DB_USER` | Username (SQL Auth) | sa |
| `DB_PASSWORD` | Password (SQL Auth) | - |
| `DB_INTEGRATED_SECURITY` | Windows Auth | true |
| `DB_ENCRYPT` | Connessione crittata | true |
| `DB_TRUST_SERVER_CERTIFICATE` | Trust self-signed cert | true |
| `ASPNETCORE_ENVIRONMENT` | Ambiente (Development/Production) | Development |
| `ASPNETCORE_URLS` | URL binding | http://localhost:5000;https://localhost:5001 |
| `JWT_SECRET_KEY` | Chiave JWT | - |
| `JWT_ISSUER` | JWT Issuer | GestioneOrganismi |

---

## 🆘 TROUBLESHOOTING

### Errore: "Cannot connect to database"
**Soluzioni**:
1. Verifica SQL Server sia in esecuzione
2. Controlla il nome del server (usa `localhost` o `.`)
3. Verifica le credenziali
4. Controlla il firewall Windows

### Errore: "Invalid connection string"
**Soluzioni**:
1. Verifica la sintassi nel `.env`
2. Controlla che non ci siano spazi vuoti
3. Usa `;` per separare i parametri

### Errore: "Package DotNetEnv not found"
**Soluzione**:
```bash
dotnet add package DotNetEnv
```

---

## 🔐 SICUREZZA

**IMPORTANTE**: 
- ✅ NON committare il file `.env` su GitHub
- ✅ Usa Windows Authentication in produzione quando possibile
- ✅ Cambia le password di default
- ✅ Usa Azure Key Vault o simile per i secrets in produzione

---

## 📚 PROSSIMI STEP

1. Modifica il `.env` con i tuoi dati
2. Aggiungi `DotNetEnv` package: `dotnet add package DotNetEnv`
3. Aggiorna il `Program.cs` per leggere le variabili
4. Test la connessione
5. Aggiungi `.env` a `.gitignore`

---

**Status**: ✅ FILE .env CREATI E PRONTI!
