﻿namespace ChildCareCalendar.Domain.ViewModels.Specility
{
	public class SpecialityDetailViewModel
	{
		public int Id { get; set; }
		public string? SpecialtyName { get; set; }
		public string? Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? CreatedBy { get; set; }
		public bool IsDelete { get; set; } = false;
	}
}
