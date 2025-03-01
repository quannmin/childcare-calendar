using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.PrescriptionDetail
{
    public class PrescriptionDetailCreateViewModel
    {
        public int MedicineId { get; set; }
        public int ExaminationReportId { get; set; }
        public int Dosage { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Slot { get; set; }
    }
}
