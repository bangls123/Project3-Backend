using Abp.Application.Services;
using System.Threading.Tasks;
using System.Web.Http;

using Vnext.Intern.Authentication.Dtos;

namespace Vnext.Intern.Authentication
{
    public interface IAuthAppService : IApplicationService
    {
        [HttpPost]
        Task<LoginOutput> CheckLogin(LoginInput input);
        
        [HttpGet]
        CheckTokenOutput CheckToken();
    }
}

