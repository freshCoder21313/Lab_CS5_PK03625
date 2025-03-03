using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models
{
    public class tblSanPham
    {
        [Key]
        public int MaSanPham { get; set; }
        public string? TenSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal DonGia { get; set; }
    }
}
