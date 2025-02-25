using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class WorkHour
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;
        public ICollection<Schedule>? Schedules { get; set; }
    }
}
