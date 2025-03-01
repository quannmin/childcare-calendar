using ChildCareCalendar.Domain.ViewModels.PrescriptionDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.ExaminationReport
{
    public class ExaminationReportDetailViewModel
    {
        public int Id { get; set; }
        public int ChildrenRecordId { get; set; }
        public string? ChildrenName { get; set; }
        public int AppointmentId { get; set; }
        public DateTime ExamDate { get; set; }
        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }
        public decimal TotalAmount { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public List<PrescriptionDetailViewModel>? PrescriptionDetails { get; set; }
    }

}
