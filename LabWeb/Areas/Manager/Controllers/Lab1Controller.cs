using Lab.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LabWeb.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class Lab1Controller : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7094/api/manager/nhanvien");
        private readonly HttpClient _http;
        public Lab1Controller()
        {
            _http = new HttpClient();
            _http.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<tblNhanVien>? tbls = new List<tblNhanVien>();
                HttpResponseMessage httpResponseMessage = await _http.GetAsync(_http.BaseAddress + "/get");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string data = await httpResponseMessage.Content.ReadAsStringAsync();
                    tbls = JsonConvert.DeserializeObject<List<tblNhanVien>>(data);
                }
                return Json(new{ data = tbls });
            } catch(Exception ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                throw;
            }
        }
        public async Task<IActionResult> Get(int id)
        {
            try
            {

                List<tblNhanVien>? tbls = new List<tblNhanVien>();
                HttpResponseMessage httpResponseMessage = await _http.GetAsync(_http.BaseAddress + "get/" + id);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string data = await httpResponseMessage.Content.ReadAsStringAsync();
                    tbls = JsonConvert.DeserializeObject<List<tblNhanVien>>(data);
                }
                return Json(new{ data = tbls });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gặp lỗi: " + ex.Message);
                throw;
            }
        }

    }
}
