using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class AuthenticationService : Services.Interfaces.IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SignInAsync(ClaimsPrincipal principal, AuthenticationProperties properties, CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("HttpContext is not available");

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);
        }

        public async Task SignOutAsync(AuthenticationProperties properties, CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("HttpContext is not available");

            await _httpContextAccessor.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                properties);
        }

        public async Task<ClaimsPrincipal?> GetAuthenticatedUserAsync(CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("HttpContext is not available");

            var result = await _httpContextAccessor.HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return result?.Principal;
        }
    }
}