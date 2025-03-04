using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.PrescriptionDetail
{
    public class PrescriptionDetailSearchViewModel
    {
        public int? Id { get; set; }
        public int? MedicineId { get; set; }
        public int? ExaminationReportId { get; set; }
        public bool? IsDelete { get; set; }
    }
}
