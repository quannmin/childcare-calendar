using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Appointment;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static ChildCareCalendar.Utilities.Constants.SystemConstant;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRefundReportService _refundReportService;
        private readonly IRepository<Appointment> _appointmentRepository;

        public AppointmentService(IRepository<Appointment> appointmentRepository, IRefundReportService refundReportService)
        {
            _appointmentRepository = appointmentRepository;
            _refundReportService = refundReportService;
        }
        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepository.AddAsync(appointment);
        }
        public async Task<IEnumerable<Appointment>> FindAppointmentsAsync(Expression<Func<Appointment, bool>> predicate,
                                                                   params Expression<Func<Appointment, object>>[] includes)
        {
            return await _appointmentRepository.FindAsync(predicate, includes);
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await GetAppointmentByIdAsync(appointmentId);
            if (appointment != null && appointment.Status.Trim().Equals("Ordered", StringComparison.OrdinalIgnoreCase))
            {
                int hoursSinceBooking = (int)(DateTime.Now - appointment.CreatedAt).TotalHours;

                string refundPercentage;
                decimal refundAmount;

                if (hoursSinceBooking < 24)
                {
                    refundPercentage = "20";
                    refundAmount = appointment.TotalAmount * 0.20m;
                }
                else if (hoursSinceBooking < 48)
                {
                    refundPercentage = "50";
                    refundAmount = appointment.TotalAmount * 0.50m;
                }
                else
                {
                    refundPercentage = "70";
                    refundAmount = appointment.TotalAmount * 0.70m;
                }

                var refundReport = new RefundReport
                {
                    RefundAmount = refundAmount,
                    RefundDate = DateTime.Now,
                    RefundPercentage = refundPercentage,
                    AppointmentId = appointmentId,
                    IsDelete = false
                };

                await _refundReportService.AddRefundReportAsync(refundReport);
            }
        }
        public async Task ChangeAppointmentStatusAsync(int appointmentId, string status)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            appointment.Status = status;
            await UpdateAppointmentAsync(appointment);
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await GetAppointmentByIdAsync(id);
            if (appointment != null)
            {
                appointment.IsDelete = true;
                await _appointmentRepository.UpdateAsync(appointment, appointment.Id);
            }
        }

        public async Task<IEnumerable<Appointment>> FindAppointmentAsync(string keyword)
        {
            return await _appointmentRepository.FindAsync(a => keyword.Contains(a.Parent.FullName) || keyword.Contains(a.ChildrenRecord.FullName),
                a => a.Doctor,
                a => a.Parent,
                a => a.ChildrenRecord);
        }

        public Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(params Expression<Func<Appointment, object>>[] includes)
        {
            return _appointmentRepository.GetAllAsync(includes);
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id, params Expression<Func<Appointment, object>>[] includes)
        {
            return await _appointmentRepository.GetByIdAsync(id, includes);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepository.UpdateAsync(appointment, appointment.Id);
        }
        public async Task<(IEnumerable<Appointment> appointments, int totalCount)> GetPagedAppointmentsAsync(
    int pageIndex,
    int pageSize,
    string keyword = null)
        {
            Expression<Func<Appointment, bool>> filter = x => !x.IsDelete;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x =>
                    !x.IsDelete &&
                    (x.Parent.FullName.Contains(keyword) || x.ChildrenRecord.FullName.Contains(keyword));
            }

            return await _appointmentRepository.GetPagedAsync(
                pageIndex,
                pageSize,
                filter,
                x => x.Parent,
                x => x.ChildrenRecord,
                x => x.Doctor,
                x => x.Service
            );
        }


        public async Task<(IEnumerable<Appointment> appointments, int totalCount)> GetPagedAppointmentsByDoctorIdAsync(
    int doctorId, int pageIndex, int pageSize, string keyword = null)
        {

            Expression<Func<Appointment, bool>> filter = x =>
            x.DoctorId == doctorId &&
            !x.IsDelete &&
            (x.Status.ToLower() == AppointmentStatus.Checked_In.ToString().ToLower() ||
             x.Status.ToLower() == AppointmentStatus.Completed.ToString().ToLower());


            if (!string.IsNullOrEmpty(keyword))
            {
                
                string lowerKeyword = keyword.ToLower();
                filter = x => x.DoctorId == doctorId &&
                     !x.IsDelete &&
                     (x.Status.ToLower() == AppointmentStatus.Checked_In.ToString().ToLower() ||
                      x.Status.ToLower() == AppointmentStatus.Completed.ToString().ToLower()) &&
                     (x.Parent.FullName.ToLower().Contains(lowerKeyword) ||
                      x.ChildrenRecord.FullName.ToLower().Contains(lowerKeyword));
            }

        
            var query = _appointmentRepository.GetQueryable()
                .Where(filter)
                .Include(a => a.Parent) 
                .Include(a => a.ChildrenRecord); 

          
            var totalCount = await query.CountAsync();
            var appointments = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (appointments, totalCount);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId, params Expression<Func<Appointment, object>>[] includes)
        {
           return await _appointmentRepository.FindAsync(a => a.DoctorId == doctorId && !a.IsDelete, includes);
        }

        public async Task<Appointment?> FindLatestAppointmentAsync(int parentId, int doctorId)
        {
            return (await _appointmentRepository.FindAsync(
                a => a.ParentId == parentId && a.DoctorId == doctorId,
                a => a.Payment
            )).OrderByDescending(a => a.CreatedAt).FirstOrDefault();
        }

        public async Task<int> GetTodayAppointmentsCountAsync()
        {
            var today = DateTime.Today;
            return await _appointmentRepository.CountAsync(a =>
                a.CheckupDateTime.Date == today && !a.IsDelete);
        }
        public async Task<int> GetWeekAppointmentsCountAsync()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(6);

            return await _appointmentRepository.CountAsync(a =>
                a.CheckupDateTime.Date >= startOfWeek &&
                a.CheckupDateTime.Date <= endOfWeek &&
                !a.IsDelete);
        }

        // Get current month's appointments count
        public async Task<int> GetMonthAppointmentsCountAsync()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return await _appointmentRepository.CountAsync(a =>
                a.CheckupDateTime.Date >= startOfMonth &&
                a.CheckupDateTime.Date <= endOfMonth &&
                !a.IsDelete);
        }

        // Get appointments by date range for charts
        public async Task<Dictionary<DateTime, int>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var appointments = await _appointmentRepository.FindAsync(a =>
                a.CheckupDateTime.Date >= startDate.Date &&
                a.CheckupDateTime.Date <= endDate.Date &&
                !a.IsDelete);

            return appointments
                .GroupBy(a => a.CheckupDateTime.Date)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Get appointments by hour for today
        public async Task<Dictionary<int, int>> GetTodayAppointmentsByHourAsync()
        {
            var today = DateTime.Today;
            var appointments = await _appointmentRepository.FindAsync(a =>
                a.CheckupDateTime.Date == today && !a.IsDelete);

            return appointments
                .GroupBy(a => a.CheckupDateTime.Hour)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // Get appointments by status
        public async Task<Dictionary<string, int>> GetAppointmentsByStatusAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            return appointments
                .Where(a => !a.IsDelete)
                .GroupBy(a => a.Status)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key ?? "Unknown", g => g.Count());
        }

        public async Task<List<RevenueViewModel>> GetMonthlyRevenue(int year)
        {
            var appointments = await GetAllAppointmentsAsync();
            var revenueData = appointments.Where(a => a.CheckupDateTime.Year == year && !a.IsDelete)
                .GroupBy(a => a.CheckupDateTime.Month)
                .Select(g => new RevenueViewModel
                {
                    Month = g.Key,
                    Revenue = g.Sum(a => a.TotalAmount)
                })
                .OrderBy(r => r.Month)
                .ToList();

            return revenueData;
        }

        public async Task<List<DailyAppointmentCountViewModel>> GetWeeklyAppointmentsAsync(DateTime startDate)
        {
            // Calculate the end date (7 days from start date)
            DateTime endDate = startDate.AddDays(7);

            // Get all appointments within the date range
            var appointments = await _appointmentRepository.FindAsync(
                a => a.CreatedAt >= startDate &&
                     a.CreatedAt < endDate &&
                     !a.IsDelete,
                a => a.Parent,
                a => a.ChildrenRecord);

            // Group appointments by day and count them
            var result = Enumerable.Range(0, 7)
                .Select(dayOffset => {
                    var currentDate = startDate.AddDays(dayOffset);
                    var count = appointments.Count(a => a.CreatedAt.Date == currentDate.Date);

                    return new DailyAppointmentCountViewModel
                    {
                        Date = currentDate,
                        DayOfWeek = currentDate.DayOfWeek.ToString(),
                        Count = count
                    };
                })
                .ToList();

            return result;
        }

    }
}