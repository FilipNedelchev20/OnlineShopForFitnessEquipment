using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class DummyEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // You can log this, write to console, etc.
        Console.WriteLine($"[Fake Email] To: {email}, Subject: {subject}");
        return Task.CompletedTask;
    }
}
