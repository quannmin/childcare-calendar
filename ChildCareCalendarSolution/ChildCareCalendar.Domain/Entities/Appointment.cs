using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public DateTime CheckupDateTime { get; set; }
        public decimal TotalAmount { get; set; }

        [DefaultValue(false)]
        public bool IsDelete { get; set; } = false; public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ParentId { get; set; }
        public AppUser? Parent { get; set; }
        public int DoctorId { get; set; }
        public AppUser? Doctor { get; set; }
        public int? ChildrenRecordId { get; set; }
        public ChildrenRecord? ChildrenRecord { get; set; }
        public int? FollowUpAppointmentId { get; set; }
        public Appointment? FollowUpAppointment { get; set; }
        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public Schedule? Schedule { get; set; }
        public int ScheduleId { get; set; }
        public int? ExaminationReportId { get; set; }
        public ExaminationReport? ExaminationReport { get; set; }
        public ICollection<RefundReport>? RefundReports { get; set; }
    }
}
