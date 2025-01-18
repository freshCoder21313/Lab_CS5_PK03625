using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2_CS5_PK03625.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public NhanVienController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns>Lấy toàn bộ dữ liệu</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<tblNhanVien>? nhanViens = (await _unit.NhanViens.GetAllAsync()).ToList();
                if (nhanViens == null)
                {
                    return NotFound();
                }
                return Ok(nhanViens);
            }
            catch (Exception ex)
            {
                return BadRequest("Gặp lỗi: " + ex.Message);
                throw;
            }
        }
        /// <summary> 
        /// Lấy người dùng theo ID. 
        /// </summary> 
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Thông tin người dùng.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                tblNhanVien? nhanVien = await _unit.NhanViens.GetAsync(x => x.MaNhanVien == id);
                if (nhanVien == default)
                {
                    return NotFound();
                }
                return Ok(nhanVien);
            }
            catch (Exception ex)
            {
                return BadRequest("Gặp lỗi: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Thêm dữ liệu 1 đối tượng
        /// </summary>
        /// <returns>Thêm dữ liệu dữ liệu 1 đối tượng</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NhanVienDTO nhanVien)
        {
            try
            {
                tblNhanVien nv = new tblNhanVien { HoTen = nhanVien.HoTen, SoDienThoai = nhanVien.SoDienThoai, NgaySinh = nhanVien.NgaySinh };
                await _unit.NhanViens.AddAsync(nv);

                await _unit.SaveAsync();

                return Ok($"Đã thêm dữ liệu nhân viên mang mã: {nv.MaNhanVien}");
            }
            catch (Exception ex)
            {
                return BadRequest("Gặp lỗi: " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Cập nhập dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Cập nhập dữ liệu dữ liệu 1 đối tượng theo id</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NhanVienDTO nhanVien)
        {
            try
            {
                await _unit.NhanViens.Update(id, nhanVien);
                await _unit.SaveAsync();

                return Ok($"Đã thay đổi dữ liệu nhân viên mang mã: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest("Gặp lỗi: " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Xóa dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Xóa dữ liệu dữ liệu 1 đối tượng theo id</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                tblNhanVien? infoGoc = await _unit.NhanViens.GetAsync(x => x.MaNhanVien == id);
                if (infoGoc == default)
                {
                    return NotFound("Không tìm thấy dữ liệu muốn xóa.");
                }
                _unit.NhanViens.Remove(infoGoc);

                await _unit.SaveAsync();

                return Ok($"Đã xóa dữ liệu nhân viên mang mã: {infoGoc.MaNhanVien}");
            }
            catch (Exception ex)
            {
                return BadRequest("Gặp lỗi: " + ex.Message);
                throw;
            }
        }
    }
}
