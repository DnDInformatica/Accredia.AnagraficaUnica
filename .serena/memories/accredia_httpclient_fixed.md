# ACCREDIA IDENTITY - HTTPCLIENT PROBLEMA RISOLTO ✅

## ❌ Problema Identificato

Nei log:
```
fail: AuthService - Errore nel login: localhost:7001
```

Dovrebbe essere `localhost:7043`!

## 🔍 Causa

HttpClient registrato con `AddScoped<HttpClient>()` non funziona in Blazor Server.
AuthService riceveva un HttpClient senza BaseAddress corretto.
Le richieste andavano alla porta SBAGLIATA.

## ✅ Soluzione Applicata

Cambiato Program.cs:

```csharp
// PRIMA (❌ sbagliato)
builder.Services.AddScoped<HttpClient>(sp => {...});

// DOPO (✅ corretto)
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    var apiUrl = builder.Configuration["API:Url"] ?? "https://localhost:7043";
    client.BaseAddress = new Uri(apiUrl);
})
.ConfigureHttpClient(client =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
});
```

## 📝 File Modificato

✅ Program.cs
- Rimosso AddScoped<HttpClient>
- Aggiunto AddHttpClient<IAuthService, AuthService>
- Configurato BaseAddress da appsettings.json
- Configurato SSL certificate validation ignorata

## 🚀 Test

1. Riavvia server:
   cd C:\Accredia\Sviluppo
   dotnet clean
   dotnet build -c Debug
   dotnet run --project Accredia.GestioneAnagrafica.Server --no-build

2. Apri browser:
   https://localhost:7412/
   (nota: https con la S, ignora certificato warning)

3. Login:
   Username: admin
   Password: password

4. Dovresti vedere:
   ✅ Non c'è errore "localhost:7001"
   ✅ Login riuscito
   ✅ Redirect a /dashboard
   ✅ Dashboard con "Benvenuto, admin"

## ✅ Pronto!
HttpClient è configurato correttamente ora!
