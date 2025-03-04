using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.PrescriptionDetail
{
    public class PrescriptionDetailViewModel
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public decimal MedicinePrice { get; set; }
        public int ExaminationReportId { get; set; }
        public int Dosage { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Slot { get; set; }
        public bool IsDelete { get; set; }
    }
}
