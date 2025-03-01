using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.ExaminationReport
{
    public class ExaminationReportViewModel
    {
        public int Id { get; set; }
        public int ChildrenRecordId { get; set; }
        public string? ChildrenName { get; set; }
        public int AppointmentId { get; set; }
        public DateTime ExamDate { get; set; }
        public string? Diagnosis { get; set; }
        public bool IsDeleted { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
