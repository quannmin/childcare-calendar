using ChildCareCalendar.Domain.EF;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ChildCareCalendarContext>
    {
        public ChildCareCalendarContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChildCareCalendarContext>();
            optionsBuilder.UseSqlServer("DefaultConnection"); 

            return new ChildCareCalendarContext(optionsBuilder.Options);
        }
    }
}
