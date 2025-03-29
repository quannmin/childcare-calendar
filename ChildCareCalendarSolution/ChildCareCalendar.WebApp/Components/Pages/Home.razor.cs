using AutoMapper;
using ChildCareCalendar.Utilities.Helper;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using static ChildCareCalendar.Utilities.Constants.SystemConstant;

namespace ChildCareCalendar.WebApp.Components.Pages
{
	public partial class Home
	{
		private List<UserViewModel> Doctors { get; set; } = new();
		[Inject]
		private IUserService UserService { get; set; } = default!;
		[Inject]
		private IMapper Mapper { get; set; } = default!;

		protected override async Task OnInitializedAsync()
		{
			var listDoctor = await UserService.FindUsersAsync(x => !x.IsDelete && x.Role == AccountsRole.BacSi.ToString(), x => x.Speciality);
			var topDoctors = listDoctor
				.OrderByDescending(x => x.DoctorAppointments != null ? x.DoctorAppointments.Count : 0)
				.Take(4)
				.ToList();
			Doctors = Mapper.Map<List<UserViewModel>>(topDoctors);
		}
	}
}
