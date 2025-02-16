
using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.Specility
{
    public class SpecialitySearchViewModel
    {
        [MaxLength(255, ErrorMessage = "Từ khóa tối đa 255 ký tự.")]
        public string Keyword {  get; set; }
    }
}
