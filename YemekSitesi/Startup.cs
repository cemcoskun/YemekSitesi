using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YemekSitesi.Startup))]
namespace YemekSitesi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
