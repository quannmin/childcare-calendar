using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.CompositeViewModels;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class Registration
    {
        private List<string> ErrorMessage = new();
        private IBrowserFile? selectedFile;
        private string? PreviewImageUrl;
        private string? UploadedImageUrl;
        [SupplyParameterFromForm]
        public User_ChildrenCreateViewModel compositeViewModel { get; set; } = new();
        [Inject] private IUserService userService { get; set; } = default!;
        [Inject] private IChildrenRecordService childrenRecordService { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private IMapper mapper { get; set; } = default!;
        [Inject] private CloudinaryService cloudinaryService { get; set; } = default!;
        private bool isSubmitting = false;
        private EditContext editContext;
        protected override void OnInitialized()
        {
            editContext = new EditContext(compositeViewModel);
        }
        private void ValidateConfirmPassword()
        {
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => compositeViewModel._userCreateViewModel.ConfirmPassword));
        }
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
