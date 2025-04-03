using ChildCareCalendar.Utilities.CheckValidInput;
using ChildCareCalendar.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.ChildrenRecord
{
    public class ChildrenRecordEditViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ và tên không thể trống")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Ngày tháng năm sinh không thể trống")]
        [PastDateValidation]
        public DateTime? Dob { get; set; }
        public SystemConstant.Gender Gender { get; set; }

        public string? MedicalHistory { get; set; }
    }
}
