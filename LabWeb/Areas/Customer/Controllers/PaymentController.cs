using Lab.Models;
using Lab.Utility.Extensions;
using Lab.Utility;
using Microsoft.AspNetCore.Mvc;

namespace LabWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseAddress = "https://localhost:7094/api/customer/ThanhToan";
        public PaymentController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PaymentCallBack()
        {
            // Lấy dữ liệu từ query string
            var vnpayData = HttpContext.Request.Query;

            // Chuyển đổi dữ liệu thành một dictionary để dễ dàng truy cập
            var data = vnpayData.ToDictionary(key => key.Key, value => value.Value.ToString());

            // Gửi dữ liệu vào View
            return View(data);
        }
    }
}
