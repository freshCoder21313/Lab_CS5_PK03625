using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Lab.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab.API.Areas.Manager
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = SD.RoleAdmin)]
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
        /// <returns>Lấy toàn bộ dữ liệu</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var nhanViens = await _unit.NhanViens.GetAllAsync();
            return Ok(new ResponseAPI<List<tblNhanVien>>
            {
                Success = true,
                Data = nhanViens.ToList()
            });
        }
        /// <summary> 
        /// Lấy người dùng theo ID. 
        /// </summary> 
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Thông tin người dùng.</returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var nhanVien = await _unit.NhanViens.GetAsync(x => x.MaNhanVien == id);
            if (nhanVien == null)
            {
                return NotFound(new { Message = "Không tìm thấy nhân viên." });
            }

            return Ok(new ResponseAPI<tblNhanVien>
            {
                Success = true,
                Data = nhanVien
            });
        }

        /// <summary>
        /// Thêm dữ liệu 1 đối tượng
        /// </summary>
        /// <returns>Thêm dữ liệu dữ liệu 1 đối tượng</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NhanVienDTO nhanVien)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var response = await _unit.NhanViens.AddAsyncByDTO(nhanVien);

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
        /// <param name="nhanVien">Dữ liệu người dùng cập nhập.</param>
        /// <returns>Cập nhập dữ liệu dữ liệu 1 đối tượng theo id</returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] NhanVienDTO nhanVien)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Success = false, Message = "Dữ liệu không hợp lệ." });
                }

                await _unit.NhanViens.Update(nhanVien);

                return Ok(new ResponseAPI<dynamic>
                {
                    Success = true,
                    Message = $"Đã thay đổi dữ liệu nhân viên mang mã: {nhanVien.MaNhanVien}"
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
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Xóa dữ liệu dữ liệu 1 đối tượng theo id</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest(new { Message = "ID không hợp lệ." });
            }

            var infoGoc = await _unit.NhanViens.GetAsync(x => x.MaNhanVien == id);
            if (infoGoc == null)
            {
                return NotFound(new { Message = "Không tìm thấy dữ liệu muốn xóa." });
            }

            _unit.NhanViens.Remove(infoGoc);

            return Ok(new ResponseAPI<dynamic>
            {
                Success = true,
                Message = $"Đã xóa dữ liệu nhân viên mang mã: {id}"
            });
        }
    }
}
