using Lab.API.Services;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.ViewModels;
using Lab.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            if (dangNhap is null || string.IsNullOrWhiteSpace(dangNhap.UserName) || string.IsNullOrWhiteSpace(dangNhap.Password))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin truy cập không hợp lệ, vui lòng điền thông tin truy cập phù hợp."
                });
            }

            var userLogin = await _unit.NhanViens.GetAsync(x => x.TenDangNhap == dangNhap.UserName && x.MatKhau == dangNhap.Password);

            if (userLogin != null)
            {
                TokenVM tokenVM = _jwt.GenerateToken(userLogin);
                await _tokenService.StoreTokenAsync(userLogin.MaNhanVien.ToString(), tokenVM.AccessToken, TimeSpan.FromSeconds(30)); //Note: Key

                return Ok(new
                {
                    success = true,
                    message = tokenVM
                });
            }

            return Unauthorized(new
            {
                success = false,
                message = "Mã xác thực không hợp lệ, quyền truy cập bị cấm."
            });
        }

        /// <summary>
        /// Xem dữ liệu được lưu trong claims sau khi đăng nhập với tài khoản nhân viên
        /// </summary>
        /// <param name="token">Mã token nhận được sau khi đăng nhập.</param>
        /// <returns>Xem dữ liệu được lưu trong claims sau khi đăng nhập với tài khoản nhân viên</returns>
        [Authorize]
        [HttpGet("ValidateToken/{token}")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Vui lòng nhập mã token."
                });
            }

            try
            {
                var principal = await _jwt.TakeDataTokenAsync(token);
                return Ok(new
                {
                    success = true,
                    claims = principal.Claims.Select(c => new { c.Type, c.Value })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Lỗi không mong muốn xảy ra. Mã Token của bạn có thể đã hết hạn hoặc không hợp lệ.",
                    error = ex.Message
                });
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
                return BadRequest(new
                {
                    success = false,
                    message = "Vui lòng nhập mã refresh token."
                });
            }

            try
            {
                var principal = _jwt.ValidateRefreshToken(refreshToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Kiểm tra xem refresh token có hợp lệ không và lấy thông tin người dùng
                var user = await _unit.NhanViens.GetAsync(u => u.MaNhanVien.ToString() == userId);
                if (user == null)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Refresh token không hợp lệ."
                    });
                }

                // Tạo mới access token và refresh token
                TokenVM newTokenVM = _jwt.GenerateToken(user);

                return Ok(new
                {
                    success = true,
                    message = newTokenVM
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Lỗi không mong muốn xảy ra khi làm mới token.",
                    error = ex.Message
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout(string accessToken)
        {
            var principal = await _jwt.TakeDataTokenAsync(accessToken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                await _tokenService.RevokeTokenAsync(accessToken); // Xóa token khỏi Redis
                return Ok(new { success = true, message = "Đăng xuất thành công." });
            }

            return BadRequest(new { success = false, message = "Token không hợp lệ." });
        }
    }
}