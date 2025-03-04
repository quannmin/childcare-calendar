using ChildCareCalendar.Infrastructure.Services;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.WebApp.Components;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//var cultureInfo = new CultureInfo("vi-VN");
//CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
//CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.UseRequestLocalization(new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture("vi-VN"),
//    SupportedCultures = new List<CultureInfo> { cultureInfo },
//    SupportedUICultures = new List<CultureInfo> { cultureInfo }
//});

app.Run();
