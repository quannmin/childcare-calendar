using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.ChildrenRecord;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChildCareCalendar.WebApp.Components.Pages.ChildrenRecords
{
    public partial class Edit
    {
        private bool IsAuthenticated = false;
        private AppUser? Parent;
        private int userIdFromSession;

        [Parameter] 
        public int id { get; set; }
        private ChildrenRecordEditViewModel ChildRecord { get; set; }

        [Inject]
        private IChildrenRecordService ChildrenRecordService { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ProtectedSessionStorage SessionStorage { get; set; } = default!;

        private string ErrorMessage = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var userIdResult = await SessionStorage.GetAsync<int>("userId");
                if (userIdResult.Success)
                {
                    userIdFromSession = userIdResult.Value;

                    Parent = (await UserService.FindUsersAsync(a => a.Id.Equals(userIdFromSession)))?.FirstOrDefault();
                    if (Parent != null && Parent.Role.Equals("PhuHuynh"))
                    {
                        IsAuthenticated = true;

                        var cr = (await ChildrenRecordService.FindUsersAsync(c => c.Id.Equals(id))).FirstOrDefault();

                        ChildRecord = Mapper.Map<ChildrenRecordEditViewModel>(cr);
                    }
                    else
                    {
                        Navigation.NavigateTo("/Login", forceLoad: true);
                    }
                    StateHasChanged();
                }
                else
                {
                    Console.WriteLine("Không lấy được dữ liệu userId từ session.");
                    Navigation.NavigateTo("/Login", forceLoad: true);
                }
            }
        }

        private async Task SaveChanges()
        {
            await ChildrenRecordService.UpdateChildrenRecordAsync(id, ChildRecord);
            Navigation.NavigateTo("/AccountDetail", forceLoad: true);
        }

        private void Cancel()
        {
            Navigation.NavigateTo("/AccountDetail", forceLoad: true);
        }
    }
}