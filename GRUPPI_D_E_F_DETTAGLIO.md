# 📋 DETTAGLIO GRUPPI D, E, F - DA IMPLEMENTARE

## 📅 Data: 01 Novembre 2025

---

## 🟦 GRUPPO D - RISORSE UMANE (Schema: RisorseUmane)

### Entità da Implementare: 4

#### 1. **Dipendente** 👤
**Tabella**: `RisorseUmane.Dipendente`

**Campi Principali:**
- `DipendenteId` (PK)
- `EntitaAziendaleId` (FK) - riferimento all'entità aziendale
- `CodiceFiscale` (32 caratteri, required)
- `Matricola` (32 caratteri, required, univoco)
- `LoginID` (512 caratteri, required)
- `Mansione` (100 caratteri, required)
- `TitoloOnorificoId` (FK opzionale)
- `RepartoId` (FK opzionale)
- `TurnoId` (FK opzionale)
- Audit trail completo (CreatoDa, DataCreazione, etc.)
- Soft delete (DataCancellazione)
- Temporal validity (DataInizioValidita, DataFineValidita)
- UniqueRowId (GUID)

**Relazioni:**
- N:1 con EntitaAziendale
- N:1 con TitoloOnorifico (Tipologica)
- N:1 con Reparto
- N:1 con Turno

**Funzionalità da Implementare:**
- ✅ CRUD completo
- ✅ Validazione CF univoco
- ✅ Validazione Matricola univoca
- ✅ Soft delete
- ✅ Filtri: reparto, turno, mansione, CF
- ✅ Ricerca per matricola
- ✅ Lista dipendenti per reparto
- ✅ Lista dipendenti per turno

---

#### 2. **Dipartimento** 🏢
**Tabella**: `RisorseUmane.Dipartimento`

**Campi Principali:**
- `DipartimentoId` (PK)
- `Nome` (200 caratteri, required)
- `RepartoId` (FK opzionale)
- Audit trail completo
- Soft delete
- Temporal validity
- UniqueRowId (GUID)

**Relazioni:**
- N:1 con Reparto (opzionale)
- 1:N con Reparto (un dipartimento può contenere più reparti)

**Funzionalità da Implementare:**
- ✅ CRUD completo
- ✅ Validazione nome univoco
- ✅ Soft delete
- ✅ Lista reparti per dipartimento
- ✅ Gerarchia dipartimento-reparto

---

#### 3. **Reparto** 🏭
**Tabella**: `RisorseUmane.Reparto`

**Campi Principali:**
- `RepartoId` (PK)
- `Nome` (100 caratteri, required)
- `ManagerDipendenteId` (FK opzionale) - Manager del reparto
- Audit trail completo
- Soft delete
- Temporal validity
- UniqueRowId (GUID)

**Relazioni:**
- N:1 con Dipartimento (opzionale)
- N:1 con Dipendente (manager)
- 1:N con Dipendente (dipendenti del reparto)

**Funzionalità da Implementare:**
- ✅ CRUD completo
- ✅ Validazione nome univoco nel dipartimento
- ✅ Soft delete
- ✅ Assegnazione manager
- ✅ Lista dipendenti per reparto
- ✅ Cambio manager

---

#### 4. **Turno** ⏰
**Tabella**: `RisorseUmane.Turno`

**Campi Principali:**
- `TurnoId` (PK)
- `Descrizione` (100 caratteri, required) - es: "Mattina", "Pomeriggio", "Notte"
- `OraInizio` (TimeSpan, required) - es: 08:00
- `OraFine` (TimeSpan, required) - es: 16:00
- Audit trail completo
- Soft delete
- Temporal validity
- UniqueRowId (GUID)

**Relazioni:**
- 1:N con Dipendente (dipendenti del turno)

**Funzionalità da Implementare:**
- ✅ CRUD completo
- ✅ Validazione orari (OraInizio < OraFine)
- ✅ Validazione sovrapposizioni turni
- ✅ Soft delete
- ✅ Lista dipendenti per turno
- ✅ Calcolo durata turno

**Endpoints da Implementare (Gruppo D): ~16 endpoints**
```
POST   /api/dipendenti
GET    /api/dipendenti
GET    /api/dipendenti/{id}
GET    /api/dipendenti/by-matricola/{matricola}
GET    /api/dipendenti/by-reparto/{repartoId}
PUT    /api/dipendenti/{id}
DELETE /api/dipendenti/{id}

POST   /api/dipartimenti
GET    /api/dipartimenti
GET    /api/dipartimenti/{id}
PUT    /api/dipartimenti/{id}
DELETE /api/dipartimenti/{id}

POST   /api/reparti
GET    /api/reparti
GET    /api/reparti/{id}
GET    /api/reparti/by-dipartimento/{dipartimentoId}
PUT    /api/reparti/{id}
PUT    /api/reparti/{id}/manager (assegna manager)
DELETE /api/reparti/{id}

POST   /api/turni
GET    /api/turni
GET    /api/turni/{id}
PUT    /api/turni/{id}
DELETE /api/turni/{id}
```

---

## 🟨 GRUPPO E - TIPOLOGICHE (Schema: Tipologica)

### Caratteristiche delle Tipologiche
Le tabelle tipologiche sono **tabelle di lookup/codifica** utilizzate come riferimento da altre entità.
Sono generalmente **READ-ONLY** per gli utenti normali, gestite solo da amministratori.

### Entità da Implementare: 5

#### 1. **TipoEmail** 📧
**Tabella**: `Tipologica.TipoEmail`

**Campi:**
- `TipoEmailId` (PK)
- `Codice` (100 caratteri) - es: "LAVORO", "PERSONALE", "PEC"
- `Descrizione` (400 caratteri) - es: "Email Lavoro", "Email Personale", "PEC"
- Audit trail completo
- Soft delete
- Temporal validity

**Utilizzata da**: Email

**Valori Tipici:**
- LAVORO - Email Lavoro
- PERSONALE - Email Personale
- PEC - Posta Elettronica Certificata
- ALTRO - Altro tipo di email

---

#### 2. **TipoTelefono** 📱
**Tabella**: `Tipologica.TipoTelefono`

**Campi:**
- `TipoTelefonoId` (PK)
- `Codice` (100 caratteri) - es: "FISSO", "MOBILE", "FAX"
- `Descrizione` (400 caratteri)
- Audit trail completo
- Soft delete
- Temporal validity

**Utilizzata da**: Telefono

**Valori Tipici:**
- FISSO - Telefono Fisso
- MOBILE - Telefono Mobile/Cellulare
- FAX - Numero Fax
- UFFICIO - Telefono Ufficio
- CASA - Telefono Casa

---

#### 3. **TipoIndirizzo** 🏠
**Tabella**: `Tipologica.TipoIndirizzo`

**Campi:**
- `TipoIndirizzoId` (PK)
- `Codice` (100 caratteri) - es: "RESIDENZA", "DOMICILIO", "SEDE"
- `Descrizione` (400 caratteri)
- Audit trail completo
- Soft delete
- Temporal validity

**Utilizzata da**: PersonaIndirizzo

**Valori Tipici:**
- RESIDENZA - Residenza Anagrafica
- DOMICILIO - Domicilio
- SEDE_LEGALE - Sede Legale
- SEDE_OPERATIVA - Sede Operativa
- RECAPITO - Indirizzo di Recapito

---

#### 4. **TipoEnteAccreditamento** 🏛️
**Tabella**: `Tipologica.TipoEnteAccreditamento`

**Campi:**
- `TipoEnteAccreditamentoId` (PK)
- `Codice` (100 caratteri)
- `Descrizione` (510 caratteri)
- `UniqueRowId` (GUID)
- Audit trail completo
- Soft delete
- Temporal validity

**Utilizzata da**: EnteAccreditamento

**Valori Tipici:**
- ACCREDIA - Accredia
- ISO - Organismo ISO
- UNI - Ente Unificazione Italiano
- ALTRO - Altro tipo

---

#### 5. **TitoloOnorifico** 🎓
**Tabella**: `Tipologica.TitoloOnorifico`

**Campi:**
- `TitoloOnorificoId` (PK)
- `Codice` (20 caratteri) - es: "DOTT", "ING", "PROF"
- `Descrizione` (100 caratteri) - es: "Dottore", "Ingegnere"
- `TitoloMaschile` (100 caratteri) - es: "Dott.", "Ing."
- `TitoloFemminile` (100 caratteri) - es: "Dott.ssa", "Ing."
- Audit trail completo
- Soft delete
- Temporal validity

**Utilizzata da**: Persona, Dipendente

**Valori Tipici:**
- DOTT - Dottore/Dottoressa
- ING - Ingegnere
- PROF - Professore/Professoressa
- AVV - Avvocato
- GEOM - Geometra
- ARCH - Architetto
- DR - Dottor di Ricerca
- SIG - Signor/Signora (default)

**Endpoints da Implementare (Gruppo E): ~5 endpoints (READ-ONLY)**
```
GET /api/tipologiche/tipo-email
GET /api/tipologiche/tipo-telefono
GET /api/tipologiche/tipo-indirizzo
GET /api/tipologiche/tipo-ente-accreditamento
GET /api/tipologiche/titolo-onorifico

Opzionalmente (solo per admin):
POST   /api/tipologiche/{tipo}
PUT    /api/tipologiche/{tipo}/{id}
DELETE /api/tipologiche/{tipo}/{id}
```

---

## 🟩 GRUPPO F - INDIRIZZI (Schema: Persone/Accreditamento)

### Entità da Implementare: 2

#### 1. **PersonaIndirizzo** 🏡
**Tabella**: `Persone.PersonaIndirizzo`

**Campi:**
- `PersonaIndirizzoId` (PK)
- `PersonaId` (FK opzionale)
- `IndirizzoId` (FK opzionale)
- `TipoIndirizzoId` (FK) - Tipo indirizzo (Residenza, Domicilio, etc.)
- `EntitaAziendaleId` (FK opzionale)
- `Attivo` (bool) - Se l'indirizzo è attualmente attivo
- Audit trail completo
- Soft delete
- Temporal validity

**Relazioni:**
- N:1 con Persona
- N:1 con Indirizzo (da creare)
- N:1 con TipoIndirizzo (Tipologica)
- N:1 con EntitaAziendale

**Funzionalità:**
- ✅ Collegamento Persona-Indirizzo
- ✅ Gestione indirizzi multipli per persona
- ✅ Storicizzazione indirizzi
- ✅ Tipo indirizzo (Residenza, Domicilio, etc.)
- ✅ Attivazione/disattivazione indirizzo

---

#### 2. **Indirizzo** 📍
**Tabella**: Da creare (probabilmente `Persone.Indirizzo` o `Accreditamento.Indirizzo`)

**Campi Suggeriti:**
- `IndirizzoId` (PK)
- `Via` (200 caratteri)
- `NumeroCivico` (20 caratteri)
- `CAP` (10 caratteri)
- `Citta` (100 caratteri)
- `Provincia` (2 caratteri) - es: "RM", "MI"
- `Regione` (100 caratteri)
- `Stato` (100 caratteri) - default: "Italia"
- `ComuneId` (FK opzionale) - riferimento a tabella Comuni
- `Latitudine` (decimal, opzionale) - per geolocalizzazione
- `Longitudine` (decimal, opzionale) - per geolocalizzazione
- `Note` (500 caratteri)
- Audit trail completo
- Soft delete
- Temporal validity
- UniqueRowId (GUID)

**Relazioni:**
- 1:N con PersonaIndirizzo
- N:1 con Comune (se esiste tabella Comuni)

**Funzionalità:**
- ✅ CRUD completo indirizzi
- ✅ Validazione CAP (5 cifre per Italia)
- ✅ Validazione Provincia (sigla 2 caratteri)
- ✅ Geolocalizzazione (Lat/Lng)
- ✅ Ricerca per CAP
- ✅ Ricerca per città
- ✅ Normalizzazione indirizzo
- ✅ Verifica duplicati

**Endpoints da Implementare (Gruppo F): ~8 endpoints**
```
POST   /api/indirizzi
GET    /api/indirizzi
GET    /api/indirizzi/{id}
GET    /api/indirizzi/by-cap/{cap}
GET    /api/indirizzi/by-citta/{citta}
PUT    /api/indirizzi/{id}
DELETE /api/indirizzi/{id}

POST   /api/persone/{personaId}/indirizzi
GET    /api/persone/{personaId}/indirizzi
DELETE /api/persone/{personaId}/indirizzi/{indirizzoId}
PUT    /api/persone/{personaId}/indirizzi/{indirizzoId}/attiva
```

---

## 📊 RIEPILOGO IMPLEMENTAZIONI

### Gruppo D - Risorse Umane
- **Entità**: 4 (Dipendente, Dipartimento, Reparto, Turno)
- **Endpoints stimati**: ~20
- **Complessità**: Media-Alta
- **Relazioni**: Multiple (gerarchiche)

### Gruppo E - Tipologiche
- **Entità**: 5 (TipoEmail, TipoTelefono, TipoIndirizzo, TipoEnteAccreditamento, TitoloOnorifico)
- **Endpoints stimati**: ~5-15 (READ-ONLY base + admin)
- **Complessità**: Bassa
- **Note**: Principalmente lookup/dropdown

### Gruppo F - Indirizzi
- **Entità**: 2 (Indirizzo, PersonaIndirizzo)
- **Endpoints stimati**: ~10
- **Complessità**: Media
- **Funzionalità extra**: Geolocalizzazione

---

## 🎯 PRIORITÀ SUGGERITE

### Priorità 1 (Essenziale)
1. **Gruppo E - Tipologiche** ✨
   - Più semplice
   - Necessario per dropdown
   - Veloce da implementare (~2-3 ore)
   - Blocca altri sviluppi

### Priorità 2 (Importante)
2. **Gruppo F - Indirizzi** 🏠
   - Completa Gruppo C (Persone)
   - Necessario per dati completi persona
   - Stimato: ~4-5 ore

### Priorità 3 (Avanzato)
3. **Gruppo D - Risorse Umane** 👥
   - Più complesso
   - Relazioni multiple
   - Può essere sviluppato dopo
   - Stimato: ~6-8 ore

---

## 💡 SUGGERIMENTO

**Ordine di Implementazione Ottimale:**
```
1️⃣ Gruppo E (Tipologiche) - 2-3 ore
    ↓
2️⃣ Gruppo F (Indirizzi) - 4-5 ore
    ↓
3️⃣ Gruppo D (Risorse Umane) - 6-8 ore
```

**Totale stimato: 12-16 ore di sviluppo**

---

**Vuoi che proceda con uno di questi gruppi?** 
Consiglio di iniziare con **Gruppo E (Tipologiche)** perché è il più veloce e sblocca gli altri! 🚀
