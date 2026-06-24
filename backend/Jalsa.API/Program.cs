using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Jalsa.API.Configurations;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Infrastructure.Repositories;
using Jalsa.API.DTOs.Patient;
using Jalsa.API.Services.Implementations;

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
builder.Services.AddScoped<IPatientService ,PatientService>();

builder.Services.AddDbContext<Galsa_DBDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWork ,UnitOfWork>();
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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
