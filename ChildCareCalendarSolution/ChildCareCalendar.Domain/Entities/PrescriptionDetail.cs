using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class PrescriptionDetail
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }
        public int ExaminationReportId { get; set; }
        public ExaminationReport? ExaminationReport { get; set; }
        public int Dosage { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Slot { get; set; }

        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;
    }
}
