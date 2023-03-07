using Demo_Api.Application.Interfaces;
using Demo_API.Application.Exceptions;
using Demo_API.Infrastructure.ExternalServices.TargetAsset.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Demo_API.Infrastructure.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class TargetAssetService : ServiceBase, ITargetAssetService
    {
        private readonly ILogger<TargetAssetService> _logger;
        private static readonly Uri TargetAssetUri = new("https://06ba2c18-ac5b-4e14-988c-94f400643ebf.mock.pstmn.io/targetAsse");
      
        public TargetAssetService(IHttpClientHandler httpClientHandler, ILogger<TargetAssetService> logger) : base(httpClientHandler)
        {
            _logger = logger;
        }

        /// <summary>
        /// The service class to format the external API response
        /// </summary>
        /// <returns></returns>
        public async Task<List<TargetAssetApiResponse>> GetTargetAssets()
        {
            var targetAssetApiResponse = new List<TargetAssetApiResponse>();
            string result;

            try
            {
                 result = await ExecuteGet(TargetAssetUri);
                targetAssetApiResponse = JsonConvert.DeserializeObject<List<TargetAssetApiResponse>>(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Target assets not found.");
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while calling the external service");
                throw new TargetAssetServiceException("Exception while calling the external service");
            }
            return targetAssetApiResponse;
        }
    }
}
