using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Vnext.Intern.EntityFramework.InternDb;
using Vnext.Intern.Utility.Authentication;

namespace Vnext.Intern.Authentication
{
    public class AuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;
            var path = request.RequestUri.LocalPath;

            if (path != "/api/services/app/auth/checkLogin")
            {
                if (authorization == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                    return;
                }

                var token = authorization.ToString();
                var principal = await AuthenticateToken(token);

                if (principal == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid Token", request);
                }
                else
                {
                    context.Principal = principal;
                }
            }
        }

        protected Task<IPrincipal> AuthenticateToken(string token)
        {
            if (ValidateToken(token, out string username))
            {
                InternDbDbContext internDbDbContext = new InternDbDbContext();
                var userData = internDbDbContext.Employees
                    .Where(x => x.Username.Equals(username))
                    .FirstOrDefault();
                if (userData == null)
                {
                    return Task.FromResult<IPrincipal>(null);
                }

                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, username)
                };

                var identity = new ClaimsIdentity(claims, "Jwt");

                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        private static bool ValidateToken(string token, out string username)
        {
            AuthenticationService authenticationService = new AuthenticationService();

            username = "";
            if (!authenticationService.IsTokenValid(token))
            {
                return false;
            }

            var tokenClaims = authenticationService.GetTokenClaims(token).ToList();
            if (tokenClaims == null)
            {
                return false;
            }

            username = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            
            return true;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}

