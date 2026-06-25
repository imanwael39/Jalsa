using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Jalsa.API.Configurations;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Implementations;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.API.Services.Implementations.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.Interfaces.Repositores;
using Hangfire;
using Hangfire.SqlServer;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Application.Jobs;
using Jalsa.Application.Services;
using Jalsa.Infrastructure.Repositories;
using Jalsa.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Jalsa.Application.Validators.Exercise;
using Jalsa.API.DTOs.Patient;

var builder = WebApplication.CreateBuilder(args);

//create jwt token
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);
var JwtSettings= builder.Configuration.GetSection("Jwt")
                 .Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters=
    new TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,
        ValidIssuer=JwtSettings!.Issuer,
        ValidAudience=JwtSettings.Audience,
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key))
    };
});
builder.Services.AddAuthorization();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email")
);
builder.Services.Configure<OpenAiSettings>(
    builder.Configuration.GetSection("OpenAI")
);
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmbeddingService, EmbeddingService>();
builder.Services.AddScoped<IVectorStore, VectorStore>();
builder.Services.AddScoped<IConversationMemoryService, ConversationMemoryService>();
builder.Services.AddScoped<IChatAiService, ChatAiService>();
builder.Services.AddScoped<ICrisisDetectionService, CrisisDetectionService>();
builder.Services.AddScoped<ISummarizationService, SummarizationService>();
builder.Services.AddScoped<IReportGenerationService, ReportGenerationService>();
builder.Services.AddScoped<IOcrService, OcrService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService ,PatientService>();

builder.Services.AddDbContext<Galsa_DBDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddScoped<IUnitOfWork ,UnitOfWork>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseLogRepository, ExerciseLogRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddScoped<INotificationService, EmailNotificationService>();
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

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        if (exception is ApiException apiEx)
        {
            context.Response.StatusCode = apiEx.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = apiEx.Message });
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate<ExerciseReminderJob>(
    "exercise-reminder",
    job => job.SendRemindersAsync(),
    Cron.Daily(9));

app.Run();