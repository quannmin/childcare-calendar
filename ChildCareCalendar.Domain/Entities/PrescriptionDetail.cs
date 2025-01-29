using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class PrescriptionDetail
    {
        public int PrescriptionDetailId { get; set; }
        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }
        public int ExaminationReportId { get; set; }
        public ExaminationReport? ExaminationReport { get; set; }
        public int Dosage { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Slot { get; set; }
    }
}
