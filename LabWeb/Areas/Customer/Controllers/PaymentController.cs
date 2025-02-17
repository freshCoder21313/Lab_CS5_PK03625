using Microsoft.AspNetCore.Mvc;

namespace LabWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PaymentCallBack()
        {
            return View();
        }
    }
}
