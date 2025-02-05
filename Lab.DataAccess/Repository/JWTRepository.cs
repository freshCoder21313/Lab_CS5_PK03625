using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DataAccess.Repository
{
    public class JWTRepository : IJWTRepository
    {
        private readonly AppSetting _appSetting;
        private readonly IUnitOfWork _unit;

        public JWTRepository(IOptionsMonitor<AppSetting> optionsMonitor, IUnitOfWork unit)
        {
            _appSetting = optionsMonitor.CurrentValue;
            _unit = unit;
        }
        public string GenerateToken(tblNhanVien nhanVien) // Hàm tạo mã Token từ tài khoản đăng nhập thành công
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nhanVien.TenDangNhap),
                    new Claim(ClaimTypes.Role, nhanVien.VaiTro),
                    new Claim("RandomAccess", (new Random().NextDouble() > 0.5).ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5), // Dùng 5 phút
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
        public ClaimsPrincipal TakeDataToken(string token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            SecurityToken validatedToken;
            ClaimsPrincipal principal = jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

            return principal;
        }
    }
}
