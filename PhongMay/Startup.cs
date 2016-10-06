using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhongMay.Startup))]
namespace PhongMay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
