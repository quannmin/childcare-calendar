using ChildCareCalendar.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Infrastructure.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(Appointment model, HttpContext context);
        Payment PaymentExecute(IQueryCollection collections);
    }
}
