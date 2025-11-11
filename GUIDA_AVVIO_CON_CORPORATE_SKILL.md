# 🚀 Come Avviare il Progetto con Accredia Corporate Skill

## 📋 Prerequisiti

- .NET 9.0 SDK installato
- Visual Studio 2022 o Visual Studio Code
- Browser moderno (Chrome, Firefox, Safari, Edge)

---

## 🔨 Step 1: Compilazione

### Opzione A: Da Visual Studio 2022

1. Apri `C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.sln`
2. In Solution Explorer, fai click destro sulla soluzione
3. Seleziona **"Rebuild Solution"**
4. Attendi il completamento della compilazione

### Opzione B: Da Terminal/PowerShell

```powershell
cd C:\Accredia\Sviluppo
dotnet clean
dotnet restore
dotnet build
```

### Opzione C: Build Solo Componente Web

```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet build
```

---

## ▶️ Step 2: Avvio dell'Applicazione

### Opzione A: Da Visual Studio

1. Apri la soluzione
2. Seleziona il progetto `Accredia.GestioneAnagrafica.Web`
3. Premi **F5** o click su "Start Debugging"
4. L'app si avvierà nel browser predefinito

### Opzione B: Da Terminal

```powershell
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web
dotnet watch run
```

Questo comando:
- ✅ Avvia l'applicazione
- ✅ Ricompila automaticamente al salvataggio file
- ✅ Aggiorna il browser automaticamente

### Opzione C: Build and Run Completo

```powershell
cd C:\Accredia\Sviluppo
dotnet build
cd Accredia.GestioneAnagrafica.Web
dotnet run
```

---

## 🌐 Step 3: Accesso all'Applicazione

### URL Predefiniti

- **Pagina Principale**: `https://localhost:5001`
- **HTTPS Dev Port**: `5001`
- **HTTP Dev Port**: `5000`

### Pagina di Esempio Corporate

Una volta avviata, visita:

```
https://localhost:5001/example-corporate
```

Qui potrai vedere:
- ✅ Pulsanti branded
- ✅ Card corporate
- ✅ Badge con vari stati
- ✅ Alert di vari tipi
- ✅ Form responsive
- ✅ Tabella CSS variables

---

## 🎨 Step 4: Testare il Tema Corporate

### Test Light Mode

1. La pagina si carica automaticamente in **light mode**
2. Verifica che il colore blu primario sia `#003366`
3. Controlla il layout responsive ridimensionando la finestra

### Test Dark Mode

1. Clicca l'icona di sole/luna in alto a destra (AppBar)
2. Verifica che il tema diventi scuro
3. I colori devono adattarsi correttamente

### Test Componenti

Sulla pagina `/example-corporate` puoi testare:

- **Pulsanti**: 
  - CSS: `.btn-accredia-primary`, `.btn-accredia-secondary`, `.btn-accredia-outline`
  - MudBlazor: `<MudButton Color="Color.Primary">`

- **Card**:
  - CSS: `.accredia-card`
  - MudBlazor: `<MudCard>`

- **Badge**:
  - CSS: `.badge-accredia`
  - MudBlazor: `<MudChip>`

- **Alert**:
  - CSS: `.alert-info`, `.alert-success`, `.alert-warning`, `.alert-error`
  - MudBlazor: `<MudAlert Severity="Severity.Info">`

---

## 🔍 Step 5: Verificare l'Implementazione

### Controllare i File

Verifica che questi file siano presenti:

```
C:\Accredia\Sviluppo\
├── Accredia.GestioneAnagrafica.sln
├── Accredia.GestioneAnagrafica.Web\
│   ├── Program.cs ✅ (Aggiornato)
│   ├── Themes\
│   │   └── AccrediaTheme.cs ✅ (Nuovo)
│   ├── Layouts\
│   │   └── MainLayout.razor ✅ (Aggiornato)
│   ├── Pages\
│   │   └── ExampleCorporate.razor ✅ (Nuovo)
│   └── wwwroot\
│       ├── index.html ✅ (Aggiornato)
│       └── css\
│           └── app.css ✅ (Aggiornato)
├── ACCREDIA_CORPORATE_SKILL_IMPLEMENTATION.md ✅
├── ACCREDIA_CORPORATE_QUICK_REFERENCE.md ✅
└── CHECKLIST_ACCREDIA_CORPORATE_SKILL.md ✅
```

### Controllare la Console del Browser

Premi **F12** e vai al tab **Console**. Dovresti vedere:

```
Accredia Corporate Theme initialized
```

Se usi dark mode:

```
Dark mode detected
```

---

## 🛠️ Step 6: Troubleshooting

### Il tema non si applica

**Soluzione:**
```powershell
# Pulisci build cache
dotnet clean

# Svuota cache browser (Ctrl+Shift+Delete)

# Ricompila
dotnet build
```

### L'applicazione non avvia

**Soluzione 1 - Porta occupata:**
```powershell
# Cambia porta in launchSettings.json
# Oppure libera la porta 5001:
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

**Soluzione 2 - Dipendenze mancanti:**
```powershell
cd C:\Accredia\Sviluppo
dotnet restore
dotnet build
```

### Font non carica

**Soluzione:**
1. Verifica connessione internet
2. Controlla Google Fonts CDN raggiungibile
3. Fallback automatico a font di sistema
4. Controlla console browser per errori

### CSS non aggiornato

**Soluzione:**
```
1. Premi Ctrl+Shift+Delete nel browser
2. Svuota cache sito web
3. Premi F5 per ricaricare
```

---

## 📊 Step 7: Monitorare Performance

### DevTools Browser

1. Apri F12 → Tab **Network**
2. Ricarica la pagina (F5)
3. Verifica che:
   - ✅ CSS carichi (< 500ms)
   - ✅ Font carichino (< 2s)
   - ✅ JS carichi correttamente

### Performance Tab

1. Apri F12 → Tab **Performance**
2. Premi Record
3. Esegui azioni (click, scroll, theme toggle)
4. Premi Stop
5. Analizza i risultati

**Target:**
- ✅ First Contentful Paint: < 1.5s
- ✅ Largest Contentful Paint: < 2.5s
- ✅ Cumulative Layout Shift: < 0.1

---

## 📝 Step 8: Documentazione

### Leggi

1. 📄 **ACCREDIA_CORPORATE_SKILL_IMPLEMENTATION.md**
   - Documentazione tecnica completa
   - API e configurazioni
   - Best practice

2. 📄 **ACCREDIA_CORPORATE_QUICK_REFERENCE.md**
   - Quick reference rapido
   - Esempi di codice
   - CSS classes

3. 📄 **CHECKLIST_ACCREDIA_CORPORATE_SKILL.md**
   - Checklist implementazione
   - File modificati/creati
   - Test suggeriti

### Consulta

- [MudBlazor Docs](https://mudblazor.com/docs)
- [CSS Variables MDN](https://developer.mozilla.org/en-US/docs/Web/CSS/--*)
- [Google Fonts](https://fonts.google.com/)

---

## 💡 Best Practice

### Durante lo Sviluppo

1. ✅ Usa variabili CSS per i colori
2. ✅ Applica classi semantic corporate
3. ✅ Testa il dark mode
4. ✅ Verifica responsive su mobile
5. ✅ Mantieni coerenza branding

### Quando Aggiungi Componenti

```razor
<!-- ✅ CORRETTO -->
<button class="btn-accredia-primary">Accedi</button>

<!-- ❌ EVITA -->
<button style="background-color: #003366;">Accedi</button>
```

### Personalizzazione Colori

Se devi cambiare i colori corporate:

1. Modifica `wwwroot/css/app.css` (sezione `:root`)
2. Aggiorna `Themes/AccrediaTheme.cs`
3. Aggiorna `wwwroot/index.html` (se necessario)
4. Verifica su light e dark mode

---

## 🚀 Comandi Rapidi

```powershell
# Full rebuild
cd C:\Accredia\Sviluppo && dotnet clean && dotnet build

# Watch and run
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web && dotnet watch run

# Clean specific project
cd C:\Accredia\Sviluppo\Accredia.GestioneAnagrafica.Web && dotnet clean

# Restore nuget
cd C:\Accredia\Sviluppo && dotnet restore

# Run tests (se disponibili)
dotnet test
```

---

## ✅ Checklist di Avvio

- [ ] Soluzione compilata senza errori
- [ ] Applicazione avviata su localhost:5001
- [ ] Pagina di esempio carica su /example-corporate
- [ ] Tema light visible di default
- [ ] Toggle dark mode funziona
- [ ] Pulsanti blue corporate visibili
- [ ] Font corporate caricati
- [ ] Responsive su mobile
- [ ] Console browser senza errori
- [ ] Network tab OK (no 404)

---

## 🎯 Output Atteso

Quando visiti `https://localhost:5001/example-corporate` dovresti vedere:

```
┌─────────────────────────────────────────────────────┐
│  🔷 Accredia - Gestione Anagrafica              ☀️ ⚙️  │
├─────────────────────────────────────────────────────┤
│                                                     │
│  Componenti Corporate Accredia                      │
│  Esempi di utilizzo della skill corporate           │
│                                                     │
│  ┌──────────────────────────────────────────────┐  │
│  │ Pulsanti Corporate                           │  │
│  │ ┌─────────────┐ ┌─────────────┐ ┌────────┐  │  │
│  │ │  Primario   │ │ Secondario  │ │ Outline│  │  │
│  │ └─────────────┘ └─────────────┘ └────────┘  │  │
│  └──────────────────────────────────────────────┘  │
│                                                     │
│  ... (altri componenti di esempio)                  │
│                                                     │
│  © 2025 Accredia. Tutti i diritti riservati        │
│                                                     │
└─────────────────────────────────────────────────────┘
```

---

## 🎓 Risorse Aggiuntive

- 📚 [MudBlazor Getting Started](https://mudblazor.com/getting-started/installation)
- 📚 [Blazor WebAssembly Docs](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- 📚 [CSS Custom Properties](https://developer.mozilla.org/en-US/docs/Web/CSS/--*)
- 📚 [Material Design](https://material.io/design)

---

## ✨ Completato!

🎉 **L'implementazione della Accredia Corporate Skill è completata!**

Puoi iniziare a sviluppare con il tema corporate integrato nella tua applicazione.

**Data**: 2025-11-04
**Status**: ✅ PRONTO PER L'USO
**Versione**: 1.0

Buon sviluppo! 🚀
