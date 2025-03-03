using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models.DTOs.NguoiDungUngDung
{
    public class RegisterDto
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? Password { get; set; }

        public List<string>? Roles { get; set; }
    }
}
