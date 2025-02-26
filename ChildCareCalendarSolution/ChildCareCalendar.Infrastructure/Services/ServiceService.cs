using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System.Linq.Expressions;

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
            if (service != null && !service.IsDelete)
            {
                service.IsDelete = true;
                await _serviceRepository.UpdateAsync(service, service.Id);
            }
        }

		public async Task<IEnumerable<Service>> FindServicesAsync(
            Expression<Func<Service, bool>> predicate, 
            params Expression<Func<Service, object>>[] includes)
		{
			return await _serviceRepository.FindAsync(predicate, includes);
		}

		public async Task UpdateServiceAsync(Service service)
        {
            await _serviceRepository.UpdateAsync(service, service.Id);
        }
    }
}
