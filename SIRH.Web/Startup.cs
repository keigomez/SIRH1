using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SIRH.Web.Startup))]
namespace SIRH.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
