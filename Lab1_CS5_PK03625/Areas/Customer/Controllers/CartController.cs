using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lab.API.Areas.Customer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        /// <summary>
        /// Function Constructor
        /// </summary>
        /// <param name="unit"></param>
        public CartController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        /// <summary>
        /// User get cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpGet("{productId}&{quantity}")]
        public async Task<IActionResult> AddCart(int? productId, int? quantity)
        {
            ResponseAPI<GioHang> response = new ResponseAPI<GioHang>();
            try
            {
                var responseSanPhamVM = await _unit.SanPhams.GetAsyncVM(x => x.MaSanPham == productId);
                response.Data = new GioHang { MaSanPham = productId.Value, DonGia = responseSanPhamVM.Data.DonGia, SoLuong = quantity.GetValueOrDefault(1), TenSanPham = responseSanPhamVM.Data.TenSanPham};
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                throw;
            }
            return Ok(response);
        }
    }
}
