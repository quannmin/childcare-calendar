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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phụ huynh không thể trống")]
        public int ParentId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bác sĩ không thể trống")]
        public int DoctorId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sơ yếu lý lịch không thể trống")]
        public int ChildrenRecordId { get; set; }
        public DateTime CheckupDateTime { get; set; }
    }
}
