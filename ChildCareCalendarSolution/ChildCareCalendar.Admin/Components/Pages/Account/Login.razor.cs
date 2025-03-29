using ChildCareCalendar.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
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

        [IgnoreAntiforgeryToken]
        private async Task Authenticate()
        {
            var userAccount = appDbContext.Users.Where(x => x.Email == loginModel.Email && x.Role != null).FirstOrDefault();
            if (userAccount is null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, userAccount.Password))
            {
                errorMessage = "Invalid Username or Password";
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginModel.Email),
                new Claim(ClaimTypes.Role, userAccount.Role),
                new Claim("FullName", userAccount.FullName)
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