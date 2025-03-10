﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models
{
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }
        public string? NguoiDungId { get; set; }
        public DateTime NgayDatHang { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal TongTienDonHang { get; set; }
        public string? TrangThaiThanhToan { get; set; } = string.Empty;
        public string? TenNguoiNhan { get; set; } = string.Empty;
        public string? Duong { get; set; } = string.Empty;
        public string? ThanhPho { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; } = string.Empty;
        public string? MaPhienThanhToan { get; set; } = string.Empty;
        public IEnumerable<ChiTietDonHang>? ChiTietDonHangs { get; set; }
    }

}
