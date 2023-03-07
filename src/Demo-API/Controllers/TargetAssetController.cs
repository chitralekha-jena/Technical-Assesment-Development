using Demo_Api.Application.Queries.GetTargetAssets;
using Demo_API.Application.Exceptions;
using Demo_API.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Demo_API.Controllers
{  
    /// <summary>
    /// Target Asset
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TargetAssetController : BaseApiController
    {
        private readonly ILogger<TargetAssetController> _logger;
        public TargetAssetController(ILogger<TargetAssetController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get the enrich target asset from external api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<TargetAssetDto>>> Get()
        {
            try
            {
                _logger.LogInformation("Welcome, To TargetAssetController!");

                var result = await Mediator.Send(new GetTargetAssetsQuery());

                _logger.LogInformation("The final target asset from API: {Result}", result);

                if (result == null || !result.Any())
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Target assets not found.");
                return NotFound("Error: Target assets not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught executing the request for targetasset api");
                throw new Exception("Error: Something went wrong while processing your request");
            }
        }
    }
}
