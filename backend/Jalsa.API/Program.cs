using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

using Jalsa.API.Configurations;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Implementations;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.API.Services.Implementations.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.Interfaces.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Application.Jobs;
using Jalsa.Application.Services;
using Jalsa.Infrastructure.Repositories;
using Jalsa.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Jalsa.Application.Validators.Exercise;

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

builder.Services.Configure<LangfuseSettings>(
    builder.Configuration.GetSection("Langfuse"));

builder.Services.AddHttpClient<
    ILlmObservabilityService,
    LangfuseObservabilityService>();

builder.Services.AddSingleton<
    IPromptService,
    PromptService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmbeddingService, EmbeddingService>();
builder.Services.AddScoped<IVectorStore, VectorStore>();
builder.Services.AddScoped<IConversationMemoryService, ConversationMemoryService>();
builder.Services.AddScoped<IChatAiService, ChatAiService>();
builder.Services.AddScoped<ICrisisDetectionService, CrisisDetectionService>();
builder.Services.AddScoped<ISummarizationService, SummarizationService>();
builder.Services.AddScoped<IReportGenerationService, ReportGenerationService>();
builder.Services.AddScoped<IOcrService, OcrService>();
builder.Services.AddScoped<ISttService, SttService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddDbContext<JalsaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsOrigins =
    builder.Configuration
    .GetSection("CorsOrigins")
    .Get<string[]>()
    ?? new[] { "http://localhost:4200" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins(corsOrigins)
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
builder.Services.AddScoped<INotificationService, EmailNotificationService>();
builder.Services.AddScoped<IChatService, ChatService>();
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
            CommandBatchMaxTimeout =
                TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout =
                TimeSpan.FromMinutes(5),
            QueuePollInterval =
                TimeSpan.Zero,
            UseRecommendedIsolationLevel =
                true
        }));

builder.Services.AddHangfireServer();

var app = builder.Build();

app.Run();