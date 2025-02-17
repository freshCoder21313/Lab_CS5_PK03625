using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models.DTOs.SanPham
{
    public class SanPhamDTO
    {
        public int? MaSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }
}
