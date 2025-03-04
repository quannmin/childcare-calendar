using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.CompositeViewModels;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class Registration
    {
        private List<string> ErrorMessage = new();

        [SupplyParameterFromForm]
        public User_ChildrenCreateViewModel compositeViewModel { get; set; } = new();
        [Inject] private IUserService userService { get; set; } = default!;
        [Inject] private IChildrenRecordService childrenRecordService { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private IMapper mapper { get; set; } = default!;
        [Inject] private CloudinaryService cloudinaryService { get; set; } = default!;
        [Inject] private IEmailService emailService { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;
        private DotNetObjectReference<Registration> objRef;
        private bool _jsInitialized;

        private bool isSubmitting = false;
        private EditContext editContext;
        protected override void OnInitialized()
        {
            editContext = new EditContext(compositeViewModel);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_jsInitialized)
            {
                _jsInitialized = true; // Prevent multiple calls

                objRef = DotNetObjectReference.Create(this);

                try
                {
                    await JS.InvokeVoidAsync("GLOBAL.SetDotnetReference", objRef);
                }
                catch (JSDisconnectedException)
                {
                    Console.WriteLine("Blazor JS runtime disconnected.");
                }

                // Ensure JS has DotNetReference before triggering JS event bindings
                await Task.Delay(100);

                try
                {
                    await JS.InvokeVoidAsync("registerFormEvents");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error invoking JS function: {ex.Message}");
                }
            }
        }

        // Dispose to prevent memory leaks
        public void Dispose()
        {
            objRef?.Dispose();
        }

        private void ValidateConfirmPassword()
        {
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => compositeViewModel._userCreateViewModel.ConfirmPassword));
        }

        private IBrowserFile? selectedFile;
        private string? PreviewImageUrl;
        private string? UploadedImageUrl;
        private async Task HandleFileSelection(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
            if (selectedFile == null)
            {
                PreviewImageUrl = null;
                return;
            }

            // Đọc file và hiển thị ảnh xem trước
            using var stream = selectedFile.OpenReadStream(5 * 1024 * 1024);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            PreviewImageUrl = $"data:{selectedFile.ContentType};base64,{Convert.ToBase64String(imageBytes)}";
        }

        /// <summary>
        /// Xử lý upload ảnh lên Cloudinary
        /// </summary>
        private async Task<bool> UploadImage()
        {
            if (selectedFile == null) return false;

            try
            {
                using var stream = selectedFile.OpenReadStream(5 * 1024 * 1024);
                UploadedImageUrl = await cloudinaryService.UploadImageAsync(stream, selectedFile.Name);

                if (string.IsNullOrEmpty(UploadedImageUrl))
                {
                    ErrorMessage.Add("Lỗi upload ảnh, vui lòng thử lại.");
                    return false;
                }

                compositeViewModel._userCreateViewModel.Avatar = UploadedImageUrl;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.Add($"Lỗi upload ảnh: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Xử lý tạo tài khoản
        /// </summary>
        private async Task HandleCreateUser()
        {
            compositeViewModel._userCreateViewModel.Role = Utilities.Constants.SystemConstant.AccountsRole.PhuHuynh;

            if (isSubmitting) return;
            isSubmitting = true;

            ErrorMessage.Clear();

            // Upload ảnh nếu có file được chọn
            if (selectedFile != null && !await UploadImage())
            {
                isSubmitting = false;
                return;
            }

            // Mapping dữ liệu
            AppUser user = mapper.Map<AppUser>(compositeViewModel._userCreateViewModel);
            if (user == null)
            {
                ErrorMessage.Add("Không thể tạo tài khoản phụ huynh.");
                isSubmitting = false;
                return;
            }

            await userService.AddUserAsync(user);

            ChildrenRecord childrenRecord = mapper.Map<ChildrenRecord>(compositeViewModel._childCreateViewModel);
            if (childrenRecord == null)
            {
                ErrorMessage.Add("Không thể tạo hồ sơ cho trẻ.");
                isSubmitting = false;
                return;
            }
            childrenRecord.UserId = user.Id;

            isSubmitting = false;

            await childrenRecordService.AddChildrenRecordAsync(childrenRecord);

            navigationManager.NavigateTo("/users/index");
        }

        private string generatedOtp = string.Empty;
        private string otpInput = string.Empty;
        private bool isOtpSent = false;
        private bool isOtpConfirmed = false;
        private void VerifyOtp()
        {
            if (otpInput == generatedOtp)
            {
                isOtpConfirmed = true;
                ErrorMessage.Clear();
            }
            else
            {
                ErrorMessage.Add("Mã OTP không chính xác, vui lòng thử lại!");
            }
        }

        private async Task SendOtp()
        {
            if (string.IsNullOrEmpty(compositeViewModel._userCreateViewModel.Email))
            {
                ErrorMessage.Add("Vui lòng nhập email!");
                return;
            }

            generatedOtp = new Random().Next(1000, 9999).ToString();

            string subject = "Mã xác nhận OTP của bạn";
            string body = $"Mã OTP của bạn là: <b>{generatedOtp}</b>. Vui lòng nhập mã này để xác nhận.";

            await emailService.SendEmailAsync(compositeViewModel._userCreateViewModel.Email, subject, body);
            isOtpSent = true;

            // Call JavaScript function after OTP is sent
            await JS.InvokeVoidAsync("otpSentSuccess");
        }

        [JSInvokable("HandleNextClick")]
        public async Task HandleNextClick()
        {
            await SendOtp();
        }
    }
}
