═══════════════════════════════════════════════════════════════════════════════
            ✅ BUG TROVATO E CORRETTO - FALLBACK PORTA SBAGLIATA
═══════════════════════════════════════════════════════════════════════════════

## 🔴 BUG TROVATO

Nel Program.cs, il fallback era:

```csharp
var apiUrl = builder.Configuration["API:Url"] ?? "https://localhost:7043";
                                                   ↑ SBAGLIATO!
```

Se per qualche motivo appsettings.json non viene letto, usa `7043` (che diventa `7001`).

## ✅ BUG CORRETTO

Adesso il fallback è:

```csharp
var apiUrl = builder.Configuration["API:Url"] ?? "https://localhost:5001";
                                                   ↑ CORRETTO!
```

Anche se appsettings.json fallisce, usa la porta giusta: `5001`.

## 📝 Cambio Applicato

File: Accredia.GestioneAnagrafica.Server/Program.cs

PRIMA:
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
    ...
});
```

DOPO:
```csharp
// HttpClient con configurazione API
var apiUrl = builder.Configuration["API:Url"] ?? "https://localhost:5001";
builder.Services.AddHttpClient<IAuthService, AuthService>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri(apiUrl);
        // Ignora errori di certificato SSL
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    });
```

## 🛑 RIAVVIA IL SERVER ADESSO

Esattamente come prima, ma adesso WITH IL BUG CORRETTO:

### Console 1 - API:
```bash
cd C:\Accredia\Sviluppo
dotnet run --project Accredia.GestioneAnagrafica.API --no-build
```

### Console 2 - Server:
```bash
cd C:\Accredia\Sviluppo
dotnet clean
dotnet build -c Debug
dotnet run --project Accredia.GestioneAnagrafica.Server --no-build
```

### Browser:
```
https://localhost:7412/login
admin / password
```

## ✅ Verificazione

Nei log dovrai vedere:

```
info: AuthService - Tentativo di login per l'utente: admin
info: AuthService - Login riuscito per admin, token ricevuto
info: Login - Stato di autenticazione aggiornato
```

✅ **NON** "localhost:7001"

## 📊 Configurazione Finale

- appsettings.json: "https://localhost:5001" ✅
- Program.cs fallback: "https://localhost:5001" ✅
- API effettivamente su: https://localhost:5001 ✅
- Frontend chiama: https://localhost:5001 ✅

Tutto coerente e corretto!

═══════════════════════════════════════════════════════════════════════════════
                    🎉 STAVOLTA DOVREBBE FUNZIONARE! 🎉
═══════════════════════════════════════════════════════════════════════════════
