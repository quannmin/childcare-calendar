using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Appointment
{
    public class AppointmentDetailViewModel
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public DateTime CheckupDateTime { get; set; }
        public decimal TotalAmount { get; set; }

        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false; 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ServiceName { get; set; }

        public string? ParentName { get; set; }
        public string? DoctorName { get; set; }
        public string? ChildName { get; set; }
        public DateTime? FollowUpAppointment { get; set; }
        public int? ExaminationReportId { get; set; }
        public int? ChildrenRecordId { get; set; }



    }
}
