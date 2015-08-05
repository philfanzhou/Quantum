using System.Configuration;
using System.Web.Configuration;

namespace Core.DistributedServices.WCF
{
    public class ConfigurationProvider
    {
        static ConfigurationProvider()
        {
            try
            {
                Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            catch
            {
                Configuration = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            }
        }

        public static System.Configuration.Configuration Configuration { get; private set; }

        public static void SetConfiguration(System.Configuration.Configuration config)
        {
            Configuration = config;
        }
    }
}
