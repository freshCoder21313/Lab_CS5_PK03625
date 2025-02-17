using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models.DTOs.DonHang
{
    public class DonHangDTO
    {
        public int MaDonHang { get; set; }
        public string? NguoiDungId { get; set; } = string.Empty;
        public DateTime NgayDatHang { get; set; }
        public decimal TongTienDonHang { get; set; }
        public string? TrangThaiThanhToan { get; set; } = string.Empty;
        public string? TenNguoiNhan { get; set; } = string.Empty;
        public string? Duong { get; set; } = string.Empty;
        public string? ThanhPho { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; } = string.Empty;
        public string? MaPhienThanhToan { get; set; } = string.Empty;
    }
}
