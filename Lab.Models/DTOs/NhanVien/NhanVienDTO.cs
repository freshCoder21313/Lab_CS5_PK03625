
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab.Models.DTOs.NhanVien
{
    public class NhanVienDTO
    {
        [DisplayName("Mã nhân viên")]
        [Required(ErrorMessage = ValidationConstants.InvalidPositiveIntegerMessage)]
        [RegularExpression(ValidationConstants.ValidatePositiveInteger, ErrorMessage = ValidationConstants.InvalidPositiveIntegerMessage)]
        public int? MaNhanVien { get; set; } = 0;

        [DisplayName("Họ tên nhân viên")]
        [RegularExpression(ValidationConstants.ValidateStringName, ErrorMessage = ValidationConstants.InvalidStringNameMessage)]
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường Họ tên.")]
        public string? HoTen { get; set; } = string.Empty;

        [DisplayName("Số điện thoại nhân viên")]
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường Số điện thoại.")]
        [RegularExpression(ValidationConstants.ValidatePhoneNumber, ErrorMessage = ValidationConstants.InvalidPhoneNumberMessage)]
        public string? SoDienThoai { get; set; } = string.Empty;

        [DisplayName("Ngày sinh nhân viên")]
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường Ngày sinh.")]
        [CustomValidation(typeof(ValidationConstants), nameof(ValidationConstants.ValidateBirthDate))]
        public DateTime NgaySinh { get; set; } = DateTime.Now;

        [DisplayName("Tên đăng nhập nhân viên")]
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường Tên đăng nhập.")]
        [RegularExpression(ValidationConstants.ValidateUsername, ErrorMessage = ValidationConstants.InvalidUsernameMessage)]
        public string? TenDangNhap { get; set; } = string.Empty;

        [DisplayName("Mật khẩu nhân viên")]
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường Mật khẩu.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [RegularExpression(ValidationConstants.ValidateStrongPassword, ErrorMessage = ValidationConstants.InvalidStrongPasswordMessage)]
        public string? MatKhau { get; set; } = string.Empty;

        [DisplayName("Vai trò nhân viên")]
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường Vai trò.")]
        public string? VaiTro { get; set; } = string.Empty;
    }
}
