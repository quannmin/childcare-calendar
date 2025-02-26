namespace ChildCareCalendar.Domain.ViewModels.Service
{
	public class ServiceViewModel
	{
		public int Id { get; set; }
		public string? ServiceName { get; set; }
		public double Price { get; set; }
		public string? Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? CreatedBy { get; set; }
		public bool IsDelete { get; set; } = false;
	}
}
