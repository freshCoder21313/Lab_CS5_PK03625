using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab2_CS5_PK03625.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly AppSetting _appSetting;
        private readonly IUnitOfWork _unit;
        public NhanVienController(IOptionsMonitor<AppSetting> optionsMonitor, IUnitOfWork unit)
        {
            _appSetting = optionsMonitor.CurrentValue;
            _unit = unit;
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <param name="token">Mã token của người dùng đăng nhập.</param>
        /// <returns>Lấy toàn bộ dữ liệu</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string token)
        {
            try
            {
                var principal = TakeDataToken(token);
                return Ok(new
                {
                    success = true,
                    data = (await _unit.NhanViens.GetAllAsync()).ToList(),
                    message = $"Bạn đang đăng nhập với tài khoản mang tên {principal.FindFirst(ClaimTypes.Name)?.Value ?? "Undefined"}"
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
        /// Lấy người dùng theo ID. 
        /// </summary> 
        /// <param name="id">ID của người dùng.</param>
        /// <param name="token">Mã token của người dùng đăng nhập.</param>
        /// <returns>Thông tin người dùng.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string token)
        {
            try
            {
                var principal = TakeDataToken(token);
                return Ok(new
                {
                    success = true,
                    data = await _unit.NhanViens.GetAsync(x => x.MaNhanVien == id),
                    message = $"Bạn đang đăng nhập với tài khoản mang tên {principal.FindFirst(ClaimTypes.Name)?.Value ?? "Undefined"}"
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
        /// Thêm dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="token">Mã token của người dùng đăng nhập.</param>
        /// <returns>Thêm dữ liệu dữ liệu 1 đối tượng</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NhanVienDTO nhanVien, [FromQuery] string token)
        {
            try
            {
                var principal = TakeDataToken(token);

                string yourRole = principal.FindFirst(ClaimTypes.Role)?.Value ?? "Undefined";
                if (yourRole != "Admin")
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"Bạn không có quyền thực hiện hành động này vì vai trò của bạn là {yourRole}"
                    });
                }
                string[] roles = new string[] { "User", "Admin" };
                if (!roles.Contains(nhanVien.VaiTro))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Vui lòng tạo tài khoản với vai trò là 'User' hoặc 'Admin'!"
                    });
                }

                tblNhanVien nv = new tblNhanVien
                {
                    HoTen = nhanVien.HoTen,
                    SoDienThoai = nhanVien.SoDienThoai,
                    NgaySinh = nhanVien.NgaySinh,
                    TenDangNhap = nhanVien.TenDangNhap,
                    MatKhau = nhanVien.MatKhau,
                    VaiTro = nhanVien.VaiTro
                };
                await _unit.NhanViens.AddAsync(nv);

                await _unit.SaveAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Bạn đang đang thực hiện hành động này với với vai trò là {yourRole}.\n" +
                    $"Đã thêm dữ liệu nhân viên mang mã: {nv.MaNhanVien}"
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
        /// Cập nhập dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="id">ID của người dùng.</param>
        /// <param name="token">Mã token của người dùng đăng nhập.</param>
        /// <returns>Cập nhập dữ liệu dữ liệu 1 đối tượng theo id</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NhanVienDTO nhanVien, [FromQuery] string token)
        {
            try
            {
                var principal = TakeDataToken(token);

                string yourRole = principal.FindFirst(ClaimTypes.Role)?.Value ?? "Undefined";
                if (yourRole != "Admin")
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"Bạn không có quyền thực hiện hành động này vì vai trò của bạn là {yourRole}"
                    });
                }

                string[] roles = new string[] { "User", "Admin" };
                if (!roles.Contains(nhanVien.VaiTro))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Vui lòng tạo tài khoản với vai trò là 'User' hoặc 'Admin'!"
                    });
                }

                await _unit.NhanViens.Update(id, nhanVien);
                await _unit.SaveAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Bạn đang đang thực hiện hành động này với với vai trò là {yourRole}. " +
                    $"Đã thay đổi dữ liệu nhân viên mang mã: {id}"
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
        /// Xóa dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="id">ID của người dùng.</param>
        /// <param name="token">Mã token của người dùng đăng nhập.</param>
        /// <returns>Xóa dữ liệu dữ liệu 1 đối tượng theo id</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string token)
        {
            try
            {
                var principal = TakeDataToken(token);

                string yourRole = principal.FindFirst(ClaimTypes.Role)?.Value ?? "Undefined";
                if (yourRole != "Admin")
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"Bạn không có quyền thực hiện hành động này vì vai trò của bạn là {yourRole}"
                    });
                }

                tblNhanVien? infoGoc = await _unit.NhanViens.GetAsync(x => x.MaNhanVien == id);
                if (infoGoc == default)
                {
                    return NotFound("Không tìm thấy dữ liệu muốn xóa.");
                }
                _unit.NhanViens.Remove(infoGoc);

                await _unit.SaveAsync();

                return Ok(new
                {
                    success = true,
                    data = await _unit.SanPhams.GetAllAsyncVM(),
                    message = $"Bạn đang đang thực hiện hành động này với với vai trò là {yourRole}. " +
                    $"Đã xóa dữ liệu nhân viên mang mã:  {id}"
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
