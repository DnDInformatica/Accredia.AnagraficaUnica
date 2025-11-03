# Comandi EntityFramework Core - Package Manager Console

## Configurazione iniziale

### 1. Imposta il startup project
```powershell
# In Visual Studio - Package Manager Console
# Assicurati di avere selezionato "Accredia.GestioneAnagrafica.API" come startup project
```

### 2. Crea la prima migration
```powershell
Add-Migration InitialCreate -Context PersoneDbContext -OutputDir "Data/Migrations"
```

### 3. Aggiorna il database
```powershell
Update-Database -Context PersoneDbContext
```

---

## Comandi comuni

### Creare una nuova migration
```powershell
# Per il bounded context Persone
Add-Migration NomeMigration -Context PersoneDbContext -OutputDir "Data/Migrations"

# Per il bounded context Accreditamento (quando creeremo il DbContext)
Add-Migration NomeMigration -Context AccreditamentoDbContext -OutputDir "Data/Migrations"
```

### Applicare le migrations al database
```powershell
# Aggiorna a una migration specifica
Update-Database -Migration NomeMigration -Context PersoneDbContext

# Aggiorna all'ultima migration
Update-Database -Context PersoneDbContext
```

### Rimuovere l'ultima migration (se non ancora applicata)
```powershell
Remove-Migration -Context PersoneDbContext
```

### Visualizzare lo script SQL della migration (senza applicare)
```powershell
Script-Migration -Context PersoneDbContext -From Migration1 -To Migration2
```

### Rollback a una migration precedente
```powershell
Update-Database -Migration NomeMigration -Context PersoneDbContext
```

---

## Comandi CLI (.NET CLI)

### Se preferisci usare .NET CLI invece di Package Manager Console

```bash
# Creare migration
dotnet ef migrations add InitialCreate --context PersoneDbContext --output-dir Data/Migrations

# Aggiornare database
dotnet ef database update --context PersoneDbContext

# Visualizzare script SQL
dotnet ef migrations script --context PersoneDbContext

# Rimuovere ultima migration
dotnet ef migrations remove --context PersoneDbContext
```

---

## Database Providers

### Per SQL Server
```powershell
# Nessuna configurazione speciale, il connectionstring in appsettings.json è sufficiente
```

### Per PostgreSQL
```powershell
# Assicurati di avere installato il package Npgsql in .csproj
# Il connectionstring deve essere PostgreSQL (vedi appsettings.json)
```

---

## Ordine operations comuni

### Setup iniziale
```powershell
1. Add-Migration InitialCreate -Context PersoneDbContext -OutputDir "Data/Migrations"
2. Update-Database -Context PersoneDbContext
```

### Aggiungere nuovi fields a una entity
```powershell
1. Modifica il model (es. Models/Persona.cs)
2. Add-Migration AddNewFieldName -Context PersoneDbContext -OutputDir "Data/Migrations"
3. Update-Database -Context PersoneDbContext
```

### Aggiungere nuova entità
```powershell
1. Crea la nuova model in Models/
2. Aggiungi DbSet<NuovaEntita> nel DbContext
3. Configura il modello in OnModelCreating()
4. Add-Migration AddNuovaEntita -Context PersoneDbContext -OutputDir "Data/Migrations"
5. Update-Database -Context PersoneDbContext
```

---

## Troubleshooting

### "The term 'Add-Migration' is not recognized"
- Assicurati di avere Package Manager Console aperto (Tools > NuGet Package Manager > Package Manager Console)
- Verifica che Accredia.GestioneAnagrafica.API sia il progetto predefinito

### "No database provider has been configured"
- Verifica che PersoneDbContextFactory sia implementato correttamente
- Controlla la connection string in appsettings.json

### Migration fallisce
```powershell
# Visualizza l'SQL che sta per essere eseguito
Script-Migration -Context PersoneDbContext

# Rollback all'ultima migration applicata
Update-Database -Migration PreviousMigrationName -Context PersoneDbContext

# Rimuovi la migration problematica
Remove-Migration -Context PersoneDbContext
```

---

## Notes

- Ogni migration è **idempotente** (può essere applicata più volte senza errori)
- Le migrations sono versionali (puoi rollback senza perdere dati strutturali)
- Usa `-Verbose` per vedere i dettagli:
  ```powershell
  Add-Migration NomeMigration -Context PersoneDbContext -Verbose
  ```
