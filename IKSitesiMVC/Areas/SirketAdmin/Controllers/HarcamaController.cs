using IKSitesiMVC.Areas.SirketAdmin.ViewModels.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace IKSitesiMVC.Areas.SirketAdmin.Controllers
{

    public class HarcamaController : BaseController
    {
        public HarcamaController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> HarcamaTalepler()
        {
            var client = CreateClient();
            var talepler = await client.GetFromJsonAsync<List<HarcamaTalebiList_VM>>("api/KurumHarcamaTalebi/tümtalepler");
            return View(talepler);
        }

        [HttpPost]
        public Task<IActionResult> Onayla(int harcamaTalebiID)
        {
            return OnaylaAsync("api/KurumHarcamaTalebi/onayla", harcamaTalebiID, "Harcama talebi onaylandı.", "Harcama talebi onaylanamadı");
        }

        [HttpPost]
        public Task<IActionResult> Reddet(int harcamaTalebiID)
        {
            return ReddetAsync("api/KurumHarcamaTalebi/reddet", harcamaTalebiID, "Harcama talebi reddedildi.", "Harcama talebi reddedilemedi");
        }
    }

}