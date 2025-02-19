using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class UserCreate
    {
        private List<string> ErrorMessage = new List<string>();
        private IBrowserFile? selectedFile; // Lưu file khi chọn
        private byte[]? selectedFileBytes; // Lưu dữ liệu file
        private string? UploadedImageUrl;  // Lưu link ảnh sau khi upload

        [SupplyParameterFromForm]
        public UserCreateViewModel userCreateViewModel { get; set; } = new();

        [Inject]
        IUserService userService { get; set; } = default!;

        [Inject]
        NavigationManager navigationManager { get; set; } = default!;

        [Inject]
        IMapper mapper { get; set; } = default!;

        [Inject]
        CloudinaryService cloudinaryService { get; set; } = default!;

        private async Task HandleCreateDoctor()
        {
            ErrorMessage.Clear(); // Reset lỗi

            // Kiểm tra nếu có file mới upload
            if (selectedFile != null)
            {
                try
                {
                    long maxFileSize = 5 * 1024 * 1024; // 5MB
                    using var stream = selectedFile.OpenReadStream(maxFileSize);
                    UploadedImageUrl = await cloudinaryService.UploadImageAsync(stream, selectedFile.Name);

                    if (string.IsNullOrEmpty(UploadedImageUrl))
                    {
                        ErrorMessage.Add("Lỗi upload ảnh, vui lòng thử lại.");
                        return;
                    }

                    userCreateViewModel.ProfilePictureUrl = UploadedImageUrl;
                }
                catch (Exception ex)
                {
                    ErrorMessage.Add($"Lỗi upload ảnh: {ex.Message}");
                    return;
                }
            }

            // Chuyển đổi ViewModel sang Entity
            AppUser doctor = mapper.Map<AppUser>(userCreateViewModel);
            if (doctor == null)
            {
                ErrorMessage.Add("Không thể tạo tài khoản bác sĩ.");
                return;
            }

            await userService.AddUserAsync(doctor);
            navigationManager.NavigateTo("/");
        }

        private async Task HandleFileSelection(InputFileChangeEventArgs e)
        {
            long maxFileSize = 5 * 1024 * 1024; // 5MB
            selectedFile = e.File;

            using var stream = selectedFile.OpenReadStream(maxFileSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            selectedFileBytes = memoryStream.ToArray();
            Console.WriteLine($"📁 File đã chọn: {selectedFile.Name}, Size: {selectedFile.Size} bytes");
        }
    }
}
