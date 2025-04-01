using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Account
{
    public class NewUsersByRoleViewModel
    {
        public DateTime Date { get; set; }
        public int ParentCount { get; set; }
        public int DoctorCount { get; set; }
        public int TotalCount => ParentCount + DoctorCount;
    }
}
