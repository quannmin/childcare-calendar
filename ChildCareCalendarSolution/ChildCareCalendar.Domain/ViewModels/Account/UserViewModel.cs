using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Utilities.CheckValidInput;
using ChildCareCalendar.Utilities.Constants;
using ChildCareCalendar.Utilities.Helper;
using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.Account
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? Role { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
