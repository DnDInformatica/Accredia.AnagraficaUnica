# 🎨 Accredia Corporate Skill - Quick Reference

## ✅ Implementazione Completata

La skill **Accredia Corporate** è stata applicata con successo alla componente web del progetto.

### 📦 File Modificati

- ✅ `Program.cs` - Configurazione MudBlazor con tema corporate
- ✅ `Layouts/MainLayout.razor` - Layout con branding Accredia
- ✅ `wwwroot/index.html` - Meta tag e font corporate
- ✅ `wwwroot/css/app.css` - Stili CSS corporate completi

### 📁 File Creati

- ✨ `Themes/AccrediaTheme.cs` - Tema MudBlazor corporate
- 📄 `Pages/ExampleCorporate.razor` - Pagina di esempio
- 📚 `ACCREDIA_CORPORATE_SKILL_IMPLEMENTATION.md` - Documentazione dettagliata

## 🎯 Colori Principali

| Nome | Hex | Utilizzo |
|------|-----|----------|
| Blu Primario | `#003366` | Pulsanti, header, testo principale |
| Blu Secondario | `#0066CC` | Accent, link, elementi secondari |
| Successo | `#28A745` | Alert positivi, badge |
| Warning | `#FFC107` | Alert di avviso |
| Errore | `#DC3545` | Alert di errore |
| Testo | `#212121` | Testo principale |
| Background | `#FAFAFA` | Sfondo pagina |

## 🔤 Font Corporate

```
'Segoe UI', 'Roboto', 'Helvetica Neue', 'Arial', sans-serif
```

## 🔗 CSS Classes Disponibili

### Pulsanti
```html
<button class="btn-accredia-primary">Primario</button>
<button class="btn-accredia-secondary">Secondario</button>
<button class="btn-accredia-outline">Outline</button>
```

### Card
```html
<div class="accredia-card">
    <div class="accredia-card-header"><h3>Titolo</h3></div>
    <div class="accredia-card-body">Contenuto</div>
</div>
```

### Badge
```html
<span class="badge-accredia">Default</span>
<span class="badge-accredia badge-success">Success</span>
<span class="badge-accredia badge-error">Error</span>
```

### Alert
```html
<div class="alert-info">Info</div>
<div class="alert-success">Success</div>
<div class="alert-warning">Warning</div>
<div class="alert-error">Error</div>
```

## 📱 CSS Variables

Tutte le variabili CSS sono definite in `:root` nel file `app.css`:

```css
/* Colori */
--accredia-primary: #003366
--accredia-secondary: #0066CC
--color-success: #28A745

/* Spacing */
--spacing-xs: 0.25rem
--spacing-sm: 0.5rem
--spacing-md: 1rem
--spacing-lg: 1.5rem
--spacing-xl: 2rem

/* Border Radius */
--border-radius-md: 4px

/* Shadow */
--shadow-md: 0 4px 8px rgba(0, 0, 0, 0.15)
```

## 🌙 Dark Mode

Il tema supporta automaticamente il dark mode del sistema. Nel MainLayout:

```razor
<MudThemeProvider Theme="@GetAccrediaTheme(isDarkMode)" @bind-IsDarkMode="isDarkMode" />
```

## 💡 Esempi di Utilizzo

### Esempio 1: Form Branded
```razor
@page "/form"

<div class="accredia-header">
    <h1>Form Accredia</h1>
</div>

<MudContainer MaxWidth="MaxWidth.Small">
    <MudCard Class="mt-6">
        <MudCardContent>
            <MudTextField Label="Nome" Variant="Variant.Outlined" FullWidth="true" />
            <MudTextField Label="Email" Variant="Variant.Outlined" FullWidth="true" Class="mt-4" />
            
            <MudButton Variant="Variant.Filled" Color="Color.Primary" Class="mt-6">
                Invia
            </MudButton>
        </MudCardContent>
    </MudCard>
</MudContainer>
```

### Esempio 2: Dashboard Card
```razor
<div class="accredia-card">
    <div class="accredia-card-header">
        <h3>📊 Dashboard</h3>
    </div>
    <div class="accredia-card-body">
        <p>Benvenuto nel sistema Accredia</p>
        <button class="btn-accredia-primary">Accedi</button>
    </div>
</div>
```

### Esempio 3: Badge Status
```razor
<span class="badge-accredia">Nuovo</span>
<span class="badge-accredia badge-success">Approvato</span>
<span class="badge-accredia badge-warning">In Sospeso</span>
<span class="badge-accredia badge-error">Rifiutato</span>
```

## 🔧 Configurazione MudBlazor

La configurazione nel `Program.cs`:

```csharp
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = 
        Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
});
```

## 📖 Documentazione Completa

Per una documentazione completa, consultare:

📄 **ACCREDIA_CORPORATE_SKILL_IMPLEMENTATION.md**

## 🧪 Test Componenti

Accedi alla pagina di esempio per testare tutti i componenti corporate:

```
http://localhost:PORT/example-corporate
```

## 🎯 Prossimi Passi

1. **Applica classi corporate** ai tuoi componenti Razor
2. **Testa il dark mode** durante lo sviluppo
3. **Verifica responsive** su mobile/tablet
4. **Personalizza i colori** se necessario in `app.css`
5. **Documenta** eventuali customizzazioni

## ❓ Troubleshooting

### Il tema non si applica
1. Verifica che sia presente `AccrediaTheme.cs`
2. Svuota cache browser (Ctrl+Shift+Delete)
3. Riavvia l'applicazione

### I colori sono diversi
1. Controlla il toggle dark/light in MainLayout
2. Verifica le preferenze di sistema
3. Controlla le variabili CSS in `app.css`

### Font non carica
1. Verifica connessione internet
2. Fallback automatico a font di sistema
3. Controllare console browser per errori

## 📞 Support

- Consultare MudBlazor Docs: https://mudblazor.com
- Verificare file di implementazione nel progetto
- Leggere documentazione dettagliata allegata

---

**Status**: ✅ **IMPLEMENTAZIONE COMPLETATA**

Data: 2025-11-04
Versione: 1.0
