using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using ChildCareCalendar.Infrastructure.Services.Interfaces;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SpecialtyRoute
    {
        private bool isLoading = true;
        private bool IsAuthenticated = false;
        private AppUser? Parent;
        private int UserId;

        private int? selectedSpecialtyId;
        private string? selectedSpecialtyName;
        private int? selectedServiceId;
        private string? selectedServiceName;

        [Inject] private ProtectedSessionStorage SessionStorage { get; set; } = default!;

        [Inject] private NavigationManager Navigation { get; set; } = default!;

        [Inject] private IUserService UserService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var userIdResult = await SessionStorage.GetAsync<int>("userId");
                if (userIdResult.Success)
                {
                    UserId = userIdResult.Value;

                    Parent = (await UserService.FindUsersAsync(a => a.Id.Equals(UserId)))?.FirstOrDefault();
                    if (Parent != null && Parent.Role.Equals("PhuHuynh"))
                    {
                        IsAuthenticated = true;
                        isLoading = false;
                    }
                    else
                    {
                        Navigation.NavigateTo("/Login", forceLoad: true);
                    }
                    StateHasChanged();
                }
                else
                {
                    Navigation.NavigateTo("/Login", forceLoad: true);
                }
                StateHasChanged();
            }
            catch (Exception)
            {
                IsAuthenticated = false;
            }
        }
        
        private void HandleSpecialtySelection(Speciality speciality)
        {
            selectedSpecialtyId = speciality.Id;
            selectedSpecialtyName = speciality.SpecialtyName;
            StateHasChanged();
        }
        private void HandleServiceSelection(Service service)
        {
            selectedServiceId = service.Id;
            selectedServiceName = service.ServiceName;
            StateHasChanged();
        }
        void GoBackToSpecialtySelection()
        {
            selectedSpecialtyId = null;
            selectedSpecialtyName = string.Empty;
        }

        void GoBackToServiceSelection()
        {
            selectedServiceId = null;
            selectedServiceName = string.Empty;
        }
    }
}