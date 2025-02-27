using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Medicie
{
    public class MedicineCreateViewModel
    {
        [Required(ErrorMessage = "Tên thuốc không được để trống.")]
        [MaxLength(255, ErrorMessage = "Tên thuốc không được quá 255 ký tự.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Giá thuốc không được để trống.")]
        [Range(0, 100000000, ErrorMessage = "Giá thuốc không được nhỏ hơn 0 và lớn hơn 100 triệu VND")]
        public decimal Price { get; set; }
    }
}
