using Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Application.Features.SanPhams.Queries
{
    public interface IPaymentRepository : IRepository<DonHang>
    {
        Task<ResponseAPI<dynamic>> CreatePayment(int userId, IEnumerable<GioHang> gioHangs);
    }
}
