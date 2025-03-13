using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public decimal Amount { get; set; }



        // Thuộc tính riêng của VNPAY
        public string? VnPayResponseCode { get; set; }

        // Thuộc tính riêng của PayPal
        public string? PayerId { get; set; }
    }
}
