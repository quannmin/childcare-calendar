using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services;
using System.Linq.Expressions;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using ChildCareCalendar.Domain.ViewModels.Account;

namespace ChildCareCalendar.Admin.Components.Pages.Account
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
        private Infrastructure.Services.Interfaces.IAuthenticationService AuthService { get; set; } = default!;

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

                // Kiểm tra người dùng tồn tại bằng email thay vì số điện thoại
                var users = await UserService.FindUsersAsync(u =>
                    u.Email == loginModel.Email.Trim() && !u.IsDelete);
                var user = users.FirstOrDefault();

                if (user == null)
                {
                    // Kiểm tra xem email có tồn tại nhưng bị xóa không
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

                // Xác thực mật khẩu
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

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };


                //await AuthService.SignInAsync(
                //    new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                //    authProperties);

                if (user.Role == "PhuHuynh")
                {
                    NavigationManager.NavigateTo("/", replace: true);
                }
                else if (user.Role == "Admin")
                {
                    NavigationManager.NavigateTo("/", replace: true);
                }
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