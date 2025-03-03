using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class Speciality
    {
        public int SpecialityId { get; set; }
        public ICollection<AppUser>? AppUsers { get; set; }
        public string? SpecialtyName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false;
        public ICollection<Service>? Services { get; set; }
    }
}
