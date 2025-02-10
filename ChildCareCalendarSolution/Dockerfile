# 1. Sử dụng hình ảnh base của Microsoft chứa sẵn .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. Copy toàn bộ source code trước khi restore
COPY . ./

# 3. Chạy restore
RUN dotnet restore childcare-calendar.sln

# 4. Build và publish
RUN dotnet publish -c Release -o out

# 5. Dùng runtime để chạy ứng dụng nhẹ hơn
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# 6. Cấu hình cổng và chạy ứng dụng
EXPOSE 8080 5269 7012
CMD ["sh", "-c", "dotnet out/ChildCareCalendar.Admin.dll & dotnet out/ChildCareCalendar.WebApp.dll"]
