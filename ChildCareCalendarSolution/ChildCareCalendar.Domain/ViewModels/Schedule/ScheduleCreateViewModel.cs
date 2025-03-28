using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.Schedule
{
	public class ScheduleCreateViewModel
    {
        public DateTime? WorkDay { get; set; }
        public string? day { get; set; }
		[Required(ErrorMessage = "Hãy chọn Giờ làm việc")]
		public int? WorkHourId { get; set; }
		[Required(ErrorMessage = "Hãy chọn Bác sĩ")]
		public int? UserId { get; set; }
    }
}
