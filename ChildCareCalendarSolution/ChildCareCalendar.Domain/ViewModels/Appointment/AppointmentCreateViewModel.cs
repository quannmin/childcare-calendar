using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Appointment
{
    public class AppointmentCreateViewModel
    {
        public int ParentId { get; set; }
        public int DoctorId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sơ yếu lý lịch không thể trống")]
        public int? ChildrenRecordId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phương thức thanh toán không thể trống")]
        public int? PaymentId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Dịch vụ không thể trống")]
        public int ServiceId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ngày khám không thể trống")]
        public DateTime CheckupDateTime { get; set; }
    }
}
