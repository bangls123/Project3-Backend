using System.Net.Http;
using System.Web.Http.Cors;

namespace Vnext.Intern.Web.Config
{
    public class CorsPolicyFactory : ICorsPolicyProviderFactory
    {
        ICorsPolicyProvider _provider = new CorsPolicyConfig();

        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return _provider;
        }
    }
}
