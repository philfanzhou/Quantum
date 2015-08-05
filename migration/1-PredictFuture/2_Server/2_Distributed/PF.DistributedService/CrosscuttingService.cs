using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Core.Infrastructure.Crosscutting;
using Core.Infrastructure.Crosscutting.Service;
using PF.DistributedService.Contracts.Crosscutting;

namespace PF.DistributedService
{
    public class CrosscuttingService : WCFServiceBase, ICrosscuttingService
    {

        private static IMailService mailSvc;
        private static ICaptcha captchaSvc;

        private static readonly string from = "pfagent@yeah.net";

        public CrosscuttingService()
        {
            if (mailSvc == null)
            {
                mailSvc = ContainerHelper.Instance.Resolve<IMailService>();
                mailSvc.ConfigureServer("smtp.yeah.net", 25, "pfagent", "pf18616379842");
            }
            if (captchaSvc == null)
            {
                captchaSvc = ContainerHelper.Instance.Resolve<ICaptcha>();
            }            
        }
        
        #region Mail
        public bool SendMail(string to, string subject, string body)
        {
            bool result = false;
            using (MailMessage mail = new MailMessage(from, to, subject, body))
            {
                mail.From = new MailAddress(from, "Predict Future");
                mail.To.Clear();
                mail.To.Add(new MailAddress(to));
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = false;

                result = mailSvc.Send(mail);
            }
            return result;
        }
        #endregion

        #region Captcha
        public string GenerateCaptcha(string key)
        {
            return captchaSvc.GenerateCaptcha(key);
        }

        public bool VerifyCaptcha(string key, string captcha)
        {
            return captchaSvc.VerifyCaptcha(key, captcha);
        }
        #endregion
    }
}
