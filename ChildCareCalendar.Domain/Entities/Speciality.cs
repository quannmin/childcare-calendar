﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class Speciality
    {
        public int SpecialityId { get; set; }
        public string? SpecialtyName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public ICollection<Service>? Services { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
    }
}
