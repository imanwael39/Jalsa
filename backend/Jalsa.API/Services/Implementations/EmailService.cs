using System.Net;
using System.Net.Mail;
using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendOtpAsync(string toEmail, string otp)
    {
        if (string.IsNullOrEmpty(_settings.SmtpHost))
        {
            _logger.LogInformation("OTP for {Email}: {Otp}", toEmail, otp);
            return;
        }

        using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort);
        client.Credentials = new NetworkCredential(_settings.SmtpUsername, _settings.SmtpPassword);
        client.EnableSsl = true;

        var mail = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = "Your Password Reset OTP",
            Body = $"Your OTP for password reset is: {otp}. It expires in 5 minutes.",
            IsBodyHtml = false
        };
        mail.To.Add(toEmail);

        await client.SendMailAsync(mail);
    }
}
