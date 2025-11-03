# TEST ENDPOINTS GRUPPO E: TIPOLOGICHE

## Prerequisiti
1. Compilare il progetto: `dotnet build`
2. Avviare il server: `dotnet run` (di default su http://localhost:5000)
3. Accedere a Swagger: http://localhost:5000/swagger

## Test Cases

### 1. GET /api/tipologiche (Aggregato - PRINCIPALE)
**Descrizione**: Recupera tutte le tipologiche in una singola richiesta (perfetto per il frontend).

**Request**:
```http
GET http://localhost:5000/api/tipologiche
```

**Expected Response (200 OK)**:
```json
{
  "tipiEmail": [
    {
      "tipoEmailId": 1,
      "codice": "PERS",
      "descrizione": "Email Personale",
      "dataCreazione": "2024-01-15T10:00:00Z",
      "dataInizioValidita": "2024-01-15T00:00:00Z",
      "dataFineValidita": "9999-12-31T23:59:59Z"
    },
    {
      "tipoEmailId": 2,
      "codice": "AZIEN",
      "descrizione": "Email Aziendale",
      "dataCreazione": "2024-01-15T10:05:00Z",
      "dataInizioValidita": "2024-01-15T00:00:00Z",
      "dataFineValidita": "9999-12-31T23:59:59Z"
    }
  ],
  "tipiTelefono": [...],
  "tipiIndirizzo": [...],
  "tipiEnteAccreditamento": [...],
  "titoliOnorifici": [...]
}
```

---

### 2. GET /api/tipologiche/tipi-email (Lista Paginata)
**Descrizione**: Recupera la lista paginata dei tipi di email.

**Request**:
```http
GET http://localhost:5000/api/tipologiche/tipi-email?page=1&pageSize=10
```

**Query Parameters**:
- `page` (default: 1) - Numero della pagina
- `pageSize` (default: 50) - Elementi per pagina

**Expected Response (200 OK)**:
```json
{
  "data": [
    {
      "tipoEmailId": 1,
      "codice": "PERS",
      "descrizione": "Email Personale",
      "dataCreazione": "2024-01-15T10:00:00Z",
      "dataInizioValidita": "2024-01-15T00:00:00Z",
      "dataFineValidita": "9999-12-31T23:59:59Z"
    }
  ],
  "totalRecords": 2,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

### 3. GET /api/tipologiche/tipi-email/{id} (Singolo Elemento)
**Descrizione**: Recupera i dettagli di un singolo tipo di email.

**Request**:
```http
GET http://localhost:5000/api/tipologiche/tipi-email/1
```

**Expected Response (200 OK)**:
```json
{
  "tipoEmailId": 1,
  "codice": "PERS",
  "descrizione": "Email Personale",
  "dataCreazione": "2024-01-15T10:00:00Z",
  "dataInizioValidita": "2024-01-15T00:00:00Z",
  "dataFineValidita": "9999-12-31T23:59:59Z"
}
```

**Expected Response (404 Not Found)**:
```json
{
  "success": false,
  "message": "Tipo Email non trovato"
}
```

---

### 4. GET /api/tipologiche/tipi-telefono (Lista Paginata)
**Descrizione**: Recupera la lista paginata dei tipi di telefono.

**Request**:
```http
GET http://localhost:5000/api/tipologiche/tipi-telefono?page=1&pageSize=20
```

**Expected Response (200 OK)**:
```json
{
  "data": [
    {
      "tipoTelefonoId": 1,
      "codice": "MOBILE",
      "descrizione": "Telefono Mobile",
      "dataCreazione": "2024-01-15T10:00:00Z",
      "dataInizioValidita": "2024-01-15T00:00:00Z",
      "dataFineValidita": "9999-12-31T23:59:59Z"
    }
  ],
  "totalRecords": 3,
  "pageNumber": 1,
  "pageSize": 20
}
```

---

### 5. GET /api/tipologiche/tipi-indirizzo (Lista Paginata)
**Request**:
```http
GET http://localhost:5000/api/tipologiche/tipi-indirizzo?page=1&pageSize=50
```

---

### 6. GET /api/tipologiche/tipi-ente-accreditamento (Lista Paginata)
**Request**:
```http
GET http://localhost:5000/api/tipologiche/tipi-ente-accreditamento?page=1&pageSize=50
```

**Note**: Questo tipo include anche il campo `uniqueRowId` (GUID).

---

### 7. GET /api/tipologiche/titoli-onorifici (Lista Paginata)
**Request**:
```http
GET http://localhost:5000/api/tipologiche/titoli-onorifici?page=1&pageSize=50
```

**Expected Response**:
```json
{
  "data": [
    {
      "titoloOnorificoId": 1,
      "codice": "DR",
      "descrizione": "Dottore",
      "titoloMaschile": "Dott.",
      "titoloFemminile": "Dott.ssa",
      "dataCreazione": "2024-01-15T10:00:00Z",
      "dataInizioValidita": "2024-01-15T00:00:00Z",
      "dataFineValidita": "9999-12-31T23:59:59Z"
    }
  ],
  "totalRecords": 5,
  "pageNumber": 1,
  "pageSize": 50
}
```

---

## Test con cURL

### Test Aggregato:
```bash
curl -X GET "http://localhost:5000/api/tipologiche" -H "accept: application/json"
```

### Test Lista Paginata:
```bash
curl -X GET "http://localhost:5000/api/tipologiche/tipi-email?page=1&pageSize=10" \
  -H "accept: application/json"
```

### Test Singolo Elemento:
```bash
curl -X GET "http://localhost:5000/api/tipologiche/tipi-email/1" \
  -H "accept: application/json"
```

### Test 404:
```bash
curl -X GET "http://localhost:5000/api/tipologiche/tipi-email/99999" \
  -H "accept: application/json"
```

---

## Test con Postman

1. Creare una nuova Collection: "Tipologiche"
2. Aggiungere le seguenti richieste:
   - **GET Aggregato**: `{{base_url}}/api/tipologiche`
   - **GET Email List**: `{{base_url}}/api/tipologiche/tipi-email?page=1&pageSize=10`
   - **GET Email by ID**: `{{base_url}}/api/tipologiche/tipi-email/1`
   - **GET Telefono List**: `{{base_url}}/api/tipologiche/tipi-telefono?page=1&pageSize=10`
   - **GET Indirizzo List**: `{{base_url}}/api/tipologiche/tipi-indirizzo?page=1&pageSize=10`
   - **GET Ente Accreditamento List**: `{{base_url}}/api/tipologiche/tipi-ente-accreditamento?page=1&pageSize=10`
   - **GET Titoli Onorifici List**: `{{base_url}}/api/tipologiche/titoli-onorifici?page=1&pageSize=10`

3. Impostare la variabile `base_url = http://localhost:5000`

---

## Verifiche Obbligatorie

- ✅ Endpoint aggregato `/api/tipologiche` ritorna tutte le tipologiche
- ✅ Paginazione funziona correttamente (page e pageSize)
- ✅ Ordinamento per Codice funziona
- ✅ GET singolo elemento ritorna 404 per ID non esistente
- ✅ Tutti gli endpoint hanno tag "Tipologiche" in Swagger
- ✅ Documentazione Swagger è visibile
- ✅ Response JSON usa camelCase per i nomi
- ✅ Campi DataCreazione, DataInizioValidita, DataFineValidita sono presenti
- ✅ TitoloOnorifico include TitoloMaschile e TitoloFemminile
- ✅ TipoEnteAccreditamento include UniqueRowId

---

## Dati di Test Consigliati (Script SQL per seed)

```sql
-- Schema: Tipologica

-- TipoEmail
INSERT INTO Tipologica.TipoEmail (Codice, Descrizione, DataCreazione, DataInizioValidita, DataFineValidita) VALUES
('PERS', 'Email Personale', GETUTCDATE(), GETUTCDATE(), '9999-12-31'),
('AZIEN', 'Email Aziendale', GETUTCDATE(), GETUTCDATE(), '9999-12-31');

-- TipoTelefono
INSERT INTO Tipologica.TipoTelefono (Codice, Descrizione, DataCreazione, DataInizioValidita, DataFineValidita) VALUES
('MOBILE', 'Telefono Mobile', GETUTCDATE(), GETUTCDATE(), '9999-12-31'),
('FISSO', 'Telefono Fisso', GETUTCDATE(), GETUTCDATE(), '9999-12-31'),
('FAX', 'Fax', GETUTCDATE(), GETUTCDATE(), '9999-12-31');

-- TipoIndirizzo
INSERT INTO Tipologica.TipoIndirizzo (Codice, Descrizione, DataCreazione, DataInizioValidita, DataFineValidita) VALUES
('RES', 'Residenza', GETUTCDATE(), GETUTCDATE(), '9999-12-31'),
('DOM', 'Domicilio', GETUTCDATE(), GETUTCDATE(), '9999-12-31'),
('LAVORO', 'Lavoro', GETUTCDATE(), GETUTCDATE(), '9999-12-31');

-- TitoloOnorifico
INSERT INTO Tipologica.TitoloOnorifico (Codice, Descrizione, TitoloMaschile, TitoloFemminile, DataCreazione, DataInizioValidita, DataFineValidita) VALUES
('DR', 'Dottore', 'Dott.', 'Dott.ssa', GETUTCDATE(), GETUTCDATE(), '9999-12-31'),
('ING', 'Ingegnere', 'Ing.', 'Ing.', GETUTCDATE(), GETUTCDATE(), '9999-12-31'),
('AVV', 'Avvocato', 'Avv.', 'Avv.', GETUTCDATE(), GETUTCDATE(), '9999-12-31');
```

---

## Note Importanti

1. **READ-ONLY**: Questi endpoint sono SOLO GET. Nessuna creazione/modifica da API.
2. **Paginazione Default**: `pageSize=50` se non specificato
3. **Ordinamento**: Sempre per `Codice` (ASC)
4. **Validità Temporale**: Sono inclusi i campi per tracking della validità temporale
5. **Frontend Integration**: L'endpoint `/api/tipologiche` è ottimale per dropdown/select nei form
