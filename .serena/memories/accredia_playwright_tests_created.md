# Accredia - Playwright Tests per Corporate Skill Verification

## ✅ File Creati

### 1. Test File
- **`tests/accredia-corporate-skill.spec.ts`** (530 linee)
  - 14 test completi per verificare Corporate Skill
  - Test colori: Grafite, Ocra, Écru, Bianco
  - Test typography: Montserrat, Open Sans
  - Test MudBlazor theme application
  - Test responsività (desktop e mobile)
  - Test accessibility (contrasto colori)

### 2. Configurazione
- **`playwright.config.ts`** - Config Playwright
  - Browser: Chrome, Firefox, Webkit
  - Mobile: Pixel 5, iPhone 12
  - Reporter: HTML, JSON, JUnit
  - Base URL: https://localhost:7412
  - Ignora HTTPS errors (per localhost)

### 3. NPM Configuration
- **`package.json`** - Dipendenze e script
  - Scripts: test, test:ui, test:debug, test:corporate, etc.
  - DevDependencies: @playwright/test ^1.40.0

### 4. Script Esecuzione
- **`run-playwright-tests.ps1`** - Script PowerShell
- **`run-playwright-tests.bat`** - Script Batch
- Entrambi auto-installano dipendenze

### 5. Documentazione
- **`PLAYWRIGHT_TEST_GUIDE.md`** - Guida completa

## 🧪 Test Inclusi

### Test Corporate Skill (14 test)
1. ✓ Verifica colori primari - Grafite e Ocra
2. ✓ Verifica colore testo - Bianco su sfondo scuro
3. ✓ Verifica background pagina - Écru
4. ✓ Verifica Typography - Font families
5. ✓ Verifica applicazione tema MudBlazor
6. ✓ Verifica Drawer con tema Accredia
7. ✓ Verifica Footer con colori Accredia
8. ✓ Verifica Button styling
9. ✓ Verifica layout responsivo
10. ✓ Verifica toggle tema Light/Dark
11. ✓ Verifica CSS Variables
12. ✓ Verifica contrasto colori (Accessibility)
13. ✓ Test integrazione completo
14. ✓ Verifica branding e responsività

## 🚀 Come Eseguire

### Modo 1: PowerShell (Consigliato)
```powershell
.\run-playwright-tests.ps1 -UI
```

### Modo 2: Batch
```cmd
run-playwright-tests.bat
```

### Modo 3: NPM Diretto
```bash
# Installa dipendenze
npm install
npx playwright install

# Esegui test
npm run test:corporate:ui  # Con UI
npm run test:corporate     # Headless
npm run test:debug         # Debug mode
```

## 📊 Endpoint Verificati
- Web: https://localhost:7412
- API: http://localhost:5000
- Swagger: https://localhost:5001/swagger

## 🎯 Risultati Attesi
Tutti i 14 test dovrebbero passare con output:
```
✓ 14 passed (15.1s)
```

Report HTML in: playwright-report/index.html

## 🔍 What Gets Tested

### Colori Accredia
- Grafite: #1a1a2e (AppBar, Drawer)
- Ocra: #d4a574 (Accenti, link)
- Écru: #f8f7f5 (Background)
- Bianco: #ffffff (Testo su scuro)

### Typography
- Headers: Montserrat
- Body: Open Sans
- Fallback: Segoe UI

### Layout
- MudBlazor AppBar
- MudBlazor Drawer
- MudBlazor Container
- MudBlazor Components

### Responsive
- Desktop (1920x1080)
- Mobile (375x667)
- Tablet support

## ⚠️ Prerequisiti
- Node.js >= 18
- Web app su https://localhost:7412
- API su http://localhost:5000
- Certificato HTTPS accettato
