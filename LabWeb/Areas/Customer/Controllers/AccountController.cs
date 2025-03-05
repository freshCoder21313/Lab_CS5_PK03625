using Lab.Models;
using Lab.Models.ViewModels;
using Lab.Utility.SharedData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace LabWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7094/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(DangNhap model)
        {
            if (!ModelState.IsValid) return View(model);

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/TruyCap/DangNhap", content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var jsonObject = JObject.Parse(jsonResponse);
                var success = jsonObject["success"]?.ToObject<bool>() ?? false;

                if (success)
                {
                    var accessToken = jsonObject["data"]?["accessToken"]?.ToString();
                    var refreshToken = jsonObject["data"]?["refreshToken"]?.ToString();

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Lưu token vào session
                        HttpContext.Session.SetString(ConstantsValue.AccessToken, accessToken);
                        HttpContext.Session.SetString(ConstantsValue.RefreshToken, refreshToken!);

                        return RedirectToAction("Index", "Lab1", new {area = "Manager"});
                    }
                }
            }


            ViewBag.Error = "Đăng nhập thất bại. Vui lòng kiểm tra lại!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(ConstantsValue.AccessToken);
            HttpContext.Session.Remove(ConstantsValue.RefreshToken);
            return RedirectToAction("Login");
        }
    }
}
