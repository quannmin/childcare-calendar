using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels.Refundreport
{
    public class RefundReportViewModel
    {
        public int RefundReportId { get; set; }
        public decimal RefundAmount { get; set; }
        public DateTime RefundDate { get; set; }
        public double RefundPercentage { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
