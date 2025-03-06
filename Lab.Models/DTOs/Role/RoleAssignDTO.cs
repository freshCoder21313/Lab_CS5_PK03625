using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab.Models.DTOs.Role
{
    public class RoleAssignDTO
    {
        public string UserId {get; set;} = null!;
        public string RoleId {get; set;} = null!;
    }
}