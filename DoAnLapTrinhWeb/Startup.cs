using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoAnLapTrinhWeb.Startup))]
namespace DoAnLapTrinhWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
