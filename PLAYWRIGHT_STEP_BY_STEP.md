## 🎭 GUIDA PASSO-PASSO: Eseguire Playwright Tests

### ⏱️ Tempo Stimato: 5 minuti

---

## 🔴 PREREQUISITO IMPORTANTE

**La Web App DEVE essere in esecuzione!**

1. Apri Visual Studio 2022
2. Apri soluzione: `C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.sln`
3. Premi **F5** per avviare API e Web
4. Attendi che il browser si apra con https://localhost:7412

✅ **NON PROCEDERE FINCHÉ NON VEDI L'APP WEB**

---

## 📝 STEP 1: Apri PowerShell

1. Premi **Win+R**
2. Digita: `powershell`
3. Premi **INVIO**

Dovresti vedere un prompt PowerShell:
```
PS C:\Users\YourName>
```

---

## 📂 STEP 2: Naviga alla Cartella Progetto

Nel PowerShell, digita:
```powershell
cd C:\Accredia\Sviluppo
```

Premi **INVIO**

Verifica che sei nella cartella giusta:
```
PS C:\Accredia\Sviluppo>
```

---

## 📦 STEP 3: Installa Dipendenze (PRIMA VOLTA)

Digita:
```powershell
npm install
```

Premi **INVIO** e attendi il completamento (~30 secondi)

Vedrai qualcosa simile a:
```
added 100 packages in 45s
```

---

## 🎭 STEP 4: Installa Browser Playwright

Digita:
```powershell
npx playwright install
```

Premi **INVIO** e attendi il completamento (~2-3 minuti)

Vedrai:
```
✓ Chromium downloaded
✓ Firefox downloaded  
✓ Webkit downloaded
```

---

## ▶️ STEP 5: Esegui Test CON INTERFACCIA INTERATTIVA

Digita:
```powershell
.\run-playwright-tests.ps1 -UI
```

Premi **INVIO**

Vedrai output come:
```
╔════════════════════════════════════════╗
║  Accredia - Playwright Tests           ║
║  Corporate Skill Verification          ║
╚════════════════════════════════════════╝

[INFO] Verifica Node.js...
[OK] v18.17.0
[INFO] Verifica npm...
[OK] npm 9.6.7
[INFO] Esecuzione test...
```

---

## 🌐 STEP 6: Interfaccia Playwright UI

Si apre automaticamente la **UI di Playwright** con:

- **Sinistra:** Lista dei test
- **Centro:** Preview della pagina durante il test
- **Destra:** Dettagli del test

### Azioni disponibili:
- ▶️ **Play** - Esegui test
- ⏭️ **Prossimo** - Vai al prossimo test
- 🔍 **Zoom** - Ingrandisci preview
- 📸 **Screenshot** - Vedi screenshot

---

## ✅ STEP 7: Visualizza Risultati

Dopo l'esecuzione:

### Se TUTTI PASSANO ✓
```
✓ 14 passed (15.1s)

[OK] Tutti i test completati con successo!
```

### Se ALCUNI FALLISCONO ✗
```
✗ 2 failed, 12 passed

Dettagli nel report HTML...
```

---

## 📊 STEP 8: Visualizza Report HTML

Il report si apre **automaticamente** nel browser:
- URL: `file:///...../playwright-report/index.html`
- Mostra: Dettagli test, screenshot, tracce

### Naviga il report:
1. Clicca sui test per vedere dettagli
2. Clicca su "Trace" per replay interattivo
3. Visualizza screenshot

---

## 🎯 INTERPRETARE I RISULTATI

### ✓ TEST PASSATO = Green

```
✓ Verifica colori primari - Grafite e Ocra (1.2s)
```

Significa che i colori Accredia sono applicati correttamente.

### ✗ TEST FALLITO = Red

```
✗ Verifica colori primari - Grafite e Ocra (1.2s)
  Error: Expected rgb(26,26,46) but got rgb(0,0,0)
```

Significa che il colore non è quello atteso.

---

## 🔄 PROSSIME VOLTE

Dopo la prima esecuzione, i test successivi sono più veloci:

```powershell
# Senza interfaccia UI (più veloce)
.\run-playwright-tests.ps1

# Con interfaccia UI
.\run-playwright-tests.ps1 -UI

# Con browser visibile (vedi cosa accade)
.\run-playwright-tests.ps1 -Headed
```

---

## 🆘 PROBLEMI COMUNI

### ❌ "Port already in use"

```
Error: listen EADDRINUSE :::5000
```

**Soluzione:**
```powershell
Stop-Process -Name dotnet -Force
# Poi F5 di nuovo in Visual Studio
```

### ❌ "Connection refused"

```
Error: connect ECONNREFUSED 127.0.0.1:7412
```

**Soluzione:**
- Verifica che Web app sia in esecuzione (F5 in Visual Studio)
- Controlla URL browser: deve essere https://localhost:7412

### ❌ "CERTIFICATE AUTHORITY error"

```
Error: CERTIFICATE_VERIFY_FAILED
```

**Soluzione:** ✅ Automatico - i test ignorano certificati self-signed

### ❌ "npx: command not found"

```
npx: command not found
```

**Soluzione:**
- npm non è nel PATH
- Installa Node.js da https://nodejs.org/
- Riavvia PowerShell dopo installazione

---

## 📚 ULTERIORI COMANDI

### Vedere report precedente
```powershell
npx playwright show-report
```

### Eseguire solo UN test
```powershell
npx playwright test --grep "colori"
```

### Debug mode (step-by-step)
```powershell
.\run-playwright-tests.ps1 -Debug
```

### Esecuzione veloce (headless)
```powershell
npx playwright test
```

---

## ✅ CHECKLIST SUCCESSO

Quando tutto funziona:

- [ ] PowerShell aperto
- [ ] Navigato a `C:\Accredia\Sviluppo`
- [ ] npm install completato
- [ ] Playwright browser installati
- [ ] Web app in esecuzione (F5 Visual Studio)
- [ ] Comando eseguito: `.\run-playwright-tests.ps1 -UI`
- [ ] Playwright UI aperto nel browser
- [ ] Test in esecuzione con preview
- [ ] 14 test passati ✓
- [ ] Report HTML visualizzato

---

## 🎉 FATTO!

Se arrivi fin qui, i test Playwright sono **perfettamente configurati** e verificano che la **Corporate Skill Accredia sia applicata correttamente**.

### Prossimi step:
1. Esegui regolarmente i test durante lo sviluppo
2. Aggiungi nuovi test per nuove features
3. Integra nei CI/CD (GitHub Actions, Azure Pipelines, etc.)

---

**Domande?** Vedi `PLAYWRIGHT_TEST_GUIDE.md` per guida completa.

**Data:** Novembre 2025
