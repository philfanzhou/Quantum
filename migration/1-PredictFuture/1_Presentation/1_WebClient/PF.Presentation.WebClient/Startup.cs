using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PF.Presentation.WebClient.Startup))]
namespace PF.Presentation.WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
