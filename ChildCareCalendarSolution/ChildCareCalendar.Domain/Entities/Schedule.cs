using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public DateTime? WorkDay { get; set; }
        public string? day { get; set; }
        public int WorkHourId { get; set; }
        public WorkHour? WorkHour { get; set; }
        public int UserId { get; set; }
        public AppUser? Doctor { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;
    }
}
