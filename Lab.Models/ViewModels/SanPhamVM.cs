﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models.ViewModels
{
    public class SanPhamVM
    {
        public int MaSanPham { get; set; }
        public string? TenSanPham { get; set; } = "N/A";
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }
}
