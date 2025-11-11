# 🎭 Esecuzione Completa - Playwright Tests Corporate Skill

## 📋 Requisiti

Prima di eseguire i test, assicurati che:

```
☐ Visual Studio 2022 aperto
☐ Soluzione caricata: C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.sln
☐ F5 premuto per avviare API e Web
☐ Web app visibile su https://localhost:7412 ✓
☐ API disponibile su http://localhost:5000 ✓
```

---

## 🚀 ESECUZIONE RAPIDA (5 minuti)

### 1️⃣ Apri PowerShell

```
Win+R → powershell → INVIO
```

### 2️⃣ Naviga alla cartella

```powershell
cd C:\Accredia\Sviluppo
```

### 3️⃣ Installa dipendenze (PRIMA VOLTA)

```powershell
npm install
npx playwright install
```

### 4️⃣ Esegui test CON UI

```powershell
.\run-playwright-tests.ps1 -UI
```

### 5️⃣ Visualizza risultati

Si apre automaticamente:
- Playwright UI nel browser
- Mostra i 14 test in real-time
- Report HTML alla fine

---

## ✅ Risultati Attesi

### Output Console
```
[OK] Node.js v18.17.0
[OK] npm 9.6.7
[INFO] Esecuzione test Corporate Skill...

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

### Report HTML
- Aperto automaticamente: `playwright-report/index.html`
- Mostra: Test timeline, screenshots, traces
- Click su test per dettagli

---

## 🔍 Interpretazione Risultati

### ✅ TUTTO PASSATO (14/14)

Significa che la **Corporate Skill Accredia è perfettamente applicata**:
- ✓ Colori corretti (Grafite, Ocra, Écru, Bianco)
- ✓ Typography corretta (Montserrat, Open Sans)
- ✓ MudBlazor theme integrato
- ✓ Layout responsive
- ✓ Theme Light/Dark funzionante
- ✓ Accessibilità OK

**Azione:** Procedi allo sviluppo.

---

### ⚠️ ALCUNI FALLITI

Vedi quale test è fallito nel report e:

1. **Colori sbagliati?**
   - Leggi: `CORPORATE_SKILL_TECHNICAL_VERIFICATION.md`
   - Verifica: `AccrediaTheme.cs`

2. **Font sbagliate?**
   - Verifica: Font Montserrat/Open Sans caricate
   - Controlla: CSS in `MainLayout.razor`

3. **MudBlazor non applicato?**
   - Verifica: `MudThemeProvider` in `MainLayout.razor`
   - Controlla: `Program.cs` - servizi MudBlazor registrati

---

## 📚 Comandi Alternativi

### Test Headless (veloce, no UI)
```powershell
npm run test:corporate
```

### Test con Browser Visibile
```powershell
.\run-playwright-tests.ps1 -Headed
```

### Debug Mode (step-by-step)
```powershell
.\run-playwright-tests.ps1 -Debug
```

### Visualizza Report Precedente
```powershell
npx playwright show-report
```

### Esegui solo UN test
```powershell
npx playwright test --grep "colori"
```

### Esegui su UN browser
```powershell
npx playwright test --project=chromium
```

---

## 🐛 Troubleshooting

### ❌ "Port already in use"
```powershell
Stop-Process -Name dotnet -Force
# Poi F5 di nuovo in Visual Studio
```

### ❌ "Connection refused - https://localhost:7412"
- Verifica che Web app sia in esecuzione (F5)
- Controlla URL nel browser

### ❌ "npm: command not found"
- Installa Node.js da https://nodejs.org/
- Riavvia PowerShell

### ❌ "CERTIFICATE error"
- Normale per localhost
- Test lo ignora automaticamente

### ❌ Test timeout
```powershell
npx playwright test --timeout 120000
```

---

## 📊 Cosa Testa Esattamente

### 1. Colori (3 test)
```
✓ AppBar = rgb(26, 26, 46) Grafite
✓ Text AppBar = rgb(255, 255, 255) Bianco
✓ Background = rgb(248, 247, 245) Écru
```

### 2. Typography (1 test)
```
✓ H1-H6 font-family include "Montserrat"
✓ Body font-family include "Open Sans"
```

### 3. Layout (4 test)
```
✓ MudAppBar presente e visibile
✓ MudDrawer presente e visibile
✓ Footer presente con colori Accredia
✓ Button styling applicato
```

### 4. Features (4 test)
```
✓ Desktop (1920x1080) responsive
✓ Mobile (375x667) responsive
✓ Theme toggle Light/Dark funziona
✓ CSS Variables presenti
```

### 5. Integration (2 test)
```
✓ Overall integration - tutti elementi presenti
✓ Branding Accredia visibile
```

---

## 🎯 Workflow Consigliato

### 🔄 Durante Sviluppo

1. **Prima di committare:**
   ```powershell
   npm run test:corporate
   ```

2. **Dopo modifiche al tema:**
   ```powershell
   .\run-playwright-tests.ps1 -UI
   ```

3. **Se fallisce un test:**
   - Leggi il report HTML
   - Correggi il codice
   - Esegui di nuovo i test

### 📤 Prima di Deploy

1. Esegui test completo:
   ```powershell
   npm test
   ```

2. Verifica risultati:
   - Almeno 14/14 test passati
   - Nessun warning

3. Visualizza report:
   ```powershell
   npx playwright show-report
   ```

---

## 📁 File Referenza

| Cosa Cerchi | Leggi | Linee |
|-------------|-------|-------|
| Guida rapida | PLAYWRIGHT_QUICK_START.md | 50 |
| Passo-passo dettagliato | PLAYWRIGHT_STEP_BY_STEP.md | 150 |
| Guida completa | PLAYWRIGHT_TEST_GUIDE.md | 300 |
| Verifica tecnica | CORPORATE_SKILL_TECHNICAL_VERIFICATION.md | 400 |
| Test Playwright | tests/accredia-corporate-skill.spec.ts | 530 |
| Questo file | PLAYWRIGHT_EXECUTION_GUIDE.md | 400 |

---

## ✅ Checklist Finale

- [ ] Visual Studio con F5 (Web app running)
- [ ] PowerShell aperto
- [ ] cd C:\Accredia\Sviluppo eseguito
- [ ] npm install completato
- [ ] npx playwright install completato
- [ ] Certificato HTTPS accettato (una volta)
- [ ] Eseguito: `.\run-playwright-tests.ps1 -UI`
- [ ] Visto: Playwright UI con test in esecuzione
- [ ] Risultato: 14 passed ✓
- [ ] Visualizzato: Report HTML

---

## 🎉 Success!

Se tutti i 14 test passano ✓:

✅ Corporate Skill Accredia è correttamente applicata  
✅ Colori OK  
✅ Typography OK  
✅ MudBlazor theme OK  
✅ Layout responsive OK  
✅ Accessibility OK  

**Procedi con lo sviluppo con fiducia!**

---

## 📞 Link Utili

- **Playwright Docs:** https://playwright.dev
- **MudBlazor Docs:** https://mudblazor.com
- **Accredia Corporate Skill:** ACCREDIA_CORPORATE_QUICK_REFERENCE.md
- **Report HTML:** `playwright-report/index.html` (dopo test)

---

**Pronto?** Esegui:
```powershell
.\run-playwright-tests.ps1 -UI
```

**Data:** Novembre 2025  
**Per:** Accredia Gestione Anagrafica v1.0
