using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Medicine
{
    public class MedicineEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên thuốc không được để trống.")]
        [MaxLength(255, ErrorMessage = "Tên thuốc không được quá 255 ký tự.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Giá thuốc không được để trống.")]
        [Range(0, 9999999, ErrorMessage = "Giá thuốc phải nằm trong khoảng từ 0 đến 9999999")]
        public decimal Price { get; set; }
    }
}
