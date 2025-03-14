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
        private IJSObjectReference? module = default!;

        private bool isSubmitting = false;
        private bool isSendOTP = false;


        private EditContext editContext = default!;
        protected override void OnInitialized()
        {
            editContext = new EditContext(compositeViewModel);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JS.InvokeAsync<IJSObjectReference>("import",
                "./Components/Pages/Account/Registration.razor.js");
                await module.InvokeVoidAsync("registerBlazorInstance", DotNetObjectReference.Create(this));
            }
        }

        private void ValidateConfirmPassword()
        {
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => compositeViewModel._userCreateViewModel.ConfirmPassword));
        }
        private bool isDuplicatedEmail = false;
        private async Task ResetInputEmail(ChangeEventArgs e)
        {
            if (isDuplicatedEmail)
            {
                await JS.InvokeVoidAsync("resetInputEmail");
                isDuplicatedEmail = false;
            }
        }
        private string generatedOtp = string.Empty;
        private async void ResendOTP()
        {
            generatedOtp = new Random().Next(1000, 9999).ToString();

            string subject = "Mã xác nhận OTP của bạn";
            string body = $"Mã OTP của bạn là: <b>{generatedOtp}</b>. Vui lòng nhập mã này để xác nhận.";
            if (compositeViewModel._userCreateViewModel.Email == null)
            {
                return;
            }
            await emailService.SendEmailAsync(compositeViewModel._userCreateViewModel.Email, subject, body);
            await JS.InvokeVoidAsync("showAlertResendOTP");
        }
        private string existEmail = String.Empty;
        private async void SendOtp()
        {
            bool isValid = await module.InvokeAsync<bool>("handleNextClickFirstForm");
            if (isValid)
            {
                if (isSendOTP && existEmail.Equals(compositeViewModel._userCreateViewModel.Email))
                {
                    await module.InvokeVoidAsync("showOTP");
                }
                else
                {
                    existEmail = compositeViewModel._userCreateViewModel.Email;
                    var checkDuplicateEmail = await userService.FindUsersAsync(x => x.Email.ToLower()
                .Equals(compositeViewModel._userCreateViewModel.Email.ToLower()));

                    if (checkDuplicateEmail.Any())
                    {
                        await JS.InvokeVoidAsync("displayEmailDuplicate");
                        isSubmitting = false;
                        isDuplicatedEmail = true;
                        return;
                    }
                    generatedOtp = new Random().Next(1000, 9999).ToString();

                    string subject = "Mã xác nhận OTP của bạn";
                    string body = $"Mã OTP của bạn là: <b>{generatedOtp}</b>. Vui lòng nhập mã này để xác nhận.";
                    if (compositeViewModel._userCreateViewModel.Email == null)
                    {
                        return;
                    }
                    await emailService.SendEmailAsync(compositeViewModel._userCreateViewModel.Email, subject, body);
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
    }
}
