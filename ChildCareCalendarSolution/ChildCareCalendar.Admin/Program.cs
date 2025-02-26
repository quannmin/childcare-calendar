using ChildCareCalendar.Admin.Components;
using ChildCareCalendar.Admin.Extensions;
using ChildCareCalendar.Domain.EF;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Pubs.BackendApi.Mappings;
using ChildCareCalendar.Domain.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String: {connectionString}");

builder.Services.AddDbContext<ChildCareCalendarContext>(options => {
    options.UseSqlServer(connectionString);
	options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
// Add services to the container.
builder.Services.AddDependencyInjection();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Seed(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
