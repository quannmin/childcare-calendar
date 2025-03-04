using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Appointment
{
    public class AppointmentEditViewModel
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime CheckupDateTime { get; set; }
        public string? Status { get; set; }
    }
}
