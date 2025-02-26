using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(params Expression<Func<Appointment, object>>[] includes);
        Task<Appointment> GetAppointmentByIdAsync(int id);
        Task AddAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(int id);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAppointmentsByChildrenRecordIdAsync(int childrenRecordId);
        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<IEnumerable<Appointment>> GetAppointmentsByParentIdAsync(int parentId);
        Task ChangeAppointmentStatusAsync(int appointmentId, string status);
        Task CancelAppointmentAsync(int appointmentId);
        Task<IEnumerable<Appointment>> FindAppointmentAsync(string keyword);
        Task<IEnumerable<Appointment>> FindAppointmentsAsync(Expression<Func<Appointment, bool>> predicate,
                                                                params Expression<Func<Appointment, object>>[] includes);
    }
}
