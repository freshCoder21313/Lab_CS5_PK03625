using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruyCapController : ControllerBase
    {
        private readonly AppSetting _appSetting;
        private readonly IUnitOfWork _unit;

        public TruyCapController(IOptionsMonitor<AppSetting> optionsMonitor, IUnitOfWork unit)
        {
            _appSetting = optionsMonitor.CurrentValue;
            _unit = unit;
        }

        /// <summary>
        /// Đăng nhập tài khoản nhân viên
        /// </summary>
        /// <param name="dangNhap">Thông tin đăng nhập. (Vui lòng chạy mã /api/Manager/NhanVien/Get để có thông tin tài khoản có thể truy cập.)</param>
        /// <returns>Đăng nhập tài khoản nhân viên để lấy được mã Token truy cập</returns>
        [HttpPost]
        public async Task<IActionResult> DangNhap([FromBody]DangNhap dangNhap)
        {
            if (dangNhap is null || string.IsNullOrWhiteSpace(dangNhap.UserName) || string.IsNullOrWhiteSpace(dangNhap.Password))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin truy cập không hợp lệ, vui lòng điền thông tin truy cập phù hợp."
                });
            }

            var UserLogin = await _unit.NhanViens.GetAsync(x => x.TenDangNhap == dangNhap.UserName && x.MatKhau == dangNhap.Password);

            if (UserLogin != null)
            {
                return Ok(new
                {
                    success = true,
                    message = GenerateToken(UserLogin)
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

        [HttpGet("ValidateToken/{token}")]
        public IActionResult ValidateToken(string token)
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
                var principal = TakeDataToken(token);
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
        
        private string GenerateToken(tblNhanVien nhanVien) // Hàm tạo mã Token từ tài khoản đăng nhập thành công
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

        private ClaimsPrincipal TakeDataToken(string token)
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
            var principal = jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

            return principal;
        }
    }
}
