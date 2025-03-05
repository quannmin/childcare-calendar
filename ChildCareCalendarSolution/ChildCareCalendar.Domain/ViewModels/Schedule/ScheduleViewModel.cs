using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.WorkHour;

namespace ChildCareCalendar.Domain.ViewModels.Schedule
{
	public class ScheduleViewModel
	{
		public int Id { get; set; }
		public int SpecialityId { get; set; }
		public DateTime? WorkDay { get; set; }
		public string? day { get; set; }
		public int WorkHourId { get; set; }
		public WorkHourViewModel? WorkHour { get; set; }
		public int UserId { get; set; }
		public UserViewModel? Doctor { get; set; }
	}
}
