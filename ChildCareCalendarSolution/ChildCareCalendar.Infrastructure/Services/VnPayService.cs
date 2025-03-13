using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using ChildCareCalendar.Utilities.Libraries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(Appointment model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = $"{context.Request.Scheme}://{context.Request.Host}/PaymentCallback";

            // Lấy số tiền từ `Appointment`
            var amount = ((int)model.TotalAmount * 100).ToString();

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", amount);
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Appointment: {model.Id} - Doctor: {model.DoctorId}");
            pay.AddRequestData("vnp_OrderType", "Healthcare");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public Payment PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = new Payment();

            // Kiểm tra dữ liệu trả về từ VNPay
            if (collections.Count == 0)
                throw new Exception("VNPay response is empty!");

            var vnpSecureHash = collections["vnp_SecureHash"];
            var isValidSignature = pay.ValidateSignature(vnpSecureHash, _configuration["Vnpay:HashSecret"]);

            if (!isValidSignature)
            {
                response.Success = false;
                response.OrderDescription = "Invalid signature";
                return response;
            }

            response.TransactionId = collections["vnp_TransactionNo"];
            response.OrderId = collections["vnp_TxnRef"];
            response.VnPayResponseCode = collections["vnp_ResponseCode"];
            response.PaymentMethod = "VNPay";

            // Lấy số tiền và chia cho 100
            if (collections.ContainsKey("vnp_Amount"))
            {
                response.Amount = Convert.ToDecimal(collections["vnp_Amount"]) / 100;
            }

            response.Success = collections["vnp_ResponseCode"] == "00"; // "00" là thành công
            return response;
        }
    }
}
