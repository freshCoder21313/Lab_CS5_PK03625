using Lab.DataAccess.Data;
using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab_CS5_PK03625.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class NhanVienController : ControllerBase
    {
        private readonly AppSetting _appSetting;
        private readonly IUnitOfWork _unit;
        private readonly IJWTRepository _jwt;
        public NhanVienController(IOptionsMonitor<AppSetting> optionsMonitor, IUnitOfWork unit, IJWTRepository jwt)
        {
            _appSetting = optionsMonitor.CurrentValue;
            _unit = unit;
            _jwt = jwt;
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns>Lấy toàn bộ dữ liệu</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new
            {
                success = true,
                data = (await _unit.NhanViens.GetAllAsync()).ToList()
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
            return Ok(new
            {
                success = true,
                data = await _unit.NhanViens.GetAsync(x => x.MaNhanVien == id)
            });
        }

        /// <summary>
        /// Thêm dữ liệu 1 đối tượng
        /// </summary>
        /// <returns>Thêm dữ liệu dữ liệu 1 đối tượng</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NhanVienDTO nhanVien)
        {
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
                message = $"Đã thêm dữ liệu nhân viên mang mã: {nv.MaNhanVien}"
            });
        }

        /// <summary>
        /// Cập nhập dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Cập nhập dữ liệu dữ liệu 1 đối tượng theo id</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NhanVienDTO nhanVien)
        {
            await _unit.NhanViens.Update(id, nhanVien);
            await _unit.SaveAsync();

            return Ok(new
            {
                success = true,
                message = $"Đã thay đổi dữ liệu nhân viên mang mã: {id}"
            });
        }

        /// <summary>
        /// Xóa dữ liệu 1 đối tượng
        /// </summary>
        /// <param name="id">ID của người dùng.</param>
        /// <returns>Xóa dữ liệu dữ liệu 1 đối tượng theo id</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
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
                message = $"Đã xóa dữ liệu nhân viên mang mã:  {id}"
            });
        }

    }
}
