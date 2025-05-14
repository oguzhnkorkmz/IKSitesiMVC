using IKSitesiMVC.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// — MVC & API controller’ları
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// — HttpClient (ApiClient isimli client, base URL ve JSON headers hazır)
var configuration = builder.Configuration;

builder.Services.AddHttpContextAccessor(); // Cookie'ye erişim için

builder.Services.AddTransient<CustomAuthorizationHandler>(); // Handler'ı servis olarak ekle

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<CustomAuthorizationHandler>();


// — Authentication: önce Cookie, sonra JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Login/Login";
    options.AccessDeniedPath = "/Login/AccessDenied";   // Düzeltilmiş AccessDenied yolu
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        RoleClaimType = ClaimTypes.Role
    };
});

// — Diğer servisleriniz (DI kayıtları: ILoginService, IPersonelService, IIzinTalebiService, vb.)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// — Middleware sırası
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // 1️⃣ Kimlik doğrulama
app.UseAuthorization();   // 2️⃣ Yetkilendirme

// Özel Middleware: Eğer istek root ("/") ise ve kullanıcı kimliği doğrulanmışsa, 
// ilgili rolün anasayfasına yönlendirme yap.
app.Use(async (context, next) =>
{
    // Sadece root isteğine uyguluyoruz.
    if (context.Request.Path == "/" && context.User.Identity?.IsAuthenticated == true)
    {
        if (context.User.IsInRole("Personel"))
        {
            context.Response.Redirect("/Personel/");
            return;
        }
        else if (context.User.IsInRole("SiteAdmin"))
        {
            context.Response.Redirect("/SiteAdmin/Dashboard/Index");
            return;
        }
        else if (context.User.IsInRole("KurumAdmin"))
        {
            context.Response.Redirect("/SirketAdmin/Dashboard/Index");
            return;
        }
        else
        {
            // Rol belirtilmediyse varsayılan anasayfaya yönlendir.
            context.Response.Redirect("/Home/Index");
            return;
        }
    }
    await next();
});

// Alan (Area) bulunan MVC routelar
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Normal MVC routelar
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Attribute-routed API controller’ları
app.MapControllers();

app.Run();