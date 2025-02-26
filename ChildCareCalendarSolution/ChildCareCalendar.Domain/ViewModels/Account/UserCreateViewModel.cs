﻿using ChildCareCalendar.Utilities.CheckValidInput;
using ChildCareCalendar.Utilities.Constants;
using System.ComponentModel.DataAnnotations;
using ChildCareCalendar.Utilities.Helper.CustomeValidation;

namespace ChildCareCalendar.Domain.ViewModels.Account
{
    public class UserCreateViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không thể trống")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không thể trống")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string? ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ và tên không thể trống")]
        public string? FullName { get; set; }

        public SystemConstant.AccountsRole Role { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không thể trống")]
        [ValidPhoneNumber(ErrorMessage = "Phone number must start with 09, 03, 07, 08, or 05 and have 8 to 9 digits.")]
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public SystemConstant.Gender Gender { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Ngày tháng năm sinh không thể trống")]
        [UserDobValidation]
        public DateTime? Dob { get; set; }
    }
}
