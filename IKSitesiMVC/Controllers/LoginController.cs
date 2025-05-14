using IKSitesiMVC.ApiResponses;
using IKSitesiMVC.ViewModels.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace IKSitesiMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new Login_VM { GirisTipi = "Personel" }; // Varsayılan olarak Personel
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login_VM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("ApiClient");
            HttpResponseMessage response;

            // GirisTipi'ne göre doğru API endpoint'ine istek atıyoruz
            if (model.GirisTipi == "Personel")
            {
                response = await client.PostAsJsonAsync("api/Login/personnel-login", model);
            }
            else if (model.GirisTipi == "Kurum")
            {
                response = await client.PostAsJsonAsync("api/Login/company-login", model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Giriş tipi seçilmedi.");
                return View(model);
            }

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

                if (tokenResponse?.Token != null)
                {
                    // Test ortamında HTTPS kullanmıyorsanız Secure'u false yapın.
                    var expiration = tokenResponse.TokenExpiration;
                    Response.Cookies.Append("JWToken", tokenResponse.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Geliştirme aşamasında HTTP kullanıyorsanız false
                        Expires = expiration
                    });

                    // Token çözümleme işlemi
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var jwtToken = jwtHandler.ReadJwtToken(tokenResponse.Token);

                    // Rol bilgisini token'dan alıyoruz
                    var userRole = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                    if (string.IsNullOrEmpty(userRole))
                    {
                        ModelState.AddModelError(string.Empty, "Rol bulunamadı.");
                        return View(model);
                    }

                    // Cookie için temel claim'ler
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Role, userRole)
                    };

                    // Token içerisinde varsa PersonelID claim'ini ekliyoruz
                    var personelId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "PersonelID")?.Value;
                    if (!string.IsNullOrEmpty(personelId))
                    {
                        claims.Add(new Claim("PersonelID", personelId));
                    }

                    // Aynı şekilde KurumID claim'ini ekleyelim
                    var kurumId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "KurumID")?.Value;
                    if (!string.IsNullOrEmpty(kurumId))
                    {
                        claims.Add(new Claim("KurumID", kurumId));
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Rolüne göre yönlendirme
                    if (userRole == "SiteAdmin")
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "SiteAdmin" });
                    }
                    else if (userRole == "KurumAdmin")
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "SirketAdmin" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Personel");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Token oluşturulamadı.");
                    return View(model);
                }
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Geçersiz giriş: " + errorMessage);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new Register_VM();

            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("api/Paket");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var paketList = JsonConvert.DeserializeObject<List<PaketResponse>>(jsonString);

                model.Paketler = paketList.Select(p => new SelectListItem
                {
                    Text = p.PaketAdi,
                    Value = p.PaketID.ToString()
                }).ToList();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Paketler yüklenemedi.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register_VM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiClient");

            // API'ye kayıt isteği gönder
            var response = await client.PostAsJsonAsync("api/Login/register", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Kayıt işlemi başarısız: {errorContent}");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Cookie'deki token'ı temizle
            Response.Cookies.Delete("JWToken");

            // Kullanıcıyı CookieAuthentication'dan çıkış yaptır
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}