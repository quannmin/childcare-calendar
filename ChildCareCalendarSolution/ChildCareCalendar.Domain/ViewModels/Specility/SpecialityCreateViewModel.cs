using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.Specility
{
    public class SpecialityCreateViewModel
    {
        [Required(ErrorMessage = "Tên chuyên khoa không được để trống.")]
        [MaxLength(255, ErrorMessage = "Tên chuyên khoa không được quá 255 ký tự.")]
        public string? SpecialtyName { get; set; }
        [MaxLength(255, ErrorMessage = "Mô tả không được quá 255 ký tự.")]
        public string? Description { get; set; }
    }
}
