using ChildCareCalendar.Utilities.CheckValidInput;
using ChildCareCalendar.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.Account
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ và tên không thể trống")]
        public string? FullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không thể trống")]
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Ngày tháng năm sinh không thể trống")]
        public DateTime? Dob { get; set; }
    }
}
