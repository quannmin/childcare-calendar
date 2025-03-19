using ChildCareCalendar.Domain.ViewModels.PrescriptionDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.ExaminationReport
{
    public class ExaminationReportCreateViewModel
    {
        [Required]
        public int ChildrenRecordId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        public string? Diagnosis { get; set; }
        public string? Notes { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        public decimal TotalAmount { get; set; }

        public string? CreatedBy { get; set; }

        public List<PrescriptionDetailCreateViewModel> PrescriptionDetails { get; set; } = new();
    }  
}
