using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Jalsa.API.Exceptions;

namespace Jalsa.API.Controllers;

/// <summary>
/// Base controller providing common functionality for all API controllers.
/// </summary>
[EnableRateLimiting("general")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Extracts the current user's ID from the JWT claims.
    /// Checks both ClaimTypes.NameIdentifier and "sub" claim.
    /// </summary>
    /// <returns>The user's GUID identifier.</returns>
    /// <exception cref="ApiException">Thrown when the user ID claim is missing or invalid.</exception>
    protected Guid GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var userId))
            throw new ApiException(401, "Invalid authentication token");

        return userId;
    }
}
