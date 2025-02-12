using Lab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Lab.Utility.Extensions;
using Lab.Models.DTOs.NhanVien;

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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tbls = await _httpClient.GetFromApiAsync<List<NhanVienDTO>>(_httpClient.BaseAddress + "/get", _httpContextAccessor);
                return Json(new { data = tbls });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var nhanVien = await _httpClient.GetFromApiAsync<NhanVienDTO>(_httpClient.BaseAddress + $"/get/{id}", _httpContextAccessor);
                return Json(new { data = nhanVien });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            try
            {
                var nhanVien = new NhanVienDTO();
                if (id == null)
                {
                    return PartialView(nhanVien);
                }

                nhanVien = await _httpClient.GetFromApiAsync<NhanVienDTO>(_httpClient.BaseAddress + $"/get/{id}", _httpContextAccessor);
                return PartialView(nhanVien);

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(NhanVienDTO nhanVienDTO)
        {
            try
            {
                string? jsonResponse = null;

                if (!ModelState.IsValid)
                {
                    string htmlWithValidate = await this.RenderViewAsync("Upsert", nhanVienDTO, true);

                    return Json(new { htmlWithValidate });
                }

                if (nhanVienDTO.MaNhanVien == 0) //Tạo
                {
                    jsonResponse = await _httpClient.PostToApiAsync<NhanVienDTO>(_httpClient.BaseAddress + $"/post", nhanVienDTO, _httpContextAccessor);
                    return Json(new { jsonResponse });
                }
                // Cập nhập
                jsonResponse = await _httpClient.PutToApiAsync<NhanVienDTO>(_httpClient.BaseAddress + $"/put", nhanVienDTO, _httpContextAccessor);
                return Json(new { jsonResponse });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                string? jsonResponse = null;
                
                // Xóa
                jsonResponse = await _httpClient.DeleteFromApiAsync(_httpClient.BaseAddress + $"/delete/{id}", _httpContextAccessor);

                return Json(new { jsonResponse });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
