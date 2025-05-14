using IKSitesiMVC.ViewModels.IzinTalebi;
using IKSitesiMVC.ViewModels.HarcamaTalebi;
using IKSitesiMVC.ViewModels.AvansTalebi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace IKSitesiMVC.Controllers
{
    [Authorize(Roles = "Personel")]
    public class PersonelController : Controller
    {
        private readonly HttpClient _client;

        public PersonelController(IHttpClientFactory httpClientFactory)
        {
            // "ApiClient" baseAddress ve CustomAuthorizationHandler ile hazır.
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        private int? GetCurrentPersonelID()
        {
            if (int.TryParse(User.FindFirst("PersonelID")?.Value, out var id))
                return id;
            return null;
        }

        public IActionResult Index() => View();

        // GET/POST pattern for Talep gönderimleri:
        private async Task<IActionResult> HandleTalepCreateAsync<TVm>(TVm model, string apiEndpoint, string successMsg)
        {
            if (!ModelState.IsValid)
                return View(model);

            var personelId = GetCurrentPersonelID();
            if (personelId == null)
                return Forbid("PersonelID alınamadı.");

            // her VM'in PersonelID özelliği olduğunu varsayıyoruz
            dynamic dyn = model;
            dyn.PersonelID = personelId.Value;

            var response = await _client.PostAsJsonAsync(apiEndpoint, model);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = successMsg;
                return RedirectToAction(nameof(Index));
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"{successMsg} sırasında hata: {error}");
            return View(model);
        }

        [HttpGet]
        public IActionResult IzinTalebi() =>
            View(new IzinTalebi_VM { PersonelID = GetCurrentPersonelID() ?? 0, BaslangicTarihi = DateTime.Today, BitisTarihi = DateTime.Today });

        [HttpPost, ValidateAntiForgeryToken]
        public Task<IActionResult> IzinTalebi(IzinTalebi_VM m) =>
            HandleTalepCreateAsync(m, "api/IzinTalebi/ekle", "İzin talebi başarıyla gönderildi");

        [HttpGet]
        public IActionResult HarcamaTalebi() =>
            View(new HarcamaTalebi_VM { PersonelID = GetCurrentPersonelID() ?? 0 });

        [HttpPost, ValidateAntiForgeryToken]
        public Task<IActionResult> HarcamaTalebi(HarcamaTalebi_VM m) =>
            HandleTalepCreateAsync(m, "api/HarcamaTalebi/ekle", "Harcama talebi başarıyla gönderildi");

        [HttpGet]
        public IActionResult AvansTalebi() =>
            View(new AvansTalebi_VM { PersonelID = GetCurrentPersonelID() ?? 0 });

        [HttpPost, ValidateAntiForgeryToken]
        public Task<IActionResult> AvansTalebi(AvansTalebi_VM m) =>
            HandleTalepCreateAsync(m, "api/AvansTalebi/ekle", "Avans talebi başarıyla gönderildi");

        // Approved taleplerin hepsi aynı akışa sahip, endpoint’i ve VM tipini değiştiriyoruz
        private async Task<IActionResult> HandleApprovedAsync<TDto, TVm>(string apiEndpoint, Func<TDto, TVm> map)
        {
            var list = await _client.GetFromJsonAsync<List<TDto>>(apiEndpoint)
                       ?? new List<TDto>();

            var vmList = list.Select(map).ToList();
            return View(vmList);
        }

        public Task<IActionResult> ApprovedIzinTalepleri() =>
            HandleApprovedAsync<OnayliIzinTalebi_VM, OnayliIzinTalebi_VM>("api/IzinTalebi/onaylanmis", dto => dto);

        public Task<IActionResult> ApprovedHarcamaTalepleri() =>
            HandleApprovedAsync<OnayliHarcamaTalebi_VM, OnayliHarcamaTalebi_VM>("api/HarcamaTalebi/onaylanmis", dto => dto);

        public Task<IActionResult> ApprovedAvansTalepleri() =>
            HandleApprovedAsync<OnayliAvansTalebi_VM, OnayliAvansTalebi_VM>("api/AvansTalebi/onaylanmis", dto => dto);
    }
}
