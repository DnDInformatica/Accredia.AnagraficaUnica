using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudBlazor;
using Accredia.GestioneAnagrafica.Web;
using Accredia.GestioneAnagrafica.Web.Themes;
using Accredia.GestioneAnagrafica.Web.Services;
using Accredia.GestioneAnagrafica.Shared.State;
using Accredia.GestioneAnagrafica.Shared.Auth;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root Components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// =============================
// HTTP CLIENT CON AUTHORIZATION
// =============================
// Legge l'URL dell'API dalla configurazione
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;

// Registra l'handler per l'autorizzazione
builder.Services.AddScoped<AuthorizationMessageHandler>();

// Configura HttpClient con l'handler di autorizzazione
builder.Services.AddHttpClient("API", (sp, client) =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

// HttpClient principale per compatibilità
builder.Services.AddScoped(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return httpClientFactory.CreateClient("API");
});

// =============================
// AUTH & STATE
// =============================
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<UserState>();
builder.Services.AddScoped<AppState>();

// =============================
// API SERVICES
// =============================
builder.Services.AddScoped<IApiHttpClient, ApiHttpClient>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPersoneService, PersoneService>();
builder.Services.AddScoped<IOrganismiService, OrganismiService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Servizi per gestione contatti Persona
builder.Services.AddScoped<IIndirizziService, IndirizziService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITelefoniService, TelefoniService>();

// =============================
// MUDBLAZOR CONFIG (theme Accredia)
// =============================
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
});

// Puoi in futuro registrare un tema personalizzato
builder.Services.AddScoped<AccrediaTheme>();

await builder.Build().RunAsync();