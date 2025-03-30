using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    public partial class UserEdit
    {
        private List<string> ErrorMessage = new();
        private IBrowserFile? selectedFile;
        private string? PreviewImageUrl;
        private string? UploadedImageUrl;
        private bool isSubmitting = false;

        [Parameter]
        public int id { get; set; }

        [SupplyParameterFromForm(FormName = "UserUpdate")]
        private UserEditViewModel userEditViewModel { get; set; } = new();

        [Inject] private IUserService userService { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private IMapper mapper { get; set; } = default!;
        [Inject] private CloudinaryService cloudinaryService { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (id != 0 && userEditViewModel.Id == 0)
                {
                    var userEdit = await userService.FindUsersAsync(u => u.Id == id && !u.IsDelete);
                    userEditViewModel = mapper.Map<UserEditViewModel>(userEdit.FirstOrDefault());
                    PreviewImageUrl = userEditViewModel.Avatar;
                }
                if (userEditViewModel.Dob == default)
                {
                    userEditViewModel.Dob = DateTime.Today;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong OnInitializedAsync: {ex.Message}");
            }
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

                userEditViewModel.Avatar = UploadedImageUrl;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.Add($"Lỗi upload ảnh: {ex.Message}");
                return false;
            }
        }
        private async Task HandleUpdate()
        {
            isSubmitting = true;

            if (selectedFile != null && !await UploadImage())
            {
                isSubmitting = false;
                return;
            }

            var userEntity = await userService.FindUsersAsync(u => u.Id == id && !u.IsDelete);
            if (userEntity == null || !userEntity.Any())
            {
                isSubmitting = false;
                ErrorMessage.Add("Người dùng không tồn tại.");
                return;
            }
            var user = userEntity.FirstOrDefault();
            mapper.Map(userEditViewModel, user);
            await userService.UpdateUserAsync(user);
            navigationManager.NavigateTo("/users/index");
        }
    }
}
