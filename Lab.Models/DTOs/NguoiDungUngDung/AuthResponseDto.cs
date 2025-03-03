using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Models.DTOs.NguoiDungUngDung
{

    public class AuthResponseDto
    {
        public string? Token { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
