using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab.API.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public SanPhamController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _unit.SanPhams.GetAllAsyncVM());
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

            return Ok(alterSortByName);
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

            return Ok(alterActionSanPham);
        }
    }
}
