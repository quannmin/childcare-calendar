using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace ChildCareCalendar.Infrastructure.Extensions
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
		{
			services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

			services.AddTransient<ISpecialityService, SpecialityService>();
			services.AddTransient<IScheduleService, ScheduleService>();
			services.AddTransient<IServiceService, ServiceService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IAppointmentService, AppointmentService>();
			services.AddTransient<IRefundReportService, RefundReportService>();
			services.AddTransient<IMedicineService, MedicineService>();
			services.AddTransient<IChildrenRecordService, ChildrenRecordService>();
			services.AddTransient<IEmailService, EmailService>();
			services.AddTransient<IExaminationReportService, ExaminationReportService>();
			services.AddSingleton<CloudinaryService>();
			services.AddTransient<IWorkHourService, WorkHourService>();
			services.AddSingleton<CloudinaryService>();
			return services;
		}
	}
}
