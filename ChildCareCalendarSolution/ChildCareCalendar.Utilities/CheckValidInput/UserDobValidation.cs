using System;
using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Utilities.CheckValidInput
{
    public class UserDobValidation : ValidationAttribute
    {
        public UserDobValidation()
        {
            ErrorMessage = "Người dùng phải lớn hơn 18 tuổi và nhỏ hơn 100 tuổi";
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is DateTime dateTime)
            {
                var currenDate = DateTime.UtcNow.Year;
                int year = dateTime.Year;
                return year >= currenDate - 100 && year <= currenDate - 18;
            }

            return false;
        }
    }
}
