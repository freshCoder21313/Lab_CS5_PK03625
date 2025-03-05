using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.ViewModels;
using Lab.Services.Redis;
using Lab.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TruyCapController : ControllerBase
    {
        private readonly AppSetting _appSetting;
        private readonly IUnitOfWork _unit;
        private readonly IJWTRepository _jwt;
        private readonly TokenService _tokenService;

        public TruyCapController(IOptionsMonitor<AppSetting> optionsMonitor, IUnitOfWork unit, IJWTRepository jwt, TokenService tokenService)
        {
            _appSetting = optionsMonitor.CurrentValue;
            _unit = unit;
            _jwt = jwt;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Đăng nhập tài khoản nhân viên
        /// </summary>
        /// <remarks>
        ///     POST
        ///         {
        ///           "userName": "noname0",
        ///           "password": "nopass0"
        ///         }
        /// </remarks>
        /// <param name="dangNhap">Thông tin đăng nhập. (Vui lòng chạy mã /api/Manager/NhanVien/Get để có thông tin tài khoản có thể truy cập.) [username: noname0] - [password: nopass0]</param>
        /// <returns>Đăng nhập tài khoản nhân viên để lấy được mã Token truy cập</returns>
        [HttpPost]
        public async Task<IActionResult> DangNhap([FromBody] DangNhap dangNhap)
        {
            if (dangNhap == null || string.IsNullOrWhiteSpace(dangNhap.UserName) || string.IsNullOrWhiteSpace(dangNhap.Password))
            {
                return BadRequest(new { success = false, message = "Thông tin truy cập không hợp lệ." });
            }

            var userLogin = await _unit.NguoiDungs.GetAsync(x => x.UserName == dangNhap.UserName);
            if (userLogin == null)
            {
                return Unauthorized(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không chính xác." });
            }

            var passwordHasher = new PasswordHasher<NguoiDungUngDung>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(userLogin, userLogin.PasswordHash!, dangNhap.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không chính xác." });
            }

            TokenVM tokenVM = _jwt.GenerateToken(userLogin);
            await _tokenService.StoreTokenAsync(userLogin.Id.ToString(), tokenVM.AccessToken!, TimeSpan.FromMinutes(15));

            return Ok(new { success = true, data = tokenVM });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// {
        ///   "userName": "supernam",
        ///   "password": "123QWEqwe@",
        ///   "email": "supername@gmail.com",
        ///   "gioiTinh": true,
        ///   "hoTen": "Văn Nhân",
        ///   "soDienThoai": "0999934999",
        ///   "diaChi": "string",
        ///   "ngaySinh": "2025-03-05T15:35:30.840Z"
        /// }
        /// </remarks>
        /// <param name="dangKyDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DangKy([FromBody] DangKy dangKyDTO)
        {
            if (dangKyDTO == null || string.IsNullOrWhiteSpace(dangKyDTO.UserName) ||
                string.IsNullOrWhiteSpace(dangKyDTO.Email) || string.IsNullOrWhiteSpace(dangKyDTO.Password))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin đăng ký không hợp lệ, vui lòng kiểm tra lại."
                });
            }

            var response = await _unit.NguoiDungs.DangKyAsync(dangKyDTO);

            if (!response.Success!.Value)
            {
                return StatusCode(response.Status!.Value, new
                {
                    success = false,
                    message = response.Message,
                    errors = response.Data
                });
            }

            return Ok(new
            {
                success = true,
                message = response.Message,
                userId = response.Data
            });
        }

        /// <summary>
        /// Xem dữ liệu được lưu trong claims sau khi đăng nhập với tài khoản nhân viên
        /// </summary>
        /// <param name="token">Mã token nhận được sau khi đăng nhập.</param>
        /// <returns>Xem dữ liệu được lưu trong claims sau khi đăng nhập với tài khoản nhân viên</returns>
        [Authorize]
        [HttpGet("ValidateToken/{token}")]
        public IActionResult ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new { success = false, message = "Vui lòng nhập mã token." });
            }

            try
            {
                var principal = _jwt.TakeDataTokenAsync(token);
                return Ok(new { success = true, claims = principal.Claims.Select(c => new { c.Type, c.Value }) });
            }
            catch (Exception)
            {
                return Unauthorized(new { success = false, message = "Token không hợp lệ hoặc đã hết hạn." });
            }
        }

        /// <summary>
        /// Làm mới access token bằng refresh token
        /// </summary>
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new { success = false, message = "Vui lòng nhập mã refresh token." });
            }

            try
            {
                var principal = _jwt.ValidateRefreshToken(refreshToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _unit.NguoiDungs.GetAsync(u => u.Id.ToString() == userId);
                if (user == null)
                {
                    return Unauthorized(new { success = false, message = "Refresh token không hợp lệ." });
                }

                TokenVM newTokenVM = _jwt.GenerateToken(user);
                return Ok(new { success = true, token = newTokenVM });
            }
            catch (Exception)
            {
                return Unauthorized(new { success = false, message = "Không thể làm mới token." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await _tokenService.RevokeTokenAsync(userId);
                return Ok(new { success = true, message = "Đăng xuất thành công." });
            }

            return BadRequest(new { success = false, message = "Token không hợp lệ." });
        }
    }
}