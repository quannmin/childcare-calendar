using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IRepository<Service> _serviceRepository;

        public ServiceService(IRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task AddServiceAsync(Service service)
        {
            await _serviceRepository.AddAsync(service);
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service != null)
            {
                await _serviceRepository.DeleteAsync(service);
            }
        }

        public async Task<Service> GetServiceByIdAsync(int id)
        {
            return await _serviceRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Service>> GetServicesBySpecialityIdAsync(int id)
        {
            return await _serviceRepository.FindAsync(x => x.SpecialityId == id);
        }

        public async Task UpdateServiceAsync(Service service)
        {
            await _serviceRepository.UpdateAsync(service, service.Id);
        }
    }
}
