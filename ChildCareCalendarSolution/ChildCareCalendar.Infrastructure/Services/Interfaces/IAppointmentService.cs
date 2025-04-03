using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(params Expression<Func<Appointment, object>>[] includes);
        Task<Appointment> GetAppointmentByIdAsync(int id, params Expression<Func<Appointment, object>>[] includes);
        Task AddAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(int id);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task ChangeAppointmentStatusAsync(int appointmentId, string status);
        Task CancelAppointmentAsync(int appointmentId);
        Task<IEnumerable<Appointment>> FindAppointmentAsync(string keyword);
        //Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId, params Expression<Func<Appointment, object>>[] includes);
        Task<IEnumerable<Appointment>> FindAppointmentsAsync(Expression<Func<Appointment, bool>> predicate,
                                                                params Expression<Func<Appointment, object>>[] includes);
        Task<(IEnumerable<Appointment> appointments, int totalCount)> GetPagedAppointmentsAsync(
        int pageIndex,
        int pageSize,
        string keyword = null);
        Task<(IEnumerable<Appointment> appointments, int totalCount)> GetPagedAppointmentsByDoctorIdAsync(
        int doctorId, int pageIndex, int pageSize, string keyword = null);
        Task<Appointment?> FindLatestAppointmentAsync(int parentId, int doctorId);
        Task<List<Domain.Entities.Appointment>> GetAppointmentsByDoctorIdAndDateTimeAsync(
           int doctorId,
           DateTime date,
           TimeSpan startTime,
           TimeSpan endTime);

    }
}