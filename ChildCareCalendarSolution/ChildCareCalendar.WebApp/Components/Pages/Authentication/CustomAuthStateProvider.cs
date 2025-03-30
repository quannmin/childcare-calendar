using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace ChildCareCalendar.WebApp.Components.Pages.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userIdResult = await _sessionStorage.GetAsync<int>("userId");
                var emailResult = await _sessionStorage.GetAsync<string>("email");
                var roleResult = await _sessionStorage.GetAsync<string>("role");
                var nameResult = await _sessionStorage.GetAsync<string>("fullname");
                var avatarResult = await _sessionStorage.GetAsync<string>("avatar");

                var userId = userIdResult.Success ? userIdResult.Value : 0;
                var email = emailResult.Success ? emailResult.Value : null;
                var role = roleResult.Success ? roleResult.Value : null;
                var name = nameResult.Success ? nameResult.Value : null;
                var avatar = avatarResult.Success ? avatarResult.Value : null;


                if (string.IsNullOrEmpty(email))
                    return new AuthenticationState(_anonymous);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role ?? "User"),
                    new Claim(ClaimTypes.Name, name ?? ""),
                    new Claim("avatar", avatar ?? "")
                };

                var identity = new ClaimsIdentity(claims, "Custom Authentication");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
