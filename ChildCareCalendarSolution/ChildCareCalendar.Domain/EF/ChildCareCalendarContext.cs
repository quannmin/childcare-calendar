using ChildCareCalendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChildCareCalendar.Domain.EF
{
    public class ChildCareCalendarContext : DbContext
    {
        public ChildCareCalendarContext(DbContextOptions<ChildCareCalendarContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<WorkHour> WorkHours { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ChildrenRecord> ChildrenRecords { get; set; }
        public DbSet<ExaminationReport> ExaminationReports { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<RefundReport> RefundReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Gender).HasConversion<String>();
                entity.Property(e => e.Role).HasConversion<String>();
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.PaymentMethod).HasConversion<String>().IsRequired();
                entity.Property(e => e.Amount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.HasKey(e => e.SpecialityId);
                entity.Property(e => e.SpecialtyName).HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.ServiceId);
                entity.HasOne(e => e.Speciality)
                      .WithMany(s => s.Services)
                      .HasForeignKey(e => e.SpecialityId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WorkHour>(entity =>
            {
                entity.HasKey(e => e.WorkHourId);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId);
                entity.HasOne(e => e.Doctor)
                      .WithMany(u => u.Schedules)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Speciality)
                      .WithMany(s => s.Schedules)
                      .HasForeignKey(e => e.SpecialityId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.WorkHour)
                      .WithMany(h => h.Schedules)
                      .HasForeignKey(e => e.WorkHourId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ChildrenRecord>(entity =>
            {
                entity.HasKey(e => e.ChildrenRecordId);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
                entity.HasOne(e => e.Parent)
                      .WithMany(u => u.ChildrenRecords)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ExaminationReport>(entity =>
            {
                entity.HasKey(e => e.ExaminationReportId);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.HasOne(e => e.ChildrenRecord)
                      .WithMany(c => c.ExaminationReports)
                      .HasForeignKey(e => e.ChildrenRecordId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(e => e.MedicineId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.Price).IsRequired();
            });

            modelBuilder.Entity<PrescriptionDetail>(entity =>
            {
                entity.HasKey(e => e.PrescriptionDetailId);
                entity.Property(e => e.Slot).HasConversion<String>();
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.HasOne(e => e.Medicine)
                      .WithMany(m => m.PrescriptionDetails)
                      .HasForeignKey(e => e.MedicineId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ExaminationReport)
                      .WithMany(r => r.PrescriptionDetails)
                      .HasForeignKey(e => e.ExaminationReportId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.AppointmentId);
                entity.Property(e => e.Status).HasConversion<String>();
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

                entity.HasOne(a => a.ExaminationReport)
                      .WithOne(er => er.Appointment)
                      .HasForeignKey<ExaminationReport>(er => er.AppointmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Parent)
                      .WithMany(p => p.ParentAppointments)
                      .HasForeignKey(a => a.ParentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Doctor)
                      .WithMany(d => d.DoctorAppointments)
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ChildrenRecord)
                      .WithMany(c => c.Appointments)
                      .HasForeignKey(e => e.ChildrenRecordId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.FollowUpAppointment)
                      .WithMany()
                      .HasForeignKey(e => e.FollowUpAppointmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Payment)
                      .WithOne()
                      .HasForeignKey<Appointment>(a => a.PaymentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RefundReport>(entity =>
            {
                entity.HasKey(e => e.RefundReportId);
                entity.Property(e => e.RefundPercentage).HasConversion<String>();
                entity.Property(e => e.RefundAmount).HasPrecision(18, 2);
                entity.HasOne(e => e.Appointment)
                      .WithMany(a => a.RefundReports)
                      .HasForeignKey(e => e.AppointmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
