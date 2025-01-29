using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int SpecialityId { get; set; }
        public Speciality? Speciality { get; set; }
        public int WorkDayId { get; set; }
        public WorkDay? WorkDay { get; set; }
        public int WorkHourId { get; set; }
        public WorkHour? WorkHour { get; set; }
        public int UserId { get; set; }
        public AppUser? Doctor { get; set; }
    }
}
