using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int SpecialityId { get; set; }
        public Speciality? Speciality { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;
    }
}
