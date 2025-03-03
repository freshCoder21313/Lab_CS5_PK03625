using Lab.Models;
using Lab.Models.DTOs.NhanVien;
using Lab.Utility.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var responseAPI = await _httpClient.GetFromApiAsync<ResponseAPI<List<NhanVienDTO>>>(_httpClient.BaseAddress + "/get", _httpContextAccessor);
                return Json(responseAPI);
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
                var responseAPI = await _httpClient.GetFromApiAsync<ResponseAPI<NhanVienDTO>>(_httpClient.BaseAddress + $"/get/{id}", _httpContextAccessor);
                return Json(responseAPI);
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
                if (id == null)
                {
                    return PartialView(new ResponseAPI<NhanVienDTO>());
                }

                var responseApi = await _httpClient.GetFromApiAsync<ResponseAPI<NhanVienDTO>>(_httpClient.BaseAddress + $"/get/{id}", _httpContextAccessor);
                if (responseApi?.Data == null)
                {
                    return StatusCode(500, "Không tìm thấy dữ liệu nhân viên.");
                }
                return PartialView(responseApi);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(ResponseAPI<NhanVienDTO> response)
        {
            NhanVienDTO? nhanVienDTO = response.Data;
            var responseAPI = new ResponseAPI<NhanVienDTO>();

            try
            {
                // Kiểm tra tính hợp lệ của ModelState trước khi tiếp tục
                if (!ModelState.IsValid)
                {
                    // Truyền lại dữ liệu vào response để giữ giá trị đã nhập
                    responseAPI.Data = nhanVienDTO;

                    // Render lại view với lỗi validate
                    responseAPI.HtmlWithValidate = await this.RenderViewAsync("Upsert", responseAPI, true);

                    // Thông báo lỗi
                    responseAPI.Message = "Vui lòng nhập đúng yêu cầu dữ liệu.";
                    responseAPI.Success = false;

                    // Trả về response với lỗi validate
                    return Json(responseAPI);
                }

                // Kiểm tra trường hợp tạo mới
                if (nhanVienDTO?.Id == 0)
                {
                    var responseActionPost = await _httpClient.PostToApiAsync<NhanVienDTO>(
                        _httpClient.BaseAddress + "/post", nhanVienDTO, _httpContextAccessor
                    );

                    // Cập nhật các giá trị trả về từ API
                    responseAPI.Status = responseActionPost.Status;
                    responseAPI.Success = responseActionPost.Success;
                    responseAPI.Message = responseActionPost.Message;

                    return Json(responseAPI);
                }

                // Trường hợp cập nhật
                var responseActionPut = await _httpClient.PutToApiAsync<NhanVienDTO>(
                    _httpClient.BaseAddress + "/put", nhanVienDTO, _httpContextAccessor
                );

                responseAPI.Success = responseActionPut.Success;
                responseAPI.Message = responseActionPut.Message;

                return Json(responseAPI);
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi khi có vấn đề với yêu cầu HTTP
                responseAPI.Success = false;
                responseAPI.Message = "Có lỗi xảy ra khi kết nối với server.";
                Console.WriteLine("Lỗi: " + ex.Message);

                return Json(responseAPI); // Trả về lỗi trong response API
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác ngoài HttpRequestException
                responseAPI.Success = false;
                responseAPI.Message = "Đã xảy ra lỗi không xác định.";
                Console.WriteLine("Lỗi: " + ex.Message);

                return Json(responseAPI); // Trả về lỗi chung trong response API
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("ID không hợp lệ.");
                }

                var responseActionDelete = await _httpClient.DeleteFromApiAsync(_httpClient.BaseAddress + $"/delete/{id}", _httpContextAccessor);
                return Json(responseActionDelete);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
