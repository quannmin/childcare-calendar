using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChildCareCalendar.WebApp.Components.Pages.AppointmentBooking
{
    public partial class SelectPayment
    {
        [Parameter] public EventCallback<string> OnPaymentSelected { get; set; }
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private string? selectedPaymentMethod;
        private bool CanProceed => !string.IsNullOrEmpty(selectedPaymentMethod);

        private async Task ProcessPayment()
        {
            if (!CanProceed)
                return;

            Console.WriteLine($"Processing payment with method: {selectedPaymentMethod}");

            if (OnPaymentSelected.HasDelegate)
            {
                await OnPaymentSelected.InvokeAsync(selectedPaymentMethod);
            }
            else
            {
                Console.WriteLine("OnPaymentSelected is not assigned!");
            }
        }
    }
}
