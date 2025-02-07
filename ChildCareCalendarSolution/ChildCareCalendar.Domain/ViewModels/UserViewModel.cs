using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Utilities.CheckValidInput;
using ChildCareCalendar.Utilities.Helper;
using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide email")]
        public string? Email { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide full name")]
        public string? FullName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide phone number")]
        [ValidPhoneNumber(ErrorMessage = "Phone number must start with 09, 03, 07, 08, or 05 and have 8 to 9 digits.")]
        public string? PhoneNumber { get; set; }

        public string? GenderDisplay { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob {  get; set; }
        public string? RoleDisplay { get; set; }

        public static UserViewModel FromEntity(AppUser user)
        {
            return new UserViewModel
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                RoleDisplay = EnumHelper.GetRoleDisplay(user.Role),
                Address = user.Address,
                Dob = user.Dob,
                GenderDisplay = EnumHelper.GetGenderDisplay(user.Gender),
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
