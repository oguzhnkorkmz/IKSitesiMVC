using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;



namespace IKSitesiMVC.Areas.SirketAdmin.Controllers
{
    [Area("SirketAdmin")]
    [Authorize(Roles = "KurumAdmin")]
    public class BaseController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient CreateClient()
        {
            return _httpClientFactory.CreateClient("ApiClient");
        }

        // Ortak Onaylama işlemi
        protected async Task<IActionResult> OnaylaAsync(string apiEndpoint, int talepID, string successMessage, string errorMessage)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"{apiEndpoint}/{talepID}", new { });

            if (response.IsSuccessStatusCode)
                TempData["Success"] = successMessage;
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"{errorMessage}: {errorContent}";
            }
            return RedirectToAction("Index", "Dashboard", new { area = "SirketAdmin" });
        }

        // Ortak Reddetme işlemi
        protected async Task<IActionResult> ReddetAsync(string apiEndpoint, int talepID, string successMessage, string errorMessage)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"{apiEndpoint}/{talepID}", new { });

            if (response.IsSuccessStatusCode)
                TempData["Success"] = successMessage;
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"{errorMessage}: {errorContent}";
            }
            return RedirectToAction("Index", "Dashboard", new { area = "SirketAdmin" });
        }
    }
}