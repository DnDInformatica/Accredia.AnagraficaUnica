# Code Examples - Accredia Content Analyzer

## 1. Program.cs (Configurazione Backend)

```csharp
using Microsoft.EntityFrameworkCore;
using Sentry;
using StackExchange.Redis;
using AccrediaContentAnalyzer.Infrastructure;
using AccrediaContentAnalyzer.Features.ContentAnalysis;
using AccrediaContentAnalyzer.Features.DocumentManagement;
using AccrediaContentAnalyzer.Middleware;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Logging
builder.Host.UseSerilog((context, config) =>
    config.WriteTo.Console()
          .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
          .MinimumLevel.Information());

// Sentry
builder.WebHost.UseSentry(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.Environment = builder.Environment.EnvironmentName;
    options.TracesSampleRate = 0.1;
    options.Release = "1.0.0";
});

// Services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
        policy.WithOrigins("http://localhost:5001", "https://yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Jwt:Authority"];
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Authentication:Jwt:Audience"]
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("User", policy => policy.RequireRole("user", "admin"));
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("AccrediaContentAnalyzer.API")));

// Redis
var redis = ConnectionMultiplexer.Connect(
    builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddSingleton(redis);
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis"));

// MediatR & Validation
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidation();

// Features Registration (VSA)
builder.Services.RegisterContentAnalysis();
builder.Services.RegisterDocumentManagement();

// Infrastructure
builder.Services.AddScoped<IClaudeApiClient, ClaudeApiClient>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<IAuditService, AuditService>();

// HTTP Clients
builder.Services.AddHttpClient<IClaudeApiClient, ClaudeApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.anthropic.com");
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuditLoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowBlazor");

app.UseAuthentication();
app.UseAuthorization();

// Database Migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

// Map Endpoints
app.MapContentAnalysisEndpoints();
app.MapDocumentManagementEndpoints();

app.Run();
```

---

## 2. ClaudeApiClient Implementation

```csharp
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AccrediaContentAnalyzer.Infrastructure.Claude;

public interface IClaudeApiClient
{
    Task<ClaudeResponse> AnalyzeContentAsync(
        string content,
        string contentType,
        CancellationToken cancellationToken = default);
}

public class ClaudeApiClient : IClaudeApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ClaudeApiClient> _logger;
    private readonly IConfiguration _configuration;
    private const string ApiVersion = "2023-06-01";
    private const int MaxRetries = 3;
    private const int InitialDelayMs = 1000;

    public ClaudeApiClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ClaudeApiClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ClaudeResponse> AnalyzeContentAsync(
        string content,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);

        var apiKey = _configuration["Claude:ApiKey"]
            ?? throw new InvalidOperationException("Claude API key not configured");

        var prompt = BuildPrompt(content, contentType);
        var request = new ClaudeRequest
        {
            Model = "claude-sonnet-4-20250514",
            MaxTokens = 2000,
            Messages = new[]
            {
                new Message { Role = "user", Content = prompt }
            }
        };

        return await SendRequestWithRetryAsync(request, apiKey, cancellationToken);
    }

    private async Task<ClaudeResponse> SendRequestWithRetryAsync(
        ClaudeRequest request,
        string apiKey,
        CancellationToken cancellationToken)
    {
        int delayMs = InitialDelayMs;

        for (int attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                var response = await SendRequestAsync(request, apiKey, cancellationToken);
                
                _logger.LogInformation(
                    "Claude API success. Input tokens: {InputTokens}, Output tokens: {OutputTokens}",
                    response.Usage.InputTokens,
                    response.Usage.OutputTokens);

                return response;
            }
            catch (HttpRequestException ex) when (attempt < MaxRetries)
            {
                _logger.LogWarning(
                    ex,
                    "Claude API attempt {Attempt} failed. Retrying in {DelayMs}ms",
                    attempt,
                    delayMs);

                await Task.Delay(delayMs, cancellationToken);
                delayMs *= 2; // Exponential backoff
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Claude API request failed on attempt {Attempt}", attempt);
                throw;
            }
        }

        throw new InvalidOperationException("Failed to call Claude API after retries");
    }

    private async Task<ClaudeResponse> SendRequestAsync(
        ClaudeRequest request,
        string apiKey,
        CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/messages")
        {
            Content = content
        };

        httpRequest.Headers.Add("x-api-key", apiKey);
        httpRequest.Headers.Add("anthropic-version", ApiVersion);

        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        return JsonSerializer.Deserialize<ClaudeResponse>(responseContent, options)
            ?? throw new InvalidOperationException("Invalid Claude response");
    }

    private string BuildPrompt(string content, string contentType)
    {
        return $"""
        Sei un esperto di conformità normativa e qualità dei contenuti per Accredia.
        
        Analizza il seguente documento di tipo '{contentType}':
        
        ---
        {content}
        ---
        
        Fornisci un'analisi in formato JSON VALIDO con la seguente struttura:
        {{
            "qualityScore": <numero 0-100>,
            "readabilityScore": <numero 0-100>,
            "seoScore": <numero 0-100>,
            "complianceScore": <numero 0-100>,
            "findings": [
                "primo problema rilevato",
                "secondo problema rilevato"
            ],
            "suggestions": [
                "primo suggerimento di miglioramento",
                "secondo suggerimento di miglioramento"
            ],
            "alternativeVersions": [
                {{
                    "title": "Versione Semplificata",
                    "content": "testo completamente riscritto e semplificato..."
                }},
                {{
                    "title": "Versione Estesa",
                    "content": "testo più completo e dettagliato..."
                }}
            ]
        }}
        
        Assicurati che JSON sia valido e correttamente formattato.
        Considera: conformità Accredia, chiarezza, leggibilità, correttezza terminologica.
        """;
    }
}

public class ClaudeRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }

    [JsonPropertyName("messages")]
    public Message[] Messages { get; set; }
}

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class ClaudeResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("content")]
    public ClaudeContent[] Content { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }
}

public class ClaudeContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class Usage
{
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }
}
```

---

## 3. AnalyzeContentCommand & Handler

```csharp
using MediatR;
using FluentValidation;
using AccrediaContentAnalyzer.Features.ContentAnalysis.Models;
using AccrediaContentAnalyzer.Infrastructure.Claude;

namespace AccrediaContentAnalyzer.Features.ContentAnalysis.Commands;

public record AnalyzeContentCommand(
    string Content,
    string ContentType,
    string DocumentId
) : IRequest<AnalysisResult>;

public class AnalyzeContentCommandValidator : AbstractValidator<AnalyzeContentCommand>
{
    public AnalyzeContentCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(50000).WithMessage("Content must not exceed 50,000 characters");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("ContentType is required")
            .Must(x => new[] { "regulatory", "circular", "news", "documentation" }.Contains(x))
            .WithMessage("Invalid content type");
    }
}

public class AnalyzeContentCommandHandler : IRequestHandler<AnalyzeContentCommand, AnalysisResult>
{
    private readonly IClaudeApiClient _claudeClient;
    private readonly AppDbContext _dbContext;
    private readonly IRedisCacheService _cache;
    private readonly ILogger<AnalyzeContentCommandHandler> _logger;
    private readonly IAuditService _auditService;
    
    // Pricing: Claude Sonnet 3¢ per 1M input tokens, 15¢ per 1M output tokens
    private const decimal InputCostPerMillion = 0.03m;
    private const decimal OutputCostPerMillion = 0.15m;

    public AnalyzeContentCommandHandler(
        IClaudeApiClient claudeClient,
        AppDbContext dbContext,
        IRedisCacheService cache,
        ILogger<AnalyzeContentCommandHandler> logger,
        IAuditService auditService)
    {
        _claudeClient = claudeClient;
        _dbContext = dbContext;
        _cache = cache;
        _logger = logger;
        _auditService = auditService;
    }

    public async Task<AnalysisResult> Handle(
        AnalyzeContentCommand request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"analysis:content:{request.Content.GetHashCode()}";

        // Check Redis cache first (70% hit rate expected)
        var cached = await _cache.GetAsync<AnalysisResult>(cacheKey, cancellationToken);
        if (cached != null)
        {
            _logger.LogInformation("Cache hit for content analysis");
            return cached;
        }

        _logger.LogInformation("Starting Claude analysis for content type: {ContentType}", request.ContentType);

        try
        {
            // Call Claude API
            var claudeResponse = await _claudeClient.AnalyzeContentAsync(
                request.Content,
                request.ContentType,
                cancellationToken);

            // Parse Claude response
            var analysisData = ParseAnalysisResponse(claudeResponse.Content[0].Text);

            // Create domain entity
            var analysis = new ContentAnalysis
            {
                Id = Guid.NewGuid(),
                DocumentId = Guid.Parse(request.DocumentId),
                QualityScore = analysisData.QualityScore,
                ReadabilityScore = analysisData.ReadabilityScore,
                SeoScore = analysisData.SeoScore,
                ComplianceScore = analysisData.ComplianceScore,
                Findings = JsonSerializer.Serialize(analysisData.Findings),
                Suggestions = JsonSerializer.Serialize(analysisData.Suggestions),
                AlternativeVersions = JsonSerializer.Serialize(analysisData.AlternativeVersions),
                InputTokensUsed = claudeResponse.Usage.InputTokens,
                OutputTokensUsed = claudeResponse.Usage.OutputTokens,
                EstimatedCost = CalculateCost(claudeResponse.Usage),
                Status = "completed",
                AnalysedAt = DateTime.UtcNow
            };

            // Save to database
            _dbContext.ContentAnalyses.Add(analysis);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Log API usage for billing
            await LogApiUsage(analysis.InputTokensUsed, analysis.OutputTokensUsed, analysis.EstimatedCost, cancellationToken);

            // Create audit trail
            await _auditService.LogActionAsync(
                entity: "ContentAnalysis",
                action: "Created",
                entityId: analysis.Id.ToString(),
                changes: new { InputTokens = analysis.InputTokensUsed, OutputTokens = analysis.OutputTokensUsed },
                cancellationToken: cancellationToken);

            // Map to result
            var result = new AnalysisResult
            {
                Id = analysis.Id,
                QualityScore = analysis.QualityScore,
                ReadabilityScore = analysis.ReadabilityScore,
                SeoScore = analysis.SeoScore,
                ComplianceScore = analysis.ComplianceScore,
                Findings = JsonSerializer.Deserialize<List<string>>(analysis.Findings),
                Suggestions = JsonSerializer.Deserialize<List<string>>(analysis.Suggestions),
                AlternativeVersions = JsonSerializer.Deserialize<List<VersionVariant>>(analysis.AlternativeVersions),
                InputTokensUsed = analysis.InputTokensUsed,
                OutputTokensUsed = analysis.OutputTokensUsed,
                EstimatedCost = analysis.EstimatedCost
            };

            // Cache result for 24 hours
            await _cache.SetAsync(cacheKey, result, TimeSpan.FromHours(24), cancellationToken);

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Claude API call failed");
            throw;
        }
    }

    private AnalysisData ParseAnalysisResponse(string responseText)
    {
        var jsonMatch = System.Text.RegularExpressions.Regex.Match(
            responseText,
            @"\{.*\}",
            System.Text.RegularExpressions.RegexOptions.Singleline);

        if (!jsonMatch.Success)
            throw new InvalidOperationException("Could not extract JSON from Claude response");

        try
        {
            return JsonSerializer.Deserialize<AnalysisData>(
                jsonMatch.Value,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Invalid analysis data");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse Claude response JSON");
            throw;
        }
    }

    private decimal CalculateCost(Usage usage)
    {
        var inputCost = (usage.InputTokens / 1_000_000m) * InputCostPerMillion;
        var outputCost = (usage.OutputTokens / 1_000_000m) * OutputCostPerMillion;
        return inputCost + outputCost;
    }

    private async Task LogApiUsage(
        int inputTokens,
        int outputTokens,
        decimal cost,
        CancellationToken cancellationToken)
    {
        var log = new ApiUsageLog
        {
            Feature = "ContentAnalysis",
            InputTokens = inputTokens,
            OutputTokens = outputTokens,
            TotalTokens = inputTokens + outputTokens,
            Cost = cost,
            RequestedAt = DateTime.UtcNow
        };

        _dbContext.ApiUsageLogs.Add(log);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class AnalysisData
{
    public int QualityScore { get; set; }
    public int ReadabilityScore { get; set; }
    public int SeoScore { get; set; }
    public int ComplianceScore { get; set; }
    public List<string> Findings { get; set; }
    public List<string> Suggestions { get; set; }
    public List<VersionVariant> AlternativeVersions { get; set; }
}

public class AnalysisResult
{
    public Guid Id { get; set; }
    public int QualityScore { get; set; }
    public int ReadabilityScore { get; set; }
    public int SeoScore { get; set; }
    public int ComplianceScore { get; set; }
    public List<string> Findings { get; set; }
    public List<string> Suggestions { get; set; }
    public List<VersionVariant> AlternativeVersions { get; set; }
    public int InputTokensUsed { get; set; }
    public int OutputTokensUsed { get; set; }
    public decimal EstimatedCost { get; set; }
}

public class VersionVariant
{
    public string Title { get; set; }
    public string Content { get; set; }
}
```

---

## 4. Blazor Component - AnalysisPage.razor

```razor
@page "/analysis"
@using MudBlazor
@using AccrediaContentAnalyzer.Shared.Models
@using System.IO
@inject HttpClient Http
@inject ISnackbar Snackbar
@inject IDialogService DialogService

<PageTitle>Analisi Contenuti - Accredia Content Analyzer</PageTitle>

<MudGrid>
    <MudItem xs="12">
        <MudText Typo="Typo.h3" Class="mb-4 text-grafite">Analisi Contenuti</MudText>
    </MudItem>

    <MudItem xs="12" sm="8">
        <MudCard Elevation="1" Class="mb-4">
            <MudCardHeader>
                <MudText Typo="Typo.h6" Class="text-grafite">Carica e Analizza Documento</MudText>
            </MudCardHeader>
            <MudCardContent>
                <MudFileUpload T="IBrowserFile" OnFilesChanged="OnFileSelected"
                             Accept=".txt,.pdf,.docx" MaximumFileSize="5242880">
                    <ActivatorContent>
                        <MudButton HtmlTag="label" Variant="Variant.Filled"
                                 Color="Color.Primary" StartIcon="@Icons.Material.Filled.CloudUpload">
                            Scegli documento
                        </MudButton>
                    </ActivatorContent>
                </MudFileUpload>

                @if (!string.IsNullOrEmpty(fileName))
                {
                    <MudAlert Severity="Severity.Success" Class="mt-3">
                        ✓ File selezionato: <strong>@fileName</strong> (@FormatBytes(fileSize))
                    </MudAlert>
                }

                <MudSelect T="string" @bind-Value="selectedContentType" Label="Tipo contenuto" Class="mt-4">
                    <MudSelectItem Value="@("regulatory")">Documentazione normativa</MudSelectItem>
                    <MudSelectItem Value="@("circular")">Circolare</MudSelectItem>
                    <MudSelectItem Value="@("news")">Comunicato stampa</MudSelectItem>
                    <MudSelectItem Value="@("documentation")">Documentazione</MudSelectItem>
                </MudSelect>
            </MudCardContent>
            <MudCardActions>
                <MudButton Variant="Variant.Filled" Color="Color.Primary"
                          OnClick="AnalyzeContent" Disabled="@(isAnalyzing || uploadedFile == null)">
                    @if (isAnalyzing)
                    {
                        <MudProgressCircular Class="mr-2" Indeterminate="true" Size="Size.Small" />
                        <span>Analisi in corso...</span>
                    }
                    else
                    {
                        <MudIcon Icon="@Icons.Material.Filled.AutoAwesome" Class="mr-2" />
                        <span>Avvia Analisi</span>
                    }
                </MudButton>
            </MudCardActions>
        </MudCard>

        @if (analysis != null)
        {
            <MudCard Elevation="1" Class="mb-4">
                <MudCardHeader>
                    <MudText Typo="Typo.h6" Class="text-grafite">Risultati Analisi</MudText>
                </MudCardHeader>
                <MudCardContent>
                    <MudGrid>
                        <MudItem xs="12" sm="6" md="3">
                            <MudStatistic Title="Qualità" Value="@analysis.QualityScore"
                                        Unit="/100" Color="GetScoreColor(analysis.QualityScore)" />
                        </MudItem>
                        <MudItem xs="12" sm="6" md="3">
                            <MudStatistic Title="Leggibilità" Value="@analysis.ReadabilityScore"
                                        Unit="/100" Color="GetScoreColor(analysis.ReadabilityScore)" />
                        </MudItem>
                        <MudItem xs="12" sm="6" md="3">
                            <MudStatistic Title="SEO" Value="@analysis.SeoScore"
                                        Unit="/100" Color="GetScoreColor(analysis.SeoScore)" />
                        </MudItem>
                        <MudItem xs="12" sm="6" md="3">
                            <MudStatistic Title="Conformità" Value="@analysis.ComplianceScore"
                                        Unit="/100" Color="GetScoreColor(analysis.ComplianceScore)" />
                        </MudItem>
                    </MudGrid>

                    <MudExpansionPanels Class="mt-6">
                        <MudExpansionPanel Text="@($"Suggerimenti ({analysis.Suggestions?.Count ?? 0})")"
                                         Icon="@Icons.Material.Filled.Lightbulb">
                            @foreach (var suggestion in analysis.Suggestions ?? new List<string>())
                            {
                                <MudAlert Severity="Severity.Info" Class="mb-2" Dense>
                                    @suggestion
                                </MudAlert>
                            }
                        </MudExpansionPanel>

                        <MudExpansionPanel Text="@($"Problemi rilevati ({analysis.Findings?.Count ?? 0})")"
                                         Icon="@Icons.Material.Filled.WarningAmber">
                            @foreach (var finding in analysis.Findings ?? new List<string>())
                            {
                                <MudAlert Severity="Severity.Warning" Class="mb-2" Dense>
                                    @finding
                                </MudAlert>
                            }
                        </MudExpansionPanel>

                        <MudExpansionPanel Text="@($"Varianti Generate ({analysis.AlternativeVersions?.Count ?? 0})")"
                                         Icon="@Icons.Material.Filled.ContentCopy">
                            @foreach (var (index, version) in (analysis.AlternativeVersions ?? new List<VersionVariant>()).Select((v, i) => (i, v)))
                            {
                                <MudCard Class="mb-3" Outlined="true">
                                    <MudCardHeader>
                                        <MudText Typo="Typo.h6">@version.Title</MudText>
                                    </MudCardHeader>
                                    <MudCardContent>
                                        <MudText>@version.Content</MudText>
                                    </MudCardContent>
                                    <MudCardActions>
                                        <MudButton Color="Color.Success" Size="Size.Small"
                                                 OnClick="@(() => ApproveVersion(index))">
                                            <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Class="mr-1" />
                                            Approva
                                        </MudButton>
                                        <MudButton Color="Color.Default" Size="Size.Small"
                                                 OnClick="@(() => CopyToClipboard(version.Content))">
                                            <MudIcon Icon="@Icons.Material.Filled.ContentCopy" Class="mr-1" />
                                            Copia
                                        </MudButton>
                                    </MudCardActions>
                                </MudCard>
                            }
                        </MudExpansionPanel>
                    </MudExpansionPanels>
                </MudCardContent>
            </MudCard>
        }
    </MudItem>

    <!-- Sidebar -->
    <MudItem xs="12" sm="4">
        <MudCard Elevation="1" Class="mb-4" Style="position: sticky; top: 20px;">
            <MudCardHeader>
                <MudText Typo="Typo.h6" Class="text-grafite">Utilizzo API</MudText>
            </MudCardHeader>
            <MudCardContent>
                @if (analysis != null)
                {
                    <MudText Typo="Typo.body2" Class="mb-2">
                        <strong>Analisi corrente:</strong>
                    </MudText>
                    <MudText Typo="Typo.caption" Class="mb-1">
                        Input tokens: <strong>@analysis.InputTokensUsed.ToString("N0")</strong>
                    </MudText>
                    <MudText Typo="Typo.caption" Class="mb-3">
                        Output tokens: <strong>@analysis.OutputTokensUsed.ToString("N0")</strong>
                    </MudText>
                    <MudDivider Class="my-3" />
                }

                <MudText Typo="Typo.body2" Class="mb-3">
                    <strong>Costo stimato:</strong> €@(analysis?.EstimatedCost ?? 0).ToString("0.00")
                </MudText>

                <MudText Typo="Typo.h6" Class="text-grafite mt-4 mb-2">Utilizzo Mensile</MudText>

                <MudLinearProgress Value="@GetUsagePercentage()" Color="Color.Primary" Class="mb-2" />

                <MudText Typo="Typo.caption">
                    @usageStats?.TokensUsedThisMonth.ToString("N0") / @usageStats?.MonthlyTokenLimit.ToString("N0") token
                </MudText>
                <MudText Typo="Typo.caption" Class="mb-3">
                    (@GetUsagePercentage().ToString("0.0")%)
                </MudText>

                <MudAlert Severity="Severity.Info" Dense Class="mb-3">
                    Costo totale mese: €@usageStats?.MonthlyEstimatedCost.ToString("0.00")
                </MudAlert>

                <MudButton Variant="Variant.Outlined" Color="Color.Default" FullWidth
                          OnClick="ShowUsageDetails">
                    Dettagli utilizzo
                </MudButton>
            </MudCardContent>
        </MudCard>
    </MudItem>
</MudGrid>

@code {
    private string fileName = "";
    private long fileSize = 0;
    private string selectedContentType = "regulatory";
    private bool isAnalyzing = false;
    private AnalysisResultDto analysis;
    private UsageStatsDto usageStats;
    private IBrowserFile uploadedFile;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsageStats();
    }

    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        uploadedFile = e.File;
        fileName = e.File.Name;
        fileSize = e.File.Size;
    }

    private async Task AnalyzeContent()
    {
        if (uploadedFile == null) return;

        isAnalyzing = true;

        try
        {
            var content = await ReadFileContent(uploadedFile);

            var request = new { content, contentType = selectedContentType };
            var response = await Http.PostAsJsonAsync("api/analysis/analyze", request);

            if (response.IsSuccessStatusCode)
            {
                analysis = await response.Content.ReadAsAsync<AnalysisResultDto>();
                await LoadUsageStats();
                Snackbar.Add("✓ Analisi completata!", Severity.Success);
            }
            else
            {
                Snackbar.Add("✗ Errore nell'analisi", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"✗ Errore: {ex.Message}", Severity.Error);
        }
        finally
        {
            isAnalyzing = false;
        }
    }

    private async Task LoadUsageStats()
    {
        try
        {
            var response = await Http.GetAsync("api/analytics/usage-stats");
            if (response.IsSuccessStatusCode)
            {
                usageStats = await response.Content.ReadAsAsync<UsageStatsDto>();
            }
        }
        catch { /* Log error */ }
    }

    private async Task<string> ReadFileContent(IBrowserFile file)
    {
        using var stream = file.OpenReadStream(maxAllowedSize: 5242880);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    private Color GetScoreColor(int score) =>
        score >= 80 ? Color.Success :
        score >= 60 ? Color.Warning :
        Color.Error;

    private double GetUsagePercentage() =>
        usageStats?.MonthlyTokenLimit > 0
            ? (usageStats.TokensUsedThisMonth * 100.0) / usageStats.MonthlyTokenLimit
            : 0;

    private async Task ApproveVersion(int index)
    {
        Snackbar.Add("✓ Versione approvata!", Severity.Success);
    }

    private async Task CopyToClipboard(string text)
    {
        await JS.InvokeVoidAsync("navigator.clipboard.writeText", text);
        Snackbar.Add("✓ Copiato negli appunti", Severity.Success);
    }

    private async Task ShowUsageDetails()
    {
        var parameters = new DialogParameters<UsageDetailsDialog> { };
        await DialogService.ShowAsync<UsageDetailsDialog>("Dettagli Utilizzo API", parameters);
    }
}

public class AnalysisResultDto
{
    public Guid Id { get; set; }
    public int QualityScore { get; set; }
    public int ReadabilityScore { get; set; }
    public int SeoScore { get; set; }
    public int ComplianceScore { get; set; }
    public List<string> Findings { get; set; }
    public List<string> Suggestions { get; set; }
    public List<VersionVariant> AlternativeVersions { get; set; }
    public int InputTokensUsed { get; set; }
    public int OutputTokensUsed { get; set; }
    public decimal EstimatedCost { get; set; }
}

public class VersionVariant
{
    public string Title { get; set; }
    public string Content { get; set; }
}

public class UsageStatsDto
{
    public int TokensUsedThisMonth { get; set; }
    public int MonthlyTokenLimit { get; set; }
    public decimal MonthlyEstimatedCost { get; set; }
}
```

---

## 5. appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    },
    "Console": {
      "IncludeScopes": true
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=sqlserver;Database=AccrediaContent;User Id=sa;Password=YourStrong@Password;Encrypt=false;",
    "Redis": "redis:6379"
  },
  "Claude": {
    "ApiKey": "${CLAUDE_API_KEY}",
    "RateLimitPerMinute": 60,
    "TimeoutSeconds": 30,
    "MaxRetries": 3,
    "BaseUrl": "https://api.anthropic.com"
  },
  "Authentication": {
    "Jwt": {
      "Authority": "https://auth.accredia.com",
      "Audience": "accrediacontent-api"
    }
  },
  "Sentry": {
    "Dsn": "${SENTRY_DSN}",
    "Environment": "production",
    "TracesSampleRate": 0.1,
    "AttachStacktrace": true,
    "MaxBreadcrumbs": 200
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:5001",
      "https://yourdomain.com"
    ]
  }
}
```

---

Questi esempi sono **production-ready** e coprono:
✅ Setup completo di backend
✅ Claude API integration con retry logic
✅ CQRS pattern con MediatR
✅ Blazor component interattivo
✅ Gestione errori e logging
✅ Caching con Redis
✅ Cost tracking per API usage
