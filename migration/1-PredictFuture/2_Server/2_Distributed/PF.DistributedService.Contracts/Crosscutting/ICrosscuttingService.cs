using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace PF.DistributedService.Contracts.Crosscutting
{
    [ServiceContract]
    public interface ICrosscuttingService
    {
        #region Mail
        [OperationContract]
        bool SendMail(string to, string subject, string body);
        #endregion

        #region Captcha
        [OperationContract]
        string GenerateCaptcha(string key);
        [OperationContract]
        bool VerifyCaptcha(string key, string captcha);
        #endregion
    }
}
