using System.Net.Mail;

namespace Core.Infrastructure.Crosscutting
{
    public interface IMailService
    {
        void ConfigureServer(string host, int port, string username, string password);
        bool Send(MailMessage mail);
    }
}
