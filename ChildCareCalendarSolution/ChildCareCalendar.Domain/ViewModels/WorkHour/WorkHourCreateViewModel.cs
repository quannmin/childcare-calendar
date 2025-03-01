using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.WorkHour
{
	public class WorkHourCreateViewModel
	{
		[Required(ErrorMessage = "Giờ bắt đầu không được để trống.")]
		public TimeSpan StartTime { get; set; }
		[Required(ErrorMessage = "Giờ bắt đầu không được để trống.")]
		public TimeSpan EndTime { get; set; }
	}
}
