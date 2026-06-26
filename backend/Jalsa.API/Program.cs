using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Jalsa.API.Configurations;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Application.Mappings;
using Jalsa.API.Services.Implementations;
using Jalsa.Application.Services;
using Jalsa.Infrastructure.Repositories;
using Jalsa.Domain.Models.Identity;
using Jalsa.Domain.Models.Session;
using FluentValidation;
using Jalsa.Application.Validators;

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

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDbContext<Galsa_DBDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ICrisisService, CrisisService>();
builder.Services.AddScoped<IGenericRepository<SessionNote>>(sp =>
{
    var unitOfWork = sp.GetRequiredService<IUnitOfWork>();
    return unitOfWork.Repository<SessionNote>();
});

builder.Services.AddAutoMapper(typeof(SessionMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ExerciseMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ChatMappingProfile).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<SessionCreateDtoValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Galsa_DBDbContext>();
    var roles = new[] { "Therapist", "Patient", "Admin" };
    foreach (var roleName in roles)
    {
        if (!context.Roles.Any(r => r.Name == roleName))
        {
            context.Roles.Add(new Role { Id = Guid.NewGuid(), Name = roleName });
        }
    }
    await context.SaveChangesAsync();

    var patientRole = context.Roles.FirstOrDefault(r => r.Name == "Patient");
    var patientUser = context.Users.FirstOrDefault(u => u.Email == "patient3@jalsa.com");
    if (patientUser != null && patientRole != null)
    {
        var hasPatientRole = context.UserRoles.Any(ur => ur.UserId == patientUser.Id && ur.RoleId == patientRole.Id);
        if (!hasPatientRole)
        {
            context.UserRoles.Add(new UserRole
            {
                UserId = patientUser.Id,
                RoleId = patientRole.Id,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        var existingPatient = context.Patients.FirstOrDefault(p => p.UserId == patientUser.Id);
        if (existingPatient == null)
        {
            var therapist = context.Therapists.FirstOrDefault();
            if (therapist == null)
            {
                var therapistRole = context.Roles.FirstOrDefault(r => r.Name == "Therapist");
                var therapistUser = context.Users.FirstOrDefault(u => u.UserRoles.Any(ur => ur.RoleId == therapistRole.Id));
                if (therapistUser == null)
                {
                    therapistUser = new Jalsa.Domain.Models.Identity.User
                    {
                        Id = Guid.NewGuid(),
                        Email = "dr@jalsa.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Therapist123!"),
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    context.Users.Add(therapistUser);
                    if (therapistRole != null)
                    {
                        context.UserRoles.Add(new Jalsa.Domain.Models.Identity.UserRole
                        {
                            UserId = therapistUser.Id,
                            RoleId = therapistRole.Id,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                    await context.SaveChangesAsync();
                }

                therapist = new Jalsa.Domain.Models.Clinic.Therapist
                {
                    Id = Guid.NewGuid(),
                    UserId = therapistUser.Id,
                    FullName = "Dr. Test",
                    LicenseNumber = "LIC-001",
                    CreatedAt = DateTime.UtcNow
                };
                context.Therapists.Add(therapist);
                await context.SaveChangesAsync();
            }

            var patient = new Jalsa.Domain.Models.Patient.Patient
            {
                Id = Guid.NewGuid(),
                TherapistId = therapist.Id,
                UserId = patientUser.Id,
                FullName = "Patient 3",
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        }
    }
}

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
