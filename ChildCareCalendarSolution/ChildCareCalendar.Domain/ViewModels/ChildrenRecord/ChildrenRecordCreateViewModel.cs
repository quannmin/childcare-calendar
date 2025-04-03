using ChildCareCalendar.Utilities.CheckValidInput;
using ChildCareCalendar.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.ChildrenRecord
{
    public class ChildrenRecordCreateViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ và tên không thể trống")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Ngày tháng năm sinh không thể trống")]
        [PastDateValidation]
        public DateTime? Dob { get; set; }
        public SystemConstant.Gender Gender { get; set; }

        public string? MedicalHistory {  get; set; }
    }
}
