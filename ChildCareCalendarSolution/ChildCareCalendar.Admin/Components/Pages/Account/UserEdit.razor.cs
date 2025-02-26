using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

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
                    var userEdit = await userService.GetUserByIdAsync(id);
                    userEditViewModel = mapper.Map<UserEditViewModel>(userEdit);
                    PreviewImageUrl = userEdit.Avatar;
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

            var userEntity = await userService.GetUserByIdAsync(userEditViewModel.Id);
            if (userEntity == null)
            {
                isSubmitting = false;
                ErrorMessage.Add("Người dùng không tồn tại.");
                return;
            }

            mapper.Map(userEditViewModel, userEntity);
            await userService.UpdateUserAsync(userEntity);
            navigationManager.NavigateTo("/users/index");
        }


    }
}
        //< div class= "col-md-6 p-2" >
        //    < div class= "text-center w-100" >
        //        < label class= "w-100 text-start" > Ảnh người dùng</label>
        //    </div>
        //    <div class= "col-md-12" >
        //        < div class= "input-group w-100" >
        //            < label class= "btn btn-primary w-100" >
        //                < i class= "bi bi-upload" ></ i > Chọn ảnh
        //                <InputFile OnChange = "HandleFileSelection" accept="image/*" class= "d-none" />
        //            </ label >
        //        </ div >
        //    </ div >
        //    @if(!string.IsNullOrEmpty(PreviewImageUrl))
        //    {
        //        < div class= "form-group m-2 text-center" style = "margin-left: 19px" >
        //            < p class= "fw-bold" > Ảnh xem trước:</ p >
        //            < img src = "@PreviewImageUrl" alt = "Preview Image" class= "img-thumbnail shadow" style = "width: 250px; height: 250px; object-fit: cover; border-radius: 10px;" />
        //        </ div >
        //    }
        //</ div >