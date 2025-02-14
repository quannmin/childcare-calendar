using ChildCareCalendar.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    public partial class Login
    {
        [CascadingParameter]
        HttpContext httpContext { get; set; }

        [SupplyParameterFromForm]
        private LoginViewModel loginModel { get; set; } = new();

        [Inject]
        private NavigationManager navigationManager { get; set; }

        private string? errorMessage;
        private async Task Authenticate()
        {
            var userAccount = appDbContext.Users.Where(x => x.Email == loginModel.Email).FirstOrDefault();
            if (userAccount is null || userAccount.Password != loginModel.Password)
            {
                errorMessage = "Invalid Username or Password";
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginModel.Email),
                new Claim(ClaimTypes.Role, userAccount.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
            navigationManager.NavigateTo("/");
        }
    }
}
