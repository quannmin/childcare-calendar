﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class RefundReport
    {
        public int Id { get; set; } 
        public decimal RefundAmount { get; set; }
        public DateTime RefundDate { get; set; }
        public string? RefundPercentage { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
