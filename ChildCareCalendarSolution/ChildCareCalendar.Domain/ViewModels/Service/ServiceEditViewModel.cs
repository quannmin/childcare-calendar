using ChildCareCalendar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.ServiceVM
{
	public class ServiceEditViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Tên dịch vụ không được để trống.")]
		[MaxLength(255, ErrorMessage = "Tên dịch vụ không được quá 255 ký tự.")]
		public string? ServiceName { get; set; }
		[Required(ErrorMessage = "Giá dịch vụ không được để trống.")]
		public double Price { get; set; }
		public int SpecialityId { get; set; }
		public string? Description { get; set; }
	}
}
