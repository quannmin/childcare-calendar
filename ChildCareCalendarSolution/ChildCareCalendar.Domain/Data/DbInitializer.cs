using BCrypt.Net;
using ChildCareCalendar.Domain.EF;
using ChildCareCalendar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                        new Speciality { SpecialtyName = "Da liễu", Description = "Skin healthcare" },
                         new Speciality { SpecialtyName = "Nhi khoa", Description = "Chăm sóc sức khỏe trẻ em" },
        new Speciality { SpecialtyName = "Tâm lý học", Description = "Tham vấn tâm lý trẻ em" },
        new Speciality { SpecialtyName = "Dinh dưỡng", Description = "Tư vấn chế độ ăn uống cho trẻ" }
                    );
                    context.SaveChanges();
                }

                context.Users.AddRange(
                    new AppUser { Email = "manager@example.com", FullName = "Quản lý Tèo", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "QuanLy", CreatedAt = DateTime.Now.AddDays(-7) },
                    new AppUser { Email = "doctor1@example.com", FullName = "Quân", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 1, Gender = "Nam", CreatedAt = DateTime.Now.AddDays(-5) },
                    new AppUser
                    {
                        Email = "doctor2@example.com",
                        FullName = "Lương",
                        Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256),
                        Role = "BacSi",
                        Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg",
                        SpecialityId = 2,
                        Gender = "Nam"
                    },
                    new AppUser { Email = "doctor3@example.com", FullName = "Quốc", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 1, Gender = "Nam", CreatedAt = DateTime.Now.AddDays(-6) },
                    new AppUser { Email = "doctor4@example.com", FullName = "Qui", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 2, Gender = "Nam", CreatedAt = DateTime.Now.AddDays(-7) },
                    new AppUser { Email = "parent1@example.com", FullName = "Lương", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "PhuHuynh", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", PhoneNumber = "0123456789", Address = "123 Đường Cây Dừa, Xã Cây Cau, Huyện Cây Chuối, Tỉnh Bến Tre" , CreatedAt = DateTime.Now.AddDays(-4) },
                    new AppUser
                     {
                         Email = "nguyenkhaiqui2003@gmail.com",
                         FullName = "Qui",
                         Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256),
                         Role = "BacSi",
                         Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg",
                         PhoneNumber = "0123456789",
                         Address = "123 Đường Cây Dừa, Xã Cây Cau, Huyện Cây Chuối, Tỉnh Bến Tre",
                         SpecialityId = 2,
                         Gender = "Nam"
                     },
                    new AppUser { Email = "doctor5@example.com", FullName = "Tiên", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 1, Gender = "Nữ", CreatedAt = DateTime.Now.AddDays(-3) },
                    new AppUser { Email = "doctor6@example.com", FullName = "Khánh", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "BacSi", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", SpecialityId = 2, Gender = "Nam", CreatedAt = DateTime.Now.AddDays(-2) },
                    new AppUser { Email = "parent2@example.com", FullName = "Chị Tư", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "PhuHuynh", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", PhoneNumber = "0123456789", Address = "123 Đường Cây Dừa, Xã Cây Cau, Huyện Cây Chuối, Tỉnh Bến Tre", Gender = "Nữ", CreatedAt = DateTime.Now.AddDays(-7) },
                    new AppUser { Email = "parent3@example.com", FullName = "Chị Năm", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "PhuHuynh", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", PhoneNumber = "0123456789", Address = "123 Đường Cây Dừa, Xã Cây Cau, Huyện Cây Chuối, Tỉnh Bến Tre", Gender = "Nữ" },
                    new AppUser { Email = "parent4@example.com", FullName = "Chị Sáu", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "PhuHuynh", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", PhoneNumber = "0123456789", Address = "123 Đường Cây Dừa, Xã Cây Cau, Huyện Cây Chuối, Tỉnh Bến Tre", Gender = "Nữ" },

                    new AppUser { Email = "parent5@example.com", FullName = "Chị Ba", Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256), Role = "PhuHuynh", Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg", PhoneNumber = "0123456789", Address = "123 Đường Cây Dừa, Xã Cây Cau, Huyện Cây Chuối, Tỉnh Bến Tre", Gender = "Nữ" },
                    new AppUser
                    {
                        Email = "doctornhi@example.com",
                        FullName = "Nhi",
                        Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256),
                        Role = "BacSi",
                        Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg",
                        SpecialityId = 3,
                        Gender = "Nữ"
                    },
            new AppUser
            {
                Email = "doctortamly@example.com",
                FullName = "Bác sĩ Tâm Lý",
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256),
                Role = "BacSi",
                Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg",
                SpecialityId = 4,
                Gender = "Nam"
            },
            new AppUser
            {
                Email = "doctordinhduong@example.com",
                FullName = "Bác sĩ Dinh Dưỡng",
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123456", HashType.SHA256),
                Role = "BacSi",
                Avatar = "https://res.cloudinary.com/dpv6ag6bd/image/upload/v1741011756/uploads/z6371167496504_2db428e17a8859153b0704bcaa604017.jpg",
                SpecialityId = 5,
                Gender = "Nữ"
            }
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
                    new Service { SpecialityId = 2, ServiceName = "Khám vip", Description = "Skin disease consultation", Price = 700000 },
        // Dịch vụ Nhi khoa
                    new Service { SpecialityId = 3, ServiceName = "Khám thường có BHYT", Description = "Khám sức khỏe cơ bản cho trẻ", Price = 500000 },
                    new Service { SpecialityId = 3, ServiceName = "Khám thường không có BHYT", Description = "Khám sức khỏe cơ bản cho trẻ", Price = 700000 },
                    new Service { SpecialityId = 3, ServiceName = "Khám vip", Description = "Khám chuyên sâu cho trẻ", Price = 1000000 },

                            // Dịch vụ Tâm lý học
                    new Service { SpecialityId = 4, ServiceName = "Khám thường có BHYT", Description = "Tư vấn cá nhân cho trẻ", Price = 600000 },
                    new Service { SpecialityId = 4, ServiceName = "Khám thường không có BHYT", Description = "Tư vấn tâm lý cho phụ huynh và trẻ", Price = 800000 },
                    new Service { SpecialityId = 4, ServiceName = "Khám vip", Description = "Tư vấn tình trạng học tập, tâm lý trường lớp", Price = 750000 },

                    // Dịch vụ Dinh dưỡng
                    new Service { SpecialityId = 5, ServiceName = "Khám thường có BHYT", Description = "Tư vấn ăn uống khoa học theo độ tuổi", Price = 500000 },
                    new Service { SpecialityId = 5, ServiceName = "Khám thường không có BHYT", Description = "Đánh giá và lên kế hoạch dinh dưỡng", Price = 900000 },
                    new Service { SpecialityId = 5, ServiceName = "Khám vip", Description = "Định hướng điều chỉnh cân nặng", Price = 850000 }
                );
                context.SaveChanges();
            }

            if (!context.ChildrenRecords.Any())
            {
                context.ChildrenRecords.AddRange(
                    new ChildrenRecord { UserId = 6, FullName = "Bé Thế Anh", Dob = new DateTime(2005, 6, 1), Gender = "Nam", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 6, FullName = "Bé Lộc", Dob = new DateTime(2013, 6, 1), Gender = "Nữ", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 10, FullName = "Bé Nhàn", Dob = new DateTime(2021, 6, 1), Gender = "Nam", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 10, FullName = "Bé Lương", Dob = new DateTime(2020, 6, 1), Gender = "Nữ", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 11, FullName = "Bé Quốc", Dob = new DateTime(2015, 6, 1), Gender = "Nam", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 11, FullName = "Bé Quân", Dob = new DateTime(2017, 6, 1), Gender = "Nữ", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 12, FullName = "Bé Qui", Dob = new DateTime(2018, 6, 1), Gender = "Nam", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 12, FullName = "Bé Lâm", Dob = new DateTime(2019, 6, 1), Gender = "Nữ", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 13, FullName = "Bé Thắng", Dob = new DateTime(2015, 6, 1), Gender = "Nam", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 13, FullName = "Bé Hào", Dob = new DateTime(2016, 6, 1), Gender = "Nữ", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 6, FullName = "Bé Thịnh", Dob = new DateTime(2018, 6, 1), Gender = "Nam", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now },
                    new ChildrenRecord { UserId = 13, FullName = "Bé Trí", Dob = new DateTime(2021, 6, 1), Gender = "Nữ", MedicalHistory = "Dị ứng HSH", CreatedAt = DateTime.Now }
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
                    new Schedule { UserId = 2, WorkDay = DateTime.Now, WorkHourId = 2 },
                    new Schedule { UserId = 2, WorkDay = DateTime.Now, WorkHourId = 3 },
                    new Schedule { UserId = 2, WorkDay = DateTime.Now, WorkHourId = 4 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now, WorkHourId = 1 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now, WorkHourId = 2 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now, WorkHourId = 3 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now, WorkHourId = 4 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now.AddDays(-1), WorkHourId = 1 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now.AddDays(-2), WorkHourId = 2 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now.AddDays(1), WorkHourId = 3 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now.AddDays(1), WorkHourId = 4 },
                    new Schedule { UserId = 3, WorkDay = DateTime.Now.AddDays(1), WorkHourId = 5 }
                );
                context.SaveChanges();
            }

            if (!context.Appointments.Any())
            {
                context.Appointments.AddRange(
                   new Appointment
                   {
                       DoctorId = 2,
                       ParentId = 6,
                       ScheduleId = 1,
                       ChildrenRecordId = 1,
                       Status = "ORDERED",
                       ServiceId = 1,
                       TotalAmount = 550_000,
                       CheckupDateTime = new DateTime(2024, 11, 10),
                       CreatedAt = DateTime.Now
                   },
                   new Appointment { DoctorId = 2, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 750_000, CheckupDateTime = new DateTime(2025, 12, 5), CreatedAt = DateTime.Now.AddDays(21) },
                   new Appointment { DoctorId = 2, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 1_600_000, CheckupDateTime = new DateTime(2024, 9, 15), CreatedAt = DateTime.Now.AddDays(7), ExaminationReportId = 1 },
                   new Appointment { DoctorId = 2, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", ServiceId = 1, TotalAmount = 520_000, CheckupDateTime = new DateTime(2025, 8, 20), CreatedAt = DateTime.Now.AddDays(7) },
                   new Appointment { DoctorId = 3, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 680_000, CheckupDateTime = new DateTime(2024, 9, 10), CreatedAt = DateTime.Now.AddDays(7) },
                   new Appointment { DoctorId = 3, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 6_750_000, CheckupDateTime = new DateTime(2025, 1, 5), CreatedAt = DateTime.Now.AddDays(14) },
                   new Appointment { DoctorId = 3, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", ServiceId = 1, TotalAmount = 600_000, CheckupDateTime = new DateTime(2025, 9, 15), CreatedAt = DateTime.Now.AddDays(14), ExaminationReportId = 1 },
                   new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 520_000, CheckupDateTime = new DateTime(2025, 5, 20), CreatedAt = DateTime.Now.AddDays(14) },
                   new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 6_680_000, CheckupDateTime = new DateTime(2025, 6, 10), CreatedAt = DateTime.Now.AddDays(7) },
                   new Appointment { DoctorId = 4, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 750_000, CheckupDateTime = new DateTime(2025, 4, 5), CreatedAt = DateTime.Now.AddDays(21) },
                   new Appointment { DoctorId = 5, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 600_000, CheckupDateTime = new DateTime(2024, 1, 15), CreatedAt = DateTime.Now.AddDays(21), ExaminationReportId = 1 },
                   new Appointment { DoctorId = 5, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 2_520_000, CheckupDateTime = new DateTime(2025, 2, 20), CreatedAt = DateTime.Now.AddDays(28) },
                   new Appointment { DoctorId = 5, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", ServiceId = 1, TotalAmount = 680_000, CheckupDateTime = new DateTime(2025, 3, 10), CreatedAt = DateTime.Now.AddDays(28) },
                   new Appointment { DoctorId = 7, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 1_750_000, CheckupDateTime = new DateTime(2025, 1, 5), CreatedAt = DateTime.Now.AddDays(28) },
                   new Appointment { DoctorId = 7, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 1_600_000, CheckupDateTime = new DateTime(2025, 1, 15), CreatedAt = DateTime.Now.AddDays(3), ExaminationReportId = 1 },
                   new Appointment { DoctorId = 7, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", ServiceId = 1, TotalAmount = 520_000, CheckupDateTime = new DateTime(2024, 8, 20), CreatedAt = DateTime.Now.AddDays(1) },
                   new Appointment { DoctorId = 8, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 680_000, CheckupDateTime = new DateTime(2023, 4, 10), CreatedAt = DateTime.Now.AddDays(2) },
                   new Appointment { DoctorId = 8, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 2_750_000, CheckupDateTime = new DateTime(2025, 12, 5), CreatedAt = DateTime.Now.AddDays(-1) },
                   new Appointment { DoctorId = 8, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", ServiceId = 1, TotalAmount = 600_000, CheckupDateTime = new DateTime(2025, 9, 15), CreatedAt = DateTime.Now.AddDays(-1), ExaminationReportId = 1 },
                   new Appointment { DoctorId = 9, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 520_000, CheckupDateTime = new DateTime(2025, 6, 20), CreatedAt = DateTime.Now.AddDays(-2) },
                   new Appointment { DoctorId = 9, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 680_000, CheckupDateTime = new DateTime(2025, 7, 10), CreatedAt = DateTime.Now.AddDays(-2) },
                   new Appointment { DoctorId = 9, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", ServiceId = 1, TotalAmount = 750_000, CheckupDateTime = new DateTime(2025, 12, 5), CreatedAt = DateTime.Now.AddDays(-3) },
                   new Appointment { DoctorId = 14, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", ServiceId = 1, TotalAmount = 4_600_000, CheckupDateTime = new DateTime(2025, 5, 15), CreatedAt = DateTime.Now, ExaminationReportId = 1 },
                   new Appointment { DoctorId = 14, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "ORDERED", ServiceId = 1, TotalAmount = 520_000, CheckupDateTime = new DateTime(2024, 2, 20), CreatedAt = DateTime.Now },
                   new Appointment { DoctorId = 14, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "CHECKED_IN", ServiceId = 1, TotalAmount = 680_000, CheckupDateTime = new DateTime(2025, 3, 10), CreatedAt = DateTime.Now });

                //new Appointment { DoctorId = 3, ParentId = 6, ScheduleId = 1, ChildrenRecordId = 1, Status = "COMPLETED", TotalAmount = 720_000, CheckupDateTime = new DateTime(2024, 10, 1), CreatedAt = DateTime.Now },

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
                        new Medicine { Name = "Furosemide", Price = 7 }
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
}


