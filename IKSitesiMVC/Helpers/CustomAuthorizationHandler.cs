using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IKSitesiMVC.Helpers
{
    public class CustomAuthorizationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["JWToken"];
            if (!string.IsNullOrEmpty(token))
            {
                // Authorization header'ına Bearer token'ı ekliyoruz
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // İsteği işleme gönderiyoruz
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
