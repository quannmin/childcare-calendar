using BraintreeHttp;
using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PayPal.Core;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Payment = PayPal.v1.Payments.Payment;


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
            try
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
                        Total = amountInUsd.ToString("F2"),
                        Currency = "USD"
                    },
                    InvoiceNumber = paypalOrderId.ToString()
                }
            },
                    RedirectUrls = new RedirectUrls()
                    {
                        ReturnUrl = $"{urlCallBack}?payment_method=PayPal&success=1&order_id={paypalOrderId}",
                        CancelUrl = $"{urlCallBack}?payment_method=PayPal&success=0&order_id={paypalOrderId}"
                    },
                    Payer = new Payer()
                    {
                        PaymentMethod = "paypal"
                    }
                };

                var request = new PaymentCreateRequest();
                request.RequestBody(payment);

                try
                {
                    Console.WriteLine("🟢 Gửi request đến PayPal API...");

                    var response = await client.Execute(request);

                    Console.WriteLine("✅ Nhận phản hồi từ PayPal API...");
                    var statusCode = response.StatusCode;
                    Console.WriteLine($"🟢 PayPal API Status: {statusCode}");

                    if (statusCode != HttpStatusCode.Created)
                    {
                        Console.WriteLine("❌ PayPal API không tạo được giao dịch!");
                        return "";
                    }

                    var result = response.Result<PayPal.v1.Payments.Payment>();
                    Console.WriteLine($"🔍 Full PayPal Response: {System.Text.Json.JsonSerializer.Serialize(result)}");

                    foreach (var link in result.Links)
                    {
                        Console.WriteLine($"🔗 PayPal Link: {link.Rel} - {link.Href}");
                        if (link.Rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase))
                        {
                            return link.Href;
                        }
                    }

                    return "";
                }
                catch (HttpException ex)
                {
                    Console.WriteLine($"❌ Lỗi HTTP khi gọi PayPal API: {ex.StatusCode} - {ex.Message}");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi trong quá trình khởi tạo PayPal Payment URL: {ex.Message}");
                Console.WriteLine($"❌ Chi tiết lỗi: {ex.StackTrace}");
                return "";
            }
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
                    case "paymentid":
                        response.PaymentId = value;
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
