# Reingegnerizzazione Gestione Contatti Persona (Indirizzi, Email, Telefoni)

> Documento di istruzioni per l'implementazione della gestione completa di indirizzi, email e telefoni per l'entità Persona usando MudBlazor e Blazor .NET 9. Destinato allo sviluppatore/architetto che dovrà modificare le pagine `PersonaCreate.razor`, `PersonaEdit.razor` e `PersonaDetails.razor` creando componenti riutilizzabili `PersonaIndirizzo.razor`, `PersonaEmail.razor`, `PersonaTelefono.razor`.

---

## 1. Scopo

Implementare la gestione completa dei contatti (indirizzi, email, telefoni) per l'entità Persona attraverso componenti Blazor riutilizzabili, integrati nelle pagine di creazione, modifica e visualizzazione. I componenti devono essere modulari, responsivi e seguire le best practices di MudBlazor.

## 2. Obiettivi funzionali

### 2.1 Gestione Indirizzi
- Visualizzare lista indirizzi associati alla persona in una griglia MudDataGrid
- Aggiungere nuovi indirizzi tramite dialog modal
- Modificare indirizzi esistenti tramite dialog modal
- Eliminare indirizzi (soft delete)
- Supportare autocomplete per ricerca indirizzi (mock service)
- Validare formalmente CAP italiano (5 cifre)
- Gestire campi: Via, Civico, Interno, Presso, CAP, Comune, Provincia, Stato, Note

### 2.2 Gestione Email
- Visualizzare lista email in una griglia MudDataGrid
- Aggiungere nuove email tramite dialog modal
- Modificare email esistenti tramite dialog modal
- Eliminare email (soft delete)
- Validare formato email standard
- Verificare duplicati email
- Gestire tipi email: Aziendale (AZI), Personale (PER), PEC (PEC), Altro (ALT)

### 2.3 Gestione Telefoni
- Visualizzare lista telefoni in una griglia MudDataGrid
- Aggiungere nuovi telefoni tramite dialog modal
- Modificare telefoni esistenti tramite dialog modal
- Eliminare telefoni (soft delete)
- Validare numeri italiani e internazionali
- Gestire campi: Tipo, Prefisso Internazionale, Numero, Estensione, Note

### 2.4 Integrazione nelle Pagine
- **PersonaCreate.razor**: Expansion panels per gestire contatti durante la creazione
- **PersonaEdit.razor**: Expansion panels per gestire contatti in modifica
- **PersonaDetails.razor**: Expansion panels in sola lettura per visualizzazione

## 3. Requisiti tecnici

- Framework: .NET 9, Blazor Server
- UI Library: MudBlazor (versione 8.0+)
- Pattern: Componenti riutilizzabili con parametri
- Validazione: FluentValidation per modelli DTO
- Gestione stato: Componenti stateful con EventCallback per comunicazione parent-child
- API: REST endpoints separati per CRUD di indirizzi, email, telefoni
- Soft Delete: Implementato tramite flag `DataCancellazione` e `CancellatoDa`

## 4. Modelli dati

### 4.1 IndirizzoDTO

```csharp
namespace Accredia.GestioneAnagrafica.Shared.DTOs
{
    /// <summary>
    /// DTO per la gestione degli indirizzi
    /// </summary>
    public class IndirizzoDTO
    {
        public int IndirizzoId { get; set; }
        public int EntitaAziendaleId { get; set; }
        public int TipoIndirizzoId { get; set; }
        
        [Required(ErrorMessage = "La via è obbligatoria")]
        [StringLength(200, ErrorMessage = "La via non può superare 200 caratteri")]
        public string Via { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "Il civico non può superare 20 caratteri")]
        public string? Civico { get; set; }
        
        [StringLength(20, ErrorMessage = "L'interno non può superare 20 caratteri")]
        public string? Interno { get; set; }
        
        [StringLength(100, ErrorMessage = "Il campo Presso non può superare 100 caratteri")]
        public string? Presso { get; set; }
        
        [Required(ErrorMessage = "Il CAP è obbligatorio")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Il CAP deve essere composto da 5 cifre")]
        public string Cap { get; set; } = string.Empty;
        
        public int? ComuneId { get; set; }
        public int? ProvinciaId { get; set; }
        public int? StatoId { get; set; }
        
        [StringLength(500, ErrorMessage = "Le note non possono superare 500 caratteri")]
        public string? Note { get; set; }
        
        // Navigation properties per display
        public string? TipoIndirizzoDescrizione { get; set; }
        public string? ComuneNome { get; set; }
        public string? ProvinciaSigla { get; set; }
        public string? StatoNome { get; set; }
        
        // Audit
        public DateTime DataCreazione { get; set; }
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }
        
        // Computed property per visualizzazione
        public string IndirizzoCompleto => 
            $"{Via} {Civico}{(string.IsNullOrEmpty(Interno) ? "" : $"/{Interno}")}, {Cap} {ComuneNome} ({ProvinciaSigla})";
    }
    
    /// <summary>
    /// DTO per creazione indirizzo
    /// </summary>
    public class IndirizzoDTO_Create
    {
        [Required]
        public int EntitaAziendaleId { get; set; }
        
        [Required]
        public int TipoIndirizzoId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Via { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? Civico { get; set; }
        
        [StringLength(20)]
        public string? Interno { get; set; }
        
        [StringLength(100)]
        public string? Presso { get; set; }
        
        [Required]
        [RegularExpression(@"^\d{5}$")]
        public string Cap { get; set; } = string.Empty;
        
        public int? ComuneId { get; set; }
        public int? ProvinciaId { get; set; }
        public int? StatoId { get; set; }
        
        [StringLength(500)]
        public string? Note { get; set; }
    }
}
```

### 4.2 EmailDTO

```csharp
namespace Accredia.GestioneAnagrafica.Shared.DTOs
{
    /// <summary>
    /// DTO per la gestione delle email
    /// </summary>
    public class EmailDTO
    {
        public int EmailId { get; set; }
        public int EntitaAziendaleId { get; set; }
        public int TipoEmailId { get; set; }
        
        [Required(ErrorMessage = "L'indirizzo email è obbligatorio")]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        [StringLength(254, ErrorMessage = "L'email non può superare 254 caratteri")]
        public string IndirizzoEmail { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Le note non possono superare 500 caratteri")]
        public string? Note { get; set; }
        
        // Navigation property per display
        public string? TipoEmailDescrizione { get; set; }
        public string? TipoEmailCodice { get; set; }
        
        // Audit
        public DateTime DataCreazione { get; set; }
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }
    }
    
    /// <summary>
    /// DTO per creazione email
    /// </summary>
    public class EmailDTO_Create
    {
        [Required]
        public int EntitaAziendaleId { get; set; }
        
        [Required]
        public int TipoEmailId { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string IndirizzoEmail { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Note { get; set; }
    }
}
```

### 4.3 TelefonoDTO

```csharp
namespace Accredia.GestioneAnagrafica.Shared.DTOs
{
    /// <summary>
    /// DTO per la gestione dei telefoni
    /// </summary>
    public class TelefonoDTO
    {
        public int TelefonoId { get; set; }
        public int EntitaAziendaleId { get; set; }
        public int TipoTelefonoId { get; set; }
        
        [StringLength(20, ErrorMessage = "Il prefisso internazionale non può superare 20 caratteri")]
        public string? PrefissoInternazionale { get; set; }
        
        [Required(ErrorMessage = "Il numero di telefono è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il numero non può superare 100 caratteri")]
        [RegularExpression(@"^[\d\s\-\(\)]+$", ErrorMessage = "Il numero può contenere solo cifre, spazi, trattini e parentesi")]
        public string Numero { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "L'estensione non può superare 20 caratteri")]
        public string? Estensione { get; set; }
        
        [StringLength(500, ErrorMessage = "Le note non possono superare 500 caratteri")]
        public string? Note { get; set; }
        
        // Navigation property per display
        public string? TipoTelefonoDescrizione { get; set; }
        public string? TipoTelefonoCodice { get; set; }
        
        // Audit
        public DateTime DataCreazione { get; set; }
        public int? CreatoDa { get; set; }
        public DateTime? DataModifica { get; set; }
        public int? ModificatoDa { get; set; }
        public DateTime? DataCancellazione { get; set; }
        public int? CancellatoDa { get; set; }
        
        // Computed property per visualizzazione
        public string NumeroCompleto =>
            $"{(string.IsNullOrEmpty(PrefissoInternazionale) ? "" : $"+{PrefissoInternazionale} ")}{Numero}{(string.IsNullOrEmpty(Estensione) ? "" : $" int.{Estensione}")}";
    }
    
    /// <summary>
    /// DTO per creazione telefono
    /// </summary>
    public class TelefonoDTO_Create
    {
        [Required]
        public int EntitaAziendaleId { get; set; }
        
        [Required]
        public int TipoTelefonoId { get; set; }
        
        [StringLength(20)]
        public string? PrefissoInternazionale { get; set; }
        
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[\d\s\-\(\)]+$")]
        public string Numero { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? Estensione { get; set; }
        
        [StringLength(500)]
        public string? Note { get; set; }
    }
}
```

### 4.4 Tipologiche DTO

```csharp
namespace Accredia.GestioneAnagrafica.Shared.DTOs
{
    public class TipoIndirizzoDTO
    {
        public int TipoIndirizzoId { get; set; }
        public string? Codice { get; set; }
        public string? Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataInizioValidita { get; set; }
        public DateTime DataFineValidita { get; set; }
    }
    
    public class TipoEmailDTO
    {
        public int TipoEmailId { get; set; }
        public string? Codice { get; set; }
        public string? Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataInizioValidita { get; set; }
        public DateTime DataFineValidita { get; set; }
    }
    
    public class TipoTelefonoDTO
    {
        public int TipoTelefonoId { get; set; }
        public string? Codice { get; set; }
        public string? Descrizione { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataInizioValidita { get; set; }
        public DateTime DataFineValidita { get; set; }
    }
}
```

## 5. Servizi API

### 5.1 IIndirizziService

```csharp
namespace Accredia.GestioneAnagrafica.Client.Services
{
    public interface IIndirizziService
    {
        Task<List<IndirizzoDTO>> GetIndirizziByEntitaAsync(int entitaAziendaleId);
        Task<IndirizzoDTO> GetIndirizzoByIdAsync(int indirizzoId);
        Task<IndirizzoDTO> CreateIndirizzoAsync(IndirizzoDTO_Create indirizzo);
        Task<IndirizzoDTO> UpdateIndirizzoAsync(int indirizzoId, IndirizzoDTO_Create indirizzo);
        Task<bool> DeleteIndirizzoAsync(int indirizzoId);
        
        // Autocomplete service (mock)
        Task<List<IndirizzoSuggestion>> SearchIndirizziAsync(string searchText);
    }
    
    public class IndirizzoSuggestion
    {
        public string Via { get; set; } = string.Empty;
        public string? Civico { get; set; }
        public string Cap { get; set; } = string.Empty;
        public string Comune { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public int ComuneId { get; set; }
        public int ProvinciaId { get; set; }
    }
}
```

### 5.2 IEmailService

```csharp
namespace Accredia.GestioneAnagrafica.Client.Services
{
    public interface IEmailService
    {
        Task<List<EmailDTO>> GetEmailByEntitaAsync(int entitaAziendaleId);
        Task<EmailDTO> GetEmailByIdAsync(int emailId);
        Task<EmailDTO> CreateEmailAsync(EmailDTO_Create email);
        Task<EmailDTO> UpdateEmailAsync(int emailId, EmailDTO_Create email);
        Task<bool> DeleteEmailAsync(int emailId);
        Task<bool> CheckEmailDuplicateAsync(string indirizzoEmail, int entitaAziendaleId, int? excludeEmailId = null);
    }
}
```

### 5.3 ITelefoniService

```csharp
namespace Accredia.GestioneAnagrafica.Client.Services
{
    public interface ITelefoniService
    {
        Task<List<TelefonoDTO>> GetTelefoniByEntitaAsync(int entitaAziendaleId);
        Task<TelefonoDTO> GetTelefonoByIdAsync(int telefonoId);
        Task<TelefonoDTO> CreateTelefonoAsync(TelefonoDTO_Create telefono);
        Task<TelefonoDTO> UpdateTelefonoAsync(int telefonoId, TelefonoDTO_Create telefono);
        Task<bool> DeleteTelefonoAsync(int telefonoId);
    }
}
```

### 5.4 ITipologicheService

```csharp
namespace Accredia.GestioneAnagrafica.Client.Services
{
    public interface ITipologicheService
    {
        Task<List<TipoIndirizzoDTO>> GetTipiIndirizzoAsync();
        Task<List<TipoEmailDTO>> GetTipiEmailAsync();
        Task<List<TipoTelefonoDTO>> GetTipiTelefonoAsync();
    }
}
```

## 6. Mock Service per Autocomplete Indirizzi

```csharp
namespace Accredia.GestioneAnagrafica.Client.Services.Mock
{
    public class IndirizziMockService : IIndirizziService
    {
        // Mock data per autocomplete
        private static readonly List<IndirizzoSuggestion> MockIndirizzi = new()
        {
            new() { Via = "Via Roma", Cap = "00100", Comune = "Roma", Provincia = "RM", ComuneId = 1, ProvinciaId = 1 },
            new() { Via = "Via Milano", Cap = "20100", Comune = "Milano", Provincia = "MI", ComuneId = 2, ProvinciaId = 2 },
            new() { Via = "Via Napoli", Cap = "80100", Comune = "Napoli", Provincia = "NA", ComuneId = 3, ProvinciaId = 3 },
            new() { Via = "Via Torino", Cap = "10100", Comune = "Torino", Provincia = "TO", ComuneId = 4, ProvinciaId = 4 },
            new() { Via = "Via Firenze", Cap = "50100", Comune = "Firenze", Provincia = "FI", ComuneId = 5, ProvinciaId = 5 },
            new() { Via = "Via Bologna", Cap = "40100", Comune = "Bologna", Provincia = "BO", ComuneId = 6, ProvinciaId = 6 },
            new() { Via = "Via Genova", Cap = "16100", Comune = "Genova", Provincia = "GE", ComuneId = 7, ProvinciaId = 7 },
            new() { Via = "Via Venezia", Cap = "30100", Comune = "Venezia", Provincia = "VE", ComuneId = 8, ProvinciaId = 8 },
            new() { Via = "Corso Vittorio Emanuele II", Cap = "00100", Comune = "Roma", Provincia = "RM", ComuneId = 1, ProvinciaId = 1 },
            new() { Via = "Corso Buenos Aires", Cap = "20100", Comune = "Milano", Provincia = "MI", ComuneId = 2, ProvinciaId = 2 },
            new() { Via = "Via Garibaldi", Cap = "80100", Comune = "Napoli", Provincia = "NA", ComuneId = 3, ProvinciaId = 3 },
            new() { Via = "Via Po", Cap = "10100", Comune = "Torino", Provincia = "TO", ComuneId = 4, ProvinciaId = 4 },
            new() { Via = "Piazza del Duomo", Cap = "50100", Comune = "Firenze", Provincia = "FI", ComuneId = 5, ProvinciaId = 5 },
            new() { Via = "Via Indipendenza", Cap = "40100", Comune = "Bologna", Provincia = "BO", ComuneId = 6, ProvinciaId = 6 },
            new() { Via = "Via XX Settembre", Cap = "16100", Comune = "Genova", Provincia = "GE", ComuneId = 7, ProvinciaId = 7 },
            new() { Via = "Via Dante Alighieri", Cap = "00100", Comune = "Roma", Provincia = "RM", ComuneId = 1, ProvinciaId = 1 }
        };
        
        public async Task<List<IndirizzoSuggestion>> SearchIndirizziAsync(string searchText)
        {
            await Task.Delay(300); // Simula latenza rete
            
            if (string.IsNullOrWhiteSpace(searchText) || searchText.Length < 3)
                return new List<IndirizzoSuggestion>();
            
            return MockIndirizzi
                .Where(i => i.Via.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                           i.Comune.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                           i.Cap.Contains(searchText))
                .Take(10)
                .ToList();
        }
        
        // Implementare altri metodi dell'interfaccia...
    }
}
```

## 7. Componente PersonaIndirizzo.razor

### 7.1 Struttura del componente

```razor
@* PersonaIndirizzo.razor *@
@using Accredia.GestioneAnagrafica.Shared.DTOs
@using Accredia.GestioneAnagrafica.Client.Services
@inject IIndirizziService IndirizziService
@inject ITipologicheService TipologicheService
@inject IDialogService DialogService
@inject ISnackbar Snackbar

<MudCard Elevation="0" Class="mt-4">
    <MudCardHeader>
        <CardHeaderContent>
            <MudText Typo="Typo.h6">
                <MudIcon Icon="@Icons.Material.Filled.Place" Class="mr-2" />
                Indirizzi
            </MudText>
        </CardHeaderContent>
        <CardHeaderActions>
            @if (!ReadOnly)
            {
                <MudButton StartIcon="@Icons.Material.Filled.Add" 
                          Color="Color.Primary" 
                          Variant="Variant.Filled"
                          Size="Size.Small"
                          OnClick="OpenAddDialog">
                    Aggiungi Indirizzo
                </MudButton>
            }
        </CardHeaderActions>
    </MudCardHeader>
    <MudCardContent>
        @if (IsLoading)
        {
            <MudProgressLinear Color="Color.Primary" Indeterminate="true" />
        }
        else if (Indirizzi == null || !Indirizzi.Any())
        {
            <MudAlert Severity="Severity.Info">Nessun indirizzo presente.</MudAlert>
        }
        else
        {
            <MudDataGrid T="IndirizzoDTO" 
                        Items="@Indirizzi" 
                        Dense="true"
                        Hover="true"
                        Bordered="true"
                        Striped="true"
                        ReadOnly="true"
                        EditMode="DataGridEditMode.Cell">
                <Columns>
                    <PropertyColumn Property="x => x.TipoIndirizzoDescrizione" Title="Tipo" />
                    <PropertyColumn Property="x => x.Via" Title="Via" />
                    <PropertyColumn Property="x => x.Civico" Title="Civico" />
                    <PropertyColumn Property="x => x.Interno" Title="Interno" />
                    <PropertyColumn Property="x => x.Cap" Title="CAP" />
                    <PropertyColumn Property="x => x.ComuneNome" Title="Comune" />
                    <PropertyColumn Property="x => x.ProvinciaSigla" Title="Prov." />
                    
                    @if (!ReadOnly)
                    {
                        <TemplateColumn Title="Azioni" Sortable="false">
                            <CellTemplate>
                                <MudIconButton Icon="@Icons.Material.Filled.Edit" 
                                              Color="Color.Primary" 
                                              Size="Size.Small"
                                              OnClick="@(() => OpenEditDialog(context.Item))" />
                                <MudIconButton Icon="@Icons.Material.Filled.Delete" 
                                              Color="Color.Error" 
                                              Size="Size.Small"
                                              OnClick="@(() => DeleteIndirizzo(context.Item))" />
                            </CellTemplate>
                        </TemplateColumn>
                    }
                </Columns>
            </MudDataGrid>
        }
    </MudCardContent>
</MudCard>

@code {
    [Parameter] public int EntitaAziendaleId { get; set; }
    [Parameter] public bool ReadOnly { get; set; } = false;
    [Parameter] public EventCallback OnDataChanged { get; set; }
    
    private List<IndirizzoDTO> Indirizzi { get; set; } = new();
    private bool IsLoading { get; set; } = false;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadIndirizzi();
    }
    
    private async Task LoadIndirizzi()
    {
        if (EntitaAziendaleId <= 0) return;
        
        IsLoading = true;
        try
        {
            Indirizzi = await IndirizziService.GetIndirizziByEntitaAsync(EntitaAziendaleId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Errore nel caricamento degli indirizzi: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private async Task OpenAddDialog()
    {
        var parameters = new DialogParameters
        {
            ["EntitaAziendaleId"] = EntitaAziendaleId,
            ["IsEditMode"] = false
        };
        
        var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<IndirizzoDialog>("Nuovo Indirizzo", parameters, options);
        var result = await dialog.Result;
        
        if (!result.Canceled)
        {
            await LoadIndirizzi();
            await OnDataChanged.InvokeAsync();
        }
    }
    
    private async Task OpenEditDialog(IndirizzoDTO indirizzo)
    {
        var parameters = new DialogParameters
        {
            ["EntitaAziendaleId"] = EntitaAziendaleId,
            ["Indirizzo"] = indirizzo,
            ["IsEditMode"] = true
        };
        
        var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<IndirizzoDialog>("Modifica Indirizzo", parameters, options);
        var result = await dialog.Result;
        
        if (!result.Canceled)
        {
            await LoadIndirizzi();
            await OnDataChanged.InvokeAsync();
        }
    }
    
    private async Task DeleteIndirizzo(IndirizzoDTO indirizzo)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Conferma eliminazione",
            $"Sei sicuro di voler eliminare l'indirizzo in {indirizzo.Via}?",
            yesText: "Elimina", cancelText: "Annulla");
        
        if (confirmed == true)
        {
            try
            {
                await IndirizziService.DeleteIndirizzoAsync(indirizzo.IndirizzoId);
                Snackbar.Add("Indirizzo eliminato con successo", Severity.Success);
                await LoadIndirizzi();
                await OnDataChanged.InvokeAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Errore durante l'eliminazione: {ex.Message}", Severity.Error);
            }
        }
    }
    
    public async Task RefreshData()
    {
        await LoadIndirizzi();
    }
}
```

### 7.2 Dialog IndirizzoDialog.razor

```razor
@* IndirizzoDialog.razor *@
@using Accredia.GestioneAnagrafica.Shared.DTOs
@using Accredia.GestioneAnagrafica.Client.Services
@inject IIndirizziService IndirizziService
@inject ITipologicheService TipologicheService
@inject ISnackbar Snackbar

<MudDialog>
    <DialogContent>
        <MudForm @ref="form" Model="@Model" Validation="@(ValidateModel)">
            <MudGrid>
                <!-- Tipo Indirizzo -->
                <MudItem xs="12">
                    <MudSelect @bind-Value="Model.TipoIndirizzoId" 
                              Label="Tipo Indirizzo" 
                              Required="true"
                              RequiredError="Il tipo indirizzo è obbligatorio">
                        @foreach (var tipo in TipiIndirizzo)
                        {
                            <MudSelectItem Value="@tipo.TipoIndirizzoId">@tipo.Descrizione</MudSelectItem>
                        }
                    </MudSelect>
                </MudItem>
                
                <!-- Autocomplete Via -->
                <MudItem xs="12">
                    <MudAutocomplete T="IndirizzoSuggestion"
                                    @bind-Value="SelectedSuggestion"
                                    SearchFunc="@SearchIndirizzi"
                                    Label="Cerca Indirizzo"
                                    Placeholder="Inizia a digitare via, comune o CAP..."
                                    ToStringFunc="@(s => s?.Via ?? "")"
                                    ValueChanged="@OnSuggestionSelected"
                                    AdornmentIcon="@Icons.Material.Filled.Search"
                                    AdornmentColor="Color.Primary"
                                    Clearable="true"
                                    ResetValueOnEmptyText="true"
                                    CoerceText="false"
                                    CoerceValue="false">
                        <ItemTemplate Context="suggestion">
                            <MudText Typo="Typo.body2">
                                <strong>@suggestion.Via</strong>
                            </MudText>
                            <MudText Typo="Typo.caption">
                                @suggestion.Cap @suggestion.Comune (@suggestion.Provincia)
                            </MudText>
                        </ItemTemplate>
                    </MudAutocomplete>
                </MudItem>
                
                <!-- Via -->
                <MudItem xs="12" md="8">
                    <MudTextField @bind-Value="Model.Via" 
                                 Label="Via" 
                                 Required="true"
                                 RequiredError="La via è obbligatoria"
                                 MaxLength="200"
                                 Counter="200"
                                 Immediate="true" />
                </MudItem>
                
                <!-- Civico -->
                <MudItem xs="12" md="4">
                    <MudTextField @bind-Value="Model.Civico" 
                                 Label="Civico" 
                                 MaxLength="20" />
                </MudItem>
                
                <!-- Interno -->
                <MudItem xs="12" md="6">
                    <MudTextField @bind-Value="Model.Interno" 
                                 Label="Interno/Scala" 
                                 MaxLength="20" />
                </MudItem>
                
                <!-- Presso -->
                <MudItem xs="12" md="6">
                    <MudTextField @bind-Value="Model.Presso" 
                                 Label="Presso" 
                                 MaxLength="100" />
                </MudItem>
                
                <!-- CAP -->
                <MudItem xs="12" md="4">
                    <MudTextField @bind-Value="Model.Cap" 
                                 Label="CAP" 
                                 Required="true"
                                 RequiredError="Il CAP è obbligatorio"
                                 MaxLength="5"
                                 Mask="@(new PatternMask("00000"))"
                                 Immediate="true"
                                 Validation="@(new Func<string, string>(ValidateCAP))" />
                </MudItem>
                
                <!-- Comune (placeholder - da implementare con lookup reale) -->
                <MudItem xs="12" md="8">
                    <MudTextField @bind-Value="ComuneDisplay" 
                                 Label="Comune" 
                                 ReadOnly="true"
                                 Adornment="Adornment.End"
                                 AdornmentIcon="@Icons.Material.Filled.Search"
                                 OnAdornmentClick="OpenComuneSearch" />
                </MudItem>
                
                <!-- Note -->
                <MudItem xs="12">
                    <MudTextField @bind-Value="Model.Note" 
                                 Label="Note" 
                                 Lines="3"
                                 MaxLength="500"
                                 Counter="500" />
                </MudItem>
            </MudGrid>
        </MudForm>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="Cancel">Annulla</MudButton>
        <MudButton Color="Color.Primary" Variant="Variant.Filled" OnClick="Submit">
            @(IsEditMode ? "Salva" : "Crea")
        </MudButton>
    </DialogActions>
</MudDialog>

@code {
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public int EntitaAziendaleId { get; set; }
    [Parameter] public IndirizzoDTO? Indirizzo { get; set; }
    [Parameter] public bool IsEditMode { get; set; }
    
    private MudForm form = null!;
    private IndirizzoDTO_Create Model { get; set; } = new();
    private List<TipoIndirizzoDTO> TipiIndirizzo { get; set; } = new();
    private IndirizzoSuggestion? SelectedSuggestion { get; set; }
    private string ComuneDisplay { get; set; } = string.Empty;
    
    protected override async Task OnInitializedAsync()
    {
        TipiIndirizzo = await TipologicheService.GetTipiIndirizzoAsync();
        
        if (IsEditMode && Indirizzo != null)
        {
            Model.EntitaAziendaleId = Indirizzo.EntitaAziendaleId;
            Model.TipoIndirizzoId = Indirizzo.TipoIndirizzoId;
            Model.Via = Indirizzo.Via;
            Model.Civico = Indirizzo.Civico;
            Model.Interno = Indirizzo.Interno;
            Model.Presso = Indirizzo.Presso;
            Model.Cap = Indirizzo.Cap;
            Model.ComuneId = Indirizzo.ComuneId;
            Model.ProvinciaId = Indirizzo.ProvinciaId;
            Model.StatoId = Indirizzo.StatoId;
            Model.Note = Indirizzo.Note;
            ComuneDisplay = $"{Indirizzo.ComuneNome} ({Indirizzo.ProvinciaSigla})";
        }
        else
        {
            Model.EntitaAziendaleId = EntitaAziendaleId;
        }
    }
    
    private async Task<IEnumerable<IndirizzoSuggestion>> SearchIndirizzi(string value, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
            return Enumerable.Empty<IndirizzoSuggestion>();
        
        try
        {
            return await IndirizziService.SearchIndirizziAsync(value);
        }
        catch
        {
            return Enumerable.Empty<IndirizzoSuggestion>();
        }
    }
    
    private void OnSuggestionSelected(IndirizzoSuggestion? suggestion)
    {
        if (suggestion != null)
        {
            Model.Via = suggestion.Via;
            Model.Civico = suggestion.Civico;
            Model.Cap = suggestion.Cap;
            Model.ComuneId = suggestion.ComuneId;
            Model.ProvinciaId = suggestion.ProvinciaId;
            ComuneDisplay = $"{suggestion.Comune} ({suggestion.Provincia})";
        }
    }
    
    private string? ValidateCAP(string cap)
    {
        if (string.IsNullOrWhiteSpace(cap))
            return null;
        
        if (!System.Text.RegularExpressions.Regex.IsMatch(cap, @"^\d{5}$"))
            return "Il CAP deve essere composto da 5 cifre";
        
        return null;
    }
    
    private Task<bool> ValidateModel()
    {
        // Validazione custom aggiuntiva se necessaria
        return Task.FromResult(true);
    }
    
    private void OpenComuneSearch()
    {
        // TODO: Implementare dialog per ricerca comune
        Snackbar.Add("Ricerca comune non ancora implementata", Severity.Info);
    }
    
    private void Cancel()
    {
        MudDialog.Cancel();
    }
    
    private async Task Submit()
    {
        await form.Validate();
        
        if (!form.IsValid)
        {
            Snackbar.Add("Correggi gli errori nel form", Severity.Warning);
            return;
        }
        
        try
        {
            if (IsEditMode && Indirizzo != null)
            {
                await IndirizziService.UpdateIndirizzoAsync(Indirizzo.IndirizzoId, Model);
                Snackbar.Add("Indirizzo aggiornato con successo", Severity.Success);
            }
            else
            {
                await IndirizziService.CreateIndirizzoAsync(Model);
                Snackbar.Add("Indirizzo creato con successo", Severity.Success);
            }
            
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Errore durante il salvataggio: {ex.Message}", Severity.Error);
        }
    }
}
```

## 8. Componente PersonaEmail.razor

```razor
@* PersonaEmail.razor *@
@using Accredia.GestioneAnagrafica.Shared.DTOs
@using Accredia.GestioneAnagrafica.Client.Services
@inject IEmailService EmailService
@inject ITipologicheService TipologicheService
@inject IDialogService DialogService
@inject ISnackbar Snackbar

<MudCard Elevation="0" Class="mt-4">
    <MudCardHeader>
        <CardHeaderContent>
            <MudText Typo="Typo.h6">
                <MudIcon Icon="@Icons.Material.Filled.Email" Class="mr-2" />
                Email
            </MudText>
        </CardHeaderContent>
        <CardHeaderActions>
            @if (!ReadOnly)
            {
                <MudButton StartIcon="@Icons.Material.Filled.Add" 
                          Color="Color.Primary" 
                          Variant="Variant.Filled"
                          Size="Size.Small"
                          OnClick="OpenAddDialog">
                    Aggiungi Email
                </MudButton>
            }
        </CardHeaderActions>
    </MudCardHeader>
    <MudCardContent>
        @if (IsLoading)
        {
            <MudProgressLinear Color="Color.Primary" Indeterminate="true" />
        }
        else if (Emails == null || !Emails.Any())
        {
            <MudAlert Severity="Severity.Info">Nessuna email presente.</MudAlert>
        }
        else
        {
            <MudDataGrid T="EmailDTO" 
                        Items="@Emails" 
                        Dense="true"
                        Hover="true"
                        Bordered="true"
                        Striped="true"
                        ReadOnly="true">
                <Columns>
                    <TemplateColumn Title="Tipo">
                        <CellTemplate>
                            <MudChip Size="Size.Small" 
                                    Color="@GetEmailTypeColor(context.Item.TipoEmailCodice)">
                                @context.Item.TipoEmailDescrizione
                            </MudChip>
                        </CellTemplate>
                    </TemplateColumn>
                    <PropertyColumn Property="x => x.IndirizzoEmail" Title="Indirizzo Email">
                        <CellTemplate>
                            <MudLink Href="@($"mailto:{context.Item.IndirizzoEmail}")" Target="_blank">
                                @context.Item.IndirizzoEmail
                            </MudLink>
                        </CellTemplate>
                    </PropertyColumn>
                    <PropertyColumn Property="x => x.Note" Title="Note" />
                    
                    @if (!ReadOnly)
                    {
                        <TemplateColumn Title="Azioni" Sortable="false">
                            <CellTemplate>
                                <MudIconButton Icon="@Icons.Material.Filled.Edit" 
                                              Color="Color.Primary" 
                                              Size="Size.Small"
                                              OnClick="@(() => OpenEditDialog(context.Item))" />
                                <MudIconButton Icon="@Icons.Material.Filled.Delete" 
                                              Color="Color.Error" 
                                              Size="Size.Small"
                                              OnClick="@(() => DeleteEmail(context.Item))" />
                            </CellTemplate>
                        </TemplateColumn>
                    }
                </Columns>
            </MudDataGrid>
        }
    </MudCardContent>
</MudCard>

@code {
    [Parameter] public int EntitaAziendaleId { get; set; }
    [Parameter] public bool ReadOnly { get; set; } = false;
    [Parameter] public EventCallback OnDataChanged { get; set; }
    
    private List<EmailDTO> Emails { get; set; } = new();
    private bool IsLoading { get; set; } = false;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadEmails();
    }
    
    private async Task LoadEmails()
    {
        if (EntitaAziendaleId <= 0) return;
        
        IsLoading = true;
        try
        {
            Emails = await EmailService.GetEmailByEntitaAsync(EntitaAziendaleId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Errore nel caricamento delle email: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private Color GetEmailTypeColor(string? codice) => codice switch
    {
        "AZI" => Color.Primary,
        "PER" => Color.Info,
        "PEC" => Color.Success,
        "ALT" => Color.Default,
        _ => Color.Default
    };
    
    private async Task OpenAddDialog()
    {
        var parameters = new DialogParameters
        {
            ["EntitaAziendaleId"] = EntitaAziendaleId,
            ["IsEditMode"] = false
        };
        
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<EmailDialog>("Nuova Email", parameters, options);
        var result = await dialog.Result;
        
        if (!result.Canceled)
        {
            await LoadEmails();
            await OnDataChanged.InvokeAsync();
        }
    }
    
    private async Task OpenEditDialog(EmailDTO email)
    {
        var parameters = new DialogParameters
        {
            ["EntitaAziendaleId"] = EntitaAziendaleId,
            ["Email"] = email,
            ["IsEditMode"] = true
        };
        
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<EmailDialog>("Modifica Email", parameters, options);
        var result = await dialog.Result;
        
        if (!result.Canceled)
        {
            await LoadEmails();
            await OnDataChanged.InvokeAsync();
        }
    }
    
    private async Task DeleteEmail(EmailDTO email)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Conferma eliminazione",
            $"Sei sicuro di voler eliminare l'email {email.IndirizzoEmail}?",
            yesText: "Elimina", cancelText: "Annulla");
        
        if (confirmed == true)
        {
            try
            {
                await EmailService.DeleteEmailAsync(email.EmailId);
                Snackbar.Add("Email eliminata con successo", Severity.Success);
                await LoadEmails();
                await OnDataChanged.InvokeAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Errore durante l'eliminazione: {ex.Message}", Severity.Error);
            }
        }
    }
    
    public async Task RefreshData()
    {
        await LoadEmails();
    }
}
```

### 8.1 Dialog EmailDialog.razor

```razor
@* EmailDialog.razor *@
@using Accredia.GestioneAnagrafica.Shared.DTOs
@using Accredia.GestioneAnagrafica.Client.Services
@inject IEmailService EmailService
@inject ITipologicheService TipologicheService
@inject ISnackbar Snackbar

<MudDialog>
    <DialogContent>
        <MudForm @ref="form" Model="@Model" Validation="@(ValidateModel)">
            <MudGrid>
                <!-- Tipo Email -->
                <MudItem xs="12">
                    <MudSelect @bind-Value="Model.TipoEmailId" 
                              Label="Tipo Email" 
                              Required="true"
                              RequiredError="Il tipo email è obbligatorio">
                        @foreach (var tipo in TipiEmail)
                        {
                            <MudSelectItem Value="@tipo.TipoEmailId">
                                @tipo.Descrizione (@tipo.Codice)
                            </MudSelectItem>
                        }
                    </MudSelect>
                </MudItem>
                
                <!-- Indirizzo Email -->
                <MudItem xs="12">
                    <MudTextField @bind-Value="Model.IndirizzoEmail" 
                                 Label="Indirizzo Email" 
                                 Required="true"
                                 RequiredError="L'indirizzo email è obbligatorio"
                                 Validation="@(new EmailAddressAttribute())"
                                 MaxLength="254"
                                 Immediate="true"
                                 InputType="InputType.Email"
                                 AdornmentIcon="@Icons.Material.Filled.AlternateEmail"
                                 Adornment="Adornment.Start"
                                 OnBlur="CheckDuplicate" />
                    @if (IsDuplicate)
                    {
                        <MudAlert Severity="Severity.Warning" Class="mt-2">
                            Questa email è già presente per questa entità!
                        </MudAlert>
                    }
                </MudItem>
                
                <!-- Note -->
                <MudItem xs="12">
                    <MudTextField @bind-Value="Model.Note" 
                                 Label="Note" 
                                 Lines="3"
                                 MaxLength="500"
                                 Counter="500" />
                </MudItem>
            </MudGrid>
        </MudForm>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="Cancel">Annulla</MudButton>
        <MudButton Color="Color.Primary" 
                  Variant="Variant.Filled" 
                  OnClick="Submit"
                  Disabled="@IsDuplicate">
            @(IsEditMode ? "Salva" : "Crea")
        </MudButton>
    </DialogActions>
</MudDialog>

@code {
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public int EntitaAziendaleId { get; set; }
    [Parameter] public EmailDTO? Email { get; set; }
    [Parameter] public bool IsEditMode { get; set; }
    
    private MudForm form = null!;
    private EmailDTO_Create Model { get; set; } = new();
    private List<TipoEmailDTO> TipiEmail { get; set; } = new();
    private bool IsDuplicate { get; set; } = false;
    
    protected override async Task OnInitializedAsync()
    {
        TipiEmail = await TipologicheService.GetTipiEmailAsync();
        
        if (IsEditMode && Email != null)
        {
            Model.EntitaAziendaleId = Email.EntitaAziendaleId;
            Model.TipoEmailId = Email.TipoEmailId;
            Model.IndirizzoEmail = Email.IndirizzoEmail;
            Model.Note = Email.Note;
        }
        else
        {
            Model.EntitaAziendaleId = EntitaAziendaleId;
        }
    }
    
    private async Task CheckDuplicate()
    {
        if (string.IsNullOrWhiteSpace(Model.IndirizzoEmail))
        {
            IsDuplicate = false;
            return;
        }
        
        try
        {
            int? excludeId = IsEditMode && Email != null ? Email.EmailId : null;
            IsDuplicate = await EmailService.CheckEmailDuplicateAsync(
                Model.IndirizzoEmail, 
                EntitaAziendaleId, 
                excludeId);
        }
        catch
        {
            IsDuplicate = false;
        }
    }
    
    private Task<bool> ValidateModel()
    {
        return Task.FromResult(true);
    }
    
    private void Cancel()
    {
        MudDialog.Cancel();
    }
    
    private async Task Submit()
    {
        await form.Validate();
        
        if (!form.IsValid || IsDuplicate)
        {
            Snackbar.Add("Correggi gli errori nel form", Severity.Warning);
            return;
        }
        
        try
        {
            if (IsEditMode && Email != null)
            {
                await EmailService.UpdateEmailAsync(Email.EmailId, Model);
                Snackbar.Add("Email aggiornata con successo", Severity.Success);
            }
            else
            {
                await EmailService.CreateEmailAsync(Model);
                Snackbar.Add("Email creata con successo", Severity.Success);
            }
            
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Errore durante il salvataggio: {ex.Message}", Severity.Error);
        }
    }
}
```

## 9. Componente PersonaTelefono.razor

```razor
@* PersonaTelefono.razor *@
@using Accredia.GestioneAnagrafica.Shared.DTOs
@using Accredia.GestioneAnagrafica.Client.Services
@inject ITelefoniService TelefoniService
@inject ITipologicheService TipologicheService
@inject IDialogService DialogService
@inject ISnackbar Snackbar

<MudCard Elevation="0" Class="mt-4">
    <MudCardHeader>
        <CardHeaderContent>
            <MudText Typo="Typo.h6">
                <MudIcon Icon="@Icons.Material.Filled.Phone" Class="mr-2" />
                Telefoni
            </MudText>
        </CardHeaderContent>
        <CardHeaderActions>
            @if (!ReadOnly)
            {
                <MudButton StartIcon="@Icons.Material.Filled.Add" 
                          Color="Color.Primary" 
                          Variant="Variant.Filled"
                          Size="Size.Small"
                          OnClick="OpenAddDialog">
                    Aggiungi Telefono
                </MudButton>
            }
        </CardHeaderActions>
    </MudCardHeader>
    <MudCardContent>
        @if (IsLoading)
        {
            <MudProgressLinear Color="Color.Primary" Indeterminate="true" />
        }
        else if (Telefoni == null || !Telefoni.Any())
        {
            <MudAlert Severity="Severity.Info">Nessun telefono presente.</MudAlert>
        }
        else
        {
            <MudDataGrid T="TelefonoDTO" 
                        Items="@Telefoni" 
                        Dense="true"
                        Hover="true"
                        Bordered="true"
                        Striped="true"
                        ReadOnly="true">
                <Columns>
                    <PropertyColumn Property="x => x.TipoTelefonoDescrizione" Title="Tipo" />
                    <PropertyColumn Property="x => x.NumeroCompleto" Title="Numero">
                        <CellTemplate>
                            <MudLink Href="@($"tel:{context.Item.PrefissoInternazionale}{context.Item.Numero}")">
                                @context.Item.NumeroCompleto
                            </MudLink>
                        </CellTemplate>
                    </PropertyColumn>
                    <PropertyColumn Property="x => x.Note" Title="Note" />
                    
                    @if (!ReadOnly)
                    {
                        <TemplateColumn Title="Azioni" Sortable="false">
                            <CellTemplate>
                                <MudIconButton Icon="@Icons.Material.Filled.Edit" 
                                              Color="Color.Primary" 
                                              Size="Size.Small"
                                              OnClick="@(() => OpenEditDialog(context.Item))" />
                                <MudIconButton Icon="@Icons.Material.Filled.Delete" 
                                              Color="Color.Error" 
                                              Size="Size.Small"
                                              OnClick="@(() => DeleteTelefono(context.Item))" />
                            </CellTemplate>
                        </TemplateColumn>
                    }
                </Columns>
            </MudDataGrid>
        }
    </MudCardContent>
</MudCard>

@code {
    [Parameter] public int EntitaAziendaleId { get; set; }
    [Parameter] public bool ReadOnly { get; set; } = false;
    [Parameter] public EventCallback OnDataChanged { get; set; }
    
    private List<TelefonoDTO> Telefoni { get; set; } = new();
    private bool IsLoading { get; set; } = false;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadTelefoni();
    }
    
    private async Task LoadTelefoni()
    {
        if (EntitaAziendaleId <= 0) return;
        
        IsLoading = true;
        try
        {
            Telefoni = await TelefoniService.GetTelefoniByEntitaAsync(EntitaAziendaleId);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Errore nel caricamento dei telefoni: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private async Task OpenAddDialog()
    {
        var parameters = new DialogParameters
        {
            ["EntitaAziendaleId"] = EntitaAziendaleId,
            ["IsEditMode"] = false
        };
        
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<TelefonoDialog>("Nuovo Telefono", parameters, options);
        var result = await dialog.Result;
        
        if (!result.Canceled)
        {
            await LoadTelefoni();
            await OnDataChanged.InvokeAsync();
        }
    }
    
    private async Task OpenEditDialog(TelefonoDTO telefono)
    {
        var parameters = new DialogParameters
        {
            ["EntitaAziendaleId"] = EntitaAziendaleId,
            ["Telefono"] = telefono,
            ["IsEditMode"] = true
        };
        
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<TelefonoDialog>("Modifica Telefono", parameters, options);
        var result = await dialog.Result;
        
        if (!result.Canceled)
        {
            await LoadTelefoni();
            await OnDataChanged.InvokeAsync();
        }
    }
    
    private async Task DeleteTelefono(TelefonoDTO telefono)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Conferma eliminazione",
            $"Sei sicuro di voler eliminare il telefono {telefono.NumeroCompleto}?",
            yesText: "Elimina", cancelText: "Annulla");
        
        if (confirmed == true)
        {
            try
            {
                await TelefoniService.DeleteTelefonoAsync(telefono.TelefonoId);
                Snackbar.Add("Telefono eliminato con successo", Severity.Success);
                await LoadTelefoni();
                await OnDataChanged.InvokeAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Errore durante l'eliminazione: {ex.Message}", Severity.Error);
            }
        }
    }
    
    public async Task RefreshData()
    {
        await LoadTelefoni();
    }
}
```

### 9.1 Dialog TelefonoDialog.razor

```razor
@* TelefonoDialog.razor *@
@using Accredia.GestioneAnagrafica.Shared.DTOs
@using Accredia.GestioneAnagrafica.Client.Services
@inject ITelefoniService TelefoniService
@inject ITipologicheService TipologicheService
@inject ISnackbar Snackbar

<MudDialog>
    <DialogContent>
        <MudForm @ref="form" Model="@Model" Validation="@(ValidateModel)">
            <MudGrid>
                <!-- Tipo Telefono -->
                <MudItem xs="12">
                    <MudSelect @bind-Value="Model.TipoTelefonoId" 
                              Label="Tipo Telefono" 
                              Required="true"
                              RequiredError="Il tipo telefono è obbligatorio">
                        @foreach (var tipo in TipiTelefono)
                        {
                            <MudSelectItem Value="@tipo.TipoTelefonoId">@tipo.Descrizione</MudSelectItem>
                        }
                    </MudSelect>
                </MudItem>
                
                <!-- Prefisso Internazionale -->
                <MudItem xs="12" md="4">
                    <MudTextField @bind-Value="Model.PrefissoInternazionale" 
                                 Label="Prefisso" 
                                 Placeholder="+39"
                                 MaxLength="20"
                                 Adornment="Adornment.Start"
                                 AdornmentText="+" />
                </MudItem>
                
                <!-- Numero -->
                <MudItem xs="12" md="8">
                    <MudTextField @bind-Value="Model.Numero" 
                                 Label="Numero" 
                                 Required="true"
                                 RequiredError="Il numero è obbligatorio"
                                 MaxLength="100"
                                 Immediate="true"
                                 Validation="@(new Func<string, string>(ValidateNumero))"
                                 InputType="InputType.Telephone"
                                 AdornmentIcon="@Icons.Material.Filled.Phone"
                                 Adornment="Adornment.Start" />
                </MudItem>
                
                <!-- Estensione -->
                <MudItem xs="12">
                    <MudTextField @bind-Value="Model.Estensione" 
                                 Label="Estensione/Interno" 
                                 MaxLength="20"
                                 Placeholder="es. 123" />
                </MudItem>
                
                <!-- Note -->
                <MudItem xs="12">
                    <MudTextField @bind-Value="Model.Note" 
                                 Label="Note" 
                                 Lines="3"
                                 MaxLength="500"
                                 Counter="500" />
                </MudItem>
                
                <!-- Anteprima numero completo -->
                @if (!string.IsNullOrWhiteSpace(NumeroCompleto))
                {
                    <MudItem xs="12">
                        <MudAlert Severity="Severity.Info" Dense="true">
                            <strong>Anteprima:</strong> @NumeroCompleto
                        </MudAlert>
                    </MudItem>
                }
            </MudGrid>
        </MudForm>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="Cancel">Annulla</MudButton>
        <MudButton Color="Color.Primary" Variant="Variant.Filled" OnClick="Submit">
            @(IsEditMode ? "Salva" : "Crea")
        </MudButton>
    </DialogActions>
</MudDialog>

@code {
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public int EntitaAziendaleId { get; set; }
    [Parameter] public TelefonoDTO? Telefono { get; set; }
    [Parameter] public bool IsEditMode { get; set; }
    
    private MudForm form = null!;
    private TelefonoDTO_Create Model { get; set; } = new();
    private List<TipoTelefonoDTO> TipiTelefono { get; set; } = new();
    
    private string NumeroCompleto =>
        $"{(string.IsNullOrWhiteSpace(Model.PrefissoInternazionale) ? "" : $"+{Model.PrefissoInternazionale} ")}" +
        $"{Model.Numero}" +
        $"{(string.IsNullOrWhiteSpace(Model.Estensione) ? "" : $" int.{Model.Estensione}")}";
    
    protected override async Task OnInitializedAsync()
    {
        TipiTelefono = await TipologicheService.GetTipiTelefonoAsync();
        
        if (IsEditMode && Telefono != null)
        {
            Model.EntitaAziendaleId = Telefono.EntitaAziendaleId;
            Model.TipoTelefonoId = Telefono.TipoTelefonoId;
            Model.PrefissoInternazionale = Telefono.PrefissoInternazionale;
            Model.Numero = Telefono.Numero;
            Model.Estensione = Telefono.Estensione;
            Model.Note = Telefono.Note;
        }
        else
        {
            Model.EntitaAziendaleId = EntitaAziendaleId;
            Model.PrefissoInternazionale = "39"; // Default Italia
        }
    }
    
    private string? ValidateNumero(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            return null;
        
        if (!System.Text.RegularExpressions.Regex.IsMatch(numero, @"^[\d\s\-\(\)]+$"))
            return "Il numero può contenere solo cifre, spazi, trattini e parentesi";
        
        // Remove formatting characters for length check
        var cleanNumber = System.Text.RegularExpressions.Regex.Replace(numero, @"[\s\-\(\)]", "");
        if (cleanNumber.Length < 6 || cleanNumber.Length > 15)
            return "Il numero deve contenere tra 6 e 15 cifre";
        
        return null;
    }
    
    private Task<bool> ValidateModel()
    {
        return Task.FromResult(true);
    }
    
    private void Cancel()
    {
        MudDialog.Cancel();
    }
    
    private async Task Submit()
    {
        await form.Validate();
        
        if (!form.IsValid)
        {
            Snackbar.Add("Correggi gli errori nel form", Severity.Warning);
            return;
        }
        
        try
        {
            if (IsEditMode && Telefono != null)
            {
                await TelefoniService.UpdateTelefonoAsync(Telefono.TelefonoId, Model);
                Snackbar.Add("Telefono aggiornato con successo", Severity.Success);
            }
            else
            {
                await TelefoniService.CreateTelefonoAsync(Model);
                Snackbar.Add("Telefono creato con successo", Severity.Success);
            }
            
            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Errore durante il salvataggio: {ex.Message}", Severity.Error);
        }
    }
}
```

## 10. Integrazione nelle pagine Persona

### 10.1 PersonaEdit.razor - Struttura con Expansion Panels

```razor
@page "/persone/edit/{Id:int}"
@using Accredia.GestioneAnagrafica.Shared.DTOs
@using Accredia.GestioneAnagrafica.Client.Services
@inject IPersoneService PersoneService
@inject NavigationManager Navigation
@inject ISnackbar Snackbar

<MudContainer MaxWidth="MaxWidth.Large" Class="mt-4">
    <MudText Typo="Typo.h4" Class="mb-4">Modifica Persona</MudText>
    
    @if (IsLoading)
    {
        <MudProgressLinear Indeterminate="true" />
    }
    else if (Persona != null)
    {
        <!-- Expansion Panels per sezioni -->
        <MudExpansionPanels MultiExpansion="true">
            
            <!-- Dati Anagrafici -->
            <MudExpansionPanel Text="Dati Anagrafici" IsInitiallyExpanded="true">
                <MudCard Elevation="0">
                    <MudCardContent>
                        <!-- Form dati anagrafici persona -->
                        <PersonaDatiAnagraficiForm @ref="datiAnagraficiForm" 
                                                  Persona="@Persona" 
                                                  IsEditMode="true" />
                    </MudCardContent>
                    <MudCardActions>
                        <MudButton Color="Color.Primary" 
                                  Variant="Variant.Filled" 
                                  OnClick="SavePersona">
                            Salva Dati Anagrafici
                        </MudButton>
                    </MudCardActions>
                </MudCard>
            </MudExpansionPanel>
            
            <!-- Indirizzi -->
            <MudExpansionPanel Text="Indirizzi">
                <PersonaIndirizzo @ref="indirizziComponent"
                                 EntitaAziendaleId="@Id" 
                                 ReadOnly="false"
                                 OnDataChanged="OnContattiChanged" />
            </MudExpansionPanel>
            
            <!-- Email -->
            <MudExpansionPanel Text="Email">
                <PersonaEmail @ref="emailComponent"
                             EntitaAziendaleId="@Id" 
                             ReadOnly="false"
                             OnDataChanged="OnContattiChanged" />
            </MudExpansionPanel>
            
            <!-- Telefoni -->
            <MudExpansionPanel Text="Telefoni">
                <PersonaTelefono @ref="telefoniComponent"
                                EntitaAziendaleId="@Id" 
                                ReadOnly="false"
                                OnDataChanged="OnContattiChanged" />
            </MudExpansionPanel>
            
        </MudExpansionPanels>
        
        <!-- Azioni footer -->
        <MudPaper Class="mt-4 pa-4" Elevation="0">
            <MudStack Row="true" Justify="Justify.SpaceBetween">
                <MudButton Variant="Variant.Outlined" 
                          OnClick="@(() => Navigation.NavigateTo("/persone"))">
                    Annulla
                </MudButton>
                <MudButton Color="Color.Primary" 
                          Variant="Variant.Filled" 
                          OnClick="SaveAll">
                    Salva Tutto
                </MudButton>
            </MudStack>
        </MudPaper>
    }
</MudContainer>

@code {
    [Parameter] public int Id { get; set; }
    
    private PersonaDTO? Persona { get; set; }
    private bool IsLoading { get; set; } = true;
    
    private PersonaDatiAnagraficiForm? datiAnagraficiForm;
    private PersonaIndirizzo? indirizziComponent;
    private PersonaEmail? emailComponent;
    private PersonaTelefono? telefoniComponent;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadPersona();
    }
    
    private async Task LoadPersona()
    {
        IsLoading = true;
        try
        {
            Persona = await PersoneService.GetPersonaByIdAsync(Id);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Errore nel caricamento: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private async Task SavePersona()
    {
        if (datiAnagraficiForm == null) return;
        
        var isValid = await datiAnagraficiForm.ValidateAsync();
        if (!isValid)
        {
            Snackbar.Add("Correggi gli errori nel form", Severity.Warning);
            return;
        }
        
        try
        {
            await PersoneService.UpdatePersonaAsync(Id, datiAnagraficiForm.GetModel());
            Snackbar.Add("Dati anagrafici salvati con successo", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Errore durante il salvataggio: {ex.Message}", Severity.Error);
        }
    }
    
    private async Task SaveAll()
    {
        await SavePersona();
        // I contatti sono salvati automaticamente dai rispettivi componenti
    }
    
    private Task OnContattiChanged()
    {
        // Callback quando i contatti vengono modificati
        StateHasChanged();
        return Task.CompletedTask;
    }
}
```

### 10.2 PersonaDetails.razor - Visualizzazione Read-Only

```razor
@page "/persone/details/{Id:int}"
@using Accredia.GestioneAnagrafica.Shared.DTOs
@using Accredia.GestioneAnagrafica.Client.Services
@inject IPersoneService PersoneService
@inject NavigationManager Navigation

<MudContainer MaxWidth="MaxWidth.Large" Class="mt-4">
    <MudStack Row="true" Justify="Justify.SpaceBetween" Class="mb-4">
        <MudText Typo="Typo.h4">Dettaglio Persona</MudText>
        <MudButton Variant="Variant.Filled" 
                  Color="Color.Primary"
                  StartIcon="@Icons.Material.Filled.Edit"
                  OnClick="@(() => Navigation.NavigateTo($"/persone/edit/{Id}"))">
            Modifica
        </MudButton>
    </MudStack>
    
    @if (IsLoading)
    {
        <MudProgressLinear Indeterminate="true" />
    }
    else if (Persona != null)
    {
        <MudExpansionPanels MultiExpansion="true">
            
            <!-- Dati Anagrafici -->
            <MudExpansionPanel Text="Dati Anagrafici" IsInitiallyExpanded="true">
                <PersonaDatiAnagraficiDisplay Persona="@Persona" />
            </MudExpansionPanel>
            
            <!-- Indirizzi -->
            <MudExpansionPanel Text="Indirizzi">
                <PersonaIndirizzo EntitaAziendaleId="@Id" ReadOnly="true" />
            </MudExpansionPanel>
            
            <!-- Email -->
            <MudExpansionPanel Text="Email">
                <PersonaEmail EntitaAziendaleId="@Id" ReadOnly="true" />
            </MudExpansionPanel>
            
            <!-- Telefoni -->
            <MudExpansionPanel Text="Telefoni">
                <PersonaTelefono EntitaAziendaleId="@Id" ReadOnly="true" />
            </MudExpansionPanel>
            
        </MudExpansionPanels>
    }
</MudContainer>

@code {
    [Parameter] public int Id { get; set; }
    
    private PersonaDTO? Persona { get; set; }
    private bool IsLoading { get; set; } = true;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadPersona();
    }
    
    private async Task LoadPersona()
    {
        IsLoading = true;
        try
        {
            Persona = await PersoneService.GetPersonaByIdAsync(Id);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
```

## 11. Validatori FluentValidation

```csharp
namespace Accredia.GestioneAnagrafica.Shared.Validators
{
    public class IndirizzoValidator : AbstractValidator<IndirizzoDTO_Create>
    {
        public IndirizzoValidator()
        {
            RuleFor(x => x.EntitaAziendaleId)
                .GreaterThan(0).WithMessage("L'entità aziendale è obbligatoria");
            
            RuleFor(x => x.TipoIndirizzoId)
                .GreaterThan(0).WithMessage("Il tipo indirizzo è obbligatorio");
            
            RuleFor(x => x.Via)
                .NotEmpty().WithMessage("La via è obbligatoria")
                .MaximumLength(200).WithMessage("La via non può superare 200 caratteri");
            
            RuleFor(x => x.Cap)
                .NotEmpty().WithMessage("Il CAP è obbligatorio")
                .Matches(@"^\d{5}$").WithMessage("Il CAP deve essere composto da 5 cifre");
            
            RuleFor(x => x.Civico)
                .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.Civico));
            
            RuleFor(x => x.Interno)
                .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.Interno));
            
            RuleFor(x => x.Presso)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Presso));
            
            RuleFor(x => x.Note)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Note));
        }
    }
    
    public class EmailValidator : AbstractValidator<EmailDTO_Create>
    {
        public EmailValidator()
        {
            RuleFor(x => x.EntitaAziendaleId)
                .GreaterThan(0).WithMessage("L'entità aziendale è obbligatoria");
            
            RuleFor(x => x.TipoEmailId)
                .GreaterThan(0).WithMessage("Il tipo email è obbligatorio");
            
            RuleFor(x => x.IndirizzoEmail)
                .NotEmpty().WithMessage("L'indirizzo email è obbligatorio")
                .EmailAddress().WithMessage("Formato email non valido")
                .MaximumLength(254).WithMessage("L'email non può superare 254 caratteri");
            
            RuleFor(x => x.Note)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Note));
        }
    }
    
    public class TelefonoValidator : AbstractValidator<TelefonoDTO_Create>
    {
        public TelefonoValidator()
        {
            RuleFor(x => x.EntitaAziendaleId)
                .GreaterThan(0).WithMessage("L'entità aziendale è obbligatoria");
            
            RuleFor(x => x.TipoTelefonoId)
                .GreaterThan(0).WithMessage("Il tipo telefono è obbligatorio");
            
            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("Il numero è obbligatorio")
                .MaximumLength(100).WithMessage("Il numero non può superare 100 caratteri")
                .Matches(@"^[\d\s\-\(\)]+$")
                .WithMessage("Il numero può contenere solo cifre, spazi, trattini e parentesi");
            
            RuleFor(x => x.PrefissoInternazionale)
                .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.PrefissoInternazionale));
            
            RuleFor(x => x.Estensione)
                .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.Estensione));
            
            RuleFor(x => x.Note)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Note));
        }
    }
}
```

## 12. Endpoints API REST

```csharp
// Pattern degli endpoint (da implementare nel backend)
// 
// INDIRIZZI
// GET    /api/indirizzi/entita/{entitaAziendaleId}  - Lista indirizzi per entità
// GET    /api/indirizzi/{id}                        - Dettaglio indirizzo
// POST   /api/indirizzi                             - Crea indirizzo
// PUT    /api/indirizzi/{id}                        - Aggiorna indirizzo
// DELETE /api/indirizzi/{id}                        - Elimina indirizzo (soft delete)
// GET    /api/indirizzi/search?q={query}            - Ricerca indirizzi (autocomplete)
//
// EMAIL
// GET    /api/email/entita/{entitaAziendaleId}      - Lista email per entità
// GET    /api/email/{id}                            - Dettaglio email
// POST   /api/email                                 - Crea email
// PUT    /api/email/{id}                            - Aggiorna email
// DELETE /api/email/{id}                            - Elimina email (soft delete)
// GET    /api/email/check-duplicate?email={email}&entitaId={id}&excludeId={id} - Verifica duplicato
//
// TELEFONI
// GET    /api/telefoni/entita/{entitaAziendaleId}   - Lista telefoni per entità
// GET    /api/telefoni/{id}                         - Dettaglio telefono
// POST   /api/telefoni                              - Crea telefono
// PUT    /api/telefoni/{id}                         - Aggiorna telefono
// DELETE /api/telefoni/{id}                         - Elimina telefono (soft delete)
//
// TIPOLOGICHE
// GET    /api/tipologiche/tipi-indirizzo            - Tipi indirizzo
// GET    /api/tipologiche/tipi-email                - Tipi email
// GET    /api/tipologiche/tipi-telefono             - Tipi telefono
```

## 13. Acceptance Criteria e Testing

### 13.1 Acceptance Criteria

**Indirizzi:**
- [ ] AC1: Visualizzare lista indirizzi per una persona
- [ ] AC2: Aggiungere nuovo indirizzo con tutti i campi obbligatori
- [ ] AC3: Modificare indirizzo esistente
- [ ] AC4: Eliminare indirizzo con conferma
- [ ] AC5: Autocomplete indirizzi funzionante con almeno 3 caratteri
- [ ] AC6: Validazione CAP formato italiano (5 cifre)
- [ ] AC7: Campi opzionali (interno, presso, note) salvati correttamente

**Email:**
- [ ] AC8: Visualizzare lista email per una persona
- [ ] AC9: Aggiungere nuova email con validazione formato
- [ ] AC10: Modificare email esistente
- [ ] AC11: Eliminare email con conferma
- [ ] AC12: Verifica duplicati email per stessa entità
- [ ] AC13: Visualizzare tipo email con chip colorato (AZI, PER, PEC, ALT)
- [ ] AC14: Link mailto funzionante in visualizzazione

**Telefoni:**
- [ ] AC15: Visualizzare lista telefoni per una persona
- [ ] AC16: Aggiungere nuovo telefono con prefisso internazionale
- [ ] AC17: Modificare telefono esistente
- [ ] AC18: Eliminare telefono con conferma
- [ ] AC19: Validazione numero (solo cifre, spazi, trattini, parentesi)
- [ ] AC20: Anteprima numero completo nel dialog
- [ ] AC21: Link tel: funzionante in visualizzazione

**Integrazione:**
- [ ] AC22: Expansion panels funzionanti in PersonaEdit
- [ ] AC23: Modalità read-only funzionante in PersonaDetails
- [ ] AC24: EventCallback OnDataChanged invocato correttamente
- [ ] AC25: Componenti riutilizzabili in altre pagine

### 13.2 Test Manuali

1. **Test CRUD Indirizzi:**
   - Creare indirizzo completo con autocomplete
   - Creare indirizzo manuale senza autocomplete
   - Modificare tutti i campi di un indirizzo
   - Eliminare indirizzo e verificare soft delete
   - Testare validazione CAP con valori non validi

2. **Test CRUD Email:**
   - Creare email di tutti i tipi (AZI, PER, PEC, ALT)
   - Tentare inserimento email duplicata (deve bloccare)
   - Modificare tipo email esistente
   - Verificare link mailto funzionante
   - Eliminare email e verificare soft delete

3. **Test CRUD Telefoni:**
   - Creare telefono con prefisso +39
   - Creare telefono internazionale (es. +1, +44)
   - Aggiungere estensione/interno
   - Verificare anteprima numero completo
   - Testare validazione con caratteri non validi
   - Verificare link tel: funzionante

4. **Test Responsività:**
   - Testare dialog su schermi piccoli (mobile)
   - Verificare griglia responsive
   - Testare expansion panels su tablet

5. **Test Integrazione:**
   - Creare persona e aggiungere tutti i contatti
   - Modificare persona e verificare persistenza contatti
   - Visualizzare dettaglio persona (read-only)
   - Navigare tra sezioni con expansion panels

## 14. Checklist di Consegna

- [ ] DTO Models implementati (Indirizzo, Email, Telefono)
- [ ] Servizi implementati (IIndirizziService, IEmailService, ITelefoniService, ITipologicheService)
- [ ] Mock service per autocomplete indirizzi
- [ ] Componente PersonaIndirizzo.razor + IndirizzoDialog.razor
- [ ] Componente PersonaEmail.razor + EmailDialog.razor
- [ ] Componente PersonaTelefono.razor + TelefonoDialog.razor
- [ ] Integrazione in PersonaEdit.razor con expansion panels
- [ ] Integrazione in PersonaDetails.razor (read-only)
- [ ] Validatori FluentValidation
- [ ] Endpoints API documentati
- [ ] Test manuali completati
- [ ] Code review e refactoring

## 15. Note Tecniche e Best Practices

### 15.1 Gestione Errori

- Implementare try-catch in tutti i metodi dei servizi
- Mostrare messaggi utente-friendly con Snackbar
- Loggare errori lato server per debugging

### 15.2 Performance

- Implementare debounce su autocomplete (300ms)
- Caching tipi email/telefono/indirizzo (raramente cambiano)
- Paginazione lato server se liste > 50 elementi

### 15.3 Sicurezza

- Validare lato server tutti gli input
- Implementare autorizzazione su endpoint API
- Sanitizzare input utente per prevenire XSS

### 15.4 Accessibilità

- Fornire AriaLabel per icone
- Supportare navigazione da tastiera
- Colori con contrasto sufficiente (WCAG 2.1 AA)

### 15.5 Manutenibilità

- Commentare codice complesso
- Usare nomi descrittivi per metodi e variabili
- Separare logica business da UI
- Creare unit test per validatori

---

## Appendice A: Struttura File del Progetto

```
/Accredia.GestioneAnagrafica.Client
│
├── /Pages
│   ├── /Persone
│   │   ├── PersonaCreate.razor
│   │   ├── PersonaEdit.razor
│   │   ├── PersonaDetails.razor
│   │   └── PersonaList.razor
│   │
├── /Components
│   ├── /Persona
│   │   ├── PersonaIndirizzo.razor
│   │   ├── PersonaEmail.razor
│   │   ├── PersonaTelefono.razor
│   │   ├── IndirizzoDialog.razor
│   │   ├── EmailDialog.razor
│   │   ├── TelefonoDialog.razor
│   │   ├── PersonaDatiAnagraficiForm.razor
│   │   └── PersonaDatiAnagraficiDisplay.razor
│   │
├── /Services
│   ├── IIndirizziService.cs
│   ├── IndirizziService.cs
│   ├── IEmailService.cs
│   ├── EmailService.cs
│   ├── ITelefoniService.cs
│   ├── TelefoniService.cs
│   ├── ITipologicheService.cs
│   ├── TipologicheService.cs
│   └── /Mock
│       └── IndirizziMockService.cs
│
/Accredia.GestioneAnagrafica.Shared
│
├── /DTOs
│   ├── IndirizzoDTO.cs
│   ├── EmailDTO.cs
│   ├── TelefonoDTO.cs
│   ├── TipoIndirizzoDTO.cs
│   ├── TipoEmailDTO.cs
│   └── TipoTelefonoDTO.cs
│
├── /Validators
│   ├── IndirizzoValidator.cs
│   ├── EmailValidator.cs
│   └── TelefonoValidator.cs
```

---

**Fine del documento**

Questo documento fornisce una guida completa per l'implementazione della gestione di indirizzi, email e telefoni per l'entità Persona. Include modelli dati, componenti Blazor, servizi, validazioni, integrazione nelle pagine e criteri di accettazione. Il documento è pronto per essere utilizzato come specifica tecnica per lo sviluppo.
