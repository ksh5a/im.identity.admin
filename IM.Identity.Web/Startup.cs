using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IM.Identity.Web.Startup))]
namespace IM.Identity.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
