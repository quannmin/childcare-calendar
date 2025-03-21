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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing login page: {ex.Message}");
            }
        }

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
            }
        }

        private async Task LoginWithFacebook()
        {
      
            await JS.InvokeVoidAsync("console.log", "Facebook login clicked");
            await JS.InvokeVoidAsync("alert", "Đăng nhập với Facebook chưa được triển khai.");
        }

        private async Task LoginWithGmail()
        {
         
            await JS.InvokeVoidAsync("console.log", "Gmail login clicked");
            await JS.InvokeVoidAsync("alert", "Đăng nhập với Gmail chưa được triển khai.");
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

