using IKSitesiMVC.ApiResponses;
using IKSitesiMVC.Areas.SirketAdmin.ViewModels.Personel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IKSitesiMVC.Areas.SirketAdmin.Controllers
{
    [Area("SirketAdmin")]
    public class PersonelController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PersonelController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Personel/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PersonelEkle_VM();
            await PopulateBolumlerAsync(model);
            return View(model);
        }

        // POST: Personel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonelEkle_VM model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateBolumlerAsync(model);
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Personel/create", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Personel başarıyla eklendi.";
                return RedirectToAction("Index", "Dashboard", new { area = "SirketAdmin" });
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                ModelState.AddModelError(string.Empty, "Bu işlemi yapmaya yetkiniz yok.");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"API hatası: {response.StatusCode} – {error}");
            }

            await PopulateBolumlerAsync(model);
            return View(model);
        }

        // Yardımcı metot: Bölümleri API'den çek ve modele ata
        private async Task PopulateBolumlerAsync(PersonelEkle_VM model)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var list = await client.GetFromJsonAsync<List<BolumResponse>>("api/Bolum/GetAll")
                       ?? new List<BolumResponse>();

            model.Bolumler = list
                .Select(b => new SelectListItem(b.BolumAdi, b.BolumID.ToString()))
                .ToList();
        }

     
    }
}
