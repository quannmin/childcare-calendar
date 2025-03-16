using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Appointment
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public string ParentName { get; set; }
        public string DoctorName { get; set; }
        public string ChildName { get; set; }
        public string ServiceName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CheckupDateTime { get; set; }
        public string? Status { get; set; }
    }
}
