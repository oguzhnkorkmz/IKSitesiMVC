using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IKSitesiMVC.Areas.SiteAdmin.Controllers
{
    [Area("SiteAdmin")]
    [Authorize(Roles = "SiteAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
