using Jalsa.API.DTOs.Admin;

namespace Jalsa.API.Services.Interfaces;

public interface ISystemHealthService
{
    Task<SystemHealthDto> GetSystemHealthAsync();
}
