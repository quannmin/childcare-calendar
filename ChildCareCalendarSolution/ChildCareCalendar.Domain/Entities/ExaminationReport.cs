using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class ExaminationReport
    {
        public int Id { get; set; }
        public int ChildrenRecordId { get; set; }
        public ChildrenRecord? ChildrenRecord { get; set; }
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }

        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;
        public decimal TotalAmount { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public ICollection<PrescriptionDetail>? PrescriptionDetails { get; set; }
    }
}
