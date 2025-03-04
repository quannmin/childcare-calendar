using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.WebApp.Components.Pages.Appointment
{
    public partial class Create
    {
        public AppUser Parent { get; set; }
        public List<AppUser> Doctors = new();
        public List<ChildrenRecord> ChildrenRecords = new();
        public List<ChildCareCalendar.Domain.Entities.Service> Services = new();
        public List<Speciality> Specialities = new();
        public List<Schedule> Schedules = new();

        [SupplyParameterFromForm(FormName = "AppointmentCreate")]
        private AppointmentCreateVM CreateModel { get; set; } = new();

        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;

        [Inject]
        private IChildrenRecordService ChildrenRecordService { get; set; } = default!;

        [Inject]
        private IServiceService ServiceService { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private ISpecialityService SpecialityService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private string ErrorMessage = "";

        protected override async Task OnInitializedAsync()
        {
            Specialities = (await SpecialityService.FindSpecialitiesAsync(s => s.IsDelete.Equals(false))).ToList();
            //Doctors = (await UserService.FindUsersAsync(u => u.Role.Equals("Doctor") && u.IsDelete.Equals(false))).ToList();
        }

        private async Task LoadDoctors(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int specialityid))
            {
                try
                {
                    //Doctors = (await UserService.FindUsersAsync(cr => )).ToList();
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi tải Doctors: {ex.Message}");
                }
            }
        }
    }
}