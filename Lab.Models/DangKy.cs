using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.Models
{
    public class DangKy
    {
        public string UserName {get;set;} = string.Empty;
        public string Password {get;set;} = string.Empty;
        public string Email {get;set;} = string.Empty;
        public bool? GioiTinh { get; set; }
        public string? HoTen { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; } = string.Empty;
        public string? DiaChi { get; set; } = string.Empty;
        public string? LinkAnh { get; set; }
        public DateTime NgaySinh { get; set; }
    }
}