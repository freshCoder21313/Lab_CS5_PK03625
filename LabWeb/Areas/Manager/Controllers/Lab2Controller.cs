using Lab.Models.DTOs.NhanVien;
using Lab.Models.DTOs.SanPham;
using Lab.Models.ViewModels;
using Lab.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LabWeb.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class Lab2Controller : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseAddress = "https://localhost:7094/api/manager/sanpham";

        public Lab2Controller(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region//GET API
        public async Task<IActionResult> HandFilterByPriceAndManualSortByPrice(int giaNhoNhat, int giaLonNhat)
        {
            try
            {
                var tbls = await _httpClient.GetFromApiAsync<List<SanPhamVM>>(_httpClient.BaseAddress + $"/HandFilterByPriceAndManualSortByPrice?giaNhoNhat={giaNhoNhat}&giaLonNhat={giaLonNhat}", _httpContextAccessor);
                return Json(tbls);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
