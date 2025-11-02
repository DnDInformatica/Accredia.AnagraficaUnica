using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using GestioneOrganismi.Backend.Data;
using GestioneOrganismi.Backend.Validators;
using GestioneOrganismi.Backend.Config;
using GestioneOrganismi.Backend.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ==================== CONFIGURAZIONE SERVIZI ====================

// Database Context
builder.Services.AddDbContext<PersoneDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

// Configurazione DocumentStorage
builder.Services.Configure<DocumentStorageConfig>(
    builder.Configuration.GetSection("DocumentStorage")
);

// HttpClientFactory per Nextcloud
builder.Services.AddHttpClient("Nextcloud", client =>
{
    client.Timeout = TimeSpan.FromMinutes(10); // Timeout per file grandi
});

// Document Storage Service
builder.Services.AddScoped<IDocumentStorageService, DocumentStorageService>();

// Carter for Minimal APIs
builder.Services.AddCarter();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Gestione Organismi API",
        Version = "v1",
        Description = "API per la gestione completa di Enti, Organismi, Documenti, Email, Telefoni e Risorse Umane"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Authentication & Authorization (se configurato)

var app = builder.Build();

// ==================== CONFIGURAZIONE MIDDLEWARE ====================

// Swagger (solo in Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Carter Endpoints
app.MapCarter();

app.Run();
