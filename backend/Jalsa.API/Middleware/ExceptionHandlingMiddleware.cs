using System.Text.Json;
using Jalsa.API.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException ex)
        {
            _logger.LogWarning(ex, "API exception: {Message}", ex.Message);
            await WriteResponseAsync(context, ex.StatusCode, ex.Message);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update exception");
            await WriteResponseAsync(context, StatusCodes.Status409Conflict, "تعذر حفظ البيانات، قد تكون البيانات مكررة.");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Not found: {Message}", ex.Message);
            await WriteResponseAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Forbidden: {Message}", ex.Message);
            await WriteResponseAsync(context, StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation: {Message}", ex.Message);
            await WriteResponseAsync(context, StatusCodes.Status409Conflict, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, "حدث خطأ غير متوقع، يرجى المحاولة مرة أخرى.");
        }
    }

    private static Task WriteResponseAsync(HttpContext context, int statusCode, string message)
    {
        if (context.Response.HasStarted)
            return Task.CompletedTask;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var payload = JsonSerializer.Serialize(new { message });
        return context.Response.WriteAsync(payload);
    }
}
