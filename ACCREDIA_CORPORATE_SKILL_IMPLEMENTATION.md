# Accredia Corporate Skill - Implementazione

## 📋 Overview

Questo documento descrive l'implementazione della skill **Accredia Corporate** nella componente web `Accredia.GestioneAnagrafica.Web`.

La skill fornisce:
- **Tema MudBlazor Corporate** con colori e font Accredia
- **Stili CSS Corporate** coerenti con le linee guida di branding
- **Componenti UI Branded** per un'esperienza utente uniforme
- **Supporto per Dark Mode** con tema appropriato
- **Layout responsive** e professionale

## 🎨 Colori Corporate Accredia

### Palette Primaria
- **Blu Scuro Principale**: `#003366` (Primary)
- **Blu Accredia**: `#0066CC` (Secondary)
- **Blu Chiaro**: `#0099FF` (Accent per Dark Mode)

### Colori di Stato
- **Successo**: `#28A745`
- **Warning**: `#FFC107`
- **Errore**: `#DC3545`
- **Info**: `#17A2B8`

### Colori Neutri
- **Testo Primario**: `#212121`
- **Testo Secondario**: `#757575`
- **Background**: `#FAFAFA`
- **Surface**: `#FFFFFF`

## 🔤 Font Family Corporate

La skill utilizza i seguenti font in ordine di preferenza:

1. **Segoe UI** - Font primario Windows/moderna
2. **Roboto** - Font di fallback Google Fonts
3. **Helvetica Neue** - Font di fallback classico
4. **Arial** - Font di fallback universale
5. **sans-serif** - Font di sistema

```css
font-family: 'Segoe UI', 'Roboto', 'Helvetica Neue', 'Arial', sans-serif;
```

## 📁 File Modificati/Creati

### 1. **Program.cs**
- Configurazione MudBlazor Services
- Impostazione Snackbar configuration

```csharp
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
});
```

### 2. **Themes/AccrediaTheme.cs** (Nuovo)
- Classe statica con metodi per ottenere i temi MudBlazor
- `GetLightTheme()` - Tema chiaro con colori corporate
- `GetDarkTheme()` - Tema scuro ottimizzato per il comfort visivo

**Utilizzo:**
```csharp
private MudTheme GetAccrediaTheme(bool darkMode)
{
    return darkMode ? AccrediaTheme.GetDarkTheme() : AccrediaTheme.GetLightTheme();
}
```

### 3. **wwwroot/css/app.css**
- Variabili CSS Custom (CSS Variables)
- Classi utility per componenti corporate
- Stili responsive per mobile/tablet/desktop
- Supporto per Dark Mode
- Font preload per performance

**Classi Disponibili:**
- `.btn-accredia-primary` - Pulsante blu principale
- `.btn-accredia-secondary` - Pulsante blu secondario
- `.btn-accredia-outline` - Pulsante outline
- `.accredia-card` - Card con stili corporate
- `.accredia-header` - Header con gradient
- `.badge-accredia` - Badge corporate
- `.alert-*` - Alert di varie tipologie
- `.divider-accredia` - Divider corporate

### 4. **wwwroot/index.html**
- Meta tag di branding Accredia
- Font preload ottimizzati
- Google Fonts CDN
- Material Icons
- Tema color personalizzato

### 5. **Layouts/MainLayout.razor**
- Integrazione con AccrediaTheme
- AppBar con branding gradient
- Drawer con logo e nome azienda
- Toggle per dark/light mode
- Footer con copyright

## 🚀 Come Usare il Tema

### In Componenti Razor

**Utilizzare classi CSS Corporate:**
```razor
<button class="btn-accredia-primary">Accedi</button>
<button class="btn-accredia-outline">Annulla</button>

<div class="accredia-card">
    <div class="accredia-card-header">
        <h3>Titolo Card</h3>
    </div>
    <div class="accredia-card-body">
        Contenuto della card
    </div>
</div>

<span class="badge-accredia">Nuovo</span>
<span class="badge-accredia badge-success">Approvato</span>
```

**Utilizzare Colori CSS Variables:**
```css
.my-element {
    background-color: var(--accredia-primary);
    color: white;
    border-radius: var(--border-radius-md);
    padding: var(--spacing-md);
    box-shadow: var(--shadow-md);
}
```

**Utilizzare MudBlazor Components:**
```razor
<MudButton Variant="Variant.Filled" Color="Color.Primary">
    Azione Primaria
</MudButton>

<MudCard>
    <MudCardContent>
        <MudText Typo="Typo.h6">Titolo</MudText>
        <MudText Typo="Typo.body2">Contenuto</MudText>
    </MudCardContent>
</MudCard>
```

## 📱 Responsive Design

La skill implementa breakpoint responsive:

- **Mobile** (< 480px): Layout ottimizzato per smartphone
- **Tablet** (480px - 768px): Layout ibrido
- **Desktop** (> 768px): Layout completo

```css
@media (max-width: 768px) {
    /* Stili tablet/mobile */
}

@media (max-width: 480px) {
    /* Stili mobile */
}
```

## 🌙 Dark Mode Support

La skin supporta automaticamente il dark mode del sistema operativo e user preference:

```javascript
// Rilevazione preferenza sistema
if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
    document.documentElement.setAttribute('data-theme', 'dark');
}

// Listener per cambiamenti
window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
    if (e.matches) {
        document.documentElement.setAttribute('data-theme', 'dark');
    }
});
```

## 🎯 CSS Variables Disponibili

### Colori
```css
--accredia-primary: #003366
--accredia-primary-dark: #001f3f
--accredia-primary-light: #0052cc
--accredia-secondary: #0066CC
--accredia-accent: #FF6B35
--color-success: #28A745
--color-warning: #FFC107
--color-error: #DC3545
--color-info: #17A2B8
```

### Testo e Background
```css
--text-primary: #212121
--text-secondary: #757575
--bg-primary: #FAFAFA
--bg-secondary: #FFFFFF
```

### Font
```css
--font-family-base: 'Segoe UI', 'Roboto', 'Helvetica Neue', 'Arial', sans-serif
--font-family-mono: 'Courier New', monospace
```

### Spacing
```css
--spacing-xs: 0.25rem
--spacing-sm: 0.5rem
--spacing-md: 1rem
--spacing-lg: 1.5rem
--spacing-xl: 2rem
```

### Border Radius
```css
--border-radius-sm: 2px
--border-radius-md: 4px
--border-radius-lg: 8px
```

### Shadow
```css
--shadow-sm: 0 2px 4px rgba(0, 0, 0, 0.1)
--shadow-md: 0 4px 8px rgba(0, 0, 0, 0.15)
--shadow-lg: 0 8px 16px rgba(0, 0, 0, 0.2)
```

## 🔄 Integrazione con Progetti Esistenti

Se hai componenti Razor esistenti, puoi applicare il tema:

### 1. Aggiorna le importazioni namespace
```csharp
@using Accredia.GestioneAnagrafica.Web.Themes
```

### 2. Sostituisci i colori hardcoded
```razor
<!-- Prima -->
<MudButton Color="Color.Blue">Accedi</MudButton>

<!-- Dopo -->
<MudButton Color="Color.Primary">Accedi</MudButton>
```

### 3. Usa classi CSS corporate
```razor
<!-- Prima -->
<div style="background-color: #0052cc; color: white; padding: 1rem;">
    Contenuto
</div>

<!-- Dopo -->
<div class="btn-accredia-primary">
    Contenuto
</div>
```

## 🎨 Personalizzazione

Per personalizzare i colori aziendali:

1. Modifica le variabili CSS in `wwwroot/css/app.css`:
```css
:root {
    --accredia-primary: #MIO_COLORE;
    --accredia-secondary: #MIO_COLORE;
}
```

2. Aggiorna la classe `AccrediaTheme.cs`:
```csharp
Primary = "#MIO_COLORE",
Secondary = "#MIO_COLORE",
```

3. Aggiorna il gradient nel `index.html` e `MainLayout.razor`

## 📝 Best Practice

1. **Usa variabili CSS** per colori e spacing
2. **Applica classi semantic** (.btn-accredia-primary) invece di stili inline
3. **Mantieni coerenza** nei componenti UI
4. **Testa** il dark mode durante lo sviluppo
5. **Verifica responsive** su diversi device
6. **Usa font** dalla famiglia corporate
7. **Documenta** eventuali customizzazioni

## 🔧 Troubleshooting

### Il tema non viene applicato
1. Verifica che il file `AccrediaTheme.cs` sia nel namespace corretto
2. Assicurati che `MudThemeProvider` sia nel layout
3. Svuota la cache del browser (Ctrl+Shift+Delete)

### I colori appaiono diversi in dark mode
1. Verifica le impostazioni di colore del theme in `AccrediaTheme.cs`
2. Controllare le preferenze di sistema (Windows 10+)
3. Forzare il tema tramite toggle nel MainLayout

### I font non si caricano
1. Verifica la connessione internet (Google Fonts CDN)
2. Controllare la console del browser per errori
3. Fallback su font di sistema

## 📚 Risorse Esterne

- [MudBlazor Docs](https://mudblazor.com/docs)
- [Google Fonts](https://fonts.google.com/)
- [Material Design Icons](https://fonts.google.com/icons)
- [CSS Variables](https://developer.mozilla.org/en-US/docs/Web/CSS/--*)

## 📞 Support

Per domande o problemi con l'implementazione della skill, consulta:
1. La documentazione MudBlazor
2. I file di configurazione in questo progetto
3. Le linee guida di branding Accredia
