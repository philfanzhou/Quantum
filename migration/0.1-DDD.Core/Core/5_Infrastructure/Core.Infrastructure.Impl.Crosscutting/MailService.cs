using Core.Infrastructure.Crosscutting;
using System.Net;
using System.Net.Mail;

namespace Core.Infrastructure.Impl.Crosscutting
{
    internal class MailService : IMailService
    {
        private SmtpClient client;

        public void ConfigureServer(string host, int port, string username, string password)
        {
            this.client = new SmtpClient(host, port);
            this.client.UseDefaultCredentials = false;
            this.client.Credentials = new NetworkCredential(username, password);
            this.client.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        public bool Send(MailMessage mail)
        {
            if (this.client == null)
            {
                return false;
            }

            try
            {
                this.client.Send(mail);
                return true;
            }
            catch {}
            return false;
        }
    }
}
