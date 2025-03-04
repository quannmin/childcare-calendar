using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.WorkHour
{
    public class WorkHourEditViewModel
    {
        [Required(ErrorMessage = "Id không được để trống.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Giờ bắt đầu không được để trống.")]
        public TimeSpan StartTime { get; set; }
        [Required(ErrorMessage = "Giờ kết thúc không được để trống.")]
        public TimeSpan EndTime { get; set; }
    }
}
