using Demo_Api.Application.Interfaces;
using Demo_API.Infrastructure.ExternalServices.TargetAsset.Response;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Demo_API.Infrastructure.Services
{
    /// <summary>
    ///  Base service class
    /// </summary>
    public abstract class ServiceBase
    {
        protected const string MediaType = "application/json";
        private readonly IHttpClientHandler HttpClient;
        protected ServiceBase(IHttpClientHandler httpClientHandler)
        {
            HttpClient = httpClientHandler;
        }

        /// <summary>
        /// Execute Get method
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected async Task<string> ExecuteGet(Uri uri)
        {
            var request = GetRequest(uri, MediaType);

            return  await HttpClient.Get(request);
        }

        /// <summary>
        /// Fetch the request
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        private HttpRequestMessage GetRequest(Uri uri, string mediaType)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = HttpMethod.Get,
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return request;
        }
    }
}
