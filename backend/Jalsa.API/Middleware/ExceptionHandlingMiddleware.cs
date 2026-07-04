using System.Text.Json;
using Jalsa.API.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sentry;

namespace Jalsa.API.Middleware;

/// <summary>
/// Catches every exception thrown by the pipeline and turns it into a consistent
/// { message } JSON response. Without this, ApiException (and any other exception)
/// propagates unhandled and the client receives a bare 500 with no body.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private const int UniqueConstraintViolation1 = 2601;
    private const int UniqueConstraintViolation2 = 2627;

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
            await WriteResponseAsync(context, ex.StatusCode, ex.Message);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            _logger.LogWarning(ex, "Unique constraint violation");
            await WriteResponseAsync(context, StatusCodes.Status409Conflict, "البيانات المدخلة مستخدمة بالفعل.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            SentrySdk.CaptureException(ex);
            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, "حدث خطأ غير متوقع في الخادم، يرجى المحاولة مرة أخرى.");
        }
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        return ex.InnerException is SqlException sqlEx &&
               (sqlEx.Number == UniqueConstraintViolation1 || sqlEx.Number == UniqueConstraintViolation2);
    }

    private static async Task WriteResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { message }));
    }
}
