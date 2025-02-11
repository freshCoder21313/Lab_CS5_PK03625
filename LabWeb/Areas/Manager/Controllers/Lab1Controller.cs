using Lab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Lab.Utility.Extensions;

namespace LabWeb.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class Lab1Controller : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseAddress = "https://localhost:7094/api/manager/nhanvien";

        public Lab1Controller(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                var tbls = await _httpClient.GetFromApiAsync<List<tblNhanVien>>(_httpClient.BaseAddress + "/get", _httpContextAccessor);
                return Json(new { data = tbls });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                var nhanVien = await _httpClient.GetFromApiAsync<tblNhanVien>($"/get/{id}", _httpContextAccessor);
                return Json(new { data = nhanVien });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
