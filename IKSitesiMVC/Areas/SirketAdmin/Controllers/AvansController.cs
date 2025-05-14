using IKSitesiMVC.Areas.SirketAdmin.ViewModels.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace IKSitesiMVC.Areas.SirketAdmin.Controllers
{
    
    public class AvansController : BaseController
    {
        public AvansController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        // GET: /SirketAdmin/Avans/AvansTalepler
        public async Task<IActionResult> AvansTalepler()
        {
            var client = CreateClient();
            var talepler = await client.GetFromJsonAsync<List<AvansTalebiList_VM>>("api/KurumAvansTalebi/tümtalepler");
            return View(talepler);
        }

        // POST: /SirketAdmin/Avans/Onayla
        [HttpPost]
        public Task<IActionResult> Onayla(int avansTalebiID)
        {
            return OnaylaAsync("api/KurumAvansTalebi/onayla", avansTalebiID, "Avans talebi onaylandı.", "Avans talebi onaylanamadı");
        }

        // POST: /SirketAdmin/Avans/Reddet
        [HttpPost]
        public Task<IActionResult> Reddet(int avansTalebiID)
        {
            return ReddetAsync("api/KurumAvansTalebi/reddet", avansTalebiID, "Avans talebi reddedildi.", "Avans talebi reddedilemedi");
        }
    }
}