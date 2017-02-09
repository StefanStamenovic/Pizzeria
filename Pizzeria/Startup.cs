using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Pizzeria.Startup))]
namespace Pizzeria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
