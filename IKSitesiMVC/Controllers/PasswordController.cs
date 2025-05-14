using IKSitesiMVC.ViewModels.Login; // Burada ForgotPassword_VM ve ResetPassword_VM view model'larınızın bulunduğu namespace yer almalı.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace IKSitesiMVC.Controllers
{
    [AllowAnonymous] // Şifre sıfırlama işlemleri anonim erişimle yapılabilmeli.
    public class PasswordController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PasswordController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // -------------------------------
        // 1. Şifremi Unuttum Aksiyonu
        // -------------------------------

        // GET: /ResetPassword/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /ResetPassword/ForgotPassword
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword_VM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            // API'ye reset maili oluşturma isteği gönderiyoruz:
            var response = await client.PostAsJsonAsync("api/ForgotPassword/ForgotPassword", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Şifre sıfırlama maili gönderildi. Lütfen e-postanızı kontrol edin.";
                return RedirectToAction("Login", "Login");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "İşlem başarısız: " + error);
                return View(model);
            }
        }

        // -------------------------------
        // 2. Şifremi Sıfırla Aksiyonu
        // -------------------------------

        // GET: /ResetPassword/ResetPassword?email=xxx&token=yyy
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            // Eğer email veya token bilgileri URL’de yoksa yönlendirme yapalım.
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Geçersiz şifre sıfırlama bağlantısı.";
                return RedirectToAction("Login", "Login");
            }

            var model = new ResetPassword_VM
            {
                Email = email,
                Token = token,        
            };

            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword_VM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler eşleşmiyor.");
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");
            // API'ye şifre sıfırlama isteğini gönderiyoruz:
            var response = await client.PostAsJsonAsync("api/ForgotPassword/ResetPassword", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Şifreniz başarıyla sıfırlandı. Lütfen giriş yapın.";
                return RedirectToAction("Login", "Login");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Şifre sıfırlama işlemi başarısız: " + error);
                return View(model);
            }
        }
    }
}