# 🚀 GUIDA RAPIDA - Esecuzione Corretta PowerShell

## ❌ ERRORE: Sintassi non valida

Il comando:
```powershell
npm install && npx playwright install .\run-playwright-tests.ps1 -UI
```

NON funziona in PowerShell perché `&&` non è un separatore valido.

---

## ✅ SOLUZIONE 1: Eseguire un comando alla volta

Apri PowerShell e digita uno per uno (ENTER dopo ogni riga):

### Step 1: Installa npm packages
```powershell
npm install
```
Attendi il completamento (~30 secondi)

### Step 2: Installa Playwright browsers
```powershell
npx playwright install
```
Attendi il completamento (~2-3 minuti)

### Step 3: Esegui i test con UI
```powershell
.\run-playwright-tests.ps1 -UI
```

---

## ✅ SOLUZIONE 2: Comandi in una riga (PowerShell)

Se vuoi tutto in una riga, usa il `;` (punto e virgola):

```powershell
npm install; npx playwright install; .\run-playwright-tests.ps1 -UI
```

Oppure con `-and` (PowerShell operator):

```powershell
npm install -and npx playwright install -and .\run-playwright-tests.ps1 -UI
```

---

## ✅ SOLUZIONE 3: Usa il file batch

Se preferisci non preoccuparti della sintassi:

```cmd
run-playwright-tests.bat
```

Questo script fa tutto automaticamente:
- ✓ Installa dipendenze
- ✓ Verifica l'ambiente
- ✓ Esegue i test

---

## 📝 STEP-BY-STEP (CONSIGLIATO)

### 1. Apri PowerShell
```
WIN+R → powershell → ENTER
```

### 2. Naviga alla cartella
```powershell
cd C:\Accredia\Sviluppo
```

### 3. Installa npm packages
```powershell
npm install
```

Dovresti vedere:
```
added 100 packages in 45s
```

### 4. Installa Playwright
```powershell
npx playwright install
```

Dovresti vedere:
```
✓ Chromium downloaded
✓ Firefox downloaded
✓ Webkit downloaded
```

### 5. Esegui i test
```powershell
.\run-playwright-tests.ps1 -UI
```

Si apre Playwright UI nel browser

---

## 🎯 RISULTATO ATTESO

Console mostrerà:
```
✓ Verifica colori primari - Grafite e Ocra (1.2s)
✓ Verifica colore testo - Bianco su sfondo scuro (0.8s)
✓ Verifica background pagina - Écru (#f8f7f5) (0.9s)
... (e altri 11 test)

14 passed (15.1s) ✓
```

Browser aprirà automaticamente il report HTML

---

## 💡 ALTRI COMANDI UTILI

```powershell
# Solo test (headless, più veloce)
npm run test:corporate

# Test con browser visibile
.\run-playwright-tests.ps1 -Headed

# Debug mode
.\run-playwright-tests.ps1 -Debug

# Visualizza report precedente
npx playwright show-report
```

---

## ✅ CHECKLIST

- [ ] PowerShell aperto
- [ ] Posizionato in C:\Accredia\Sviluppo
- [ ] npm install eseguito ✓
- [ ] npx playwright install eseguito ✓
- [ ] .\run-playwright-tests.ps1 -UI eseguito
- [ ] Playwright UI aperto nel browser
- [ ] 14 test eseguiti
- [ ] Tutti passati ✓
- [ ] Report HTML visualizzato

---

**Pronto? Inizia con:**
```powershell
npm install
```

Poi quando finisce:
```powershell
npx playwright install
```

Poi:
```powershell
.\run-playwright-tests.ps1 -UI
```
