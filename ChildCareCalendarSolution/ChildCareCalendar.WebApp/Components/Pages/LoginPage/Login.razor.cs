using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using ChildCareCalendar.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Http;

namespace ChildCareCalendar.WebApp.Components.Pages.LoginPage
{
    public partial class Login
    {
        private LoginViewModel loginModel = new LoginViewModel();
        private bool showPassword = false;
        private bool isLoading = false;
        private string errorMessage = string.Empty;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        private void TogglePasswordVisibility()
        {
            showPassword = !showPassword;
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                isLoading = true;
                errorMessage = string.Empty;

                var user = (await UserService.FindUsersAsync(u =>
            u.Email == loginModel.Email.Trim() && !u.IsDelete)).FirstOrDefault();

                if (user == null)
                {
                    var deletedUser = (await UserService.FindUsersAsync(u =>
                        u.Email == loginModel.Email.Trim() && u.IsDelete)).FirstOrDefault();

                    if (deletedUser != null)
                    {
                        errorMessage = "Tài khoản này đã bị vô hiệu hóa";
                    }
                    else
                    {
                        errorMessage = "Email không tồn tại trong hệ thống";
                    }
                    return;
                }

         
                bool isPasswordValid = BCrypt.Net.BCrypt.EnhancedVerify(
                                 loginModel.Password,
                                 user.Password,
                                 HashType.SHA256);

                if (!isPasswordValid)
                {
                    errorMessage = "Mật khẩu không chính xác";
                    return;
                }

                // Lưu thông tin đăng nhập       
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(
                   claims,
                   CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, 
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) 
                };

                NavigationManager.NavigateTo("/", forceLoad: true);

                await HttpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
           

             

            }
            catch (Exception ex)
            {
                errorMessage = "Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại sau.";
                Console.WriteLine($"Login error: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}