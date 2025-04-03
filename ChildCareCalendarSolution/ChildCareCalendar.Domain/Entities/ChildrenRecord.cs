using ChildCareCalendar.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class ChildrenRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser? Parent { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false; 
        public string? FullName { get; set; }
        public DateTime Dob { get; set; }
        public string? Gender { get; set; }
        public string? MedicalHistory { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public ICollection<ExaminationReport>? ExaminationReports { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
