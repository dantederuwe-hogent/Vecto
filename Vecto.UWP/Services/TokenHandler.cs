using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Vecto.UWP.Services
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly string _token;

        public TokenHandler(string token, HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler())
        {
            _token = token;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}