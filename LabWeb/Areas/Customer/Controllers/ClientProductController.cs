using Lab.Models.ViewModels;
using Lab.Models;
using Lab.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LabWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ClientProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseAddress = "https://localhost:7094/api/manager/sanpham";

        public ClientProductController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SearchContent(int? giaNhoNhat, int? giaLonNhat, List<string> loaiSanPham, string thuongHieu)
        {
            try
            {
                // Tạo danh sách các tham số truy vấn
                var queryParams = new List<string>();

                if (giaNhoNhat.HasValue)
                {
                    queryParams.Add($"giaNhoNhat={Uri.EscapeDataString(giaNhoNhat.Value.ToString())}");
                }

                if (giaLonNhat.HasValue)
                {
                    queryParams.Add($"giaLonNhat={Uri.EscapeDataString(giaLonNhat.Value.ToString())}");
                }

                if (loaiSanPham != null && loaiSanPham.Any())
                {
                    string loaiSanPhamStr = string.Join(",", loaiSanPham.Select(Uri.EscapeDataString));
                    queryParams.Add($"loaiSanPham={loaiSanPhamStr}");
                }

                if (!string.IsNullOrEmpty(thuongHieu))
                {
                    queryParams.Add($"thuongHieu={Uri.EscapeDataString(thuongHieu)}");
                }

                // Nối các tham số lại với nhau
                string queryString = string.Join("&", queryParams);
                var url = $"{_httpClient.BaseAddress}/HandFilterByPriceAndManualSortByPrice?{queryString}";

                var responseAPI = await _httpClient.GetFromApiAsync<ResponseAPI<List<SanPhamVM>>>(url, _httpContextAccessor, requiredAuth: false);

                return PartialView(responseAPI);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình tìm kiếm. Vui lòng thử lại sau."); // Thông báo lỗi thân thiện với người dùng
            }
        }

    }
}
