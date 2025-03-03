using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab.Models
{
    public class NguoiDungUngDung : IdentityUser
    {
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường HoTen")]
        public string? HoTen { get; set; } = string.Empty;
        [Display(Name = "Giới Tính")]
        public bool? GioiTinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường SoDienThoai")]
        public string? SoDienThoai { get; set; } = string.Empty;

        [StringLength(1024, ErrorMessage = "Địa chỉ không được vượt quá 1024 ký tự.")]
        [Display(Name = "Địa Chỉ")]
        [RegularExpression(ValidationConstants.ValidateString, ErrorMessage = "Địa chỉ chỉ được chứa chữ cái, số và khoảng trắng.")]
        [Required(ErrorMessage = "Vui lòng nhập thông tin địa chỉ người dùng.")]
        public string DiaChi { get; set; }

        [StringLength(255, ErrorMessage = "Link ảnh không được vượt quá 255 ký tự.")]
        [ValidateNever]
        [Display(Name = "Link Ảnh")]
        public string? LinkAnh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập đủ thông tin trường NgaySinh")]
        public DateTime NgaySinh { get; set; }
        [NotMapped]
        [Display(Name = "Vai Trò")]
        public string? VaiTro { get; set; }
    }
}
