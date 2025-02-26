using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ChildCareCalendar.Utilities.CheckValidInput
{
    public class ValidPhoneNumberAttribute : ValidationAttribute
    {
        public ValidPhoneNumberAttribute()
        {
            ErrorMessage = "Invalid phone number format";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            var phoneNumber = value.ToString();
            return Regex.IsMatch(phoneNumber, "^(09|03|07|08|05)[0-9]{10,11}$");
        }
    }
}
