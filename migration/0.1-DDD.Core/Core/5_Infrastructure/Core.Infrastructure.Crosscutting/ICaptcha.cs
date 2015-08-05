using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Crosscutting
{
    public interface ICaptcha
    {
        string GenerateCaptcha(string key);
        bool VerifyCaptcha(string key, string captcha);
    }
}
