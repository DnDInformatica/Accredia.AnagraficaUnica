# GestioneOrganismi - Backend API

## 📋 ITERAZIONE 2: STRUTTURA PROGETTO + MODELLI ✅ COMPLETATA

### 📁 Struttura Cartelle Creata

```
Accredia.GestioneAnagrafica.API/
├── Responses/
│   ├── ApiResponse.cs          # Wrapper standard per API responses
│   └── PageResult.cs           # Paginazione standardizzata
├── DTOs/
│   ├── PersonaDTO.cs           # DTOs per Persona (Create, Update, Response)
│   └── EntiAccreditamentoDTO.cs (da creare in ITERAZIONE 3)
├── Models/
│   ├── Persona.cs              # Modelli DB con Soft Delete e Auditing
│   └── EnteAccreditamento.cs (da completare)
├── Data/
│   └── PersoneDbContext.cs     # DbContext segmentato per Persone
├── Config/
│   ├── JwtConfig.cs            # Configurazione JWT con Claims personalizzati
│   └── Permissions.cs          (incluso in JwtConfig.cs)
├── Validators/
│   └── CodiceFiscaleValidator.cs # Validatore per CF italiano ed estero
├── Endpoints/
│   ├── Persone/                # Endpoints per Persone
│   │   ├── GetPersoneEndpoint.cs
│   │   ├── CreatePersonaEndpoint.cs
│   │   ├── UpdatePersonaEndpoint.cs
│   │   └── DeletePersonaEndpoint.cs
│   └── EntiAccreditamento/     # Endpoints per Enti Accreditamento
├── Services/                   (da creare in ITERAZIONE 3)
├── Program.cs                  (da creare in ITERAZIONE 3)
├── appsettings.json            # Configurazione app
└── Accredia.GestioneAnagrafica.API.csproj
```

### 🎯 File Creati

#### 1. **Responses/ApiResponse.cs**
- ✅ Wrapper standardizzato `ApiResponse<T>` e `ApiResponse`
- ✅ Factory methods per Success/Error/Validation responses
- ✅ Campi: success, data, message, errors, timestamp, correlationId
- ✅ Compatibile con OpenAPI/Swagger

#### 2. **Responses/PageResult.cs**
- ✅ Classe `PageResult<T>` per paginazione
- ✅ Campi: data, totalRecords, pageNumber, pageSize, totalPages, hasNextPage, hasPreviousPage
- ✅ Factory methods per All Records e Empty results

#### 3. **DTOs/PersonaDTO.cs**
- ✅ `CreatePersonaRequest` con validazioni (Nome, Cognome, CF obbligatori)
- ✅ `UpdatePersonaRequest` estende Create
- ✅ `PersonaResponse` con tutti i dati persona
- ✅ `PersonaListItemResponse` versione semplificata per liste
- ✅ `ContattiPrincipaliResponse` per email/telefono/indirizzo principale
- ✅ Validazioni Data Annotations

#### 4. **Models/Persona.cs**
- ✅ Entity `Persona` con Soft Delete (DataCancellazione, CancellatoDa)
- ✅ Auditing fields (DataCreazione, CreatoDa, DataModifica, ModificatoDa)
- ✅ Temporal validity (DataInizioValidita, DataFineValidita)
- ✅ GUID (RowGuid)
- ✅ Properties computate: IsDeleted, IsActive
- ✅ Entity `EntitaAziendale` con relazione One-to-Many
- ✅ Entity `Email`, `Telefono`, `PersonaIndirizzo` con soft delete

#### 5. **Data/PersoneDbContext.cs**
- ✅ DbContext segmentato per bounded context "Persone"
- ✅ Configurazione EF Core con Query Filters per soft delete
- ✅ Support SQL Server e PostgreSQL
- ✅ Auto-update Audit fields in SaveChanges
- ✅ Factory design-time per migrations
- ✅ Indexes su campi critici

#### 6. **Config/JwtConfig.cs**
- ✅ `JwtConfig` classe per configurazione JWT
- ✅ `LoginRequest` e `LoginResponse` DTOs
- ✅ `UtenteJwtResponse` con userId, username, email, ruoli, permissions
- ✅ `CustomClaimTypes` - claims personalizzati
- ✅ `UserRoles` - Admin, User, Guest, Ispettore (+futuri)
- ✅ `Permissions` - CRUD e specifiche per dominio
- ✅ `PermissionsByRole` - mapping automatico permessi/ruoli

#### 7. **Validators/CodiceFiscaleValidator.cs**
- ✅ Validazione CF italiano con algoritmo ufficiale
- ✅ Supporto per "N/D", "ESTERO", "SCONOSCIUTO"
- ✅ Validazione codici internazionali
- ✅ Custom `ValidCodiceFiscaleAttribute` per DataAnnotations
- ✅ `ValidationResult` class per risultati

#### 8. **appsettings.json**
- ✅ Connection strings per SQL Server e PostgreSQL
- ✅ Configurazione JWT (segretkey, issuer, audience, expiration)
- ✅ CORS origins
- ✅ API settings (versione, page size)
- ✅ Logging configuration

#### 9. **Accredia.GestioneAnagrafica.API.csproj**
- ✅ Target .NET 9.0
- ✅ Package EF Core (SqlServer + PostgreSQL)
- ✅ Identity & Authentication
- ✅ FluentValidation
- ✅ Swagger/OpenAPI

---

## 🔧 Configurazione Iniziale

### 1. Crea il progetto
```bash
dotnet new web -n Accredia.GestioneAnagrafica.API
```

### 2. Installa NuGet packages
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Npgsql
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package FluentValidation
dotnet add package Swashbuckle.AspNetCore
```

### 3. Crea la struttura cartelle
```bash
mkdir Responses DTOs Models Data Config Validators Endpoints\Persone Endpoints\EntiAccreditamento Services
```

### 4. Aggiungi i file creati
- Copia tutti i file .cs nelle rispettive cartelle
- Aggiorna appsettings.json con i tuoi connection strings

---

## 📝 Validazioni Implementate

### CodiceFiscale
- ✅ Obbligatorio
- ✅ Formato italiano (16 char) con validazione algoritmo
- ✅ Valori speciali: "N/D", "ESTERO", "SCONOSCIUTO"
- ✅ Codici internazionali (5-30 caratteri alfanumerici)

### Persona
- ✅ Nome: obbligatorio, max 100 char
- ✅ Cognome: obbligatorio, max 100 char
- ✅ Genere: obbligatorio, valori M/F/O
- ✅ EntitaAziendaleId: obbligatorio, FK
- ✅ CodiceFiscale: validato come sopra

---

## 🚀 PROSSIMA ITERAZIONE: ITERAZIONE 3

### Cosa creeremo:
1. **Program.cs** - Configurazione DI, DbContext, CORS, JWT
2. **Services** - PersoneService, AuthService
3. **Endpoints** - Get, Create, Update, Delete per Persone
4. **Middleware** - Error handling, Logging
5. **FluentValidation** - Validatori per DTOs

---

## 🔐 Claims JWT Configurati

```csharp
// Standard claims
"sub"        -> subject (userId)
"email"      -> email
"name"       -> nomeCompleto

// Custom claims
"userId"     -> Identificativo univoco
"fullName"   -> Nome completo
"entitaAziendaleId" -> ID EntitaAziendale
"http://schemas.microsoft.com/ws/2008/06/identity/claims/role" -> Ruoli
"permission" -> Permessi specifici
```

---

## ✅ Checklist ITERAZIONE 2

- [x] Response wrapper standard creato
- [x] PageResult<T> implementato
- [x] DTOs Persona creati
- [x] Models DB con Soft Delete
- [x] DbContext segmentato
- [x] Configurazione JWT con claims personalizzati
- [x] Validatore CodiceFiscale
- [x] appsettings.json template
- [x] .csproj con tutte le dipendenze

---

## 📌 Note Importanti

1. **Database Provider**: Modifica in appsettings.json il campo `DatabaseProvider` (SqlServer o PostgreSQL)
2. **Connection Strings**: Aggiorna con i tuoi server locali
3. **JWT Secret Key**: Cambia in produzione! (min 32 caratteri)
4. **Query Filters**: I soft delete sono automatici via HasQueryFilter()
5. **Auditing**: DataCreazione/Modifica aggiornati automaticamente in SaveChanges()

---

## 🎯 PROSSIMI STEP

**ITERAZIONE 3** inizieremo con:
1. Configurazione Program.cs
2. Services layer
3. Endpoints per CRUD Persone
4. Authentication endpoint (login)

**Procediamo insieme step by step!**
