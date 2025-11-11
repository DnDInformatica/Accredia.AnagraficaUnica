╔════════════════════════════════════════════════════════════════════════════════╗
║                                                                                ║
║                    ✅ SOLUZIONE ERRORE POWERSHELL                             ║
║                                                                                ║
║              ERROR: Il token '&&' non è un separatore valido                  ║
║                                                                                ║
╚════════════════════════════════════════════════════════════════════════════════╝

## 🎯 COSA È SUCCESSO?

In PowerShell, il carattere `&&` (operatore AND di Bash) non è supportato.
Vai dritto ai comandi corretti qui sotto ↓

═══════════════════════════════════════════════════════════════════════════════════

## ✅ SOLUZIONE RAPIDA

Esegui QUESTI tre comandi, UNO PER UNO:

### COMANDO 1️⃣
Copia e incolla questo:
```
npm install
```
Premi ENTER e attendi (~30 secondi)

### COMANDO 2️⃣
Quando finisce il primo, digita:
```
npx playwright install
```
Premi ENTER e attendi (~2-3 minuti)

### COMANDO 3️⃣
Quando finisce il secondo, digita:
```
.\run-playwright-tests.ps1 -UI
```
Premi ENTER

Si apre il browser automaticamente ✓

═══════════════════════════════════════════════════════════════════════════════════

## 🎬 STEP VISIVI

STEP 1 - PowerShell Window
```
┌─────────────────────────────────────────────────────┐
│ Windows PowerShell                              [_][□][X]│
├─────────────────────────────────────────────────────┤
│ PS C:\Accredia\Sviluppo>                            │
│                                                     │
│ (Digita qui il primo comando)                      │
└─────────────────────────────────────────────────────┘
```

STEP 2 - Primo Comando
```
PS C:\Accredia\Sviluppo> npm install
added 100 packages in 45s

PS C:\Accredia\Sviluppo> _
(Digita il secondo comando quando vedi il prompt)
```

STEP 3 - Secondo Comando
```
PS C:\Accredia\Sviluppo> npx playwright install
✓ Chromium downloaded
✓ Firefox downloaded
✓ Webkit downloaded

PS C:\Accredia\Sviluppo> _
(Digita il terzo comando quando vedi il prompt)
```

STEP 4 - Terzo Comando
```
PS C:\Accredia\Sviluppo> .\run-playwright-tests.ps1 -UI
[INFO] Node.js v18.17.0
[INFO] Esecuzione test...

(Browser si apre automaticamente)
```

═══════════════════════════════════════════════════════════════════════════════════

## 📋 COPIA-INCOLLA FACILE

Apri PowerShell e fai questo:

┌─ PASSO 1 ─────────────────────────────────────────┐
│ Posizionati nella cartella:                      │
│                                                  │
│ cd C:\Accredia\Sviluppo                          │
│                                                  │
│ Premi: ENTER                                     │
└────────────────────────────────────────────────────┘

┌─ PASSO 2 ─────────────────────────────────────────┐
│ Installa dipendenze NPM:                         │
│                                                  │
│ npm install                                      │
│                                                  │
│ Premi: ENTER                                     │
│ Attendi: ~30 secondi                             │
└────────────────────────────────────────────────────┘

┌─ PASSO 3 ─────────────────────────────────────────┐
│ Installa Playwright:                             │
│                                                  │
│ npx playwright install                           │
│                                                  │
│ Premi: ENTER                                     │
│ Attendi: ~2-3 minuti                             │
└────────────────────────────────────────────────────┘

┌─ PASSO 4 ─────────────────────────────────────────┐
│ Esegui i test:                                   │
│                                                  │
│ .\run-playwright-tests.ps1 -UI                   │
│                                                  │
│ Premi: ENTER                                     │
│ Attendi: Browser si apre automaticamente         │
└────────────────────────────────────────────────────┘

═══════════════════════════════════════════════════════════════════════════════════

## ✅ QUANDO FINISCE

Vedrai nel browser:
```
┌─ Playwright UI ─────────────────────────────────┐
│ ✓ Verifica colori primari - Grafite e Ocra     │
│ ✓ Verifica colore testo - Bianco               │
│ ✓ Verifica background pagina - Écru            │
│ ✓ Verifica Typography - Font families          │
│ ✓ Verifica applicazione tema MudBlazor         │
│ ✓ Verifica Drawer con tema Accredia            │
│ ✓ Verifica Footer con colori Accredia          │
│ ✓ Verifica Button styling                      │
│ ✓ Verifica layout responsivo                   │
│ ✓ Verifica toggle tema Light/Dark              │
│ ✓ Verifica CSS Variables Accredia              │
│ ✓ Verifica contrasto colori (Accessibilità)    │
│ ✓ Test di integrazione completo                │
│ ✓ Verifica branding e responsività             │
│                                                │
│ 14 passed (15.1s) ✓                            │
└────────────────────────────────────────────────┘
```

═══════════════════════════════════════════════════════════════════════════════════

## 🎯 SUCCESSO!

Se vedi "14 passed (15.1s)" ✓ allora:

✅ Corporate Skill Accredia è correttamente applicata
✅ Colori OK
✅ Typography OK
✅ MudBlazor theme OK
✅ Layout responsivo OK
✅ Accessibility OK

═══════════════════════════════════════════════════════════════════════════════════

## 🆘 SE QUALCOSA NON FUNZIONA

### Errore: "npm: command not found"
→ Installa Node.js da https://nodejs.org/
→ Riavvia PowerShell
→ Riprova

### Errore: "Port already in use"
→ Esegui: Stop-Process -Name dotnet -Force
→ Premi F5 di nuovo in Visual Studio
→ Riprova i test

### Errore: "Cannot find path .\run-playwright-tests.ps1"
→ Assicurati di essere in C:\Accredia\Sviluppo
→ Digita: ls run-playwright-tests.ps1
→ Se non lo vedi, usa: run-playwright-tests.bat

### Browser non si apre
→ Digita: npx playwright show-report
→ Il report si apre manualmente

═══════════════════════════════════════════════════════════════════════════════════

## 📚 DOCUMENTAZIONE

Vedi questi file per ulteriore aiuto:

- POWERSHELL_SYNTAX_FIX.md (Questo file in formato lungo)
- COMANDI_CORRETTI_COPIA_INCOLLA.md (Solo comandi)
- PLAYWRIGHT_QUICK_START.md (Guida completa)
- PLAYWRIGHT_STEP_BY_STEP.md (Passo-passo dettagliato)

═══════════════════════════════════════════════════════════════════════════════════

## 🚀 PRONTO?

Apri PowerShell e digita:

    cd C:\Accredia\Sviluppo

Premi ENTER

Poi digita:

    npm install

Premi ENTER

Attendi ~30 secondi

Quando finisce, digita il prossimo comando.

═══════════════════════════════════════════════════════════════════════════════════

**Versione corrigida: Novembre 2025**
**Per: Accredia Gestione Anagrafica**
