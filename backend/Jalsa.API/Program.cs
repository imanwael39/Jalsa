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
builder.Services.AddScoped<IGenericRepository<SessionNote>>(sp =>
{
    var unitOfWork = sp.GetRequiredService<IUnitOfWork>();
    return unitOfWork.Repository<SessionNote>();
});

builder.Services.AddAutoMapper(typeof(SessionMappingProfile).Assembly);

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

    if (!context.Patients.Any())
    {
        var therapistUser = context.Users.FirstOrDefault(u => u.Email == "dr@test.com");
        if (therapistUser != null)
        {
            var therapist = context.Therapists.FirstOrDefault(t => t.UserId == therapistUser.Id);
            if (therapist == null)
            {
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
                FullName = "Ahmed Patient",
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
