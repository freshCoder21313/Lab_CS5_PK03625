using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.Models.DTOs.Role
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập tên role tạo.")]
        public string RoleName {get;set;} = null!;
    }
}