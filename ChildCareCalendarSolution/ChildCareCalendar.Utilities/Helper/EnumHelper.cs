using ChildCareCalendar.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChildCareCalendar.Utilities.Constants.SystemConstant;

namespace ChildCareCalendar.Utilities.Helper
{
    public class EnumHelper
    {
        public static string GetGenderDisplay(string genderString)
        {
            if(Enum.TryParse<SystemConstant.Gender>(genderString, out var gender)) {
                return gender switch
                {
                    SystemConstant.Gender.Male => "Nam",
                    SystemConstant.Gender.Female => "Nữ",
                    SystemConstant.Gender.Other => "Khác",
                    _ => "Không xác định"
                };
            }
            return "Không xác định";
        }

        public static string GetRoleDisplay(String roleString)
        {
            if(Enum.TryParse<SystemConstant.AccountsRole>(roleString, out var role))
            {
                return role switch
                {
                    AccountsRole.Manager => "Quản lý",
                    AccountsRole.Doctor => "Bác sĩ",
                    AccountsRole.Parent => "Phụ huynh",
                    _ => "Không xác định"
                };
            }
            return "Không xác định";
        }
    }
}
