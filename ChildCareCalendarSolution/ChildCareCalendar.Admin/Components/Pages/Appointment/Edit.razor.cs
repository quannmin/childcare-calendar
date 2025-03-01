using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Appointment
{
    public partial class Edit
    {
        [Parameter]
        public int id { get; set; }

        public List<AppUser> Doctors = new();

        [SupplyParameterFromForm(FormName = "AppointmentUpdate")]
        private AppointmentEditViewModel EditModel { get; set; } = new();

        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private string ErrorMessage = "";


        protected override async Task OnInitializedAsync()
        {
            Doctors = (await UserService.FindUsersAsync(u => u.Role.Equals("Doctor"))).ToList();
            if (!id.Equals(0) && EditModel.Id.Equals(0))
            {
                var appointment = await AppointmentService.GetAppointmentByIdAsync(id);
                EditModel = Mapper.Map<AppointmentEditViewModel>(appointment);
            }
        }

        private async Task HandleUpdate()
        {
            ErrorMessage = "";

            var exist = await AppointmentService.GetAppointmentByIdAsync(EditModel.Id);
            if (exist != null)
            {
                exist.DoctorId = EditModel.DoctorId;
                exist.CheckupDateTime = EditModel.CheckupDateTime;
                exist.Status = EditModel.Status;
                exist.UpdatedAt = DateTime.Now;
            }
            await AppointmentService.UpdateAppointmentAsync(exist);
            Navigation.NavigateTo("/appointments");
        }
    }
}