using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Constants;
using ChildCareCalendar.Utilities.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class UserCreate
    {
        private List<string> ErrorMessage = new();
        private IBrowserFile? selectedFile;
        private string? PreviewImageUrl;
        private string? UploadedImageUrl;

        [SupplyParameterFromForm]
        public UserCreateViewModel userCreateViewModel { get; set; } = new();

        [Inject] private IUserService userService { get; set; } = default!;
        [Inject] private ISpecialityService specialityService { get; set; } = default!;
        [Inject] private NavigationManager navigationManager { get; set; } = default!;
        [Inject] private IMapper mapper { get; set; } = default!;
        [Inject] private CloudinaryService cloudinaryService { get; set; } = default!;
        private EditContext editContext = default!;
        private bool isSubmitting = false;
        private bool showSpecialties = false;
        private List<SpecialityViewModel> specialtiesViewModel = new();
        [Inject]
        private IJSRuntime JS { get; set; } = default!;
        private async Task OnRoleChanged(SystemConstant.AccountsRole newRole)
        {
            userCreateViewModel.Role = newRole; // Update the bound model

            if (newRole == SystemConstant.AccountsRole.BacSi)
            {
                var specialties = await specialityService.FindSpecialitiesAsync(x => !x.IsDelete);
                specialtiesViewModel = mapper.Map<List<SpecialityViewModel>>(specialties);
                showSpecialties = true;
            }
            else
            {
                showSpecialties = false;
                specialtiesViewModel.Clear();
            }

            StateHasChanged(); // Ensure the UI updates
        }

        protected override void OnInitialized()
        {
            editContext = new EditContext(userCreateViewModel);
        }
        private void ValidateConfirmPassword()
        {
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => userCreateViewModel.ConfirmPassword));
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

                userCreateViewModel.Avatar = UploadedImageUrl;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage.Add($"Lỗi upload ảnh: {ex.Message}");
                return false;
            }
        }
        private bool isDuplicatedEmail = false;
        private async Task ResetInputText(ChangeEventArgs e)
        {
            if (isDuplicatedEmail)  // 🔴 If previously marked as duplicate
            {
                await JS.InvokeVoidAsync("resetInputEmail");  // Remove red border + error
                isDuplicatedEmail = false;  // ✅ Reset the flag
            }
        }

        /// <summary>
        /// Xử lý tạo tài khoản
        /// </summary>
        private async Task HandleCreateDoctor()
        {
            if (isSubmitting) return;
            isSubmitting = true;
            isDuplicatedEmail = false;

            ErrorMessage.Clear();

            // Upload ảnh nếu có file được chọn
            if (selectedFile != null && !await UploadImage())
            {
                isSubmitting = false;
                return;
            }

            var checkDuplicateEmail = await userService.FindUsersAsync(x => x.Email.ToLower().Equals(userCreateViewModel.Email.ToLower()));

            if (checkDuplicateEmail.Any())
            {
                await JS.InvokeVoidAsync("displayEmailDuplicate");
                isSubmitting = false;
                isDuplicatedEmail = true;
                return;
            }

            // Mapping dữ liệu

            //if (userCreateViewModel.SpecialityId == 0) userCreateViewModel.SpecialityId = null;
            AppUser doctor = mapper.Map<AppUser>(userCreateViewModel);
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
