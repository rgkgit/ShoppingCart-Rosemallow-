using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ShoppingCartApi.Startup))]

namespace ShoppingCartApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}