using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab.API.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly AppSetting _appSetting;
        private readonly IUnitOfWork _unit;
        public SanPhamController(IOptionsMonitor<AppSetting> optionsMonitor, IUnitOfWork unit)
        {
            _appSetting = optionsMonitor.CurrentValue;
            _unit = unit;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string token)
        {
            try
            {
                var principal = TakeDataToken(token);
                return Ok(new
                {
                    success = true,
                    data = await _unit.SanPhams.GetAllAsyncVM(),
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
        /// Lấy dữ liệu danh sách sản phẩm được sắp xếp theo tên tăng dần
        /// </summary>
        /// <returns> Lấy dữ liệu danh sách sản phẩm được sắp xếp theo tên tăng dần </returns>
        [HttpGet]
        public async Task<IActionResult> ManualSortByName([FromQuery] string token)
        {
            try
            {
                var principal = TakeDataToken(token);
                List<tblSanPham> sanPhams = _unit.SanPhams.GetAllAsync().Result.ToList();

                List<tblSanPham> alterSortByName = sanPhams.OrderBy(x => x.TenSanPham).ToList();

                return Ok(new
                {
                    success = true,
                    data = alterSortByName,
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
        /// Lọc theo khoảng giá và sắp xếp theo id giảm dần
        /// </summary>
        /// <param name="giaNhoNhat">Hãy chọn giá nhỏ nhất.</param>
        /// <param name="giaLonNhat">Hãy chọn giá lớn nhất.</param>
        /// <returns>Lọc theo khoảng giá và sắp xếp theo id giảm dần</returns>
        [HttpGet]
        public async Task<IActionResult> HandFilterByPriceAndManualSortByPrice(double giaNhoNhat, double giaLonNhat, [FromQuery] string token)
        {
            try
            {
                var principal = TakeDataToken(token);
                List<tblSanPham> sanPhams = _unit.SanPhams.GetAllAsync().Result.ToList();

                List<tblSanPham> alterActionSanPham = sanPhams.Where(sp => sp.DonGia > giaNhoNhat && sp.DonGia < giaLonNhat)
                                                              .OrderByDescending(x => x.DonGia)
                                                              .ToList();

                return Ok(new
                {
                    success = true,
                    data = alterActionSanPham,
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
