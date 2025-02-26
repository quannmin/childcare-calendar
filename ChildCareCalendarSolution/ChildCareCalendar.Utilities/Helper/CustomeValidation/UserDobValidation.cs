using System;
using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Utilities.Helper.CustomeValidation
{
    public class UserDobValidation : ValidationAttribute
    {
        public UserDobValidation()
        {
            ErrorMessage = "The year of birth must be between 1960 and 2030";
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is DateTime dateTime)
            {
                int year = dateTime.Year;
                return year >= 1960 && year <= 2030;
            }

            return false;
        }
    }
}
