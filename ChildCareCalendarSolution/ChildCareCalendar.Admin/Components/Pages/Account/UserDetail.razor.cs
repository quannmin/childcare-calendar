using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Account
{
    partial class UserDetail
    {
        [Parameter]
        public int id { get; set; }
        private UserViewModel DetailViewModel { get; set; } = new();
        [Inject]
        private IUserService UserService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            
            if (id != 0 && DetailViewModel.Id == 0)
            {
                var users = await UserService.FindUsersAsync(u => u.Id == id && !u.IsDelete);
                DetailViewModel = Mapper.Map<UserViewModel>(users.FirstOrDefault());
            }
        }
    }
}
