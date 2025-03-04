using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Services.VnPay.Response
{
    public class VnPayResponse
    {
        public bool Success { get; set; }

        public string? PaymentMethod { get; set; }
        public string? OrderDescription { get; set; }
        public string? OrderId { get; set; }
        public string? PaymentId { get; set; }
        public string? TransactionId { get; set; }
        public string? Token { get; set; }
        public string? VnPayResponseCode { get; set; }
    }

    public class VnPayRequest
    {
        public string? OrderId { get; set; }
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
