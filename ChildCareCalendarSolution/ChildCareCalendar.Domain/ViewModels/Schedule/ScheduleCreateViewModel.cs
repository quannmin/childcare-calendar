namespace ChildCareCalendar.Domain.ViewModels.Schedule
{
	public class ScheduleCreateViewModel
    {
        public DateTime? WorkDay { get; set; }
        public string? day { get; set; }
        public int WorkHourId { get; set; }
        public int UserId { get; set; }
    }
}
