using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Shared.Constants
{
    public class SystemConstant
    {
        public enum Gender
        {
            Male,
            Female,
            Other
        }

        public enum AccountsRole
        {
            Manager,
            Doctor,
            Parent
        }

        public enum AppointmentStatus
        {
            Ordered,
            CheckedIn,
            Cancelled,
            Completed,
            ReExamination
        }

        public enum PaymentMethod
        {
            VNPAY,
            PayPal,
            MoMo
        }

        public enum PrescriptionSlot
        {
            Day,
            Noon,
            Night
        }
    }
}
