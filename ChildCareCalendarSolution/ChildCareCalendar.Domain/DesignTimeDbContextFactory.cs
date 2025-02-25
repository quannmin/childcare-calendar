//using ChildCareCalendar.Domain.EF;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.IO;

//namespace ChildCareCalendar.Domain
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ChildCareCalendarContext>
//    {
//        public ChildCareCalendarContext CreateDbContext(string[] args)
//        {
//            // Xác định đường dẫn đến appsettings.json trong Application.Web
//            string basePath = Directory.GetCurrentDirectory(); // Thư mục hiện tại
//            string appSettingsPath = Path.Combine(basePath, "/", "ChildCareCalendar.Web", "appsettings.json");

//            if (!File.Exists(appSettingsPath))
//            {
//                throw new FileNotFoundException($"Không tìm thấy file cấu hình: {appSettingsPath}");
//            }

//            // Đọc cấu hình từ appsettings.json trong Application.Web
//            var configBuilder = new ConfigurationBuilder()
//                .SetBasePath(Path.GetDirectoryName(appSettingsPath)!) // Đặt thư mục chứa appsettings.json
//                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // Tải file cấu hình

//            IConfiguration configuration = configBuilder.Build(); // Xây dựng cấu hình

//            // Lấy chuỗi kết nối từ appsettings.json
//            var connectionString = configuration.GetConnectionString("DefaultConnection");
//            if (string.IsNullOrEmpty(connectionString))
//            {
//                throw new InvalidOperationException("Connection string 'DefaultConnection' không được tìm thấy.");
//            }

//            // Cấu hình DbContext
//            var optionsBuilder = new DbContextOptionsBuilder<ChildCareCalendarContext>();
//            optionsBuilder.UseSqlServer(connectionString);

//            return new ChildCareCalendarContext(optionsBuilder.Options);
//        }
//    }
//}
