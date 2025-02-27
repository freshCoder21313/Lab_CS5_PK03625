namespace Lab.Utility
{
    public static class SD
    {
        public const string RefreshToken = "RefreshToken";
        public const string AccessToken = "AccessToken";
        public const string NotifyLayout = "NotifyLayout";

        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";

        public const string CartSession = "CartSession";


        //Tình trạng đơn hàng
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static readonly Dictionary<string, string> OrderStatusDictionary = new Dictionary<string, string>
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
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";
    }
}
