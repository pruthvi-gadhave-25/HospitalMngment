using HospitalManagement.Helpers;
using HospitalManagement.Models.Mails;

namespace HospitalManagement.Services.Interface
{
    public interface IEmailService
    {
        Task<Result<string>> EmailSendServices(MailRequest mailRequest);
    }
}
