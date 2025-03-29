using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.WebApp.Components.Pages.Doctors
{
    public partial class DoctorDetail
    {
        [Parameter] public int DoctorId { get; set; }

        private AppUser doctor;
        private List<AppUser> relatedDoctors = new();

        [Inject] private IUserService UserService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            await LoadDoctorData();
            StateHasChanged();
        }

        private async Task LoadDoctorData()
        {
            doctor = await UserService.FindUserAsync(
                u => u.Id == DoctorId,
                u => u.Speciality
            );

            if (doctor?.SpecialityId != null)
            {
                relatedDoctors = (await UserService.FindUsersAsync(
                    u => u.SpecialityId == doctor.SpecialityId && u.Id != DoctorId,
                    u => u.Speciality
                )).ToList();
            }
        }
    }
}
