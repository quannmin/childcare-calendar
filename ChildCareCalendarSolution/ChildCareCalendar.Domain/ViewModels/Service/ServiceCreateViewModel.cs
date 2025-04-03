using System.ComponentModel.DataAnnotations;

namespace ChildCareCalendar.Domain.ViewModels.ServiceVM
{
	public class ServiceCreateViewModel
	{
		[Required(ErrorMessage = "Tên dịch vụ không được để trống.")]
		[MaxLength(255, ErrorMessage = "Tên dịch vụ không được quá 255 ký tự.")]
		public string? ServiceName { get; set; }
		[Required(ErrorMessage = "Giá dịch vụ không được để trống.")]
		[Range(0, double.MaxValue, ErrorMessage = "Giá dịch vụ không được nhỏ hơn 0")]
		public double Price { get; set; }
		public string? Description { get; set; }
		public int SpecialityId { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? CreatedBy { get; set; }
	}
}
