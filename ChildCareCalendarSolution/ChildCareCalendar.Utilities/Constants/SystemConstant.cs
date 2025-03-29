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
            [Display(Name = "Nam")]
            Nam,
            [Display(Name = "Nữ")]
            Nữ,
            [Display(Name = "Khác")]
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
            [Display(Name = "Đã đặt")]
            Ordered,
            [Display(Name = "Đã check in")]
            Checked_In,
            [Display(Name = "Đã hủy")]
            Cancelled,
            [Display(Name = "Hoàn thành")]
            Completed,
            [Display(Name = "Tái khám")]
            ReExamination
        }

        public enum PaymentMethod
        {
            [Display(Name = "VNPay")]
            VNPAY,
            [Display(Name = "PayPal")]
            PayPal,
            [Display(Name = "MoMo")]
            MoMo
        }

        public enum PrescriptionSlot
        {
            [Display(Name = "Sáng")]
            Day,
            [Display(Name = "Trưa")]
            Noon,
            [Display(Name = "Tối")]
            Night,
            [Display(Name = "Sáng, Trưa")]
            DayNoon,
            [Display(Name = "Sáng, Tối")]
            DayNight,
            [Display(Name = "Trưa, Tối")]
            NoonNight,
            [Display(Name = "Sáng, Trưa, Tối")]
            DayNoonNight
        }
    }
}
