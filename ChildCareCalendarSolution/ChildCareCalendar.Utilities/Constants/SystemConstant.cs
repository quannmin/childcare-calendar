using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildCareCalendar.Utilities.Constants
{
    public class SystemConstant
    {
        public enum Gender
        {
            Nam,
            Nữ,
            Khác
        }
        public enum AccountsRole
        {
            [Display(Name = "Quản lý")]
            QuanLy,

            [Display(Name = "Bác sĩ")]
            BacSi,

            [Display(Name = "Phụ huynh")]
            PhuHuynh
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
