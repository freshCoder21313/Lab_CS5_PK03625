using Lab.Models.ViewModels;
using Lab.Models;
using Lab.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LabWeb.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class Lab7Controller : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseAddress = "https://localhost:7094/api/manager/thongke";

        public Lab7Controller(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region[GET APIS]
        public async Task<IActionResult> GetAll()
        {
            ResponseAPI<List<tblSanPham>?> responseAPI = new();
            try
            {
                responseAPI = await _httpClient.GetFromApiAsync<ResponseAPI<List<tblSanPham>?>>(_httpClient.BaseAddress + $"/GetAll", _httpContextAccessor, requiredAuth: false);
            }
            catch (HttpRequestException ex)
            {
                responseAPI.Success = false;
                responseAPI.Status = 500;
                responseAPI.Message = "Gặp lỗi: " + ex.Message;
            }
            return Json(responseAPI);
        }
        #endregion
    }
}
