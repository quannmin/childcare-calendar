using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task SignInAsync(ClaimsPrincipal principal, AuthenticationProperties properties, CancellationToken cancellationToken = default);
        Task SignOutAsync(AuthenticationProperties properties, CancellationToken cancellationToken = default);
        Task<ClaimsPrincipal?> GetAuthenticatedUserAsync(CancellationToken cancellationToken = default);
    }
}