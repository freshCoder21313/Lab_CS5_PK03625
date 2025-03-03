using Lab.Utility.SharedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LabWeb.ViewComponents
{
    public class AccessBarViewComponent : ViewComponent
    {
        public AccessBarViewComponent()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            bool isLogin = HttpContext.Session.Get(ConstantsValue.AccessToken) != null;
            return View("Default", isLogin);
        }
    }
}
