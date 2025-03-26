using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ChildCareCalendar.WebApp.Components.Pages.AuthenPage
{
    public partial class Login
    {
        private LoginModel loginModel = new LoginModel();
        private bool showPassword = false;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        private void TogglePasswordVisibility()
        {
            showPassword = !showPassword;
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                await JS.InvokeVoidAsync("console.log", "Login attempted with:", loginModel.PhoneNumber);

                if (loginModel.PhoneNumber != null && loginModel.Password != null)
                {
                    // Thực hiện kiểm tra đăng nhập ở đây
                    // Ví dụ đơn giản: nếu số điện thoại và mật khẩu không rỗng thì cho đăng nhập
                    await JS.InvokeVoidAsync("localStorage.setItem", "auth_token", "sample-token-123");
                    NavigationManager.NavigateTo("/dashboard");
                }
                else
                {
                    await JS.InvokeVoidAsync("alert", "Đăng nhập không thành công. Vui lòng kiểm tra lại thông tin đăng nhập.");
                }
            }
            catch (Exception ex)
            {
                await JS.InvokeVoidAsync("console.error", $"Login error: {ex.Message}");
                await JS.InvokeVoidAsync("alert", $"Lỗi: {ex.Message}");
            }
        }

        public class LoginModel
        {
            [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
            [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
            public string PhoneNumber { get; set; } = "+84";

            [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
            public string Password { get; set; }
        }
    }
}