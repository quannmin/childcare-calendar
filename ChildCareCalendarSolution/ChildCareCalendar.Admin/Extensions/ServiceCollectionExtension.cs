using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Helper;

namespace ChildCareCalendar.Admin.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            services.AddTransient<ISpecialityService, SpecialityService>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<CloudinaryService>();
            return services;
        }
    }
}
