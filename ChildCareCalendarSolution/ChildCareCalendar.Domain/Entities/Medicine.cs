using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class Medicine
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;
        public ICollection<PrescriptionDetail>? PrescriptionDetails { get; set; }

    }
}
