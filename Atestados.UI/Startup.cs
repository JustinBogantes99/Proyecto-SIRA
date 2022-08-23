using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Atestados.UI.Startup))]
namespace Atestados.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
