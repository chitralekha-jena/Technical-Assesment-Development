using Demo_Api.Application.Interfaces;

namespace Demo_API.Infrastructure.Services.Handlers
{
    /// <summary>
    /// Handler class to create a connection to external system
    /// </summary>
    public class HttpClientHandler: IHttpClientHandler,IDisposable
    {
        private bool _disposed;

        private readonly HttpClient _httpclient;

        public HttpClientHandler()
        {
            _httpclient = new HttpClient();
        }

        /// <summary>
        /// Get connection to external system
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> Get(HttpRequestMessage request)
        {
            var response = await _httpclient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!disposing || _disposed)
            {
                return;
            }

            _disposed = true;
            _httpclient?.Dispose();
        }
    }
}
