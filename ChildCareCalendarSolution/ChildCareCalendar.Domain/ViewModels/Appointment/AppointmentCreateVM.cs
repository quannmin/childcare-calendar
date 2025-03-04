using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Appointment
{
    public class AppointmentCreateVM
    {
        public int ParentId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bác sĩ không thể trống")]
        public int DoctorId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sơ yếu lý lịch không thể trống")]
        public int ChildrenRecordId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Dịch vụ không thể trống")]
        public int? ServiceId { get; set; }
        public int? PaymentId { get; set; }
        public string? Status { get; set; } = "ORDERED";
        public DateTime CheckupDateTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
