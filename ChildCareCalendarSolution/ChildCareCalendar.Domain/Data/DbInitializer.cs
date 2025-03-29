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
                if (!context.Specialities.Any())
                {
                    context.Specialities.AddRange(
                        new Speciality { SpecialtyName = "Tai mũi họng", Description = "Children healthcare" },
                        new Speciality { SpecialtyName = "Da liễu", Description = "Skin healthcare" }
                    );
                    context.SaveChanges();
                }

                context.Users.AddRange(
                    new AppUser { Email = "manager@example.com", FullName = "Ông sếp", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "QuanLy" },
                    new AppUser { Email = "doctor1@example.com", FullName = "Bác sĩ Quân", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 1 },
                    new AppUser { Email = "doctor2@example.com", FullName = "Bác sĩ Lương", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg",
						SpecialityId = 2 },
                    new AppUser { Email = "doctor3@example.com", FullName = "Bác sĩ Quốc", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 1 },
                    new AppUser { Email = "doctor4@example.com", FullName = "Bác sĩ Qui", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 2 },
                    new AppUser { Email = "parent1@example.com", FullName = "Chị Ba", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "PhuHuynh", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", PhoneNumber = "0123456789", Address = "123 Đường Cây Dừa, Xã Cây Cau, Huyện Cây Chuối, Tỉnh Bến Tre" }
                );
                context.SaveChanges();
            }

            if (!context.Services.Any())
            {
                context.Services.AddRange(
                    new Service { SpecialityId = 1, ServiceName = "Khám thường có BHYT", Description = "Basic health checkup", Price = 500000 },
                    new Service { SpecialityId = 2, ServiceName = "Khám thường có BHYT", Description = "Skin disease consultation", Price = 700000 },
                    new Service { SpecialityId = 1, ServiceName = "Khám thường không có BHYT", Description = "Basic health checkup", Price = 500000 },
                    new Service { SpecialityId = 2, ServiceName = "Khám thường không có BHYT", Description = "Skin disease consultation", Price = 700000 },
                    new Service { SpecialityId = 1, ServiceName = "Khám vip", Description = "Basic health checkup", Price = 500000 },
                    new Service { SpecialityId = 2, ServiceName = "Khám vip", Description = "Skin disease consultation", Price = 700000 }
                );
                context.SaveChanges();
            }

            if (!context.ChildrenRecords.Any())
            {
                context.ChildrenRecords.AddRange(
                    new ChildrenRecord { UserId = 6, FullName = "Bé Thế Anh", Dob = new DateTime(2015, 6, 1), Gender = "Nam", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 6, FullName = "Bé Lộc", Dob = new DateTime(2015, 6, 1), Gender = "Nam", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now }
                );
                context.SaveChanges();
            }

            if (!context.WorkHours.Any())
            {
                context.WorkHours.AddRange(
                    new WorkHour { StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(9, 0, 0) },
                    new WorkHour { StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0) },
                    new WorkHour { StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0) },
                    new WorkHour { StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
                    new WorkHour { StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(13, 0, 0) },
                    new WorkHour { StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(14, 0, 0) },
                    new WorkHour { StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(15, 0, 0) },
                    new WorkHour { StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(16, 0, 0) },
                    new WorkHour { StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(17, 0, 0) }
                );
                context.SaveChanges();
            }

            if (!context.Schedules.Any())
            {
                context.Schedules.AddRange(
                    new Schedule { UserId = 2, WorkDay = DateTime.Now, WorkHourId = 1 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now.AddDays(1), WorkHourId = 1 }
                );
                context.SaveChanges();
            }

            if (!context.Appointments.Any())
            {
                 context.Appointments.AddRange(
                    new Appointment { DoctorId = 2, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED",
                        ServiceId = 1, TotalAmount = 550_000, CheckupDateTime = new DateTime(2024, 11, 10), CreatedAt = DateTime.Now },
                    new Appointment { DoctorId = 2, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 750_000, CheckupDateTime = new DateTime(2023, 12, 5), CreatedAt = DateTime.Now },
                    new Appointment { DoctorId = 2, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 600_000, CheckupDateTime = new DateTime(2024, 9, 15), CreatedAt = DateTime.Now, ExaminationReportId = 1 },

                    new Appointment { DoctorId = 3, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", ServiceId = 1, TotalAmount = 520_000, CheckupDateTime = new DateTime(2024, 8, 20), CreatedAt = DateTime.Now },
                    new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 680_000, CheckupDateTime = new DateTime(2023, 9, 10), CreatedAt = DateTime.Now }
                    //new Appointment { DoctorId = 3, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", TotalAmount = 720_000, CheckupDateTime = new DateTime(2024, 10, 1), CreatedAt = DateTime.Now },

//new Appointment { DoctorId = 3, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", TotalAmount = 530_000, CheckupDateTime = new DateTime(2024, 7, 5), CreatedAt = DateTime.Now },
//new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", TotalAmount = 710_000, CheckupDateTime = new DateTime(2023, 8, 15), CreatedAt = DateTime.Now },
//new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", TotalAmount = 680_000, CheckupDateTime = new DateTime(2024, 9, 25), CreatedAt = DateTime.Now },

//new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", TotalAmount = 590_000, CheckupDateTime = new DateTime(2024, 11, 30), CreatedAt = DateTime.Now },
//new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", TotalAmount = 720_000, CheckupDateTime = new DateTime(2023, 10, 10), CreatedAt = DateTime.Now },
//new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", TotalAmount = 650_000, CheckupDateTime = new DateTime(2024, 12, 5), CreatedAt = DateTime.Now }
);
                context.SaveChanges();
            }

            if (!context.ExaminationReports.Any())
            {
                context.ExaminationReports.AddRange(
                    new ExaminationReport { AppointmentId = 3, ChildrenRecordId = 1, Diagnosis = "Khỏe", Notes = "Theo dõi thường xuyên", CreatedAt = DateTime.Now }
                );
                context.SaveChanges();
            }

            if (!context.Medicines.Any())
            {
                context.Medicines.AddRange(
                    new Medicine { Name = "Paracetamol", Price = 5 },
                    new Medicine { Name = "Ibuprofen", Price = 8 },
                    new Medicine { Name = "Aspirin", Price = 6 },
                    new Medicine { Name = "Cetirizine", Price = 7 },
                    new Medicine { Name = "Amoxicillin", Price = 12 },
                    new Medicine { Name = "Metformin", Price = 10 },
                    new Medicine { Name = "Loratadine", Price = 5 },
                    new Medicine { Name = "Omeprazole", Price = 9 },
                    new Medicine { Name = "Simvastatin", Price = 11 },
                    new Medicine { Name = "Metronidazole", Price = 8 },
                    new Medicine { Name = "Furosemide", Price = 7 },
                    new Medicine { Name = "Prednisolone", Price = 10 },
                    new Medicine { Name = "Paracetamol", Price = 5 },
                    new Medicine { Name = "Ibuprofen", Price = 8 },
                    new Medicine { Name = "Aspirin", Price = 6 },
                    new Medicine { Name = "Cetirizine", Price = 7 },
                    new Medicine { Name = "Amoxicillin", Price = 12 },
                    new Medicine { Name = "Metformin", Price = 10 },
                    new Medicine { Name = "Loratadine", Price = 5 },
                    new Medicine { Name = "Omeprazole", Price = 9 },
                    new Medicine { Name = "Simvastatin", Price = 11 },
                    new Medicine { Name = "Metronidazole", Price = 8 },
                    new Medicine { Name = "Furosemide", Price = 7 },
                    new Medicine { Name = "Prednisolone", Price = 10 },
                    new Medicine { Name = "Paracetamol", Price = 5 },
                    new Medicine { Name = "Ibuprofen", Price = 8 },
                    new Medicine { Name = "Aspirin", Price = 6 },
                    new Medicine { Name = "Cetirizine", Price = 7 },
                    new Medicine { Name = "Amoxicillin", Price = 12 },
                    new Medicine { Name = "Metformin", Price = 10 },
                    new Medicine { Name = "Loratadine", Price = 5 },
                    new Medicine { Name = "Omeprazole", Price = 9 },
                    new Medicine { Name = "Simvastatin", Price = 11 },
                    new Medicine { Name = "Metronidazole", Price = 8 },
                    new Medicine { Name = "Furosemide", Price = 7 },
                    new Medicine { Name = "Prednisolone", Price = 10 },
                    new Medicine { Name = "Paracetamol", Price = 5 },
                    new Medicine { Name = "Ibuprofen", Price = 8 },
                    new Medicine { Name = "Aspirin", Price = 6 },
                    new Medicine { Name = "Cetirizine", Price = 7 },
                    new Medicine { Name = "Amoxicillin", Price = 12 },
                    new Medicine { Name = "Metformin", Price = 10 },
                    new Medicine { Name = "Loratadine", Price = 5 },
                    new Medicine { Name = "Omeprazole", Price = 9 },
                    new Medicine { Name = "Simvastatin", Price = 11 },
                    new Medicine { Name = "Metronidazole", Price = 8 },
                    new Medicine { Name = "Furosemide", Price = 7 },
                    new Medicine { Name = "Prednisolone", Price = 10 },
                    new Medicine { Name = "Paracetamol", Price = 5 },
                    new Medicine { Name = "Ibuprofen", Price = 8 },
                    new Medicine { Name = "Aspirin", Price = 6 },
                    new Medicine { Name = "Cetirizine", Price = 7 },
                    new Medicine { Name = "Amoxicillin", Price = 12 },
                    new Medicine { Name = "Metformin", Price = 10 },
                    new Medicine { Name = "Loratadine", Price = 5 },
                    new Medicine { Name = "Omeprazole", Price = 9 },
                    new Medicine { Name = "Simvastatin", Price = 11 },
                    new Medicine { Name = "Metronidazole", Price = 8 },
                    new Medicine { Name = "Furosemide", Price = 7 },
                    new Medicine { Name = "Prednisolone", Price = 10 },
                    new Medicine { Name = "Paracetamol", Price = 5 },
                    new Medicine { Name = "Ibuprofen", Price = 8 },
                    new Medicine { Name = "Aspirin", Price = 6 },
                    new Medicine { Name = "Cetirizine", Price = 7 },
                    new Medicine { Name = "Amoxicillin", Price = 12 },
                    new Medicine { Name = "Metformin", Price = 10 },
                    new Medicine { Name = "Loratadine", Price = 5 },
                    new Medicine { Name = "Omeprazole", Price = 9 },
                    new Medicine { Name = "Simvastatin", Price = 11 },
                    new Medicine { Name = "Metronidazole", Price = 8 },
                    new Medicine { Name = "Furosemide", Price = 7 },
                    new Medicine { Name = "Prednisolone", Price = 10 }

                );
                context.SaveChanges();
            }



            if (!context.PrescriptionDetails.Any())
            {
                context.PrescriptionDetails.AddRange(
                    new PrescriptionDetail { MedicineId = 1, ExaminationReportId = 1, Dosage = 2, Quantity = 10, TotalAmount = 50, Slot = "DAY" },
                    new PrescriptionDetail { MedicineId = 2, ExaminationReportId = 1, Dosage = 2, Quantity = 10, TotalAmount = 50, Slot = "DAY" },
                    new PrescriptionDetail { MedicineId = 3, ExaminationReportId = 1, Dosage = 2, Quantity = 10, TotalAmount = 50, Slot = "DAY" }
                );

                context.SaveChanges();
            }

            if (!context.RefundReports.Any())
            {
                context.RefundReports.AddRange(
                    new RefundReport
                    {
                        RefundAmount = 104000,
                        RefundDate = DateTime.Now.AddDays(-1),
                        RefundPercentage = "20",
                        AppointmentId = 4,
                        IsDelete = false
                    },
                    new RefundReport
                    {
                        RefundAmount = 260000,
                        RefundDate = DateTime.Now.AddDays(-2),
                        RefundPercentage = "50",
                        AppointmentId = 2,
                        IsDelete = false
                    },
                    new RefundReport
                    {
                        RefundAmount = 420000,
                        RefundDate = DateTime.Now.AddDays(-3),
                        RefundPercentage = "70",
                        AppointmentId = 3,
                        IsDelete = false
                    }
                );

                context.SaveChanges();
            }
        }
    }
}


