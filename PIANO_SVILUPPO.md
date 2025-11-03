# 📋 Piano di Sviluppo Completo - Accredia.GestioneAnagrafica.API

**Data Creazione:** 1 Novembre 2025  
**Stato:** In Progresso  
**Obiettivo:** Implementare tutte le tabelle da SchemaFull.yaml

---

## 📊 Analisi Tabelle Mancanti

### ✅ COMPLETATE (2/4 schemi principali)
- [x] **Schema Organismi**
  - [x] EnteAccreditamento ✅ (completato + endpoint + DTO + validator)
  - [x] OrganismoAccreditato ✅ (modello creato)

---

### 🔄 IN CORSO (Accreditamento - 3/3 tabelle)
- [x] AmbitoApplicazione ✅
- [x] RilascioAccreditamento ✅
- [x] Documento ✅

---

### ⏳ DA COMPLETARE

#### Schema Persone (2/6 tabelle complete)
- [x] Persona ✅ (già esistente)
- [x] EntitaAziendale ✅ (già esistente)
- [ ] Email ✅ (nel file Persona.cs)
- [ ] Telefono ✅ (nel file Persona.cs)
- [ ] PersonaIndirizzo ✅ (nel file Persona.cs)
- [ ] EntitaAnagraficaContatto ❌ **DA CREARE**

#### Schema RisorseUmane (0/4 tabelle)
- [ ] Dipendente ❌ **DA CREARE**
- [ ] Dipartimento ❌ **DA CREARE**
- [ ] Reparto ❌ **DA CREARE**
- [ ] Turno ❌ **DA CREARE**

#### Schema Tipologica (1/6 tabelle)
- [ ] TipoEmail ❌ Parzialmente creato
- [ ] TipoTelefono ❌ **DA CREARE**
- [ ] TipoIndirizzo ❌ **DA CREARE**
- [ ] TipoEnteAccreditamento ❌ **DA CREARE**
- [ ] TitoloOnorifico ❌ **DA CREARE**

---

## 🎯 Strategia di Implementazione

### Fase 1: Modelli (Models/) ⏳ IN CORSO
Completare tutti i modelli Entity Framework per ogni tabella

### Fase 2: DTOs (DTOs/)
Per ogni entità principale creare:
- `{Entity}DTO.Create`
- `{Entity}DTO.Update`
- `{Entity}DTO.Response`

### Fase 3: Validators (Validators/)
FluentValidation per ogni DTO

### Fase 4: Endpoints (Endpoints/)
Carter endpoints per operazioni CRUD:
- Create
- Read (Get/GetAll)
- Update
- Delete (Soft Delete)

### Fase 5: DbContext
Aggiornare PersoneDbContext con tutti i DbSet e configurazioni

### Fase 6: AutoMapper
Configurare mapping tra Models e DTOs

---

## 📝 Priorità di Sviluppo

### **ALTA PRIORITÀ** (Core Business)
1. ✅ EnteAccreditamento (COMPLETATO)
2. ✅ OrganismoAccreditato (Modello creato)
3. ✅ RilascioAccreditamento (Modello creato)
4. ✅ AmbitoApplicazione (Modello creato)

### **MEDIA PRIORITÀ** (Dati anagrafici)
5. ⏳ EntitaAnagraficaContatto
6. ⏳ Tipologiche (Tutti)

### **BASSA PRIORITÀ** (HR)
7. ⏳ Dipendente
8. ⏳ Dipartimento/Reparto/Turno

---

## 🔧 Prossimi Step Immediati

1. **Completare Tipologiche.cs** con tutte le tabelle
2. **Creare RisorseUmane.cs** con Dipendente, Dipartimento, Reparto, Turno
3. **Aggiornare PersoneDbContext** con tutti i DbSet
4. **Creare DTOs** per OrganismoAccreditato, RilascioAccreditamento, AmbitoApplicazione
5. **Creare Endpoints** per le nuove entità

---

## 📈 Progresso Generale

| Componente | Completato | Totale | % |
|------------|------------|--------|---|
| **Modelli** | 6 | 22 | 27% |
| **DTOs** | 1 | 8 | 12% |
| **Validators** | 1 | 8 | 12% |
| **Endpoints** | 4 | 32 | 12% |
| **DbContext** | 50% | 100% | 50% |

**Progresso Complessivo: ~23%**

---

## ⚠️ Note Tecniche

- Tutti i modelli usano **Soft Delete** (DataCancellazione)
- Tutti i modelli hanno **Auditing** (CreatoDa, ModificatoDa)
- Tutti i modelli hanno **Temporal Validity** (DataInizioValidita, DataFineValidita)
- Schema segregation: ogni tabella ha il proprio schema SQL
- Tutti gli ID sono **Identity** eccetto le relazioni 1:1

---

## 🚀 Come Procedere

**Vuoi che completi:**
1. **Tutti i modelli rimasti** (più veloce, poi DTOs/Endpoints)
2. **Una entità completa per volta** (Modello → DTO → Validator → Endpoint)
3. **Solo le entità core** (quelle ad alta priorità)

**Conferma quale approccio preferisci o dimmi se vuoi modifiche al piano!**
