using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Medicie
{
    public class MedicineSearchViewModel
    {
        [MaxLength(255, ErrorMessage = "Từ khóa tối đa 255 ký tự.")]
        public string Keyword { get; set; }
    }
}
