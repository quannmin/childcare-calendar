using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Utilities.CheckValidInput
{
    public class PastDateValidation : ValidationAttribute
    {
        public PastDateValidation()
        {
            ErrorMessage = "Ngày sinh không thể trong tương lai";
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is DateTime dateTime)
            {
                var currenDate = DateTime.Today;
                return dateTime < currenDate;
            }

            return false;
        }
    }
}
