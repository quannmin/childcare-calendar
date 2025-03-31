using ChildCareCalendar.Domain.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ChildCareCalendar.Domain
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ChildCareCalendarContext>
    {
        public ChildCareCalendarContext CreateDbContext(string[] args)
        {
            // Tìm đường dẫn đến thư mục chứa appsettings.json (thuộc Admin)
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "../ChildCareCalendar.Admin");

            // Load cấu hình từ appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ChildCareCalendarContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new ChildCareCalendarContext(optionsBuilder.Options);
        }
    }
}
