# 🚀 GUIDA RAPIDA: BUILD & TEST GRUPPO E

## 1️⃣ Compilare il Progetto

### Da Command Line
```bash
cd C:\Accredia\Sviluppo
dotnet build
dotnet run
```

### Da Visual Studio
1. Aprire `Accredia.GestioneAnagrafica.API.sln`
2. Premere `F5` o `Ctrl+Shift+B` per Build

**Output Atteso**:
```
Build succeeded in X.XXs
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:5001
```

---

## 2️⃣ Verificare Swagger

1. Aprire Browser: http://localhost:5000/swagger
2. Cercare tag "Tipologiche" nella lista
3. Espandere per vedere tutti gli endpoint

**Endpoint Visibili**:
- ✅ GET /api/tipologiche
- ✅ GET /api/tipologiche/tipi-email
- ✅ GET /api/tipologiche/tipi-email/{id}
- ✅ GET /api/tipologiche/tipi-telefono
- ✅ GET /api/tipologiche/tipi-telefono/{id}
- ✅ GET /api/tipologiche/tipi-indirizzo
- ✅ GET /api/tipologiche/tipi-indirizzo/{id}
- ✅ GET /api/tipologiche/tipi-ente-accreditamento
- ✅ GET /api/tipologiche/tipi-ente-accreditamento/{id}
- ✅ GET /api/tipologiche/titoli-onorifici
- ✅ GET /api/tipologiche/titoli-onorifici/{id}

---

## 3️⃣ Test Rapidi in Swagger

### Test 1: Aggregato (PRINCIPALE)
1. Espandere `GET /api/tipologiche`
2. Cliccare "Try it out"
3. Cliccare "Execute"
4. ✅ Verificare Response 200 OK con tutti i dati

### Test 2: Lista Paginata
1. Espandere `GET /api/tipologiche/tipi-email`
2. Cliccare "Try it out"
3. Impostare: `page=1`, `pageSize=10`
4. Cliccare "Execute"
5. ✅ Verificare Response 200 OK con PageResult

### Test 3: Singolo Elemento
1. Espandere `GET /api/tipologiche/tipi-email/{id}`
2. Cliccare "Try it out"
3. Impostare: `id=1`
4. Cliccare "Execute"
5. ✅ Verificare Response 200 OK con singolo elemento

### Test 4: 404 Not Found
1. Espandere `GET /api/tipologiche/tipi-email/{id}`
2. Cliccare "Try it out"
3. Impostare: `id=99999`
4. Cliccare "Execute"
5. ✅ Verificare Response 404 con messaggio di errore

---

## 4️⃣ Test da Postman

### Setup Postman
1. Aprire Postman
2. Creare Collection: "Tipologiche"
3. Aggiungere richieste (vedi sotto)

### Request 1: Get All Tipologiche (Aggregato)
```http
GET http://localhost:5000/api/tipologiche
```
**Expected**: 200 OK con 5 array di tipologiche

### Request 2: Get Tipi Email (Paginato)
```http
GET http://localhost:5000/api/tipologiche/tipi-email?page=1&pageSize=10
```
**Expected**: 200 OK con PageResult

### Request 3: Get Tipo Email (Singolo)
```http
GET http://localhost:5000/api/tipologiche/tipi-email/1
```
**Expected**: 200 OK con singolo elemento

### Request 4: Get Tipi Telefono
```http
GET http://localhost:5000/api/tipologiche/tipi-telefono?page=1&pageSize=10
```

### Request 5: Get Tipi Indirizzo
```http
GET http://localhost:5000/api/tipologiche/tipi-indirizzo?page=1&pageSize=10
```

### Request 6: Get Tipi Ente Accreditamento
```http
GET http://localhost:5000/api/tipologiche/tipi-ente-accreditamento?page=1&pageSize=10
```

### Request 7: Get Titoli Onorifici
```http
GET http://localhost:5000/api/tipologiche/titoli-onorifici?page=1&pageSize=10
```

---

## 5️⃣ Test da cURL

### Aggregato
```bash
curl -X GET "http://localhost:5000/api/tipologiche" ^
  -H "Content-Type: application/json"
```

### Lista Paginata
```bash
curl -X GET "http://localhost:5000/api/tipologiche/tipi-email?page=1&pageSize=10" ^
  -H "Content-Type: application/json"
```

### Singolo Elemento
```bash
curl -X GET "http://localhost:5000/api/tipologiche/tipi-email/1" ^
  -H "Content-Type: application/json"
```

### 404 Test
```bash
curl -X GET "http://localhost:5000/api/tipologiche/tipi-email/99999" ^
  -H "Content-Type: application/json"
```

---

## 6️⃣ Troubleshooting

### ❌ Problema: "Connection refused"
**Soluzione**:
1. Verificare che l'app sia in esecuzione: `dotnet run`
2. Controllare la porta: dovrebbe essere 5000
3. Verificare firewall

### ❌ Problema: "404 Not Found" su /api/tipologiche
**Soluzione**:
1. Verificare che Carter sia registrato in Program.cs: `app.MapCarter();`
2. Ricompilare: `dotnet build`
3. Riavviare l'app

### ❌ Problema: Swagger non mostra endpoint
**Soluzione**:
1. Verificare namespace: `Accredia.GestioneAnagrafica.API.Endpoints.Tipologiche`
2. Verificare classe implementa `ICarterModule`
3. Riavviare l'app

### ❌ Problema: Response vuoto da database
**Soluzione**:
1. Verificare dati nel database: ripopolare con script SQL
2. Controllare DbContext: verificare DbSet configurati
3. Eseguire migrations: `dotnet ef database update`

---

## 7️⃣ Verifiche Finali

### Checklist Pre-Produzione
- [ ] Build compila senza errori: `dotnet build`
- [ ] App avvia senza errori: `dotnet run`
- [ ] Swagger visibile: http://localhost:5000/swagger
- [ ] Endpoint "Tipologiche" presente in Swagger
- [ ] GET /api/tipologiche ritorna 200 OK
- [ ] Paginazione funziona (page, pageSize)
- [ ] Singolo elemento ritorna 404 per ID non valido
- [ ] JSON response ha camelCase
- [ ] Documentazione Swagger è visibile

---

## 8️⃣ Comandi Utili

### Build e Run
```bash
dotnet build              # Compila
dotnet run                # Esegue
dotnet run --release      # Esegue in Release mode
```

### Database
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Test
```bash
dotnet test               # Esegue unit test
```

### Pulire
```bash
dotnet clean              # Pulisce i build
```

---

## 9️⃣ File di Riferimento

| File | Descrizione |
|------|-------------|
| `DTOs/TipologicheDTO.cs` | Definizione DTOs |
| `Endpoints/Tipologiche/GetTipologicheEndpoint.cs` | Endpoint GET (10 endpoint) |
| `Endpoints/Tipologiche/GetTipologicheCompletEndpoint.cs` | Endpoint aggregato |
| `Models/Tipologiche.cs` | Modelli di dominio |
| `Data/PersoneDbContext.cs` | DbContext |
| `Program.cs` | Configurazione applicazione |

---

## 🔟 Risorse Esterne

- [Carter Documentation](https://github.com/CarterCommunity/Carter)
- [Minimal APIs .NET](https://learn.microsoft.com/it-it/aspnet/core/fundamentals/minimal-apis)
- [Entity Framework Core](https://learn.microsoft.com/it-it/ef/core/)
- [Swagger/OpenAPI](https://swagger.io/)

---

## ✅ Completamento

Quando tutti i test passano, il Gruppo E è **PRONTO PER PRODUZIONE**.

**Prossimo Step**: Testare l'integrazione con il frontend.

---

**Creato**: 2 Novembre 2024
**Versione**: 1.0
**Status**: ✅ PRONTO
