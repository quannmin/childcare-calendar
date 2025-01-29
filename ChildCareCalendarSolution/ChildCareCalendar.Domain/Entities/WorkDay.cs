using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class WorkDay
    {
        public int WorkDayId { get; set; }
        public string? Day { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
    }
}
