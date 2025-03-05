using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Schedule
{
	public class ScheduleEditViewModel
	{
		public int Id { get; set; }
		public DateTime? WorkDay { get; set; }
		public int WorkHourId { get; set; }
		public int UserId { get; set; }
		public UserViewModel? Doctor { get; set; }

	}
}
