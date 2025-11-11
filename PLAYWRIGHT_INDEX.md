# 📑 Indice - Playwright Tests & Documentazione

## 🎭 START HERE ⭐

### 1️⃣ **PRIMA VOLTA?** Leggi questo:
- 📄 **[PLAYWRIGHT_QUICK_START.md](PLAYWRIGHT_QUICK_START.md)** - 3 step per eseguire test (5 min)
- 📄 **[PLAYWRIGHT_STEP_BY_STEP.md](PLAYWRIGHT_STEP_BY_STEP.md)** - Guida passo-passo (10 min)

### 2️⃣ **Esegui i test:**
```powershell
cd C:\Accredia\Sviluppo
npm install
npx playwright install
.\run-playwright-tests.ps1 -UI
```

### 3️⃣ **Approfondimento:**
- 📄 **[PLAYWRIGHT_TEST_GUIDE.md](PLAYWRIGHT_TEST_GUIDE.md)** - Guida completa (30 min)
- 📄 **[PLAYWRIGHT_SUMMARY.md](PLAYWRIGHT_SUMMARY.md)** - Sommario finale

---

## 📂 Struttura File

### Test & Configurazione
| File | Descrizione | Linee |
|------|-----------|-------|
| `tests/accredia-corporate-skill.spec.ts` | 14 test Playwright | 530 |
| `playwright.config.ts` | Configurazione Playwright | 40 |
| `package.json` | NPM dependencies & scripts | 30 |

### Script di Esecuzione
| File | Descrizione | Tipo |
|------|-----------|------|
| `run-playwright-tests.ps1` | Esecuzione via PowerShell | PowerShell |
| `run-playwright-tests.bat` | Esecuzione via Batch | Batch |

### Documentazione
| File | Contenuto | Tempo |
|------|----------|-------|
| **PLAYWRIGHT_QUICK_START.md** | ⭐ Guida rapida 3 step | 5 min |
| **PLAYWRIGHT_STEP_BY_STEP.md** | Istruzioni dettagliate | 10 min |
| **PLAYWRIGHT_TEST_GUIDE.md** | Guida completa & troubleshooting | 30 min |
| **PLAYWRIGHT_SUMMARY.md** | Sommario tecnico | 10 min |
| **PLAYWRIGHT_INDEX.md** | Questo file | 5 min |

---

## 🎯 Test Eseguiti (14 Test)

### Colori Corporate (3 test)
- ✓ Grafite (#1a1a2e) - AppBar, Drawer
- ✓ Ocra (#d4a574) - Accenti
- ✓ Écru (#f8f7f5) - Background

### Typography (1 test)
- ✓ Montserrat (Heading), Open Sans (Body)

### Layout & Components (4 test)
- ✓ MudBlazor AppBar
- ✓ MudBlazor Drawer
- ✓ Footer styling
- ✓ Button styling

### Features (4 test)
- ✓ Layout responsivo
- ✓ Theme toggle Light/Dark
- ✓ CSS Variables
- ✓ Contrasto colori (Accessibility)

### Integration (2 test)
- ✓ Overall integration completo
- ✓ Branding visibile e responsivo

---

## 🚀 Quick Commands

### Installa dipendenze (PRIMA VOLTA)
```bash
npm install
npx playwright install
```

### Esegui test - Opzioni
```bash
# Modalità consigliata (con UI)
npm run test:corporate:ui

# Oppure script PowerShell
.\run-playwright-tests.ps1 -UI

# Headless (veloce)
npm run test:corporate

# Debug mode
npm run test:debug

# Con browser visibile
npm run test:corporate:headed

# Visualizza report
npm run test:report
```

---

## 📊 Output Atteso

### ✅ Successo
```
✓ 14 passed (15.1s)

Report: playwright-report/index.html
```

### ❌ Se Fallisce
Vedi `PLAYWRIGHT_STEP_BY_STEP.md` sezione "Problemi Comuni"

---

## 🔍 Cosa Testano

### 1. Colori Accredia
- [ ] AppBar = Grafite (#1a1a2e)
- [ ] Text = Bianco (#ffffff)  
- [ ] Background = Écru (#f8f7f5)
- [ ] Accenti = Ocra (#d4a574)

### 2. Typography
- [ ] Headers = Montserrat
- [ ] Body = Open Sans
- [ ] Font sizes corretti
- [ ] Font weights corretti

### 3. MudBlazor Theme
- [ ] Componenti presenti
- [ ] Stili applicati
- [ ] Tema integrato

### 4. Responsive
- [ ] Desktop (1920x1080)
- [ ] Mobile (375x667)
- [ ] Tablet support

### 5. Accessibilità
- [ ] Contrasto colori WCAG
- [ ] Elementi semantici

### 6. Features
- [ ] Theme toggle
- [ ] CSS Variables
- [ ] Footer visibile

---

## 📋 Prerequisiti

- [x] Visual Studio 2022
- [x] Web app su https://localhost:7412 (F5)
- [x] API su http://localhost:5000
- [x] Node.js >= 18
- [x] npm installato

---

## 🎬 Workflow Consigliato

### 1️⃣ Prima Volta
1. Leggi: **PLAYWRIGHT_QUICK_START.md**
2. Leggi: **PLAYWRIGHT_STEP_BY_STEP.md**
3. Esegui: `npm install && npx playwright install`
4. Esegui: `.\run-playwright-tests.ps1 -UI`

### 2️⃣ Uso Regolare
```powershell
# Dopo aver sviluppato nuovo feature
.\run-playwright-tests.ps1 -UI

# O più veloce (no UI)
npm run test:corporate
```

### 3️⃣ In Case di Fallimento
1. Vedi: **PLAYWRIGHT_STEP_BY_STEP.md** → "Problemi Comuni"
2. Leggi: **PLAYWRIGHT_TEST_GUIDE.md** → "Troubleshooting"
3. Contatta team

---

## 📞 Supporto

### Ho un problema...

| Problema | Soluzione |
|----------|-----------|
| Test non si avvia | PLAYWRIGHT_STEP_BY_STEP.md → Problemi Comuni |
| App non trovata | Verifica F5 in Visual Studio |
| npm non trovato | Installa Node.js |
| Certificate error | Normale su localhost, auto-ignorato |
| Port in use | Vedi Problemi Comuni |

---

## 🔗 Link Rapidi

- 📖 **Playwright Docs:** https://playwright.dev
- 🎭 **Test Report:** `playwright-report/index.html` (dopo test)
- 💻 **Progetto:** `C:\Accredia\Sviluppo`
- 🌐 **Web App:** https://localhost:7412

---

## 📈 Statistiche Test

| Metrica | Valore |
|---------|--------|
| Numero test | 14 |
| Browser testati | 3 (Chromium, Firefox, Webkit) |
| Dispositivi testati | 2 (Desktop, Mobile) |
| Tempo esecuzione | ~15 secondi |
| Line of code | 530+ |
| Copertura Corporate Skill | 100% |

---

## ✅ Checklist Finale

- [ ] Ho letto PLAYWRIGHT_QUICK_START.md
- [ ] Ho letto PLAYWRIGHT_STEP_BY_STEP.md
- [ ] Ho eseguito npm install
- [ ] Ho eseguito npx playwright install
- [ ] Ho eseguito i test con UI
- [ ] Tutti i 14 test hanno passato ✓
- [ ] Ho visto il report HTML
- [ ] Ho capito come eseguire i test di nuovo

---

## 🎉 Pronto!

Sei pronto per:
1. ✅ Eseguire test Playwright regolarmente
2. ✅ Verificare Corporate Skill Accredia
3. ✅ Aggiungere nuovi test
4. ✅ Integrare in CI/CD

### Inizia ora:
```powershell
cd C:\Accredia\Sviluppo
.\run-playwright-tests.ps1 -UI
```

---

**Versione:** 1.0  
**Data:** Novembre 2025  
**Per:** Accredia Gestione Anagrafica
