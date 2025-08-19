using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var apiKey = _config["Email:SendGridApiKey"];
        var client = new SendGridClient(apiKey);

        var fromEmail = _config["Email:FromEmail"];
        var fromName = _config["Email:FromName"];

        var from = new EmailAddress(fromEmail, fromName);
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlMessage);

        var response = await client.SendEmailAsync(msg);

        // სჯობს response გადაამოწმო Debug-ში
        if (!response.IsSuccessStatusCode)
        {
            // შეგიძლია ლოგში ჩაწერო რა მოხდა
            var body = await response.Body.ReadAsStringAsync();
            throw new System.Exception($"SendGrid failed: {body}");
        }
    }
}
