using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
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
        public UserViewModel userViewModel { get; set; } = new();

        [Inject] private IUserService userService { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private IMapper mapper { get; set; } = default!;
        [Inject] private CloudinaryService cloudinaryService { get; set; } = default!;
        private bool isSubmitting = false;
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

                userViewModel.Avatar = UploadedImageUrl;
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
        private async Task HandleCreateDoctor()
        {
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
            AppUser doctor = mapper.Map<AppUser>(userViewModel);
            if (doctor == null)
            {
                ErrorMessage.Add("Không thể tạo tài khoản bác sĩ.");
                isSubmitting = false;
                return;
            }
            isSubmitting = false;
            await userService.AddUserAsync(doctor);
            navigationManager.NavigateTo("/users/index");
        }
    }
}
