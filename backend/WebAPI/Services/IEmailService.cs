using WebAPI.Models.Redis;

namespace WebAPI.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage emailMessage);
    }
}
