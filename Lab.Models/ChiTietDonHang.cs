using Lab.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int Id { get; set; }
        public int DonHangId { get; set; }
        public int? SanPhamId { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        [ValidateNever]
        [ForeignKey("DonHangId")]
        public virtual DonHang? DonHang { get; set; } // Khai báo quan hệ với đơn hàng
        [ValidateNever]
        [ForeignKey("SanPhamId")]
        public virtual tblSanPham? SanPham { get; set; } // Khai báo quan hệ với sản phẩm

    }
}
