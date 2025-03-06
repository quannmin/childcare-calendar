using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using Org.BouncyCastle.Utilities;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectSpecialty
    {
        [Parameter]
        public EventCallback<Speciality> OnSpecialtySelected { get; set; }
        private IJSObjectReference module = default!;
        [Inject] IJSRuntime JS { get; set; } = default!;

        private List<Speciality> Specialties = new()
    {
        new Speciality {Id = 1, SpecialtyName = "CAN THIỆP MẠCH MÁU - U GAN", Description = "", CreatedAt = DateTime.Now },
        new Speciality {Id = 2, SpecialtyName = "CAN THIỆP TIM MẠCH - DSA", Description = "", CreatedAt = DateTime.Now },
        new Speciality {Id = 3, SpecialtyName = "CƠ XƯƠNG KHỚP", Description = "(Triệu chứng: Đau các khớp tay - chân / Đau lưng / Trật khớp / Bong gân / Gãy xương...)", CreatedAt = DateTime.Now },
        new Speciality {Id = 4, SpecialtyName = "DA LIỄU", Description = "(Triệu chứng: Ngứa ngoài da / Da tróc vảy / Nổi mẩn đỏ / Sạm da / Mụn / Nấm tóc...)", CreatedAt = DateTime.Now },
        new Speciality {Id = 5, SpecialtyName = "CAN THIỆP MẠCH MÁU - U GAN", Description = "", CreatedAt = DateTime.Now },
        new Speciality {Id = 6, SpecialtyName = "CAN THIỆP TIM MẠCH - DSA", Description = "", CreatedAt = DateTime.Now },
        new Speciality {Id = 7, SpecialtyName = "CƠ XƯƠNG KHỚP", Description = "(Triệu chứng: Đau các khớp tay - chân / Đau lưng / Trật khớp / Bong gân / Gãy xương...)", CreatedAt = DateTime.Now },
        new Speciality {Id = 8, SpecialtyName = "DA LIỄU", Description = "(Triệu chứng: Ngứa ngoài da / Da tróc vảy / Nổi mẩn đỏ / Sạm da / Mụn / Nấm tóc...)", CreatedAt = DateTime.Now }
    };

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if(firstRender)
        //    {
        //        Console.WriteLine("Component Initialized");
        //        await JS.InvokeVoidAsync("console.log", "JS Runtime is working");

        //        module = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/Pages/AppointmentBooking/SelectSpecialty.razor.js");
        //        await module.InvokeVoidAsync("specialtyBlazorInterface", DotNetObjectReference.Create(this));
        //    }
        //}



        private async void HandleSpecialtySelection(Speciality speciality)
        {
            Console.WriteLine("Selected Specialty: " + speciality.SpecialtyName);
            await OnSpecialtySelected.InvokeAsync(speciality);
        }

    }
}