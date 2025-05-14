using System.Diagnostics;
using IKSitesiMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKSitesiMVC.Areas.SiteAdmin.DTO_s.Paket;

namespace IKSitesiMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            // "ApiClient" tan�ml� HttpClient �zerinden, API'den paketleri �ekiyoruz.
            var client = _httpClientFactory.CreateClient("ApiClient");
            var paketList = await client.GetFromJsonAsync<List<PaketList_DTO>>("api/Paket");

            // E�er paketler bo� gelirse, view'a bo� liste yollan�r.
            return View(paketList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}