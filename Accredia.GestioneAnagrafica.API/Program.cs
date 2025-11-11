using Carter;
using FluentValidation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Accredia.GestioneAnagrafica.API.Data;
using Accredia.GestioneAnagrafica.API.Validators;
using Accredia.GestioneAnagrafica.API.Config;
using Accredia.GestioneAnagrafica.API.Services;
using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

// ==================== CARICA VARIABILI DA .env ====================
LoadEnvironmentVariables();

var builder = WebApplication.CreateBuilder(args);

// ==================== CONFIGURAZIONE SERVIZI ====================

// Database Context
builder.Services.AddDbContext<PersoneDbContext>(options =>
{
    var connectionString = BuildConnectionString();
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

// Configurazione DocumentStorage
builder.Services.Configure<DocumentStorageConfig>(
    builder.Configuration.GetSection("DocumentStorage")
);

// HttpClientFactory per Nextcloud
builder.Services.AddHttpClient("Nextcloud", client =>
{
    client.Timeout = TimeSpan.FromMinutes(10);
});

// Document Storage Service
builder.Services.AddScoped<IDocumentStorageService, DocumentStorageService>();

// Carter (Minimal APIs modulari)
builder.Services.AddCarter();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// ==================== AUTENTICAZIONE JWT ====================
var jwtKey = builder.Configuration["Jwt:Key"] ?? "super-secret-key-change-in-production-min-32-chars!!!!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "GestioneOrganismi";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "GestioneOrganismiUsers";

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = null;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorizationBuilder();

// ==================== SWAGGER / OPENAPI ====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Gestione Anagarfica API",
        Version = "v1",
        Description = "API per la gestione completa di Enti, Organismi, Documenti, Email, Telefoni e Risorse Umane"
    });

    // Configura autenticazione JWT in Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Inserisci il token JWT nel formato: **Bearer {token}**"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Fix per tipi generici e nested DTO
    options.CustomSchemaIds(type =>
    {
        if (type.IsGenericType)
        {
            var genericArgs = type.GetGenericArguments();
            var genericArgNames = string.Join("_", genericArgs.Select(t => GetFullTypeName(t)));
            var baseName = type.Name.Substring(0, type.Name.IndexOf('`'));
            return $"{baseName}_{genericArgNames}";
        }

        return GetFullTypeName(type);
    });
});

// ==================== CORS ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ==================== MIDDLEWARE ====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Carter Endpoints
app.MapCarter();

// Endpoint pubblico di test
app.MapGet("/ping", () => Results.Ok("pong"))
   .AllowAnonymous()
   .WithOpenApi();

app.Run();

// ==================== HELPER METHODS ====================
static void LoadEnvironmentVariables()
{
    try
    {
        if (File.Exists(".env"))
        {
            var envLines = File.ReadAllLines(".env");
            foreach (var line in envLines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    if (value.StartsWith("\"") && value.EndsWith("\""))
                        value = value.Substring(1, value.Length - 2);

                    Environment.SetEnvironmentVariable(key, value);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not load .env file: {ex.Message}");
    }
}

static string BuildConnectionString()
{
    var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
    var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "Accredia";
    var integratedSecurity = Environment.GetEnvironmentVariable("DB_INTEGRATED_SECURITY") == "true";
    var encrypt = Environment.GetEnvironmentVariable("DB_ENCRYPT") ?? "true";
    var trustCert = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE") ?? "true";

    if (integratedSecurity)
    {
        return $"Server={server},{port};Database={database};Integrated Security=true;Encrypt={encrypt};TrustServerCertificate={trustCert};";
    }
    else
    {
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        return $"Server={server},{port};Database={database};User Id={user};Password={password};Encrypt={encrypt};TrustServerCertificate={trustCert};";
    }
}

static string GetFullTypeName(Type type)
{
    if (type.DeclaringType != null)
    {
        var declaringName = GetFullTypeName(type.DeclaringType);
        return $"{declaringName}_{type.Name}";
    }

    if (type.IsGenericType)
    {
        var genericArgs = type.GetGenericArguments();
        var genericArgNames = string.Join("_", genericArgs.Select(t => GetFullTypeName(t)));
        var baseName = type.Name.Substring(0, type.Name.IndexOf('`'));
        return $"{baseName}_{genericArgNames}";
    }

    return type.Name;
}
