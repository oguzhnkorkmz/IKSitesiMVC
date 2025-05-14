using IKSitesiMVC.Areas.SirketAdmin.ViewModels.Talep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace IKSitesiMVC.Areas.SirketAdmin.Controllers
{

    public class IzinController : BaseController
    {
        public IzinController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public async Task<IActionResult> IzinTalepler()
        {
            var client = CreateClient();
            var talepler = await client.GetFromJsonAsync<List<IzinTalebiList_VM>>("api/KurumIzinTalebi/tümtalepler");
            return View(talepler);
        }

        [HttpPost]
        public Task<IActionResult> Onayla(int izinTalebiID)
        {
            return OnaylaAsync("api/KurumIzinTalebi/onayla", izinTalebiID, "İzin talebi onaylandı.", "İzin talebi onaylanamadı");
        }

        [HttpPost]
        public Task<IActionResult> Reddet(int izinTalebiID)
        {
            return ReddetAsync("api/KurumIzinTalebi/reddet", izinTalebiID, "İzin talebi reddedildi.", "İzin talebi reddedilemedi");
        }
    }
}