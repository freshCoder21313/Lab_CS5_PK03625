using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Utility.SharedData
{
    public static class PaymentStatus
    {

        //Tình trạng đơn hàng
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static readonly Dictionary<string, string> OrderStatuConstantsictionary = new Dictionary<string, string>
        {
            { "Pending", "Đang chờ xử lý" },
            { "Approved", "Đã duyệt" },
            { "Processing", "Đang xử lý" },
            { "Shipped", "Đã giao hàng" },
            { "Cancelled", "Đã hủy" },
            { "Refunded", "Đã hoàn tiền" }
        };

        //Tình trạng thanh toán
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatuConstantselayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";
    }
}
