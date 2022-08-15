using System.Web.Http;
using Vnext.Intern.Web.Config;

namespace Vnext.Intern.Web.App_Start
{
    public class ApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.SetCorsPolicyProviderFactory(new CorsPolicyFactory());
            config.EnableCors();
        }
    }
}
