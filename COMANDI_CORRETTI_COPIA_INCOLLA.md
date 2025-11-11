# 🚀 COMANDI CORRETTI - Copia e Incolla

## ⚠️ IMPORTANTE: Esegui PRIMA Visual Studio con F5

Assicurati che:
- ✅ Visual Studio 2022 aperto
- ✅ Soluzione caricata
- ✅ F5 premuto
- ✅ Web app su https://localhost:7412
- ✅ API su http://localhost:5000

---

## ✅ OPZIONE 1: COMANDI SINGOLI (CONSIGLIATO)

Apri PowerShell in `C:\Accredia\Sviluppo` e esegui UNO ALLA VOLTA:

### Comando 1: Installa dipendenze
```powershell
npm install
```

**Attendi che finisca (~30 secondi)**

### Comando 2: Installa Playwright browsers
```powershell
npx playwright install
```

**Attendi che finisca (~2-3 minuti)**

### Comando 3: Esegui test
```powershell
.\run-playwright-tests.ps1 -UI
```

Si apre Playwright UI nel browser automaticamente

---

## ✅ OPZIONE 2: COMANDO SINGOLO (POWERSHELL CORRETTO)

Se sei in PowerShell, usa il `;` (punto e virgola):

```powershell
npm install; npx playwright install; .\run-playwright-tests.ps1 -UI
```

---

## ✅ OPZIONE 3: BATCH FILE (PIÙ SEMPLICE)

Se non vuoi digitare, semplicemente esegui:

```cmd
run-playwright-tests.bat
```

E segui le istruzioni a schermo.

---

## 📊 COSA SUCCEDEREBBE

### npm install
```
up to date, audited 100 packages in 2s
```

### npx playwright install
```
✓ Chromium 120.0.6099.129 (linux)
✓ Firefox 121.0 (linux)  
✓ WebKit 17.4 (linux)

Browsers installed successfully.
```

### .\run-playwright-tests.ps1 -UI
```
[INFO] Node.js v18.17.0
[OK] npm 9.6.7
[INFO] Esecuzione test...
```

Quindi si apre Playwright UI nel browser con i test in esecuzione

---

## ✅ RISULTATO FINALE ATTESO

Console:
```
✓ 14 passed (15.1s)
```

Browser:
```
Playwright UI opens showing all 14 tests passing
HTML Report: playwright-report/index.html
```

---

## 🆘 SE RICEVI ERRORI

### Errore: "Port already in use"
```powershell
Stop-Process -Name dotnet -Force
```
Poi F5 di nuovo in Visual Studio

### Errore: "npm: command not found"
- Installa Node.js da https://nodejs.org/
- Riavvia PowerShell

### Errore: "Cannot find path .\run-playwright-tests.ps1"
```powershell
# Assicurati di essere in C:\Accredia\Sviluppo
cd C:\Accredia\Sviluppo

# Verifica che il file esista
ls run-playwright-tests.ps1

# Se non lo vedi, usa il batch file
run-playwright-tests.bat
```

---

## 📋 PASSO-PASSO COMPLETO

1. **Apri PowerShell**
   - WIN+R
   - Digita: powershell
   - Premi ENTER

2. **Naviga alla cartella**
   ```powershell
   cd C:\Accredia\Sviluppo
   ```

3. **Digita il primo comando**
   ```powershell
   npm install
   ```
   Attendi che finisca

4. **Digita il secondo comando**
   ```powershell
   npx playwright install
   ```
   Attendi che finisca

5. **Digita il terzo comando**
   ```powershell
   .\run-playwright-tests.ps1 -UI
   ```

6. **Guarda i test in azione**
   - Browser si apre automaticamente
   - Vedi i 14 test eseguirsi in tempo reale
   - Risultato: 14 passed ✓

---

## 🎯 QUICK REFERENCE

| Cosa vuoi fare | Comando |
|----------------|---------|
| Installa dipendenze | `npm install` |
| Installa Playwright | `npx playwright install` |
| Test con UI | `.\run-playwright-tests.ps1 -UI` |
| Test headless | `npm run test:corporate` |
| Test con browser visibile | `.\run-playwright-tests.ps1 -Headed` |
| Debug | `.\run-playwright-tests.ps1 -Debug` |
| Vedi report precedente | `npx playwright show-report` |

---

**Pronto?**

```powershell
cd C:\Accredia\Sviluppo
npm install
```

Copia e incolla il comando sopra nel PowerShell e premi ENTER.
Quando finisce, esegui il prossimo comando.
