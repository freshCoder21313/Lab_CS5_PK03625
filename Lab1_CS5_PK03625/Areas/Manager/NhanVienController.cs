using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Lab.Utility.SharedData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Lab.API.Areas.Manager
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    // [Authorize(Roles = ConstantsValue.RoleAdmin)]
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
        [ProducesResponseType(typeof(ResponseAPI<List<NguoiDungUngDung>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var NguoiDungs = await _unit.NguoiDungs.GetAllAsync();
            return Ok(new ResponseAPI<List<NguoiDungUngDung>>
            {
                Success = true,
                Data = NguoiDungs.ToList()
            });
        }
        /// <summary> 
        /// Lấy người dùng theo ID. 
        /// </summary> 
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Thông tin người dùng.</returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var nhanVien = await _unit.NguoiDungs.GetAsync(x => x.Id == id);
            if (nhanVien == null)
            {
                return NotFound(new { Message = "Không tìm thấy nhân viên." });
            }

            return Ok(new ResponseAPI<NguoiDungUngDung>
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
                var response = await _unit.NguoiDungs.AddAsyncByDTO(nhanVien);

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

                await _unit.NguoiDungs.Update(nhanVien);

                return Ok(new ResponseAPI<dynamic>
                {
                    Success = true,
                    Message = $"Đã thay đổi dữ liệu nhân viên mang mã: {nhanVien.Id}"
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
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return BadRequest(new { Message = "ID không hợp lệ." });
            }

            var infoGoc = await _unit.NguoiDungs.GetAsync(x => x.Id == id);
            if (infoGoc == null)
            {
                return NotFound(new { Message = "Không tìm thấy dữ liệu muốn xóa." });
            }

            _unit.NguoiDungs.Remove(infoGoc);

            return Ok(new ResponseAPI<dynamic>
            {
                Success = true,
                Message = $"Đã xóa dữ liệu nhân viên mang mã: {id}"
            });
        }
    }
}
