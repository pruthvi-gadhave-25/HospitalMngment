using HospitalManagement.Helpers;
using HospitalManagement.Models.Mails;
using HospitalManagement.Services.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HospitalManagement.Services
{
    public class EmailSendService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailSendService> _logger;

        public EmailSendService(IOptions<MailSettings> options, ILogger<EmailSendService> logger)
        {
            _mailSettings = options.Value;
            _logger = logger;
        }
        public async Task<Result<string>> EmailSendServices(MailRequest mailRequest)
        {
            var email = new MimeMessage();
          
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            var res = await smtp.SendAsync(email);

            //handling situTION CORRECTLT ERROR RETURN FAILED 
            smtp.Disconnect(true);
            if (!string.Equals(res, "OK", StringComparison.OrdinalIgnoreCase) &&
               !string.Equals(res, "Sent", StringComparison.OrdinalIgnoreCase) &&
               !string.IsNullOrWhiteSpace(res)) 
            {
                _logger.LogError($"Failed to Send email {mailRequest.ToEmail}");
                return Result<string>.ErrorResult($"Email sending failed: {res}");                
            }
            _logger.LogInformation($"Suucfully Sent Email {mailRequest.ToEmail}");
            return Result<string>.SuccessResult(res, "sedn succefully");
        }
    }
}
