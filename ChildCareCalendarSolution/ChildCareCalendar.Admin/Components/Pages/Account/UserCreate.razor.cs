using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class UserCreate
    {
        private List<String> ErrorMessage = new List<string>();

        [SupplyParameterFromForm]
        public UserCreateViewModel userCreateViewModel { get; set; }

        [Inject]
        IUserService userService { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }

        [Inject]
        IMapper mapper { get; set; }

		protected override void OnInitialized()
		{
			userCreateViewModel ??= new();
		}

		public async Task HandleCreateDoctor()
        {
            AppUser doctor = mapper.Map<AppUser>(userCreateViewModel);
            if (doctor == null)
            {
                ErrorMessage.Add("Doctor tìm không thấy");
                StateHasChanged(); // Cập nhật giao diện
                return;
            }
            await userService.AddUserAsync(doctor);
            navigationManager.NavigateTo("/");
        }
    }
}
