using Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository.IRepository
{
    public interface IJWTRepository
    {
        string GenerateToken(tblNhanVien nhanVien);
        ClaimsPrincipal TakeDataToken(string token);
    }
}
