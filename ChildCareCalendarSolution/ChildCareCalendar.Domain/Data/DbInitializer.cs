using BCrypt.Net;
using ChildCareCalendar.Domain.EF;
using ChildCareCalendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Data
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var context = new ChildCareCalendarContext(serviceProvider.GetRequiredService<DbContextOptions<ChildCareCalendarContext>>());

            //Renew DB Data
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new AppUser { Email = "manager@example.com", FullName = "Ông sếp", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "Manager" },
                    new AppUser { Email = "doctor1@example.com", FullName = "Bác sĩ Hans", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "Doctor" },
                    new AppUser { Email = "parent1@example.com", FullName = "Chị Ba", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "Parent" }
                );
                context.SaveChanges();
            }

            if (!context.Specialities.Any())
            {
                context.Specialities.AddRange(
                    new Speciality { SpecialtyName = "Tai mũi họng", Description = "Children healthcare" },
                    new Speciality { SpecialtyName = "Da liễu", Description = "Skin healthcare" }
                );
                context.SaveChanges();
            }

            if (!context.Services.Any())
            {
                context.Services.AddRange(
                    new Service { SpecialityId = 1, ServiceName = "Khám tổng quát", Description = "Basic health checkup", Price = 50 },
                    new Service { SpecialityId = 2, ServiceName = "Khám da liễu", Description = "Skin disease consultation", Price = 70 }
                );
                context.SaveChanges();
            }

            if (!context.ChildrenRecords.Any())
            {
                context.ChildrenRecords.AddRange(
                    new ChildrenRecord { UserId = 3, FullName = "Bé Thế Anh", Dob = new DateTime(2015, 6, 1), Gender = "Trai", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now }
                );
                context.SaveChanges();
            }

            if (!context.Appointments.Any())
            {
                context.Appointments.AddRange(
                    new Appointment { DoctorId = 2, ParentId = 3, ServiceId = 1, ChildrenRecordId = 1, Status = "ORDERED", TotalAmount = 50, CreatedAt = DateTime.Now },
                    new Appointment { DoctorId = 2, ParentId = 3, ServiceId = 2, ChildrenRecordId = 1, Status = "CHECKED_IN", TotalAmount = 70, CreatedAt = DateTime.Now }
                );
                context.SaveChanges();
            }

            

            if (!context.ExaminationReports.Any())
            {
                context.ExaminationReports.AddRange(
                    new ExaminationReport { AppointmentId = 1, ChildrenRecordId = 1, Diagnosis = "Khỏe", Notes = "Theo dõi thường xuyên", CreatedAt = DateTime.Now }
                );
                context.SaveChanges();
            }

            if (!context.Medicines.Any())
            {
                context.Medicines.AddRange(
                    new Medicine { Name = "Paracetamol", Price = 5 },
                    new Medicine { Name = "Ibuprofen", Price = 8 }
                );
                context.SaveChanges();
            }

            if (!context.Payments.Any())
            {
                context.Payments.AddRange(
                    new Payment { PaymentDate = DateTime.Now, Amount = 50, PaymentMethod = "VNPAY", Status = "Completed", CreatedAt = DateTime.Now }
                );
                context.SaveChanges();
            }

            if (!context.PrescriptionDetails.Any())
            {
                context.PrescriptionDetails.AddRange(
                    new PrescriptionDetail { MedicineId = 1, ExaminationReportId = 1, Dosage = 2, Quantity = 10, TotalAmount = 50, Slot = "DAY" }
                );
                context.SaveChanges();
            }

            if (!context.WorkHours.Any())
            {
                context.WorkHours.AddRange(
                    new WorkHour { StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(12, 0, 0) }
                );
                context.SaveChanges();
            }

            if (!context.Schedules.Any())
            {
                context.Schedules.AddRange(
                    new Schedule { UserId = 2, SpecialityId = 1, WorkDay = DateTime.Now, WorkHourId = 1}
                );
                context.SaveChanges();
            }

            
        }
    }
}
