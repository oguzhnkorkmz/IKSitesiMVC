using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IKSitesiMVC.Areas.SirketAdmin.Controllers
{
    [Area("SirketAdmin")]
    [Authorize(Roles = "KurumAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
