using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace ChildCareCalendar.Admin.Components.Pages.Appointment
{
    public partial class Index
    {
        private List<AppointmentViewModel> Appointments = new();
        // {
        //     new AppointmentViewModel { AppointmentId = 1, ParentName = "Nguyễn Văn A", ChildName = "Bé Bảo", ServiceName = "Khám Nhi", TotalAmount = 500000, Status = "Đã check in" },
        //     new AppointmentViewModel { AppointmentId = 2, ParentName = "Trần Thị B", ChildName = "Bé Hà", ServiceName = "Khám Răng", TotalAmount = 300000, Status = "Đã khám" }
        // };

        [Inject]
        IMapper Mapper { get; set; }

        [Inject]
        IAppointmentService AppointmentService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Appointments = Mapper.Map<List<AppointmentViewModel>>(await AppointmentService.GetAllAppointmentsAsync(
                a => a.Parent, a => a.ChildrenRecord, a => a.Service
                ));
        }

        private async Task UpdateStatus(int appointmentId, string newStatus)
        {

        }

    }
}