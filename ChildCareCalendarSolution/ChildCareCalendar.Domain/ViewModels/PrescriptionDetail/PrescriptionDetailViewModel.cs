using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Liều lượng không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Liều lượng phải lớn hơn 0.")]
        public int Dosage { get; set; }
        [Required(ErrorMessage = "Số lượng không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Thời gian uống không được để trống.")]
        public string? Slot { get; set; }
        public bool IsDelete { get; set; }
    }
}
