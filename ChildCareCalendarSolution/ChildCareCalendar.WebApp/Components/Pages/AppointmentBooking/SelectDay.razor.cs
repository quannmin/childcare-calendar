using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectDay
    {
        [Parameter]
        public EventCallback<DateTime> OnDateSelected { get; set; }

        private DateTime currentMonth = DateTime.Today;
        private List<DateTime?> DaysInMonth = new();
        private bool isPrevDisabled = false;
        private bool isNextDisabled = false;

        protected override void OnInitialized()
        {
            GenerateCalendar();
            UpdateNavigationState();
        }

        private void GenerateCalendar()
        {
            DaysInMonth.Clear();
            var firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            for (int i = 0; i < firstDayOfWeek; i++)
                DaysInMonth.Add(null);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(currentMonth.Year, currentMonth.Month, day);
                DaysInMonth.Add(date);
            }

            UpdateNavigationState();
        }

        private async Task SelectDate(DateTime? date)
        {
            if (date.HasValue && date.Value >= DateTime.Today)
            {
                Console.WriteLine($"🔍 Ngày đã chọn: {date.Value:dd/MM/yyyy}");
                await OnDateSelected.InvokeAsync(date.Value);
            }
        }

        private void PrevMonth()
        {
            if (currentMonth.Month > DateTime.Today.Month || currentMonth.Year > DateTime.Today.Year)
            {
                currentMonth = currentMonth.AddMonths(-1);
                GenerateCalendar();
            }
        }

        private void NextMonth()
        {
            if (currentMonth.Month == DateTime.Today.Month && currentMonth.Year == DateTime.Today.Year)
            {
                return; // Ngăn chọn tháng sau
            }

            currentMonth = currentMonth.AddMonths(1);
            GenerateCalendar();
        }

        private void UpdateNavigationState()
        {
            isPrevDisabled = currentMonth.Month == DateTime.Today.Month && currentMonth.Year == DateTime.Today.Year;
            isNextDisabled = currentMonth.Month != DateTime.Today.Month || currentMonth.Year != DateTime.Today.Year;
        }
    }
}
