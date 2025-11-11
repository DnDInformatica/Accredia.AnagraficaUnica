# 🎭 Quick Start - Playwright Tests

## ⚡ Esecuzione Rapida (3 step)

### Step 1️⃣ - Apri PowerShell nel progetto

```powershell
cd C:\Accredia\Sviluppo
```

### Step 2️⃣ - Installa dipendenze (prima volta)

```powershell
npm install
npx playwright install
```

### Step 3️⃣ - Esegui test CON UI interattiva (CONSIGLIATO)

```powershell
.\run-playwright-tests.ps1 -UI
```

Oppure senza interfaccia:

```powershell
.\run-playwright-tests.ps1
```

## 🎯 Cosa Succede

✅ Playwright avvierà il browser automaticamente  
✅ Eseguirà 14 test per verificare Corporate Skill  
✅ Mostrerà report interattivo  
✅ Screenshot su fallimenti  

## 📋 Checklist Prima del Test

- [ ] Visual Studio aperto (F5 avviato)
  - API su http://localhost:5000 ✓
  - Web su https://localhost:7412 ✓
- [ ] PowerShell aperto nella cartella progetto
- [ ] Node.js installato (`node --version` funziona)
- [ ] npm dipendenze installate

## 🚀 Comandi Veloci

| Comando | Effetto |
|---------|--------|
| `.\run-playwright-tests.ps1` | Esegui test headless |
| `.\run-playwright-tests.ps1 -UI` | Con UI interattiva |
| `.\run-playwright-tests.ps1 -Headed` | Con browser visibile |
| `npm run test:corporate:ui` | UI mode diretto |
| `npx playwright show-report` | Visualizza ultimo report |

## 📊 Output Atteso

```
✓ Verifica colori primari - Grafite e Ocra (1.2s)
✓ Verifica colore testo - Bianco su sfondo scuro (0.8s)
✓ Verifica background pagina - Écru (#f8f7f5) (0.9s)
✓ Verifica Typography - Font families (1.1s)
✓ Verifica applicazione tema MudBlazor (0.7s)
✓ Verifica Drawer con tema Accredia (0.6s)
✓ Verifica Footer con colori Accredia (1.0s)
✓ Verifica Button styling - Tema Accredia (0.8s)
✓ Verifica layout responsivo (2.1s)
✓ Verifica toggle tema Light/Dark (1.2s)
✓ Verifica CSS Variables Accredia (0.5s)
✓ Verifica contrasto colori (Accessibilità) (0.8s)
✓ Test di integrazione completo (1.5s)
✓ Verifica che l'app sia responsive e branding sia visibile (0.9s)

14 passed (15.1s) ✓
```

## ✅ Cosa Verifica

✓ **Colori Accredia** - Grafite, Ocra, Écru, Bianco  
✓ **Typography** - Montserrat, Open Sans  
✓ **MudBlazor Theme** - Applicato correttamente  
✓ **AppBar, Drawer, Footer** - Styling Accredia  
✓ **Responsività** - Desktop e mobile  
✓ **Theme Toggle** - Light/Dark  
✓ **Accessibility** - Contrasto colori  

## 🆘 Se Fallisce

### Errore: "Port already in use"
Termina il processo:
```powershell
Stop-Process -Name dotnet -Force
```

### Errore: "Connection refused"
Assicurati che:
1. Web app è avviata (F5 in Visual Studio)
2. URL è https://localhost:7412

### Errore: "CERTIFICATE AUTHORITY"
È normale per localhost - test ignora automaticamente

## 📚 Per Saperne Di Più

Vedi `PLAYWRIGHT_TEST_GUIDE.md` per guida completa

---
**Pronto? Esegui:** `.\run-playwright-tests.ps1 -UI`
