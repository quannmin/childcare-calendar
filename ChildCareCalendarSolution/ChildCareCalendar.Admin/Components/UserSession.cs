using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ChildCareCalendar.Admin.Components
{
    public static class UserSession
    {
        public static ClaimsPrincipal User { get; private set; } = new ClaimsPrincipal(new ClaimsIdentity());

        public static async Task InitializeAsync(AuthenticationStateProvider authStateProvider)
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            User = authState.User;
        }
    }

}
