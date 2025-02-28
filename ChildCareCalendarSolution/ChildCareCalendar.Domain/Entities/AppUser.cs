using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? Gender { get; set; }
        public string? Avatar {  get; set; }
        public string? Address { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false; 
        public DateTime Dob { get; set; }
        public string? Role { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public ICollection<Appointment>? ParentAppointments { get; set; }
        public ICollection<Appointment>? DoctorAppointments { get; set; }
        public ICollection<ChildrenRecord>? ChildrenRecords { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
    }
}
