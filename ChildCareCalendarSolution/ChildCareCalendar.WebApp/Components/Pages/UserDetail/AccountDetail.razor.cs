using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using static System.Net.WebRequestMethods;

namespace ChildCareCalendar.WebApp.Components.Pages.UserDetail
{
    public partial class AccountDetail
    {
        private bool IsAuthenticated = false;
        private AppUser? Parent;
        private int userIdFromSession;
        private List<ChildrenRecord>? ChildRecords;
        private List<AppointmentViewModel>? Appointments;
        private AppUser editedUser = new AppUser();


        [Inject]
        IMapper Mapper { get; set; }

        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IChildrenRecordService ChildRecordService { get; set; } = default!;
        [Inject]
        private IScheduleService ScheduleService { get; set; } = default!;


        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;

        [Inject]
        private ProtectedSessionStorage SessionStorage { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation {  get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; }

        private int? idToDelete;
        private int? childIdToDelete;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
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
                            await LoadChildrenRecordAsync();
                            await LoadAppointmentsAsync();
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnAfterRenderAsync: {ex.Message}");
            }
        }
        private async Task LoadChildrenRecordAsync()
        {
            ChildRecords = (await ChildRecordService.FindChildrenRecordsAsync(c => !c.IsDelete && c.UserId.Equals(Parent.Id))).ToList();
        }

        private async Task LoadAppointmentsAsync()
        {
            Appointments = Mapper.Map<List<AppointmentViewModel>>(await AppointmentService.FindAppointmentsAsync(a => !a.IsDelete && a.ParentId.Equals(Parent.Id),
                            a => a.Parent, a => a.ChildrenRecord, a => a.Service, a => a.Doctor
                            ));
            Appointments.Reverse();
        }

        private async Task ConfirmDelete(int id)
        {
            idToDelete = id;
            await JS.InvokeVoidAsync("showDeleteModal");
        }

        private async Task DeleteAppointment()
        {
            if (idToDelete.HasValue)
            {
                var appointment = await AppointmentService.GetAppointmentByIdAsync(idToDelete.Value);

                if (appointment != null)
                {
                    // 1. Cập nhật trạng thái
                    await AppointmentService.CancelAppointmentAsync(idToDelete.Value);
                    await AppointmentService.ChangeAppointmentStatusAsync(idToDelete.Value, "Cancelled");

                    // 2. Giải phóng slot nếu ScheduleId hợp lệ
                    if (appointment.ScheduleId > 0)
                    {
                        Console.WriteLine("ScheduleId: " + appointment.ScheduleId);
                        var schedule = await ScheduleService.GetScheduleByIdAsync(appointment.ScheduleId);
                        Console.WriteLine("Schedule null? " + (schedule == null));
                        if (schedule != null)
                        {
                            schedule.IsOccupied = false;
                            await ScheduleService.UpdateScheduleAsync(schedule);
                        }
                    }

                    idToDelete = null;
                    await LoadAppointmentsAsync();
                }
            }

            await JS.InvokeVoidAsync("hideDeleteModal");
        }


        private async Task ConfirmDeleteChild(int childId)
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
                await LoadChildrenRecordAsync();
            }
            await JS.InvokeVoidAsync("hideDeleteChildModal");
        }

        private async Task ShowEditProfileModal()
        {
            // Create a copy of the Parent user to avoid changing the original until save
            editedUser = new AppUser
            {
                Id = Parent.Id,
                FullName = Parent.FullName,
                PhoneNumber = Parent.PhoneNumber,
                Address = Parent.Address,
                Dob = Parent.Dob,
                Gender = Parent.Gender,
                // Copy only the editable properties
            };

            await JS.InvokeVoidAsync("showEditProfileModal");
        }

        private async Task SaveUserProfile()
        {
            try
            {
                // Update only the editable fields
                Parent.FullName = editedUser.FullName;
                Parent.PhoneNumber = editedUser.PhoneNumber;
                Parent.Address = editedUser.Address;
                Parent.Dob = editedUser.Dob;
                Parent.Gender = editedUser.Gender;

                // Save changes to the database
                await UserService.UpdateUserAsync(Parent);

                // Close the modal
                await JS.InvokeVoidAsync("hideEditProfileModal");
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine($"Error updating user profile: {ex.Message}");
            }
        }
    }
}