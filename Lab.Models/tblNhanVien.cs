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
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường TenDangNhap")]
        [RegularExpression("[a-z0-9]",ErrorMessage = "Vui lòng tạo tên đăng nhập với các kí tự nằm trong phạm vi a-z0-9.")]
        [Length(6, 9, ErrorMessage = "Vui lòng tạo tên đăng nhập với độ dài từ 6-9.")]
        public string TenDangNhap { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường MatKhau")]
        [RegularExpression("[a-z0-9]", ErrorMessage = "Vui lòng tạo mật khẩu với các kí tự nằm trong phạm vi a-z0-9.")]
        [Length(6, 9, ErrorMessage = "Vui lòng tạo mật khẩu với độ dài từ 6-9.")]
        public string MatKhau { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường VaiTro")]
        public string VaiTro { get; set; }
    }
}
