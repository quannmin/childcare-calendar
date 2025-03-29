using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Domain.EF;
using ChildCareCalendar.WebApp.Components;
using Microsoft.EntityFrameworkCore;
using ChildCareCalendar.Infrastructure.Extensions;
using ChildCareCalendar.Infrastructure.Mappings;
using Microsoft.AspNetCore.Components;
using ChildCareCalendar.Domain.ViewModels;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String: {connectionString}");

builder.Services.AddDbContext<ChildCareCalendarContext>(options => {
	options.UseSqlServer(connectionString);
	options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<CircuitOptions>(options =>
{
    options.DetailedErrors = true;
});
builder.Services.AddDependencyInjection();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAntDesign();
builder.Services.AddScoped(sp =>
{
    var navManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navManager.BaseUri) };
});
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<IPayPalService, PayPalService>();
builder.Services.AddHttpContextAccessor();

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
