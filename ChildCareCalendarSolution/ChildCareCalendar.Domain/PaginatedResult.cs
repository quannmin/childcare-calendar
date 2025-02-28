using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResult(List<T> items, int totalItems, int pageIndex, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
    }
}
