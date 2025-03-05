namespace ChildCareCalendar.Domain.ViewModels.Schedule
{
    public class CalendarDay
    {
        public int Day { get; set; }
        public List<ScheduleViewModel> Schedules { get; set; } = new();
    }
}
