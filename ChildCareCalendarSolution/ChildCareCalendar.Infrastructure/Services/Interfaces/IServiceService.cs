using ChildCareCalendar.Domain.Entities;
using System.Linq.Expressions;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IServiceService
    {
		Task<IEnumerable<Service>> FindServicesAsync(
			Expression<Func<Service, bool>> predicate,
			params Expression<Func<Service, object>>[] includes);
		Task AddServiceAsync(Service service);
        Task UpdateServiceAsync(Service service);
        Task DeleteServiceAsync(int id);
    }
}
