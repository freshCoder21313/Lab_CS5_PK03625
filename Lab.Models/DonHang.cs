using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models
{
    public class DonHang
    {
        public string NguoiDungId { get; set; }
        public DateTime NgayDatHang { get; set; }
        public decimal TongTienDonHang { get; set; }
        public string TrangThaiThanhToan { get; set; }
        public string TenNguoiNhan { get; set; }
        public string Duong { get; set; }
        public string ThanhPho { get; set; }
        public string SoDienThoai { get; set; }
        public string MaPhienThanhToan { get; set; }
    }

}
