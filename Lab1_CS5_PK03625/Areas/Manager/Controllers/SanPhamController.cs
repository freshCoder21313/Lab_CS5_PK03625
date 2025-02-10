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
    //[Authorize]
    public class SanPhamController : ControllerBase
    {
        private readonly AppSetting _appSetting;
        private readonly IUnitOfWork _unit;
        private readonly IJWTRepository _jwt;
        public SanPhamController(IOptionsMonitor<AppSetting> optionsMonitor, IUnitOfWork unit, IJWTRepository jwt)
        {
            _appSetting = optionsMonitor.CurrentValue;
            _unit = unit;
            _jwt = jwt;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new
            {
                success = true,
                data = await _unit.SanPhams.GetAllAsyncVM()
            });
        }
        /// <summary>
        /// Lấy dữ liệu danh sách sản phẩm được sắp xếp theo tên tăng dần
        /// </summary>
        /// <returns> Lấy dữ liệu danh sách sản phẩm được sắp xếp theo tên tăng dần </returns>
        [HttpGet]
        public async Task<IActionResult> ManualSortByName()
        {
            List<tblSanPham> sanPhams = _unit.SanPhams.GetAllAsync().Result.ToList();

            List<tblSanPham> alterSortByName = sanPhams.OrderBy(x => x.TenSanPham).ToList();

            return Ok(new
            {
                success = true,
                data = alterSortByName
            });
        }
        /// <summary>
        /// Lọc theo khoảng giá và sắp xếp theo id giảm dần
        /// </summary>
        /// <param name="giaNhoNhat">Hãy chọn giá nhỏ nhất.</param>
        /// <param name="giaLonNhat">Hãy chọn giá lớn nhất.</param>
        /// <returns>Lọc theo khoảng giá và sắp xếp theo id giảm dần</returns>
        [HttpGet]
        public async Task<IActionResult> HandFilterByPriceAndManualSortByPrice(double giaNhoNhat, double giaLonNhat)
        {
            List<tblSanPham> sanPhams = _unit.SanPhams.GetAllAsync().Result.ToList();

            List<tblSanPham> alterActionSanPham = sanPhams.Where(sp => sp.DonGia > giaNhoNhat && sp.DonGia < giaLonNhat)
                                                          .OrderByDescending(x => x.DonGia)
                                                          .ToList();

            return Ok(new
            {
                success = true,
                data = alterActionSanPham
            });
        }

    }
}
