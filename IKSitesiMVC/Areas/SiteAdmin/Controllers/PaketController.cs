using IKSitesiMVC.Areas.SiteAdmin.DTO_s.Paket;
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
    public class PaketController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PaketController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /SiteAdmin/Paket
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            // API'den paket listesini çekiyoruz
            var paketList = await client.GetFromJsonAsync<List<PaketList_DTO>>("api/Paket");
            return View(paketList);
        }

        // GET: /SiteAdmin/Paket/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var paket = await client.GetFromJsonAsync<PaketDetail_DTO>($"api/Paket/GetPaketById/{id}");
            if (paket == null)
            {
                TempData["Error"] = "Paket bulunamadı.";
                return RedirectToAction("Index");
            }
            return View(paket);
        }

        // GET: /SiteAdmin/Paket/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /SiteAdmin/Paket/Create
        [HttpPost]
        public async Task<IActionResult> Create(PaketCreate_DTO paketCreate)
        {
            if (!ModelState.IsValid)
                return View(paketCreate);

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Paket/Ekle", paketCreate);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Paket başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Paket eklenemedi: " + error;
                return View(paketCreate);
            }
        }

        // GET: /SiteAdmin/Paket/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var paket = await client.GetFromJsonAsync<PaketDetail_DTO>($"api/Paket/GetPaketById/{id}");
            if (paket == null)
            {
                TempData["Error"] = "Paket bulunamadı.";
                return RedirectToAction("Index");
            }
            // Güncelleme için PaketUpdate_DTO'ya dönüştürüyoruz
            var updateDTO = new PaketUpdate_DTO
            {
                PaketID = paket.PaketID,
                PaketAdi = paket.PaketAdi,
                Fiyat = paket.Fiyat,
                PaketSuresi = paket.PaketSuresi,
                AktifMi = paket.AktifMi,
                KapasiteSayisi = paket.KapasiteSayisi
            };
            return View(updateDTO);
        }

        // POST: /SiteAdmin/Paket/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(PaketUpdate_DTO paketUpdate)
        {
            if (!ModelState.IsValid)
                return View(paketUpdate);

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PutAsJsonAsync("api/Paket/Guncelle", paketUpdate);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Paket güncellendi.";
                return RedirectToAction("Index");
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Paket güncellenemedi: " + error;
                return View(paketUpdate);
            }
        }

        // GET: /SiteAdmin/Paket/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var paket = await client.GetFromJsonAsync<PaketDetail_DTO>($"api/Paket/GetPaketById/{id}");
            if (paket == null)
            {
                TempData["Error"] = "Paket bulunamadı.";
                return RedirectToAction("Index");
            }
            return View(paket);
        }

        // POST: /SiteAdmin/Paket/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.DeleteAsync($"api/Paket/Sil/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Paket silindi.";
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Paket silinemedi: " + error;
            }
            return RedirectToAction("Index");
        }
    }
}