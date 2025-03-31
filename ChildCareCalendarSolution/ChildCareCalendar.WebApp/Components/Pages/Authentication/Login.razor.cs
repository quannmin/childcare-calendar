using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using BCrypt.Net;
using ChildCareCalendar.Domain.ViewModels.Account;

namespace ChildCareCalendar.WebApp.Components.Pages.Authentication
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
        private AuthService AuthService { get; set; } = default!;


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
                var userAccount = appDbContext.Users.Where(x => x.Email == loginModel.Email && x.Role != null).FirstOrDefault();
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

              
                bool success = await AuthService.LoginAsync(loginModel.Email, loginModel.Password);

                if (!success)
                {
                    errorMessage = "Email hoặc mật khẩu không chính xác";
                    return;
                }

                NavigationManager.NavigateTo("/", forceLoad: true);


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
        private async Task HandleGoogleLogin()
        {
            try
            {
                isLoading = true;
                errorMessage = string.Empty;
            
                NavigationManager.NavigateTo("/signin-google", forceLoad: true);
            }
            catch (Exception ex)
            {
                errorMessage = "Đã xảy ra lỗi khi đăng nhập bằng Google";
                Console.WriteLine($"Google login error: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}