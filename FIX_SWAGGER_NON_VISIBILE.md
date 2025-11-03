# 🔧 RISOLUZIONE: Swagger Non Visibile - Porte Corrette

## ❌ PROBLEMA

Dopo aver eseguito `dotnet run`, il server era su porte sbagliate:
```
https://localhost:65515
http://localhost:65516
```

E quindi Swagger non era disponibile su `http://localhost:5000/swagger`

---

## ✅ SOLUZIONE APPLICATA

Il file `Properties/launchSettings.json` è stato corretto:

### Prima ❌
```json
"applicationUrl": "https://localhost:65515;http://localhost:65516"
```

### Dopo ✅
```json
"applicationUrl": "https://localhost:5001;http://localhost:5000"
```

---

## 🚀 COME PROCEDERE ORA

### Passo 1: Ferma il server attuale
```
Nel terminale: Ctrl+C
```

### Passo 2: Pulisci il build
```bash
dotnet clean
```

### Passo 3: Ricompila
```bash
dotnet build
```

### Passo 4: Esegui di nuovo
```bash
dotnet run
```

### Passo 5: Attendi il messaggio di avvio
Dovresti vedere nel terminale:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:5001
```

---

## ✅ TESTA SWAGGER

### Opzione 1: HTTP (Consigliato per il testing)
```
http://localhost:5000/swagger
```

### Opzione 2: HTTPS (Sicuro)
```
https://localhost:5001/swagger
```

---

## 📋 COSA VEDRAI

Una volta acceso Swagger, dovresti vedere:

1. **Top della pagina**: "Gestione Organismi API" 
2. **Sezione**: Tag "Tipologiche"
3. **Endpoint**: 
   - GET /api/tipologiche
   - GET /api/tipologiche/tipi-email
   - GET /api/tipologiche/tipi-telefono
   - etc.

---

## 🧪 TEST VELOCE

1. Apri: http://localhost:5000/swagger
2. Espandi: **GET /api/tipologiche**
3. Clicca: **Try it out**
4. Clicca: **Execute**
5. ✅ Dovresti vedere Response 200 OK con i dati

---

## 🆘 SE NON FUNZIONA ANCORA

### Check 1: Porta in uso
Se vedi errore "Address already in use":
```bash
# Cambia porta in launchSettings.json
# Da: http://localhost:5000
# A:  http://localhost:5050 (o qualsiasi porta libera)
```

### Check 2: Firewall
Se Swagger non carica, verifica firewall di Windows:
- Consenti .NET su porta 5000

### Check 3: Certificato HTTPS
Se hai errore su HTTPS, puoi disabilitare:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Check 4: Logs di errore
Guarda i log nel terminale per errori specifici:
```
ERR: ...
```

---

## 📝 COMANDO COMPLETO (QUICK FIX)

Se vuoi ricominciare da zero:

```bash
# Vai nella cartella
cd C:\Accredia\Sviluppo

# Pulisci
dotnet clean

# Ricompila
dotnet build

# Esegui
dotnet run
```

Attendi finché non vedi:
```
Now listening on: http://localhost:5000
```

Poi apri il browser:
```
http://localhost:5000/swagger
```

---

## ✅ VERIFICHE FINALI

- [ ] Terminale mostra "Now listening on: http://localhost:5000"
- [ ] Browser si apre automaticamente su Swagger
- [ ] Vedi la lista degli endpoint
- [ ] Tag "Tipologiche" è visibile
- [ ] Riesci a testare GET /api/tipologiche

**Se tutti i check passano: ✅ READY FOR TESTING**

---

## 🎯 PROSSIMO PASSO

Una volta che Swagger è visibile:

1. Clicca su endpoint
2. Click "Try it out"
3. Click "Execute"
4. Verifica Response

Se tutti gli endpoint ritornano 200 OK: **✅ TUTTO FUNZIONA**

---

**Status**: ✅ RISOLTO - Ready to go!
