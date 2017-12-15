using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Service_Camp.Startup))]
namespace Service_Camp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
