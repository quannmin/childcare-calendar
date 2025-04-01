using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Appointment
{
    public class DailyAppointmentCountViewModel
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; }
        public int Count { get; set; }
    }
}
