using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;

namespace ChildCareCalendar.WebApp.Components.Pages.UserDetail
{
    public partial class AccountDetail
    {
        private AppUser? Parent;
        private List<ChildrenRecord>? ChildRecords;
        private List<AppointmentViewModel>? Appointments;

        [Inject]
        IMapper Mapper { get; set; }

        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Parent = (await UserService.FindUsersAsync(a => a.Id.Equals(6), a => a.ParentAppointments, a => a.ChildrenRecords))
                            ?.FirstOrDefault();
                Appointments = Mapper.Map<List<AppointmentViewModel>>(await AppointmentService.FindAppointmentsAsync(a => !a.IsDelete && a.ParentId.Equals(6),
                            a => a.Parent, a => a.ChildrenRecord, a => a.Service, a => a.Doctor
                            ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải thông tin Parent: {ex.Message}");
            }
        }
    }
}