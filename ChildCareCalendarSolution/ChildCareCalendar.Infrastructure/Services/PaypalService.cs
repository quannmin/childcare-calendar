using BraintreeHttp;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PayPal.Core;
using PayPal.v1.Payments;
using System.Net;



namespace ChildCareCalendar.Infrastructure.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IConfiguration _configuration;
        private const double ExchangeRate = 22_863.0;

        public PayPalService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static double ConvertVndToDollar(double vnd)
        {
            var total = Math.Round(vnd / ExchangeRate, 2);
            return total;
        }

        public async Task<string> CreatePaymentUrl(Appointment model, HttpContext context)
        {
            Console.WriteLine("✅ Bắt đầu tạo PayPal Payment URL...");

            var envSandbox = new SandboxEnvironment(
                _configuration["Paypal:ClientId"],
                _configuration["Paypal:Secret"]
            );

            Console.WriteLine($"🔍 PayPal Client ID: {_configuration["Paypal:ClientId"]}");
            Console.WriteLine($"🔍 PayPal Secret: {_configuration["Paypal:Secret"]}");

            var client = new PayPalHttpClient(envSandbox);
            Console.WriteLine("✅ PayPal Client initialized");

            var paypalOrderId = DateTime.Now.Ticks;
            var urlCallBack = $"{context.Request.Scheme}://{context.Request.Host}/PaymentCallback";

            var amountInUsd = ConvertVndToDollar((double)model.TotalAmount);
            Console.WriteLine($"🟢 Số tiền USD: {amountInUsd}");

            var payment = new PayPal.v1.Payments.Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = amountInUsd.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = amountInUsd.ToString(),
                            }
                        },
                        ItemList = new ItemList()
                        {
                            Items = new List<Item>()
                            {
                                new Item()
                                {
                                    Name = " | Order: ",
                                    Currency = "USD",
                                    Price = amountInUsd.ToString(),
                                    Quantity = 1.ToString(),
                                    Sku = "sku",
                                    Tax = "0",
                                    Url = "https://www.code-mega.com" // Url detail of Item
                                }
                            }
                        },
                        Description = $"Invoice #",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl =
                    $"{urlCallBack}?payment_method=PayPal&success=1&order_id={paypalOrderId}",
                    CancelUrl =
                    $"{urlCallBack}?payment_method=PayPal&success=0&order_id={paypalOrderId}"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            var request = new PaymentCreateRequest();
            request.RequestBody(payment);

            var paymentUrl = "";
            Console.WriteLine("Gui request den...");
            var response = await client.Execute(request);
            var statusCode = response.StatusCode;

            if (statusCode is not (HttpStatusCode.Accepted or HttpStatusCode.OK or HttpStatusCode.Created))
                return paymentUrl;

            var result = response.Result<PayPal.v1.Payments.Payment>();
            using var links = result.Links.GetEnumerator();

            while (links.MoveNext())
            {
                var lnk = links.Current;
                if (lnk == null) continue;
                if (!lnk.Rel.ToLower().Trim().Equals("approval_url")) continue;
                paymentUrl = lnk.Href;
            }

            return paymentUrl;
        }

        public Domain.Entities.Payment PaymentExecute(IQueryCollection collections)
        {
            var response = new Domain.Entities.Payment();
            string paymentMethod = collections["payment_method"]; // Lấy phương thức thanh toán

            foreach (var (key, value) in collections)
            {
                if (string.IsNullOrEmpty(value)) continue;

                switch (key.ToLower())
                {
                    case "order_description":
                        response.OrderDescription = value;
                        break;
                    case "transaction_id":
                        response.TransactionId = value;
                        break;
                    case "order_id":
                        response.OrderId = value;
                        break;
                    case "success":
                        response.Success = Convert.ToInt32(value) > 0;
                        break;
                    case "payerid":
                        response.PayerId = value;
                        break;
                    case "amount": // Nếu hệ thống trả về "amount" 
                        response.Amount = Convert.ToDecimal(value);
                        break;
                }
            }

            // Nếu không có `payment_method` trong request, kiểm tra dữ liệu VNPay
            if (string.IsNullOrEmpty(paymentMethod))
            {
                if (collections.ContainsKey("vnp_TransactionStatus"))
                {
                    paymentMethod = "VNPay";
                    response.Success = collections["vnp_TransactionStatus"] == "00"; // "00" là thành công

                    // Lấy Amount từ VNPay (phải chia cho 100 do VNPAY trả về số nhân 100)
                    if (collections.ContainsKey("vnp_Amount"))
                    {
                        response.Amount = Convert.ToDecimal(collections["vnp_Amount"]) / 100;
                    }
                }
            }

            response.PaymentMethod = paymentMethod;
            return response;
        }


    }

}
