# Accredia.AnagraficaUnica

Sistema di gestione anagrafica unica per enti accreditati, organismi e persone.

## 🎯 Descrizione

Backend API RESTful sviluppato in .NET 9 per la gestione completa di:
- **Enti di Accreditamento**
- **Organismi Accreditati**
- **Ambiti di Applicazione**
- **Rilasci di Accreditamento**
- **Anagrafica Persone**
- **Email e Telefoni**
- **Risorse Umane**

## 🏗️ Architettura

- **Framework**: .NET 9.0
- **Pattern**: Minimal APIs con Carter
- **Database**: SQL Server
- **ORM**: Entity Framework Core 9.0
- **Validazione**: FluentValidation
- **Documentazione**: Swagger/OpenAPI

## 📦 Struttura Progetto

```
GestioneOrganismi.Backend/
├── Config/              # Configurazioni (JWT, ecc.)
├── Data/                # DbContext e Migrations
├── DTOs/                # Data Transfer Objects
├── Endpoints/           # Minimal API Endpoints (Carter)
│   ├── AmbitiApplicazione/
│   ├── Email/
│   ├── EntiAccreditamento/
│   ├── OrganismiAccreditati/
│   ├── RilasciAccreditamento/
│   └── Telefono/
├── Models/              # Entità del dominio
├── Validators/          # FluentValidation validators
└── Responses/           # Response wrappers standardizzati
```

## 🗄️ Schema Database

Il sistema gestisce 5 schemi principali:
- **Accreditamento**: AmbitoApplicazione, RilascioAccreditamento, Documento
- **Organismi**: EnteAccreditamento, OrganismoAccreditato
- **Persone**: Persona, Email, Telefono, EntitaAziendale
- **RisorseUmane**: Dipendente, Reparto, Dipartimento, Turno
- **Tipologica**: Tabelle di lookup (TipoEmail, TipoTelefono, ecc.)

## 🚀 Quick Start

### Prerequisiti
- .NET 9 SDK
- SQL Server 2019+
- Visual Studio 2022 / VS Code

### Installazione

1. Clona il repository
```bash
git clone https://github.com/DnDInformatica/Accredia.AnagraficaUnica.git
cd Accredia.AnagraficaUnica
```

2. Configura la connection string in `appsettings.json`

3. Esegui le migrations (se necessario)
```bash
dotnet ef database update
```

4. Avvia l'applicazione
```bash
dotnet run
```

5. Apri Swagger UI: `https://localhost:5001/swagger`

## 📚 API Endpoints Principali

### Ambiti Applicazione
- GET/POST/PUT/DELETE `/api/ambiti-applicazione`

### Email & Telefoni
- GET/POST/PUT/DELETE `/api/email`
- GET/POST/PUT/DELETE `/api/telefoni`

### Enti & Organismi
- GET/POST/PUT/DELETE `/api/enti-accreditamento`
- GET/POST/PUT/DELETE `/api/organismi-accreditati`

## 🔒 Funzionalità

- ✅ Audit Trail completo
- ✅ Soft Delete
- ✅ Paginazione e ricerca
- ✅ Validazione con FluentValidation
- ✅ Temporal Tables ready
- ✅ Swagger/OpenAPI documentation

## 🛠️ Tecnologie

- .NET 9.0
- Entity Framework Core 9.0
- Carter (Minimal APIs)
- FluentValidation
- SQL Server

## 📄 Licenza

Proprietario: Accredia  
Sviluppato da: DnD Informatica

---

**Versione**: 1.0.0  
**Ultimo aggiornamento**: Novembre 2025
