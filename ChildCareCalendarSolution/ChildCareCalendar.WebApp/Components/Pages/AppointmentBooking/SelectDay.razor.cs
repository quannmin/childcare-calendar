using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Globalization;


namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectDay
    {
        [Parameter] public EventCallback<DateTime> OnDateSelected { get; set; }
        [Parameter] public int? DoctorId { get; set; }
        [Inject] public IScheduleService ScheduleService { get; set; } = default!;

        private DateTime currentMonth = DateTime.Today;
        private List<DateTime> DaysInMonth = new();
        private List<DateTime> AvailableDates = new();
        private bool isPrevDisabled = true;

        protected override async Task OnParametersSetAsync()
        {
            await LoadAvailableDates();
            GenerateCalendar();
        }

        private async Task LoadAvailableDates()
        {
            if (DoctorId.HasValue)
            {
                var schedules = await ScheduleService.FindSchedulesAsync(s => s.UserId == DoctorId && s.WorkDay >= DateTime.Today);
                AvailableDates = schedules.Select(s => s.WorkDay!.Value.Date).Distinct().ToList();
            }
        }

        private void GenerateCalendar()
        {
            DaysInMonth.Clear();
            var firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            for (int i = 0; i < firstDayOfWeek; i++)
                DaysInMonth.Add(DateTime.MinValue);

            for (int day = 1; day <= daysInMonth; day++)
            {
                DaysInMonth.Add(new DateTime(currentMonth.Year, currentMonth.Month, day));
            }
        }

        private void PrevMonth()
        {
            if (currentMonth > DateTime.Today.AddMonths(-1))
            {
                currentMonth = currentMonth.AddMonths(-1);
                GenerateCalendar();
            }
        }

        private void NextMonth()
        {
            currentMonth = currentMonth.AddMonths(1);
            GenerateCalendar();
        }

        private async Task SelectDate(DateTime date)
        {
            if (AvailableDates.Contains(date))
            {
                await OnDateSelected.InvokeAsync(date);
            }
        }

    }
}
