using Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface IPaymentRepository : IRepository<DonHang>
    {
        Task<ResponseAPI<dynamic>> CreatePayment(int userId, IEnumerable<GioHang> gioHangs);
    }
}
