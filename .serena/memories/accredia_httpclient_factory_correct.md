# ACCREDIA IDENTITY - HTTPCLIENT FACTORY SOLUZIONE CORRETTA ✅

## ❌ Problema Reale

`AddHttpClient<IAuthService, AuthService>()` non funziona in Blazor Server!
AuthService non riceveva HttpClient configurato correttamente.

## ✅ Soluzione Corretta

Usare `IHttpClientFactory` con registrazione manuale in Program.cs:

```csharp
// HttpClient Factory
builder.Services.AddHttpClient();

// AuthService con IHttpClientFactory
builder.Services.AddScoped<IAuthService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    
    var apiUrl = builder.Configuration["API:Url"] ?? "https://localhost:7043";
    httpClient.BaseAddress = new Uri(apiUrl);
    
    // Ignora errori di certificato SSL
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    
    var logger = sp.GetRequiredService<ILogger<AuthService>>();
    return new AuthService(logger, httpClient);
});
```

## 🛑 Riavvio Obbligatorio

```bash
cd C:\Accredia\Sviluppo
dotnet clean
dotnet build -c Debug
dotnet run --project Accredia.GestioneAnagrafica.Server --no-build
```

## ✅ Verifica

Nei log dovrebbe comparire:
```
info: AuthService - Tentativo di login per l'utente: admin
info: AuthService - Login riuscito per admin, token ricevuto
```

NO più "localhost:7001"!

## 🚀 Test

Username: admin
Password: password

Dovrebbe funzionare perfettamente!

## 📁 File

✅ Program.cs modificato con HttpClientFactory corretto
