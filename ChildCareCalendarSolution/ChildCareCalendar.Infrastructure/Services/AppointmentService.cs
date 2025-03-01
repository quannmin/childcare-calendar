using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
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
    }
}
