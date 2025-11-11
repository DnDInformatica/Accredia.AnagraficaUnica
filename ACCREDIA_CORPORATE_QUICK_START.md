# 🚀 Guida Rapida: Accredia Corporate Skill

**Aggiornato**: 08 Novembre 2025  
**Progetto**: Accredia.GestioneAnagrafica.Web  
**Framework**: Blazor WebAssembly + MudBlazor

---

## 📦 Componenti Corporate Implementati

### 1. **Tema Accredia** (`Themes/AccrediaTheme.cs`)
Tema MudBlazor personalizzato con tutti i colori corporate.

```csharp
using Accredia.GestioneAnagrafica.Web.Themes;

// Colori disponibili
AccrediaTheme.Grafite       // #1a1a2e - Primary
AccrediaTheme.Ocra          // #d4a574 - Secondary
AccrediaTheme.Écru          // #f8f7f5 - Background
AccrediaTheme.Bianco        // #ffffff - Surface

// Colori semantici
AccrediaTheme.SuccessGreen  // #2e7d32
AccrediaTheme.WarningOrange // #f57c00
AccrediaTheme.ErrorRed      // #c62828
AccrediaTheme.InfoBlue      // #0277bd
```

### 2. **AppNavBar** (`Components/AppNavBar.razor`)
Navbar corporate con logo, menu utente e notifiche.

```razor
<AppNavBar Title="Tuo Titolo" 
           ShowUserMenu="true"
           ShowNotifications="false"
           ShowMenuToggle="true"
           OnMenuToggleClick="@ToggleDrawer"
           OnLogoutClick="@HandleLogout" />
```

**Parametri**:
- `Title` - Titolo da visualizzare
- `ShowUserMenu` - Mostra menu utente (default: true)
- `ShowNotifications` - Mostra icona notifiche (default: false)
- `ShowMenuToggle` - Mostra toggle menu mobile (default: true)
- `OnMenuToggleClick` - Callback toggle drawer
- `OnLogoutClick` - Callback logout

### 3. **AppFooter** (`Components/AppFooter.razor`)
Footer corporate a 3 colonne con informazioni azienda e link.

```razor
<AppFooter ContactAddress="Via Saliceto, 7/9&#10;00161 Roma"
           ContactPhone="+39 06 844099.1"
           ContactEmail="info@accredia.it" />
```

**Personalizzazione Link**:
```csharp
var customLinks = new List<AppFooter.FooterLink>
{
    new() { Text = "Sito", Url = "https://accredia.it", OpenInNewTab = true },
    new() { Text = "Privacy", Url = "/privacy", OpenInNewTab = false }
};
```

---

## 🎨 Quick Reference: Colori

### Uso in Razor
```razor
<!-- Con MudBlazor -->
<MudButton Color="Color.Primary">Bottone</MudButton>
<MudText Color="Color.Secondary">Testo</MudText>

<!-- Con Style inline -->
<div style="background-color: @AccrediaTheme.Grafite; color: @AccrediaTheme.Bianco;">
    Contenuto
</div>
```

### Uso in CSS
```css
:root {
    --accredia-primary: #1a1a2e;
    --accredia-secondary: #d4a574;
    --accredia-background: #f8f7f5;
    --accredia-surface: #ffffff;
}

.my-element {
    background-color: var(--accredia-primary);
    color: var(--accredia-surface);
}
```

---

## 📏 Sistema Responsive

### Breakpoints
```
xs: 0px        → Mobile portrait
sm: 600px      → Mobile landscape
md: 960px      → Tablet
lg: 1264px     → Desktop
xl: 1920px     → Large desktop
```

### Uso Grid
```razor
<MudGrid>
    <MudItem xs="12" sm="6" md="4" lg="3">
        <!-- Contenuto responsive -->
    </MudItem>
</MudGrid>
```

**Esempi**:
- `xs="12"` → 100% width su mobile
- `xs="12" sm="6"` → 100% mobile, 50% tablet+
- `xs="12" sm="6" md="4"` → 100% mobile, 50% tablet, 33% desktop

---

## 🔤 Tipografia

### Headers (Montserrat Bold)
```razor
<MudText Typo="Typo.h1">Heading 1 - 2.5rem</MudText>
<MudText Typo="Typo.h2">Heading 2 - 2rem</MudText>
<MudText Typo="Typo.h3">Heading 3 - 1.75rem</MudText>
<MudText Typo="Typo.h4">Heading 4 - 1.5rem</MudText>
<MudText Typo="Typo.h5">Heading 5 - 1.25rem</MudText>
<MudText Typo="Typo.h6">Heading 6 - 1rem</MudText>
```

### Body Text (Open Sans)
```razor
<MudText Typo="Typo.body1">Testo principale</MudText>
<MudText Typo="Typo.body2">Testo secondario</MudText>
<MudText Typo="Typo.caption">Didascalia piccola</MudText>
<MudText Typo="Typo.overline">ETICHETTA</MudText>
```

---

## 🔘 Bottoni

### Varianti
```razor
<!-- Filled (default) -->
<MudButton Variant="Variant.Filled" Color="Color.Primary">
    Primary
</MudButton>

<!-- Outlined -->
<MudButton Variant="Variant.Outlined" Color="Color.Primary">
    Outlined
</MudButton>

<!-- Text -->
<MudButton Variant="Variant.Text" Color="Color.Primary">
    Text
</MudButton>

<!-- Con icona -->
<MudButton Variant="Variant.Filled" 
           Color="Color.Primary"
           StartIcon="@Icons.Material.Filled.Check">
    Con Icona
</MudButton>
```

### Colori Semantici
```razor
<MudButton Color="Color.Success">Success</MudButton>
<MudButton Color="Color.Warning">Warning</MudButton>
<MudButton Color="Color.Error">Error</MudButton>
<MudButton Color="Color.Info">Info</MudButton>
```

---

## 📋 Form Elements

### TextField
```razor
<MudTextField @bind-Value="model.Campo" 
              Label="Etichetta"
              Variant="Variant.Outlined"
              Margin="Margin.Dense"
              FullWidth="true"
              Required="true"
              RequiredError="Campo obbligatorio" />
```

### Select
```razor
<MudSelect @bind-Value="model.Scelta" 
           Label="Seleziona"
           Variant="Variant.Outlined">
    <MudSelectItem Value="@("Opzione 1")">Opzione 1</MudSelectItem>
    <MudSelectItem Value="@("Opzione 2")">Opzione 2</MudSelectItem>
</MudSelect>
```

### CheckBox
```razor
<MudCheckBox @bind-Value="model.Accetto" 
             Label="Accetto i termini"
             Color="Color.Primary" />
```

---

## 💬 Messaggi e Alert

### Alert
```razor
<MudAlert Severity="Severity.Info">Messaggio info</MudAlert>
<MudAlert Severity="Severity.Success">Operazione riuscita</MudAlert>
<MudAlert Severity="Severity.Warning">Attenzione!</MudAlert>
<MudAlert Severity="Severity.Error">Errore!</MudAlert>
```

### Snackbar (Toast)
```csharp
@inject ISnackbar Snackbar

private void ShowNotification()
{
    Snackbar.Add("Messaggio salvato!", Severity.Success);
}
```

---

## 📊 Tabelle

```razor
<MudTable Items="@items" 
          Hover="true" 
          Striped="true"
          Dense="true">
    <HeaderContent>
        <MudTh>Colonna 1</MudTh>
        <MudTh>Colonna 2</MudTh>
    </HeaderContent>
    <RowTemplate>
        <MudTd>@context.Campo1</MudTd>
        <MudTd>@context.Campo2</MudTd>
    </RowTemplate>
</MudTable>
```

---

## 🗂️ Cards

### Card Base
```razor
<MudCard Elevation="2">
    <MudCardHeader>
        <CardHeaderContent>
            <MudText Typo="Typo.h6">Titolo Card</MudText>
        </CardHeaderContent>
    </MudCardHeader>
    <MudCardContent>
        <MudText>Contenuto della card</MudText>
    </MudCardContent>
    <MudCardActions>
        <MudButton Color="Color.Primary">Azione</MudButton>
    </MudCardActions>
</MudCard>
```

### Card con Immagine
```razor
<MudCard Elevation="2">
    <MudCardMedia Image="/images/foto.jpg" Height="200" />
    <MudCardContent>
        <MudText Typo="Typo.h6">Titolo</MudText>
        <MudText Typo="Typo.body2">Descrizione</MudText>
    </MudCardContent>
</MudCard>
```

---

## 🎯 Best Practices

### ✅ DO
- ✓ Usa sempre i colori del tema Accredia
- ✓ Applica Montserrat per headers, Open Sans per body
- ✓ Testa responsive su tutti i breakpoint (375px, 960px, 1920px)
- ✓ Usa componenti MudBlazor invece di HTML puro
- ✓ Mantieni contrasto WCAG AA: 4.5:1 per testo normale
- ✓ Usa `Variant.Outlined` per form per coerenza

### ❌ DON'T
- ✗ Non usare colori non previsti nel tema
- ✗ Non mischiare stili Bootstrap con MudBlazor
- ✗ Non ignorare i breakpoint responsive
- ✗ Non usare font diversi da Montserrat/Open Sans
- ✗ Non dimenticare alt text per immagini
- ✗ Non creare layout non responsive

---

## 📁 Struttura File Progetto

```
Accredia.GestioneAnagrafica.Web/
├── Components/
│   ├── AppNavBar.razor          ← Navbar corporate
│   ├── AppFooter.razor          ← Footer corporate
│   ├── NavMenu.razor            ← Menu laterale
│   └── ...
├── Layouts/
│   └── MainLayout.razor         ← Layout principale
├── Pages/
│   ├── CorporateShowcase.razor  ← Demo componenti
│   └── ...
├── Themes/
│   └── AccrediaTheme.cs         ← Tema MudBlazor
└── wwwroot/
    ├── css/
    │   └── app.css              ← CSS custom
    └── index.html               ← Entry point
```

---

## 🔧 Setup Nuovo Progetto

1. **Installa MudBlazor**
```bash
dotnet add package MudBlazor
```

2. **Configura Program.cs**
```csharp
builder.Services.AddMudServices();
builder.Services.AddScoped<AccrediaTheme>();
```

3. **Aggiungi in index.html**
```html
<link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />
<script src="_content/MudBlazor/MudBlazor.min.js"></script>
```

4. **Usa MainLayout**
```razor
@layout MainLayout
```

---

## 🌐 Risorse Utili

- **MudBlazor Docs**: https://mudblazor.com/docs/overview
- **Material Icons**: https://fonts.google.com/icons
- **WCAG Contrast Checker**: https://webaim.org/resources/contrastchecker/
- **Responsive Test**: DevTools (F12) → Toggle Device Toolbar

---

## 📞 Supporto

Per domande o supporto sull'implementazione corporate:
- **Email**: sviluppo@accredia.it
- **Docs Interne**: `/docs/corporate-skill`
- **Issue Tracker**: [Link interno]

---

**Versione**: 1.0.0  
**Ultimo Aggiornamento**: 08 Novembre 2025
