using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Jalsa.API.Configurations;
using Jalsa.API.Exceptions;
using Jalsa.API.Filters;
using Jalsa.API.Hubs;
using Jalsa.API.Middleware;
using Jalsa.API.Services.Implementations;
using Jalsa.API.Services.Implementations.AI;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Application.Jobs;
using Jalsa.Application.Services;
using Jalsa.Application.Validators.Exercise;
using Jalsa.Infrastructure.Data;
using Jalsa.Infrastructure.Repositories;
using Jalsa.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(o =>
{
    o.Dsn = builder.Configuration["Sentry:Dsn"] ?? "";
    o.TracesSampleRate = double.TryParse(
        builder.Configuration["Sentry:TracesSampleRate"],
        out var rate) ? rate : 0.2;

    o.Environment = builder.Environment.EnvironmentName;
    o.SendDefaultPii = false;
});

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

var JwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();

if (string.IsNullOrEmpty(JwtSettings?.Key) ||
    JwtSettings.Key == "REPLACE_WITH_ENV_VAR_OR_USER_SECRETS")
{
    throw new InvalidOperationException(
        "JWT Key is not configured. Set Jwt__Key environment variable or use dotnet user-secrets.");
}

builder.Services
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtSettings!.Issuer,
            ValidAudience = JwtSettings.Audience,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(JwtSettings.Key))
        };

    // Browsers cannot set custom headers on the WebSocket upgrade request, so the
    // SignalR JS client appends the token as an "access_token" query param instead.
    // Without this hook, JwtBearer only reads the Authorization header, so the socket
    // upgrade itself would be unauthenticated (only the initial HTTP negotiate call,
    // which does carry the header, would succeed).
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/chatHub") || path.StartsWithSegments("/notificationHub")))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode =
        StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter(
        "general",
        limiterOptions =>
        {
            limiterOptions.PermitLimit = 100;
            limiterOptions.Window =
                TimeSpan.FromMinutes(1);
            limiterOptions.QueueLimit = 0;
        });

    options.AddFixedWindowLimiter(
        "ai",
        limiterOptions =>
        {
            limiterOptions.PermitLimit = 10;
            limiterOptions.Window =
                TimeSpan.FromMinutes(1);
            limiterOptions.QueueLimit = 0;
        });

    options.OnRejected =
        async (context, cancellationToken) =>
        {
            context.HttpContext.Response.ContentType =
                "application/json";

            await context.HttpContext.Response
            .WriteAsJsonAsync(
                new
                {
                    success = false,
                    message = "Too many requests. Please try again later."
                },
                cancellationToken);
        };
});

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email"));

builder.Services.Configure<OpenAiSettings>(
    builder.Configuration.GetSection("OpenAI"));

builder.Services.Configure<GatewaySettings>(options =>
{
    options.BaseUrl = builder.Configuration["Gateway:BaseUrl"] ?? string.Empty;
    options.ApiKey = builder.Configuration["SBG_API_KEY"] ?? string.Empty;
    options.ChatModelId = builder.Configuration["Gateway:ChatModelId"]
        ?? "deepseek.v3.2";
    options.EmbeddingModelId = builder.Configuration["Gateway:EmbeddingModelId"]
        ?? "amazon.titan-embed-text-v2:0:8k";
});

builder.Services.Configure<LangfuseSettings>(
    builder.Configuration.GetSection("Langfuse"));

builder.Services.Configure<GeminiSettings>(options =>
{
    options.BaseUrl = builder.Configuration["Gemini:BaseUrl"]
        ?? "https://generativelanguage.googleapis.com/v1beta/";
    var cfgKey = builder.Configuration["Gemini:ApiKey"];
    options.ApiKey = string.IsNullOrWhiteSpace(cfgKey)
        ? (builder.Configuration["GEMINI_API_KEY"] ?? string.Empty)
        : cfgKey;
    options.ChatModelId = builder.Configuration["Gemini:ChatModelId"]
        ?? "gemini-2.5-flash";
    options.EmbeddingModelId = builder.Configuration["Gemini:EmbeddingModelId"]
        ?? "gemini-embedding-001";
    if (int.TryParse(builder.Configuration["Gemini:EmbeddingDimensions"], out var dims)
        && dims > 0)
    {
        options.EmbeddingDimensions = dims;
    }
});

builder.Services.AddHttpClient<
    ILlmObservabilityService,
    LangfuseObservabilityService>();

builder.Services.AddHttpClient<IGatewayClient, GatewayClient>((sp, client) =>
{
    var gatewaySettings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<GatewaySettings>>().Value;

    if (!string.IsNullOrWhiteSpace(gatewaySettings.BaseUrl))
    {
        var baseUrl = gatewaySettings.BaseUrl.TrimEnd('/') + "/";
        client.BaseAddress = new Uri(baseUrl);
    }

    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", gatewaySettings.ApiKey);
});

builder.Services.AddHttpClient<IGeminiClient, GeminiClient>((sp, client) =>
{
    var geminiSettings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<GeminiSettings>>().Value;

    if (string.IsNullOrWhiteSpace(geminiSettings.ApiKey))
    {
        throw new InvalidOperationException(
            "Gemini ApiKey is not configured. Set Gemini__ApiKey or GEMINI_API_KEY.");
    }

    var baseUrl = geminiSettings.BaseUrl.TrimEnd('/') + "/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddSingleton<
    IPromptService,
    PromptService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmbeddingService, EmbeddingService>();
builder.Services.AddScoped<IVectorStore, VectorStore>();
builder.Services.AddScoped<IPatientContextBuilder, PatientContextBuilder>();
builder.Services.AddScoped<IConversationMemoryService, ConversationMemoryService>();
builder.Services.AddScoped<IChatAiService, ChatAiService>();
builder.Services.AddScoped<ICrisisDetectionService, CrisisDetectionService>();
builder.Services.AddScoped<ISummarizationService, SummarizationService>();
builder.Services.AddScoped<IReportGenerationService, ReportGenerationService>();
builder.Services.AddScoped<ITherapistChatAiService, TherapistChatAiService>();
builder.Services.AddScoped<ISessionNoteEmbeddingService, SessionNoteEmbeddingService>();
builder.Services.AddScoped<
    Jalsa.Application.Interfaces.Services.ISessionNoteEmbeddingCoordinator,
    SessionNoteEmbeddingCoordinator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<ITherapistAdminService, TherapistAdminService>();
builder.Services.AddScoped<IPatientAccountAdminService, PatientAccountAdminService>();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<ISystemHealthService, SystemHealthService>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddDbContext<JalsaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var corsOrigins =
    builder.Configuration
    .GetSection("CorsOrigins")
    .Get<string[]>()
    ?? new[]
    {
        "http://localhost:4300",
        "https://localhost:4300"
    };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseLogRepository, ExerciseLogRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IIntakeService, IntakeService>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<IPatientDashboardService, PatientDashboardService>();
builder.Services.AddScoped<IPatientProgressService, PatientProgressService>();
builder.Services.AddScoped<IPatientSessionService, PatientSessionService>();
builder.Services.AddScoped<IPatientAssessmentService, PatientAssessmentService>();
builder.Services.AddScoped<INotificationService, EmailNotificationService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ICrisisAlertService, CrisisAlertService>();
builder.Services.AddScoped<ExerciseReminderJob>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ExerciseCreateDtoValidator>();

builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true
        }));

builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    if (!app.Environment.IsDevelopment())
    {
        context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AngularPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<MaintenanceModeMiddleware>();

app.UseRateLimiter();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.Run();