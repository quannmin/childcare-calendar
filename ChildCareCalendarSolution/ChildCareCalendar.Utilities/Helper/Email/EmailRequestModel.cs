using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.ViewModels
{
    public class EmailRequestModel
    {
        public string? To { get; set; }
        public string? From { get; set; }
        public string? Body {  get; set; }
    }
}
