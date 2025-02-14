using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide email")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide password")]
        public string? Password { get; set; }
    }
}
