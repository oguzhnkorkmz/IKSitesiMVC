using IKSitesiMVC.Areas.SiteAdmin.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IKSitesiMVC.Controllers
{
    [Area("SiteAdmin")]
    [Authorize(Roles = "SiteAdmin")]
    public class KurumController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public KurumController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var kurumList = await client.GetFromJsonAsync<List<KurumList_DTO>>("api/Kurum/TumKurumlar");

            return View(kurumList);
        }

        public async Task<IActionResult> KurumDetay(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var kurumDetay = await client.GetFromJsonAsync<KurumDetay_DTO>($"api/Kurum/KurumGetir/{id}");
            if (kurumDetay == null)
            {
                TempData["Error"] = "Kurum bulunamadı.";
                return RedirectToAction("Index");
            }

            return View(kurumDetay);
        }


       
    }
}