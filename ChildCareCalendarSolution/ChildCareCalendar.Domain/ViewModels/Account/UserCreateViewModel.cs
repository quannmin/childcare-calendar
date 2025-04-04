﻿using ChildCareCalendar.Utilities.CheckValidInput;
using ChildCareCalendar.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.Account
{
    public class UserCreateViewModel
    {
        public int? SpecialityId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không thể trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất một chữ hoa, một chữ thường và một số")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string? ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ và tên không thể trống")]
        public string? FullName { get; set; }

        public SystemConstant.AccountsRole Role { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không thể trống")]
        [ValidPhoneNumber(ErrorMessage = "Số điện thoại phải bắt đầu 09, 03, 07, 08, or 05 và có 10 đến 11 số.")]
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public SystemConstant.Gender Gender { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Ngày tháng năm sinh không thể trống")]
        [UserDob18Age]
        public DateTime? Dob { get; set; }
    }
}
