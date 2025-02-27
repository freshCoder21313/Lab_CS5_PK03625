using Lab.DataAccess.Repository.IRepository;
using Lab.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lab.API.Areas.Manager
{
    [Area("Manager")]
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = SD.RoleAdmin)]
    public class ThongKeController : ControllerBase
    {
        private readonly IThongKeRepository _thongke;
        public ThongKeController(IThongKeRepository thongke)
        {
            _thongke = thongke;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _thongke.GetAllSanPhamAsync();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _thongke.GetSanPhamByIdAsync(id));
        }
    }
}
