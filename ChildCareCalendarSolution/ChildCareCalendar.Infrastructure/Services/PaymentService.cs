using ChildCareCalendar.Domain.Entities;
using ChildCareCalendar.Infrastructure.Repository;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _repository;

        public PaymentService(IRepository<Payment> repository)
        {
            _repository = repository;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            await _repository.AddAsync(payment);
            return payment;
        }
    }
}
