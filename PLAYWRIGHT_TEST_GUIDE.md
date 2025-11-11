# 🎭 Playwright Tests - Accredia Corporate Skill Verification

Questa cartella contiene i test Playwright per verificare che la **Corporate Skill di Accredia** sia applicata correttamente all'applicazione Web.

## 📋 Prerequisiti

- **Node.js** >= 18.0.0
- **npm** o **yarn**
- Applicazione Web in esecuzione su `https://localhost:7412`
- API disponibile su `http://localhost:5000`

## 🚀 Installazione

### 1️⃣ Installa dipendenze

```bash
npm install
```

### 2️⃣ Installa browser Playwright

```bash
npx playwright install
```

Oppure:
```bash
npm run install:browsers
```

## ▶️ Esecuzione Test

### Eseguire tutti i test (Headless)

```bash
npm test
```

### Eseguire test Corporate Skill specificamente

```bash
npm run test:corporate
```

### Eseguire test con UI interattiva (CONSIGLIATO per debug)

```bash
npm run test:ui
```

Oppure solo test Corporate Skill:
```bash
npm run test:corporate:ui
```

### Eseguire test in modalità "headed" (con browser visibile)

```bash
npm run test:corporate:headed
```

### Debug test interattivo

```bash
npm run test:debug
```

### Visualizzare report HTML

Dopo aver eseguito i test:
```bash
npm run test:report
```

## 📊 Test Inclusi

### ✓ Test Corporate Skill

I test verificano:

1. **Colori Primary** - Grafite (#1a1a2e) e Ocra (#d4a574)
2. **Colori Background** - Écru (#f8f7f5) e Bianco (#ffffff)
3. **AppBar Styling** - Tema Grafite
4. **Typography** - Montserrat per heading, Open Sans per body
5. **MudBlazor Theme** - Componenti theme applicati
6. **Drawer Navigation** - Styling corretto
7. **Footer** - Colori Accredia
8. **Button Styling** - Tema applicato
9. **Responsive Layout** - Desktop e mobile
10. **Theme Toggle** - Light/Dark mode
11. **CSS Variables** - Custom properties
12. **Accessibility** - Contrasto colori
13. **Overall Integration** - Branding visibile

## 📁 Struttura File

```
tests/
├── accredia-corporate-skill.spec.ts    ← Test principal
playwright.config.ts                     ← Configurazione Playwright
package.json                             ← NPM dependencies
playwright-report/                       ← Report HTML (generato)
test-results/                           ← Risultati JSON/JUnit (generati)
```

## 🔍 Output Test

### Console Output Esempio

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

14 passed (15.1s)
```

### Report HTML

I report HTML vengono generati in:
- `playwright-report/index.html` - Report HTML completo
- `test-results/results.json` - Risultati JSON
- `test-results/junit.xml` - JUnit XML (CI/CD)

## 🛠️ Configurazione Avanzata

### Eseguire solo un browser

```bash
# Solo Chromium
npx playwright test --project=chromium

# Solo Firefox
npx playwright test --project=firefox

# Solo Webkit
npx playwright test --project=webkit
```

### Eseguire un test specifico

```bash
npx playwright test accredia-corporate-skill.spec.ts -g "Verifica colori"
```

### Con timeout personalizzato

```bash
npx playwright test --timeout 60000
```

### Retries sui fallimenti

```bash
npx playwright test --retries 3
```

## 🧪 Debugging

### 1. Modalità Debug (Step-by-step)

```bash
npm run test:debug
```

### 2. Pause su failure

Aggiungi nel test:
```typescript
await page.pause();
```

### 3. Inspect Mode

```bash
npx playwright codegen https://localhost:7412
```

Questo registra le azioni nel browser e genera codice test automatico.

## 📸 Screenshot e Video

- Screenshot su fallimenti: ✓ Abilitato
- Video su fallimenti: ✓ Abilitato
- Locazione: `test-results/`

## 🔐 HTTPS e Certificati

I test ignoreranno errori HTTPS per localhost (certificati auto-firmati).

Configurazione in `playwright.config.ts`:
```typescript
ignoreHTTPSErrors: true
```

## 📝 Script NPM

| Script | Descrizione |
|--------|-----------|
| `npm test` | Esegui tutti i test (headless) |
| `npm run test:ui` | Esegui test con UI interattiva |
| `npm run test:debug` | Debug mode interattivo |
| `npm run test:report` | Visualizza report HTML |
| `npm run test:corporate` | Esegui test Corporate Skill |
| `npm run test:corporate:ui` | UI mode - test Corporate Skill |
| `npm run test:corporate:headed` | Headed mode - vedi il browser |
| `npm run install:browsers` | Installa browser Playwright |

## 🚀 CI/CD Integration

Per integrare in CI/CD (GitHub Actions, Azure Pipelines, etc.):

```yaml
- name: Install dependencies
  run: npm install

- name: Install Playwright browsers
  run: npx playwright install --with-deps

- name: Run Playwright tests
  run: npm test

- name: Upload report
  if: always()
  uses: actions/upload-artifact@v3
  with:
    name: playwright-report
    path: playwright-report/
```

## ✅ Checklist Finale

- [ ] Node.js installato
- [ ] `npm install` eseguito
- [ ] Browser Playwright installati
- [ ] Web app in esecuzione su https://localhost:7412
- [ ] API in esecuzione su http://localhost:5000
- [ ] Certificato HTTPS accettato nel browser
- [ ] Esegui `npm test` o `npm run test:ui`

## 🆘 Troubleshooting

### Port already in use
```bash
# Termina processi sulle porte
netstat -ano | findstr :7412
taskkill /PID <PID> /F
```

### Certificato HTTPS non attendibile
- È normale per localhost
- Acceptance automatica in test

### Timeout su test
```bash
npm test -- --timeout 120000
```

### Memory issues
```bash
# Aumenta memoria Node
node --max-old-space-size=4096 node_modules/@playwright/test/cli.js test
```

## 📚 Documentazione

- **Playwright Docs**: https://playwright.dev
- **Accredia Corporate Skill**: Vedi `ACCREDIA_CORPORATE_SKILL_IMPLEMENTATION.md`
- **Setup Guide**: Vedi `STARTUP_GUIDE.md`

---

**Ultima modifica:** Novembre 2025  
**Versione:** Accredia Gestione Anagrafica v1.0
