﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<PrescriptionDetail>? PrescriptionDetails { get; set; }

    }
}
