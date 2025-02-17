using Lab.DataAccess.Repository.IRepository;
using Lab.Models;
using Lab.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lab.API.Areas.Customer
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
                // Kiểm tra productId đầu vào
                if (!productId.HasValue)
                {
                    response.Success = false;
                    response.Message = "Mã sản phẩm không được phép trống.";
                    return BadRequest(response);
                }

                var responseSanPhamVM = await _unit.SanPhams.GetAsyncVM(x => x.MaSanPham == productId);
                if (responseSanPhamVM.Data == null)
                {
                    response.Success = false;
                    response.Message = "Sản phẩm không tồn tại.";
                    return NotFound(response);
                }

                response.Data = new GioHang
                {
                    MaSanPham = productId.Value,
                    DonGia = responseSanPhamVM.Data.DonGia,
                    SoLuong = quantity.GetValueOrDefault(1),
                    TenSanPham = responseSanPhamVM.Data.TenSanPham
                };

                response.Success = true;
                response.Message = "Thêm sản phẩm vào giỏ hàng thành công.";
            }
            catch (Exception ex)
            {
                // Có thể ghi log cho lỗi
                // _logger.LogError(ex, "Error occurred while adding to cart.");

                // Đặt thông điệp lỗi cụ thể hơn cho người dùng
                response.Success = false;
                response.Message = "Đã xảy ra lỗi trong quá trình thêm sản phẩm vào giỏ hàng.";
            }
            return Ok(response);
        }
    }
}
