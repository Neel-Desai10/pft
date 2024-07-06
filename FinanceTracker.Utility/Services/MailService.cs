using FinanceTracker.Utility.Interface;
using FinanceTracker.Utility.Model;
using FinanceTracker.Utility.Utility;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FinanceTracker.Utility.Services;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<MailService> _logger;

    public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmail(EmailModel emailModel)
    {
        try
        {
            var builder = new BodyBuilder { HtmlBody = emailModel.Body };
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = emailModel.Subject,
                Body = builder.ToMessageBody()
            };
            email.To.Add(MailboxAddress.Parse(emailModel.ToEmail));
            var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            _logger.LogInformation(Resources.MailSentMessage, emailModel.Subject, emailModel.ToEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Resources.MailError);
            throw new Exception(Resources.MailError);
        }
    }
}