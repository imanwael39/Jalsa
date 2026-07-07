using System.Text.Json;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Middleware;

/// <summary>
/// Blocks non-Admin requests when the "MaintenanceMode" system setting is on. Admin
/// requests and the auth/settings endpoints stay open so an Admin can always turn it back off.
/// </summary>
public class MaintenanceModeMiddleware
{
    private readonly RequestDelegate _next;

    public MaintenanceModeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, JalsaDbContext dbContext)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        var isExempt = path.StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/api/auth", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("/hangfire", StringComparison.OrdinalIgnoreCase)
            || context.User.IsInRole("Admin");

        if (isExempt)
        {
            await _next(context);
            return;
        }

        var setting = await dbContext.SystemSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Key == "MaintenanceMode");

        if (bool.TryParse(setting?.Value, out var isOn) && isOn)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "النظام تحت الصيانة حاليًا، يرجى المحاولة لاحقًا." }));
            return;
        }

        await _next(context);
    }
}
