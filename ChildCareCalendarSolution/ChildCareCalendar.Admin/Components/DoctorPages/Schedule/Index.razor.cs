using AutoMapper;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Domain.ViewModels.Schedule;
using ChildCareCalendar.Domain.ViewModels.WorkHour;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Globalization;
using System.Web;
using static ChildCareCalendar.Utilities.Constants.SystemConstant;

namespace ChildCareCalendar.Admin.Components.DoctorPages.Schedule
{
    public partial class Index : ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Parameter]
        public int doctorId { get; set; } = default!;

        [Inject]
        private IScheduleService ScheduleService { get; set; } = default!;

        [Inject]
        private IWorkHourService WorkHourService { get; set; } = default!;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IAppointmentService AppointmentService { get; set; } = default!;

        [Inject]
        private IExaminationReportService ExaminationReportService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private List<ScheduleViewModel> Schedules = new();
        private List<WorkHourViewModel> WorkHours = new();
        private List<AppointmentViewModel> SelectedAppointments = new();
        private UserViewModel? Doctor;
        private Dictionary<int, int?> AppointmentReportIds = new();

        protected DateTime currentDate = DateTime.Today;
        private static readonly string[] dayNames = { "CN", "T2", "T3", "T4", "T5", "T6", "T7" };

        private bool isModalOpen = false;
        private DateTime selectedDate;
        private WorkHourViewModel? selectedWorkHour;
        private string modalTitle = "";

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var doctorClaim = user.FindFirst("Id");
                if (doctorClaim != null && int.TryParse(doctorClaim.Value, out int id))
                {
                    doctorId = id;
                }
            }
            await LoadData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("eval", @"
                    window.showAppointmentModal = function() {
                        var modalElement = document.getElementById('appointmentModal');
                        if (!modalElement) {
                            console.error('Modal element not found');
                            return;
                        }
                        var modal = new bootstrap.Modal(modalElement);
                        modal.show();
                    };

                    window.hideAppointmentModal = function() {
                        var modalElement = document.getElementById('appointmentModal');
                        if (!modalElement) {
                            console.error('Modal element not found');
                            return;
                        }
                        var modal = bootstrap.Modal.getInstance(modalElement);
                        if (modal) {
                            modal.hide();
                        } else {
                            console.error('Modal instance not found');
                        }
                    };
                ");
            }
        }

        private async Task LoadData()
        {
            var scheduleList = await ScheduleService.FindSchedulesAsync(
                x => !x.IsDelete && x.UserId == doctorId,
                x => x.WorkHour, x => x.Doctor);
            Schedules = Mapper.Map<List<ScheduleViewModel>>(scheduleList);

            var workHourList = await WorkHourService.FindWorkHoursAsync(x => !x.IsDelete);
            WorkHours = Mapper.Map<List<WorkHourViewModel>>(workHourList);

            var doctorData = await UserService.FindUsersAsync(x => !x.IsDelete && x.Id == doctorId);
            Doctor = Mapper.Map<List<UserViewModel>>(doctorData).FirstOrDefault();
        }

        protected List<CalendarDay> DaysInMonth => GetDaysInMonth(currentDate);

        private List<CalendarDay> GetDaysInMonth(DateTime date)
        {
            var days = new List<CalendarDay>();
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            DateTime firstDay = new DateTime(date.Year, date.Month, 1);
            int startingDay = (int)firstDay.DayOfWeek;

            for (int i = 0; i < startingDay; i++)
            {
                days.Add(new CalendarDay());
            }

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime cellDate = new DateTime(date.Year, date.Month, day);
                var daySchedules = Schedules
                    .Where(s => s.WorkDay.HasValue && s.WorkDay.Value.Date == cellDate.Date)
                    .ToList();
                days.Add(new CalendarDay { Day = day, Schedules = daySchedules });
            }
            return days;
        }

        protected void PrevMonth()
        {
            currentDate = currentDate.AddMonths(-1);
            StateHasChanged();
        }

        protected void NextMonth()
        {
            currentDate = currentDate.AddMonths(1);
            StateHasChanged();
        }

        private async Task ShowAppointmentsAsync(DateTime date, WorkHourViewModel workHour)
        {
            try
            {
                Console.WriteLine($"ShowAppointments called for {date.ToShortDateString()} {workHour.StartTime}-{workHour.EndTime}");

                selectedDate = date;
                selectedWorkHour = workHour;

                // Format date and time for the modal title
                string formattedDate = date.ToString("dd/MM/yyyy");
                string timeRange = $"{workHour.StartTime.ToString(@"hh\:mm")} - {workHour.EndTime.ToString(@"hh\:mm")}";
                modalTitle = $"Lịch khám ngày {formattedDate}, {timeRange}";

                // Load appointments for this date and work hour
                var appointments = await AppointmentService.GetAppointmentsByDoctorIdAndDateTimeAsync(
                    doctorId,
                    date,
                    workHour.StartTime,
                    workHour.EndTime);

                SelectedAppointments = Mapper.Map<List<AppointmentViewModel>>(appointments);

                // Load examination report IDs for appointments
                AppointmentReportIds = new Dictionary<int, int?>();
                foreach (var appointment in SelectedAppointments)
                {
                    if (appointment.Status.ToLower() == AppointmentStatus.Completed.ToString().ToLower())
                    {
                        AppointmentReportIds[appointment.AppointmentId] = await GetExaminationReportId(appointment.AppointmentId);
                    }
                }

                StateHasChanged();

                // Open the modal
                isModalOpen = true;
                await JS.InvokeVoidAsync("showAppointmentModal");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAppointmentsAsync: {ex.Message}");
                // Log or handle the exception appropriately
            }
        }

        private async Task<int?> GetExaminationReportId(int appointmentId)
        {
            return await ExaminationReportService.GetFirstExaminationReportIdByAppointmentIdAsync(appointmentId);
        }

        private async Task CloseModalAsync()
        {
            try
            {
                isModalOpen = false;
                await JS.InvokeVoidAsync("hideAppointmentModal");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CloseModalAsync: {ex.Message}");
                // Log or handle the exception appropriately
            }
        }
    }

    public class CalendarDay
    {
        public int Day { get; set; } = 0;
        public List<ScheduleViewModel> Schedules { get; set; } = new();
    }
}