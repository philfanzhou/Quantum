using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure.Crosscutting;

namespace Core.Infrastructure.Impl.Crosscutting
{
    internal class Captcha : ICaptcha
    {
        private Dictionary<string, string> store = new Dictionary<string, string>();
        private object sync = new object();

        public string GenerateCaptcha(string key)
        {
            lock (sync)
            {
                if (store.ContainsKey(key))
                {
                    store.Remove(key);
                }
                string captcha = Guid.NewGuid().ToString("N").Substring(0, 16);
                store.Add(key, captcha);
                return captcha;
            }
        }

        public bool VerifyCaptcha(string key, string captcha)
        {
            lock (sync)
            {
                if (store.ContainsKey(key))
                {
                    if (store[key] == captcha)
                    {
                        store.Remove(key);
                        return true;
                    }                    
                }
            }
            return false;
        }
    }
}
