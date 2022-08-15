using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using Vnext.Intern.Authentication.Dtos;
using Vnext.Intern.InternDb;
using Vnext.Intern.Utility;
using Vnext.Intern.Utility.Authentication;
using Vnext.Intern.Utility.Authentication.Dtos;

namespace Vnext.Intern.Authentication
{
    public class AuthAppService : InternAppServiceBase, IAuthAppService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public AuthAppService(
            IRepository<Employee> employeeRepository
        )
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<LoginOutput> CheckLogin(LoginInput input)
        {
            try
            {
                var user = await _employeeRepository.GetAll()
                    .Where(x => x.Username.Equals(input.Username))
                    .FirstOrDefaultAsync();

                if (user == null || !user.Password.Equals(Utils.MD5Hash(input.Password)))
                {
                    throw new UserFriendlyException(400, L("TheUsernameOrPasswordInvalid"));
                }

                LoginOutput result = new LoginOutput
                {
                    UserId = 1,
                    RoleList = ","
                };

                AuthenticationService authenticationService = new AuthenticationService();
                IAuthContainerModel model = authenticationService.GetJWTContainerModel(user.Username, user.Email);
                result.AccessToken = authenticationService.GenerateToken(model);
                
                return result;
            }
            catch (UserFriendlyException ex)
            {
                Logger.Error(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public CheckTokenOutput CheckToken()
        {
            try
            {
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                if (token.Length == 0)
                {
                    throw new UserFriendlyException(400, "Missing Token");
                }

                AuthenticationService authenticationService = new AuthenticationService();
                if (!authenticationService.IsTokenValid(token[0]))
                {
                    throw new UserFriendlyException(400, "TheTokenInvalid");
                }

                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();
                var username = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;

                var user = _employeeRepository.GetAll()
                    .Where(x => x.Username.Equals(username))
                    .FirstOrDefault();
                if (user == null)
                {
                    throw new UserFriendlyException(400, "TheTokenInvalid");
                }

                CheckTokenOutput result = new CheckTokenOutput
                {
                    UserId = user.Id,
                    RoleList = ","
                };

                return result;
            }
            catch (UserFriendlyException ex)
            {
                Logger.Error(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
    }
}

