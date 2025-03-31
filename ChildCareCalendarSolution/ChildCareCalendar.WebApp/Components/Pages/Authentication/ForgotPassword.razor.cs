using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ChildCareCalendar.Domain.ViewModels;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Helper;
using System;
using System.Threading.Tasks;
using ChildCareCalendar.Domain.ViewModels.Account;
using BCrypt.Net;

namespace ChildCareCalendar.WebApp.Components.Pages.Authentication
{
    public partial class ForgotPassword
    {
        [SupplyParameterFromForm]
        public ForgotPasswordViewModel forgotPasswordViewModel { get; set; } = new();

        [Inject] private IUserService userService { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private IEmailService emailService { get; set; } = default!;

        private IJSObjectReference? module = default!;
        private EditContext editContext = default!;

        private bool isSubmitting = false;
        private bool isSendOTP = false;
        private string generatedOtp = string.Empty;
        private string existEmail = String.Empty;

        protected override void OnInitialized()
        {
            editContext = new EditContext(forgotPasswordViewModel);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                "./Components/Pages/Authentication/ForgotPassword.razor.js");
                await module.InvokeVoidAsync("registerBlazorInstance", DotNetObjectReference.Create(this));
            }
        }

        private void ValidateConfirmPassword()
        {
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => forgotPasswordViewModel.ConfirmPassword));
        }

        private bool isEmailNotFound = false;
        private async Task ResetInputEmail(ChangeEventArgs e)
        {
            if (isEmailNotFound)
            {
                await JS.InvokeVoidAsync("resetInputEmail");
                isEmailNotFound = false;
            }
        }

        private async void ResendOTP()
        {
            generatedOtp = new Random().Next(1000, 9999).ToString();

            string subject = "Mã xác nhận OTP của bạn";
            string body = GetOtpEmailTemplate(generatedOtp);

            if (forgotPasswordViewModel.Email == null)
            {
                return;
            }

            await emailService.SendEmailAsync(forgotPasswordViewModel.Email, subject, body);
            await JS.InvokeVoidAsync("showAlertResendOTP");
        }

        private async void SendOtp()
        {
            bool isValid = await module.InvokeAsync<bool>("handleNextClickFirstForm");
            if (isValid)
            {
                if (isSendOTP && existEmail.Equals(forgotPasswordViewModel.Email))
                {
                    await module.InvokeVoidAsync("showOTP");
                }
                else
                {
                    existEmail = forgotPasswordViewModel.Email;
                    var user = await userService.FindUsersAsync(x => x.Email.ToLower()
                        .Equals(forgotPasswordViewModel.Email.ToLower()));

                    if (!user.Any())
                    {
                        await JS.InvokeVoidAsync("displayEmailNotFound");
                        isSubmitting = false;
                        isEmailNotFound = true;
                        return;
                    }

                    generatedOtp = new Random().Next(1000, 9999).ToString();

                    string subject = "Mã xác nhận OTP để khôi phục mật khẩu";
                    string body = GetOtpEmailTemplate(generatedOtp);

                    if (forgotPasswordViewModel.Email == null)
                    {
                        return;
                    }

                    await emailService.SendEmailAsync(forgotPasswordViewModel.Email, subject, body);
                    await module.InvokeVoidAsync("showOTP");
                    isSendOTP = true;
                }
            }
            return;
        }

        [JSInvokable]
        public async Task VerifyOTP(string otpInput)
        {
            Console.WriteLine($"Received OTP from JS: {otpInput}");
            if (otpInput == generatedOtp)
            {
                await JS.InvokeVoidAsync("showVerifySuccessOTP");
            }
            else
            {
                await JS.InvokeVoidAsync("showVerifyFailOTP");
            }
        }

        private async Task HandleResetPassword()
        {
            if (isSubmitting) return;
            isSubmitting = true;

            try
            {
                var user = await userService.GetUserByEmailAsync(forgotPasswordViewModel.Email);

                if (user != null)
                {
                    user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(forgotPasswordViewModel.NewPassword, HashType.SHA256);              
                    await userService.UpdateUserAsync(user);
                 

                    await JS.InvokeVoidAsync("showResetPasswordSuccess");
                }
                else
                {
                    await JS.InvokeVoidAsync("displayEmailNotFound");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting password: {ex.Message}");
                
            }
            finally
            {
                isSubmitting = false;
            }
        }

        private string GetOtpEmailTemplate(string otpCode)
        {
            return $@"
                    <!DOCTYPE html>
                    <html lang='vi'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Xác nhận OTP</title>
                        <style>
                            body {{
                                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                line-height: 1.6;
                                color: #333;
                                max-width: 600px;
                                margin: 0 auto;
                                padding: 0;
                                background-color: #f5f5f5;
                            }}
                            .container {{
                                background-color: #ffffff;
                                border-radius: 8px;
                                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                                overflow: hidden;
                                margin: 20px auto;
                            }}
                            .header {{
                                background-color: #2182cc;
                                color: white;
                                padding: 20px;
                                text-align: center;
                            }}
                            .content {{
                                padding: 30px;
                            }}
                            .otp-container {{
                                background-color: #f8f9fa;
                                border-radius: 4px;
                                padding: 15px;
                                text-align: center;
                                margin: 20px 0;
                                border: 1px dashed #dee2e6;
                            }}
                            .otp-code {{
                                font-size: 28px;
                                font-weight: bold;
                                color: #2182cc;
                                letter-spacing: 3px;
                            }}
                            .footer {{
                                text-align: center;
                                padding: 20px;
                                color: #6c757d;
                                font-size: 12px;
                                border-top: 1px solid #e9ecef;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                background-color: #2182cc;
                                color: white;
                                text-decoration: none;
                                border-radius: 4px;
                                margin-top: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Khôi phục mật khẩu</h1>
                            </div>
            
                            <div class='content'>
                                <p>Xin chào,</p>
                                <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn tại <strong>ChildCareCalendar</strong>. Dưới đây là mã OTP của bạn:</p>
                
                                <div class='otp-container'>
                                    <div class='otp-code'>{otpCode}</div>
                                </div>
                
                                <p>Mã OTP có hiệu lực trong vòng <strong>10 phút</strong>. Vui lòng không chia sẻ mã này với bất kỳ ai.</p>
                                <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này hoặc liên hệ với chúng tôi ngay lập tức.</p>
                
                                <p>Trân trọng,</p>
                                <p><strong>Đội ngũ ChildCareCalendar</strong></p>
                            </div>
            
                            <div class='footer'>
                                <p>© {DateTime.Now.Year} ChildCareCalendar. All rights reserved.</p>
                                <p>Đây là email tự động, vui lòng không trả lời.</p>
                            </div>
                        </div>
                    </body>
                    </html>";
        }
    }
}