using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        private IChildrenRecordService ChildRecordService { get; set; } = default!;

        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; }

        private int? idToDelete;
        private int? childIdToDelete;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadUserAsync();
                await LoadAppointmentsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải thông tin Parent: {ex.Message}");
            }
        }

        private async Task LoadUserAsync()
        {
            Parent = (await UserService.FindUsersAsync(a => a.Id.Equals(6), a => a.ParentAppointments, a => a.ChildrenRecords))
                                        ?.FirstOrDefault();
            ChildRecords = (await ChildRecordService.FindUsersAsync(c => !c.IsDelete && c.UserId.Equals(6))).ToList();
        }

        private async Task LoadAppointmentsAsync()
        {
            Appointments = Mapper.Map<List<AppointmentViewModel>>(await AppointmentService.FindAppointmentsAsync(a => !a.IsDelete && a.ParentId.Equals(6),
                            a => a.Parent, a => a.ChildrenRecord, a => a.Service, a => a.Doctor
                            ));
            Appointments.Reverse();
        }

        private async void ConfirmDelete(int id)
        {
            idToDelete = id;
            await JS.InvokeVoidAsync("showDeleteModal");
        }

        private async Task DeleteAppointment()
        {
            if (idToDelete.HasValue)
            {
                await AppointmentService.CancelAppointmentAsync(idToDelete.Value);
                await AppointmentService.ChangeAppointmentStatusAsync(idToDelete.Value, "Cancelled");
                idToDelete = null;
                await LoadAppointmentsAsync();
            }
            await JS.InvokeVoidAsync("hideDeleteModal");
        }

        private async void ConfirmDeleteChild(int childId)
        {
            childIdToDelete = childId;
            await JS.InvokeVoidAsync("showDeleteChildModal");
        }

        private async Task DeleteChildRecord()
        {
            if (childIdToDelete.HasValue)
            {
                await ChildRecordService.DeleteChildrenRecordAsync(childIdToDelete.Value);
                childIdToDelete = null;
                await LoadUserAsync();
            }
            await JS.InvokeVoidAsync("hideDeleteChildModal");
        }

    }
}