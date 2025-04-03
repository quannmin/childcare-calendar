using AutoMapper;
using ChildCareCalendar.Domain.ViewModels.Account;
using ChildCareCalendar.Domain.ViewModels.Schedule;
using ChildCareCalendar.Domain.ViewModels.WorkHour;
using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.JSInterop;
using static ChildCareCalendar.Utilities.Constants.SystemConstant;

namespace ChildCareCalendar.Admin.Components.Pages.Schedule
{
	public partial class Index
	{
		private List<ScheduleViewModel> Schedules = new();
		private List<WorkHourViewModel> WorkHours = new();
		private List<UserViewModel> Doctors = new();
		[Inject]
		private IScheduleService ScheduleService { get; set; } = default!;
		[Inject]
		private IWorkHourService WorkHourService { get; set; } = default!;
		[Inject]
		private IUserService UserService { get; set; } = default!;
		[Inject]
		private IMapper Mapper { get; set; } = default!;
		[Inject]
		IJSRuntime JS { get; set; } = default!;
		private static readonly string[] dayNames = { "CN", "T2", "T3", "T4", "T5", "T6", "T7" };
		private string popupPositionStyle = "";
		private string ErrorMessage = "";

		private ScheduleCreateViewModel CreateViewModel = new();
		private List<ScheduleViewModel> DetailViewModel = new();
		private ScheduleEditViewModel EditViewModel = new();
		private DateTime currentDate = DateTime.Today;
		private DateTime now = DateTime.Today;
		private DateTime? selectedDate = null;

		private bool isPopupOpen = false;
		private bool isPopupCreateOpen = false;
		private bool isPopupDetailOpen = false;
		private bool isPopupEditOpen = false;
		private bool isPopupDeleteOpen = false;

		private int? idToDelete;

		[BindProperty(SupportsGet = true)]
		public int PageNumber { get; set; } = 1;

		public int PageSize { get; set; } = 5;
		public int TotalPages { get; set; }

		private List<CalendarDay> DaysInMonth => GetDaysInMonth(currentDate);
		protected override async Task OnInitializedAsync()
		{
			await LoadData();

		}

		private async Task LoadData()
		{
			var listSchedules = await ScheduleService.FindSchedulesAsync(x => !x.IsDelete, x => x.Doctor);
			Schedules = Mapper.Map<List<ScheduleViewModel>>(listSchedules);

			var listWorkHours = await WorkHourService.FindWorkHoursAsync(x => !x.IsDelete);
			WorkHours = Mapper.Map<List<WorkHourViewModel>>(listWorkHours);

			var listDoctors = await UserService.FindUsersAsync(x => !x.IsDelete && x.Role == AccountsRole.BacSi.ToString());
			Doctors = Mapper.Map<List<UserViewModel>>(listDoctors);

			if (selectedDate != null)
			{
				var detailsSchedule = await ScheduleService.FindSchedulesAsync(x =>
					!x.IsDelete &&
					x.WorkDay.Value.Date == selectedDate.Value.Date,
					x => x.WorkHour, x => x.Doctor);
				DetailViewModel = Mapper.Map<List<ScheduleViewModel>>(detailsSchedule);
			}
		}

		private void PrevMonth()
		{
			currentDate = currentDate.AddMonths(-1);
		}

		private void NextMonth()
		{
			currentDate = currentDate.AddMonths(1);
		}

		private List<CalendarDay> GetDaysInMonth(DateTime date)
		{
			var days = new List<CalendarDay>();
			int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
			DateTime firstDay = new(date.Year, date.Month, 1);
			int startingDay = (int)firstDay.DayOfWeek;

			for (int i = 0; i < startingDay; i++)
			{
				days.Add(new CalendarDay());
			}

			for (int day = 1; day <= daysInMonth; day++)
			{
				DateTime cellDate = new(date.Year, date.Month, day);
				var schedules = Schedules.Where(x => x.WorkDay?.ToString("dd/MM/yyyy") == cellDate.Date.ToString("dd/MM/yyyy")).ToList();
				days.Add(new CalendarDay { Day = day, Schedules = schedules });
			}

			return days;
		}

		private void OpenPopupAsync(int day, Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
		{
			if (day > 0)
			{
				selectedDate = new DateTime(currentDate.Year, currentDate.Month, day);
				isPopupOpen = true;
				popupPositionStyle = $"position: fixed; left: {e.ClientX - 280}px; top: {e.ClientY + 10}px;";
				StateHasChanged();
			}
		}

		private void OpenCreatePopupAsync()
		{
			CreateViewModel.WorkDay = selectedDate;
			isPopupCreateOpen = true;
			isPopupDetailOpen = false;
			isPopupOpen = false;
			isPopupDeleteOpen = false;
			isPopupEditOpen = false;
			StateHasChanged();
		}

		private async void OpenDetailPopupAsync()
		{
			var query = await ScheduleService.FindSchedulesAsync(x =>
				!x.IsDelete &&
				x.WorkDay.Value.Date == selectedDate.Value.Date,
				x => x.WorkHour, x => x.Doctor);
			int totalRecords = query.Count();
			TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

			var listSchedules = query
				.Skip((PageNumber - 1) * PageSize)
				.Take(PageSize)
				.ToList();

			DetailViewModel = Mapper.Map<List<ScheduleViewModel>>(listSchedules);
			isPopupDetailOpen = true;
			isPopupOpen = false;
			isPopupCreateOpen = false;
			isPopupDeleteOpen = false;
			isPopupEditOpen = false;
			StateHasChanged();
		}

		private void ChangePage(int newPageNumber)
		{
			PageNumber = newPageNumber;
			if (PageNumber > TotalPages)
			{
				PageNumber = TotalPages;
			}

			if (PageNumber < 1)
			{
				PageNumber = 1;
			}
			OpenDetailPopupAsync();
		}
		private async void OpenEditPopupAsync(int id)
		{
			var listSchedules = await ScheduleService.FindSchedulesAsync(x =>
				!x.IsDelete &&
				x.Id == id,
				x => x.WorkHour, x => x.Doctor);
			EditViewModel = Mapper.Map<ScheduleEditViewModel>(listSchedules.FirstOrDefault());
			isPopupDetailOpen = false;
			isPopupOpen = false;
			isPopupCreateOpen = false;
			isPopupDeleteOpen = false;
			isPopupEditOpen = true;
			StateHasChanged();
		}

		private void ClosePopup()
		{
			ErrorMessage = "";

			isPopupCreateOpen = false;
			isPopupOpen = false;
			isPopupDetailOpen = false;
			isPopupEditOpen = false;
			isPopupDeleteOpen = false;

			CreateViewModel = new();
			DetailViewModel = new();
			selectedDate = null;
			StateHasChanged();
		}

		private async void HandleCreate()
		{
			ErrorMessage = "";
			var workHours = await WorkHourService.FindWorkHoursAsync(x => !x.IsDelete && x.Id == CreateViewModel.WorkHourId);
			var workHour = workHours.FirstOrDefault();
			bool check = false;

			if (workHour != null && CreateViewModel != null)
				check = await CheckIsOccupied(workHour.StartTime, workHour.EndTime, CreateViewModel.WorkDay.Value);
			else return;
			if (check)
			{
				ErrorMessage = "Bác sĩ đã có lịch trong khung giờ này";
				StateHasChanged();
				return;
			}

			await ScheduleService.AddScheduleAsync(Mapper.Map<Domain.Entities.Schedule>(CreateViewModel));
			ClosePopup();
			CreateViewModel = new();
			await LoadData();
			StateHasChanged();
		}

		private async void HandleUpdate()
		{
			ErrorMessage = "";
			var workHours = await WorkHourService.FindWorkHoursAsync(x => !x.IsDelete && x.Id == EditViewModel.WorkHourId);
			var workHour = workHours.FirstOrDefault();
			bool check = false;

			if (workHour != null && EditViewModel != null)
				check = await CheckIsOccupiedWithoutId(workHour.StartTime, workHour.EndTime, EditViewModel.WorkDay.Value, EditViewModel.Id);
			else return;
			if (check)
			{
				ErrorMessage = "Bác sĩ đã có lịch trong khung giờ này";
				StateHasChanged();
				return;
			}

			await ScheduleService.UpdateScheduleAsync(Mapper.Map<Domain.Entities.Schedule>(EditViewModel));
			CreateViewModel = new();
			isPopupCreateOpen = false;
			isPopupOpen = false;
			isPopupDetailOpen = true;
			isPopupEditOpen = false;
			isPopupDeleteOpen = false;
			await LoadData();
			OpenDetailPopupAsync();
			StateHasChanged();
		}

		private async Task<bool> CheckIsOccupied(TimeSpan startTime, TimeSpan endTime, DateTime date)
		{
			var existSchedules = await ScheduleService.FindSchedulesAsync(x => x.WorkDay.Value.Date == date.Date && x.UserId == CreateViewModel.UserId, e => e.WorkHour);
			if (existSchedules.Any())
			{
				foreach (var schedule in existSchedules)
				{
					if ((schedule.WorkHour.StartTime < startTime && schedule.WorkHour.EndTime > startTime)
					|| (schedule.WorkHour.StartTime < endTime && schedule.WorkHour.EndTime > endTime)
					|| (schedule.WorkHour.StartTime == startTime && schedule.WorkHour.EndTime == endTime))
					{
						return true;
					}
				}

			}
			return false;
		}

		private async Task<bool> CheckIsOccupiedWithoutId(TimeSpan startTime, TimeSpan endTime, DateTime date, int id)
		{
			var existSchedules = await ScheduleService.FindSchedulesAsync(x => x.WorkDay.Value.Date == date.Date && x.UserId == EditViewModel.UserId && x.Id != id, e => e.WorkHour);
			if (existSchedules.Any())
			{
				foreach (var schedule in existSchedules)
				{
					if ((schedule.WorkHour.StartTime < startTime && schedule.WorkHour.EndTime > startTime)
					|| (schedule.WorkHour.StartTime < endTime && schedule.WorkHour.EndTime > endTime)
					|| (schedule.WorkHour.StartTime == startTime && schedule.WorkHour.EndTime == endTime))
					{
						return true;
					}
				}

			}
			return false;
		}

		private async void OpenDeletePopup(int id)
		{
			idToDelete = id;
			isPopupDetailOpen = false;
			isPopupOpen = false;
			isPopupCreateOpen = false;
			isPopupEditOpen = false;
			isPopupDeleteOpen = true;
			StateHasChanged();
		}

		private async Task DeleteSchedule()
		{
			if (idToDelete.HasValue)
			{
				await ScheduleService.DeleteScheduleAsync(idToDelete.Value);
				idToDelete = null;
				await LoadData();
			}
			isPopupDetailOpen = true;
			isPopupOpen = false;
			isPopupCreateOpen = false;
			isPopupEditOpen = false;
			isPopupDeleteOpen = false;
			StateHasChanged();
		}
	}
}
