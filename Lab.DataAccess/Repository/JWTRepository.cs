﻿using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public TokenVM GenerateToken(tblNhanVien nhanVien) // Hàm tạo mã Token từ tài khoản đăng nhập thành công
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, nhanVien.MaNhanVien.ToString()),
                    new Claim(ClaimTypes.Name, nhanVien.TenDangNhap),
                    new Claim(ClaimTypes.Role, nhanVien.VaiTro),
                    new Claim(ClaimTypes.Version, "1")
                }),
                Expires = DateTime.UtcNow.AddMinutes(10), // Dùng 5 phút
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var refreshToken = GenerateRefreshToken(nhanVien.MaNhanVien.ToString());

            return new TokenVM
            {
                AccessToken = jwtTokenHandler.WriteToken(token),
                RefreshToken = refreshToken
            };
        }
        public async Task<ClaimsPrincipal> TakeDataTokenAsync(string token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            ClaimsPrincipal principal = jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);


            return principal;
        }

        public string GenerateRefreshToken(string idUser)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, idUser )
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Refresh token sống 7 ngày
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
        public async Task<TokenVM> RefreshToken(string refreshToken)
        {
            // Kiểm tra refresh token có hợp lệ không
            var principal = ValidateRefreshToken(refreshToken);
            if (principal == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nhanVien = await _unit.NhanViens.GetAsync(u => u.MaNhanVien.ToString() == userId);

            if (nhanVien == null)
            {
                throw new SecurityTokenException("Invalid user");
            }

            // Tạo mới access token và refresh token
            return GenerateToken(nhanVien);
        }
        public ClaimsPrincipal ValidateRefreshToken(string refreshToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            ClaimsPrincipal principal = jwtTokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out validatedToken);

            return principal;
        }
        public async Task<TokenVM> ChangeVersionAccessToken(string accessToken)
        {
            // Validate the token and extract claims
            var principal = await TakeDataTokenAsync(accessToken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // You should now invalidate the current token by adding a version claim or a timestamp claim
            var nhanVien = await _unit.NhanViens.GetAsync(u => u.MaNhanVien.ToString() == userId);

            if (nhanVien == null)
            {
                throw new SecurityTokenException("Invalid user");
            }

            // Here you may want to increment a version number (or any strategy you wish to implement)
            var currentVersion = principal.FindFirst("Version")?.Value;
            var newVersion = (int.Parse(currentVersion) + 1).ToString();

            // Generate a new token with the updated version claim
            var newToken = GenerateToken(nhanVien);
            newToken.AccessToken = newToken.AccessToken.Replace(currentVersion, newVersion); // This is not a valid way to update JWT, just illustrative.

            return newToken;
        }

    }
}
