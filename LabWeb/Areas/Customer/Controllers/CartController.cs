using Lab.Models;
using Lab.Models.DTOs.SanPham;
using Lab.Models.ViewModels;
using Lab.Utility;
using Lab.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LabWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseAddress = "https://localhost:7094/api/manager/sanpham";
        private readonly string _baseAddressPayment = "https://localhost:7094/api/customer/thanhtoan";

        public CartController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ChangeCart(int maSanPham)
        {
            try
            {
                List<GioHang> sanPhamVMs = GetCarts();

                if (sanPhamVMs.Any(sp => sp.MaSanPham == maSanPham))
                {
                    ResponseAPI<dynamic> response = new ResponseAPI<dynamic>
                    {
                        Status = 200,
                        Success = false,
                        Message = "Sản phẩm đã có trong giỏ hàng"
                    };
                    return Json(response);
                }

                var responseAPI = await _httpClient.GetFromApiAsync<ResponseAPI<GioHang>>(_httpClient.BaseAddress + $"/get/{maSanPham}", _httpContextAccessor, requiredAuth: false) ?? new ResponseAPI<GioHang>();

                if (responseAPI.Data == null)
                {
                    responseAPI.Success = false;
                    responseAPI.Message = "Không tìm thấy sản phẩm bạn muốn thêm vào giỏ hàng.";

                    return Json(responseAPI);
                }
                sanPhamVMs.Add(responseAPI.Data);

                SetCarts(sanPhamVMs);

                responseAPI.Success = true;
                responseAPI.Message = "Sản phẩm đã thêm vào giỏ hàng thành công.";

                return Json(responseAPI);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        public IActionResult DeleteCart(int id)
        {
            try
            {
                ResponseAPI<dynamic> response = new ResponseAPI<dynamic>();

                var spVMs = GetCarts();

                SetCarts(spVMs.Where(x => x.MaSanPham != id).ToList());

                return Json(new ResponseAPI<dynamic>
                {
                    Status = 200,
                    Success = true,
                    Message = "Đã xóa đối tượng trong giỏ hàng thành công"
                });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        public async Task<IActionResult> MakePayment()
        {
            ResponseAPI<dynamic> responseAPI = new ResponseAPI<dynamic>();
            try
            {
                List<GioHang> sanPhamVMs = GetCarts();

                responseAPI = await _httpClient.PostToApiAsync<List<GioHang>>(_baseAddressPayment + $"/GetPaymentUrl/", sanPhamVMs, _httpContextAccessor);
            }
            catch (Exception ex)
            {
                responseAPI.Message = ex.Message;
                throw;
            }
            return Json(responseAPI);
        }
        #region GET API
        public IActionResult GetAll()
        {
            return Json(GetCarts());
        }
        #endregion
        #region NonAction
        [NonAction]
        private List<GioHang> GetCarts()
        {
            return HttpContext.Session.GetComplexData<List<GioHang>>(SD.CartSession) ?? new List<GioHang>();
        }
        private void SetCarts(List<GioHang> carts)
        {
            HttpContext.Session.SetComplexData(SD.CartSession, carts);
        }
        #endregion
    }
}
