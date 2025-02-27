using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Lab.Models.DTOs.SanPham;
using Lab.Models.ViewModels;
using Lab.Services.Redis.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Lab.API.Areas.Manager
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        private readonly IRedisCacheService cache;

        public SanPhamController(IUnitOfWork unit, IRedisCacheService cache)
        {
            _unit = unit;
            this.cache = cache;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var responseSp = await _unit.SanPhams.GetAsyncVM(sp => sp.MaSanPham == id);
            return Ok(responseSp);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<tblSanPham>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

            var cachingKey = $"products_{userId}";

            var responseSpVMs = cache.GetData<IEnumerable<tblSanPham>>(cachingKey);
            if (responseSpVMs is not null)
            {
                return Ok(responseSpVMs);
            }

            responseSpVMs = await _unit.SanPhams.GetAllAsync();

            cache.SetData(cachingKey, responseSpVMs);
            return Ok(responseSpVMs);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SanPhamDTO sanPhamDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseAPI<dynamic>{ Message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var response = await _unit.SanPhams.AddAsyncByDTO(sanPhamDTO);

                if (response.Success.HasValue && response.Success.Value) //Ok
                {
                    return Ok(response);
                }
                //Lỗi
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Lỗi cập nhật dữ liệu: {ex.Message}" });
            }
        }
        /// <summary>
        /// Cập nhập dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="sanPhamDTO">Dữ liệu san phẩm cập nhập.</param>

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SanPhamDTO sanPhamDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ." });
            }
            try
            {
                await _unit.SanPhams.UpdateByDTO(sanPhamDTO);

                return Ok(new ResponseAPI<dynamic>
                {
                    Success = true,
                    Message = $"Đã thay đổi dữ liệu nhân viên mang mã: {sanPhamDTO.MaSanPham}"
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = $"Lỗi cập nhật dữ liệu: {ex.Message}" });
            }
        }
        /// <summary>
        /// Xóa dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="id">ID của sẩn phẩm.</param>
        /// <returns>Xóa dữ liệu dữ liệu 1 đối tượng theo id</returns>

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            var responseDelete = await _unit.SanPhams.DeleteById(id);

            return Ok(responseDelete);
        }
        /// <summary>
        /// Lấy dữ liệu danh sách sản phẩm được sắp xếp theo tên tăng dần
        /// </summary>
        /// <returns> Lấy dữ liệu danh sách sản phẩm được sắp xếp theo tên tăng dần </returns>
        [HttpGet]
        public async Task<IActionResult> ManualSortByName()
        {
            ResponseAPI<List<SanPhamVM>> responseSp = await _unit.SanPhams.GetAllAsyncVM();

            responseSp.Data = responseSp.Data.OrderBy(x => x.TenSanPham).ToList();

            return Ok(responseSp);
        }
        /// <summary>
        /// Lọc theo khoảng giá và sắp xếp theo id giảm dần
        /// </summary>
        /// <param name="giaNhoNhat">Hãy chọn giá nhỏ nhất.</param>
        /// <param name="giaLonNhat">Hãy chọn giá lớn nhất.</param>
        /// <returns>Lọc theo khoảng giá và sắp xếp theo id giảm dần</returns>
        [HttpGet]
        public async Task<IActionResult> HandFilterByPriceAndManualSortByPrice(decimal giaNhoNhat, decimal giaLonNhat)
        {
            ResponseAPI<List<SanPhamVM>> responseSp = await _unit.SanPhams.GetAllAsyncVM(sp => sp.DonGia > giaNhoNhat && sp.DonGia < giaLonNhat);

            return Ok(responseSp);
        }
    }
}
