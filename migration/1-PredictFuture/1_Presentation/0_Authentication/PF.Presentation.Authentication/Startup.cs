using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PF.Presentation.Authentication.Startup))]
namespace PF.Presentation.Authentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
