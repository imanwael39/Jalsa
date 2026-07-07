namespace Jalsa.Application.DTOs.Admin;

public class SystemSettingsDto
{
    public string SiteName { get; set; } = "Jalsa";
    public string DefaultLanguage { get; set; } = "ar";
    public int PasswordMinLength { get; set; } = 8;
    public int SessionTimeoutMinutes { get; set; } = 60;
    public bool MaintenanceMode { get; set; } = false;
}
