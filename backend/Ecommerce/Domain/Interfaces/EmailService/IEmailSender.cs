using Domain.Models.EmailService;
using System.Threading.Tasks;

namespace Domain.Interfaces.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
