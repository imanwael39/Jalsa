using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.DTOs.Auth;
using Jalsa.API.Services.Interfaces;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient/auth")]
public class PatientAuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public PatientAuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (!result.Roles.Contains("Patient"))
            throw new Jalsa.API.Exceptions.ApiException(403, "Access denied. Patient accounts only.");

        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize(Roles = "Patient")]
    public IActionResult Logout()
    {
        return NoContent();
    }
}
