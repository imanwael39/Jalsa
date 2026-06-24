namespace Jalsa.API.Services.Interfaces;
public interface IEmailService
{
    Task SendOtpAsync(string toEmail, string otp);
}
