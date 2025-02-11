using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models.DTOs.NhanVien
{
    public class NhanVienDTO
    {
        public int MaNhanVien { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường HoTen")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường SoDienThoai")]
        public string SoDienThoai { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường NgaySinh")]
        public DateTime NgaySinh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường TenDangNhap")]
        public string TenDangNhap { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường MatKhau")]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường VaiTro")]
        public string VaiTro { get; set; }
    }
}
