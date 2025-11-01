# Implementazione CRUD Ambiti Applicazione

## 📅 Data Implementazione
01 Novembre 2025

## 🎯 Obiettivo
Completare la gestione CRUD per l'entità **AmbitoApplicazione** nello schema **Accreditamento**.

## ✅ Componenti Implementati

### 1. **Endpoints API** (4 nuovi file)

#### `GetAmbitiApplicazioneEndpoint.cs`
- **GET /api/ambiti-applicazione** - Lista paginata con ricerca e filtri
  - Parametri: page, pageSize, search, attivo, orderBy
  - Ordinamento: codice, denominazione, datacreazione, ordine
  - Filtri: ricerca testuale, stato attivo/non attivo
  
- **GET /api/ambiti-applicazione/{id}** - Dettaglio singolo ambito
  - Restituisce tutti i campi inclusi audit trail
  
- **GET /api/ambiti-applicazione/lookup** - Lista semplificata per dropdown
  - Parametro: soloAttivi (default: true)
  - Ordinamento per campo Ordine

#### `CreateAmbitoApplicazioneEndpoint.cs`
- **POST /api/ambiti-applicazione** - Creazione nuovo ambito
  - Validazione con FluentValidation
  - Verifica univocità codice
  - Audit trail automatico
  - Restituisce 201 Created con location header

#### `UpdateAmbitoApplicazioneEndpoint.cs`
- **PUT /api/ambiti-applicazione/{id}** - Aggiornamento ambito
  - Validazione con FluentValidation
  - Verifica univocità codice (escluso record corrente)
  - Aggiornamento DataModifica automatico
  - Restituisce 200 OK con dati aggiornati

#### `DeleteAmbitoApplicazioneEndpoint.cs`
- **DELETE /api/ambiti-applicazione/{id}** - Cancellazione logica
  - Verifica utilizzo in RilasciAccreditamento
  - Soft delete (imposta DataCancellazione)
  - Disattivazione automatica (Attivo = false)
  - Restituisce 200 OK con messaggio di conferma

### 2. **Configurazione DbContext**


Aggiunte configurazioni Entity Framework in `PersoneDbContext.cs`:

```csharp
// AmbitoApplicazione Configuration
modelBuilder.Entity<AmbitoApplicazione>(entity =>
{
    entity.HasKey(e => e.AmbitoApplicazioneId);
    entity.Property(e => e.Codice).IsRequired().HasMaxLength(100);
    entity.Property(e => e.Denominazione).IsRequired().HasMaxLength(200);
    entity.Property(e => e.Descrizione).HasMaxLength(1000);
    entity.Property(e => e.Ordine).IsRequired();
    
    // Query Filter per Soft Delete
    entity.HasQueryFilter(a => a.DataCancellazione == null);
    
    // Indexes
    entity.HasIndex(e => e.Codice).IsUnique();
    entity.HasIndex(e => e.Ordine);
    entity.HasIndex(e => e.Attivo);
    entity.HasIndex(e => e.DataCancellazione);
});

// RilascioAccreditamento Configuration (aggiornata)
modelBuilder.Entity<RilascioAccreditamento>(entity =>
{
    // Relationship con AmbitoApplicazione
    entity.HasOne(e => e.AmbitoApplicazione)
        .WithMany()
        .HasForeignKey(e => e.AmbitoApplicazioneId)
        .OnDelete(DeleteBehavior.Restrict);
});
```

**UpdateAuditFields** aggiornato per includere AmbitoApplicazione:
- Impostazione automatica di DataCreazione e DataInizioValidita su INSERT
- Impostazione automatica di DataModifica su UPDATE

### 3. **Modifiche a Program.cs**
- Completato il file con la configurazione middleware e startup
- Aggiunto `app.MapCarter()` per registrare gli endpoints
- Configurazione Swagger, CORS, HTTPS

## 🔧 Funzionalità Implementate

### ✅ CRUD Completo
- Create ✅
- Read (lista paginata + dettaglio + lookup) ✅
- Update ✅
- Delete (soft delete) ✅

### ✅ Validazione
- FluentValidation per Create e Update
- Codice: obbligatorio, max 100 caratteri, formato alfanumerico con trattini
- Denominazione: obbligatoria, max 200 caratteri
- Descrizione: opzionale, max 1000 caratteri
- Ordine: obbligatorio, >= 0

### ✅ Sicurezza e Integrità
- Verifica univocità codice
- Verifica utilizzo in RilasciAccreditamento prima della cancellazione
- Soft delete (preserva i dati storici)
- Query filter automatico per DataCancellazione

### ✅ Ricerca e Filtri
- Ricerca testuale su: Codice, Denominazione, Descrizione
- Filtro per stato Attivo/Non Attivo
- Ordinamento personalizzabile: Ordine (default), Codice, Denominazione, DataCreazione

### ✅ Paginazione
- Parametri configurabili: page, pageSize
- Metadati completi nella risposta (TotalRecords, PageNumber, PageSize)

### ✅ Audit Trail
- DataCreazione + CreatoDa
- DataModifica + ModificatoDa
- DataCancellazione + CancellatoDa
- DataInizioValidita + DataFineValidita

### ✅ Risposte Standardizzate
- ApiResponse per messaggi ed errori
- PageResult per liste paginate
- DTOs specifici per ogni operazione

## 📋 DTO Utilizzati


- **AmbitoApplicazioneDTO.Create** - Per creazione
- **AmbitoApplicazioneDTO.Update** - Per aggiornamento
- **AmbitoApplicazioneDTO.Response** - Per dettaglio completo
- **AmbitoApplicazioneDTO.List** - Per lista paginata (campi essenziali)
- **AmbitoApplicazioneDTO.Lookup** - Per dropdown/select

## 🧪 Test delle API

### Esempi di chiamate

#### 1. GET Lista Paginata
```http
GET /api/ambiti-applicazione?page=1&pageSize=10&search=ISO&attivo=true&orderBy=ordine
```

#### 2. GET Dettaglio
```http
GET /api/ambiti-applicazione/1
```

#### 3. GET Lookup
```http
GET /api/ambiti-applicazione/lookup?soloAttivi=true
```

#### 4. POST Creazione
```http
POST /api/ambiti-applicazione
Content-Type: application/json

{
  "codice": "ISO17025",
  "denominazione": "Laboratori di prova e taratura",
  "descrizione": "Accreditamento secondo ISO/IEC 17025",
  "ordine": 10,
  "attivo": true
}
```

#### 5. PUT Aggiornamento
```http
PUT /api/ambiti-applicazione/1
Content-Type: application/json

{
  "codice": "ISO17025",
  "denominazione": "Laboratori di prova e taratura (aggiornato)",
  "descrizione": "Accreditamento secondo ISO/IEC 17025:2017",
  "ordine": 10,
  "attivo": true
}
```

#### 6. DELETE Cancellazione
```http
DELETE /api/ambiti-applicazione/1
```

## 🚀 Come Testare

### 1. Build del progetto
```bash
cd C:\Accredia\Sviluppo
dotnet build
```

### 2. Avvio dell'applicazione
```bash
dotnet run
```

### 3. Accesso a Swagger
Apri il browser su: `https://localhost:5001/swagger`

### 4. Test con Swagger UI
- Espandi il gruppo "AmbitiApplicazione"
- Testa tutti gli endpoint interattivamente
- Verifica le risposte e gli status code

## 📊 Struttura File

```
Endpoints/
└── AmbitiApplicazione/
    ├── GetAmbitiApplicazioneEndpoint.cs      (3 endpoint GET)
    ├── CreateAmbitoApplicazioneEndpoint.cs   (1 endpoint POST)
    ├── UpdateAmbitoApplicazioneEndpoint.cs   (1 endpoint PUT)
    └── DeleteAmbitoApplicazioneEndpoint.cs   (1 endpoint DELETE)
```

## ✅ Checklist Implementazione

- [x] Model AmbitoApplicazione verificato
- [x] DTOs verificati (Create, Update, Response, List, Lookup)
- [x] Validators verificati (Create, Update)
- [x] Endpoint GET lista paginata con ricerca e filtri
- [x] Endpoint GET dettaglio singolo
- [x] Endpoint GET lookup per dropdown
- [x] Endpoint POST creazione con validazione
- [x] Endpoint PUT aggiornamento con validazione
- [x] Endpoint DELETE cancellazione logica con verifica dipendenze
- [x] Configurazione DbContext per AmbitoApplicazione
- [x] Configurazione DbContext per RilascioAccreditamento
- [x] UpdateAuditFields aggiornato
- [x] Program.cs completato con middleware
- [x] Query filters per soft delete
- [x] Indexes per performance

## 🎉 Risultato

**Implementazione COMPLETA del CRUD per AmbitoApplicazione!**

Tutti gli endpoint sono pronti per essere utilizzati e testati tramite Swagger UI.

## 📝 Note

- Gli endpoints utilizzano Carter per Minimal APIs
- La validazione è gestita da FluentValidation
- Le risposte sono standardizzate con ApiResponse e PageResult
- Il soft delete è implementato con DataCancellazione
- Gli audit trail sono gestiti automaticamente dal DbContext
- La paginazione è configurabile con page e pageSize
- La ricerca è case-insensitive e cerca in più campi

## 🔜 Prossimi Passi

Ora che il **Gruppo A (Ambiti Applicazione)** è completato, possiamo procedere con:
- **Gruppo B**: Documenti (upload/download)
- **Gruppo C**: Persone (CRUD completo + Indirizzi + Contatti)
- **Gruppo D**: Risorse Umane
- **Gruppo E**: Tipologiche
- **Gruppo F**: Indirizzi
