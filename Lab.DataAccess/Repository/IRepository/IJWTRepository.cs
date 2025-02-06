using Lab.Models;
using Lab.Models.ViewModels;
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
        TokenVM GenerateToken(tblNhanVien nhanVien);
        ClaimsPrincipal TakeDataToken(string token);
        string GenerateRefreshToken(string idUser);
        Task<TokenVM> RefreshToken(string refreshToken);
        ClaimsPrincipal ValidateRefreshToken(string refreshToken);
    }
}
