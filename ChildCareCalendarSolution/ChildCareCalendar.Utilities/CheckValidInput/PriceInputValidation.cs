using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Utilities.CheckValidInput
{
    public class PriceInputValidation : ValidationAttribute
    {
        public PriceInputValidation()
        {
            ErrorMessage = "Giá thuốc không hợp lệ. Hãy nhập số từ 1 đến 100 triệu.";

        }
        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            if (value is decimal price)
            {
                return price >= 0 && price <= 100_000_000;
            }

            return false;
        }
    }
}