using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Specility;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace ChildCareCalendar.Admin.Components.Pages.Appointment
{
    public partial class Detail
    {
        [Parameter]
        public int id { get; set; }

        private AppointmentDetailViewModel AppointmentViewModel { get; set; } = new();

        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            if (id != 0 && AppointmentViewModel.Id == 0)
            {
                var appointment = await AppointmentService.GetAppointmentByIdAsync(id,
                    a => a.Parent,
                    a => a.ChildrenRecord,
                    a => a.Doctor,
                    a => a.FollowUpAppointment
                    );
                AppointmentViewModel = Mapper.Map<AppointmentDetailViewModel>(appointment);
            }
        }
    }
}