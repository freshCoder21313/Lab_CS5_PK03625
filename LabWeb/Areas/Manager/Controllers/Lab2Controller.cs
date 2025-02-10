using Microsoft.AspNetCore.Mvc;

namespace LabWeb.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class Lab2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
