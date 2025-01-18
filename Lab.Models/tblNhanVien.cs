using System.ComponentModel.DataAnnotations;

namespace Lab.Models
{
    public class tblNhanVien
    {
        [Key]
        public int MaNhanVien { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường HoTen")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường SoDienThoai")]
        public string SoDienThoai { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường NgaySinh")]
        public DateTime NgaySinh { get; set; }
    }
}
