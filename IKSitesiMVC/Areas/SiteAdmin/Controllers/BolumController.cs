using IKSitesiMVC.Areas.SiteAdmin.DTO_s.Bolum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace IKSitesiMVC.Areas.SiteAdmin.Controllers
{
    [Area("SiteAdmin")]
    [Authorize(Roles = "SiteAdmin")]
    public class BolumController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BolumController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // ✅ Tüm Bölümleri Listele
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var bolumler = await client.GetFromJsonAsync<List<Bolum_DTO>>("api/Bolum/GetAll");

            if (bolumler == null || bolumler.Count == 0)
            {
                TempData["Error"] = "Bölüm listesi bulunamadı.";
                return View(new List<Bolum_DTO>());
            }

            return View(bolumler);
        }

        // ✅ Yeni Bölüm Ekleme (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Yeni Bölüm Ekleme (POST)
        [HttpPost]
        public async Task<IActionResult> Create(BolumEkle_DTO bolumCreate)
        {
            if (!ModelState.IsValid)
                return View(bolumCreate);

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Bolum/Create", bolumCreate);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Bölüm başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            string error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = "Bölüm eklenemedi: " + error;
            return View(bolumCreate);
        }

        // ✅ Bölüm Güncelleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var bolum = await client.GetFromJsonAsync<Bolum_DTO>($"api/Bolum/{id}");

            if (bolum == null)
            {
                TempData["Error"] = "Güncellenecek bölüm bulunamadı.";
                return RedirectToAction("Index");
            }

            return View(bolum);
        }

        // ✅ Bölüm Güncelleme (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Bolum_DTO bolumUpdate)
        {
            if (!ModelState.IsValid)
                return View(bolumUpdate);

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PutAsJsonAsync("api/Bolum/Update", bolumUpdate);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Bölüm başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            string error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = "Bölüm güncellenemedi: " + error;
            return View(bolumUpdate);
        }

        // ✅ Bölüm Silme (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var bolum = await client.GetFromJsonAsync<Bolum_DTO>($"api/Bolum/{id}");

            if (bolum == null)
            {
                TempData["Error"] = "Silinecek bölüm bulunamadı.";
                return RedirectToAction("Index");
            }

            return View(bolum);
        }

        // ✅ Bölüm Silme (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"api/Bolum/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Bölüm başarıyla silindi.";
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Bölüm silinemedi: " + error;
            }

            return RedirectToAction("Index");
        }
    }
}