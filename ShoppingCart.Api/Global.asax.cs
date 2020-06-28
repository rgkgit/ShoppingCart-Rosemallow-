using ShoppingCart.Helper.AutoMapper;
using System.Web.Http;

namespace ShoppingCart.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfig.Configure();
        }
    }
}
