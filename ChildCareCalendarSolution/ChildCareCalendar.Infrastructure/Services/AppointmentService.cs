using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            if (appointment != null && appointment.Status.Equals("Ordered"))
            {
                int hoursSinceBooking = (DateTime.Now - appointment.CreatedAt).Hours;

                if (hoursSinceBooking < 24)
                {
                    RefundReport refundReport = new RefundReport()
                    {
                        Appointment = appointment,
                        AppointmentId = appointmentId,
                        RefundPercentage = "20",
                        RefundDate = DateTime.Now,
                        RefundAmount = appointment.TotalAmount / 100 * 20
                    };
                    await _refundReportService.AddRefundReportAsync(refundReport);
                }
                else if (hoursSinceBooking < 48)
                {
                    RefundReport refundReport = new RefundReport()
                    {
                        Appointment = appointment,
                        AppointmentId = appointmentId,
                        RefundPercentage = "20",
                        RefundDate = DateTime.Now,
                        RefundAmount = appointment.TotalAmount / 100 * 50
                    };
                    await _refundReportService.AddRefundReportAsync(refundReport);
                }
                else
                {
                    RefundReport refundReport = new RefundReport()
                    {
                        Appointment = appointment,
                        AppointmentId = appointmentId,
                        RefundPercentage = "20",
                        RefundDate = DateTime.Now,
                        RefundAmount = appointment.TotalAmount / 100 * 70
                    };
                    await _refundReportService.AddRefundReportAsync(refundReport);
                }
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
            Expression<Func<Appointment, bool>> filter = null;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x => (x.Parent.FullName.Contains(keyword) || x.ChildrenRecord.FullName.Contains(keyword)) && !x.IsDelete;
            }
            else
            {
                filter = x => !x.IsDelete;
            }
            return await _appointmentRepository.GetPagedAsync(pageIndex, pageSize, filter);
        }

        public async Task<(IEnumerable<Appointment> appointments, int totalCount)> GetPagedAppointmentsByDoctorIdAsync(
    int doctorId, int pageIndex, int pageSize, string keyword = null)
        {
            // Tạo bộ lọc ban đầu
            Expression<Func<Appointment, bool>> filter = x => x.DoctorId == doctorId && !x.IsDelete;

            // Thêm điều kiện tìm kiếm nếu có từ khóa
            if (!string.IsNullOrEmpty(keyword))
            {
                // Sử dụng ToLower() để tìm kiếm không phân biệt hoa thường
                string lowerKeyword = keyword.ToLower();
                filter = x => x.DoctorId == doctorId && !x.IsDelete &&
                              (x.Parent.FullName.ToLower().Contains(lowerKeyword) ||
                               x.ChildrenRecord.FullName.ToLower().Contains(lowerKeyword));
            }

            // Sử dụng Include để tải thông tin liên quan đến Parent và ChildrenRecord
            var query = _appointmentRepository.GetQueryable()
                .Where(filter)
                .Include(a => a.Parent) // Bao gồm thông tin Parent
                .Include(a => a.ChildrenRecord); // Bao gồm thông tin ChildrenRecord

            // Thực hiện phân trang
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
    }
}