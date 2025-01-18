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
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường HoTen")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường SoDienThoai")]
        public string SoDienThoai { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường NgaySinh")]
        public DateTime NgaySinh { get; set; }
    }
}
