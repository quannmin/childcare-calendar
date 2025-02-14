using ChildCareCalendar.Domain.Entities;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetServicesBySpecialityIdAsync(int id);
        Task<Service> GetServiceByIdAsync(int id);
        Task AddServiceAsync(Service service);
        Task UpdateServiceAsync(Service service);
        Task DeleteServiceAsync(int id);
    }
}
