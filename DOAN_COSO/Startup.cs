using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DOAN_COSO.Startup))]
namespace DOAN_COSO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
