using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository
{
    public class PaymentRepository : Repository<DonHang>, IPaymentRepository
    {
        private ApplicationDbContext _db;
        public PaymentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ResponseAPI<dynamic>> CreatePayment(string userId, IEnumerable<GioHang> gioHangs)
        {
            ResponseAPI<dynamic> response = new ResponseAPI<dynamic>();

            var chiTiets = gioHangs.Select(gh => new ChiTietDonHang
            {
                SoLuong = gh.SoLuong,
                DonGia = gh.DonGia
            }).ToList();


            // Xử lý callback thanh toán (giả định hoàn tất thành công)
            var order = new DonHang
            {
                NguoiDungId = userId,
                NgayDatHang = DateTime.Now,
                TongTienDonHang = 30000,
                TrangThaiThanhToan = "Đã thanh toán",
                TenNguoiNhan = "Nguyễn Văn A",
                Duong = "Đường 1",
                ThanhPho = "TP.HCM",
                SoDienThoai = "0123456789",
                MaPhienThanhToan = "sampleTransactionId"
            };

            await _db.AddAsync(order);
            await _db.SaveChangesAsync();

            foreach (var obj in chiTiets)
            {
                obj.DonHangId = order.MaDonHang;
            }
            await _db.AddRangeAsync(chiTiets);
            await _db.SaveChangesAsync();

            // Tính tổng tiền
            var totalAmount = chiTiets.Sum(c => c.SoLuong * c.DonGia);

            return new ResponseAPI<dynamic>
    {
                Success = true,
                Status = 200,
                Data = totalAmount,
                Message = "Đã tạo đường dẫn thanh toán thành công."
            };
        }
    }
}
