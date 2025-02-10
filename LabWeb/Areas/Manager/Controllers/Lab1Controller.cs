using Lab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

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
                List<tblNhanVien> tbls = new List<tblNhanVien>();

                // Lấy token từ Session
                var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (!string.IsNullOrEmpty(token))
                {
                    // Set Authorization header with token
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    return Unauthorized("Token không hợp lệ hoặc hết hạn.");
                }

                // API call to get all data
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/get");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonResponse);

                    tbls = JsonConvert.DeserializeObject<List<tblNhanVien>>(jsonObject["data"]?.ToString());
                }

                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
                    return StatusCode((int)response.StatusCode, errorMessage);
                }

                return Json(new { data = tbls });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, "Lỗi máy chủ");
            }
        }

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                tblNhanVien nhanVien = null;

                // Lấy token từ Session
                var token = _httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
                if (!string.IsNullOrEmpty(token))
                {
                    // Set Authorization header with token
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    return Unauthorized("Token không hợp lệ hoặc hết hạn.");
                }

                // API call to get data by ID
                HttpResponseMessage response = await _httpClient.GetAsync($"/get/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    nhanVien = JsonConvert.DeserializeObject<tblNhanVien>(data);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Message: {errorMessage}");
                    return StatusCode((int)response.StatusCode, errorMessage);
                }

                return Json(new { data = nhanVien });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, "Lỗi máy chủ");
            }
        }
    }
}
