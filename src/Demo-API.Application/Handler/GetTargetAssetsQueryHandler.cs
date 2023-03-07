using AutoMapper;
using Demo_Api.Application.Interfaces;
using Demo_Api.Application.Queries.GetTargetAssets;
using Demo_API.Application.Exceptions;
using Demo_API.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo_Api.Application.Handler
{
    /// <summary>
    /// GetTargetAssetsQueryHandler 
    /// </summary>
    public class GetTargetAssetsQueryHandler : IRequestHandler<GetTargetAssetsQuery, List<TargetAssetDto?>>
    {
        private readonly ITargetAssetService _targetAssetService;
        private readonly ITargetAssetRules _targetAssetRules;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTargetAssetsQueryHandler> _logger;

        public GetTargetAssetsQueryHandler(ITargetAssetService targetAssetService, ITargetAssetRules targetAssetRules, IMapper mapper, ILogger<GetTargetAssetsQueryHandler> logger)
        {
            _targetAssetService = targetAssetService;
            _targetAssetRules = targetAssetRules;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Handler to invoke the query request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<TargetAssetDto?>> Handle(GetTargetAssetsQuery request, CancellationToken cancellationToken)
        {

                var result = await _targetAssetService.GetTargetAssets();

               _logger.LogInformation("The target asset from API: ", result);

               if (result == null || result.Count < 1)
               {
                 throw new NotFoundException("No target assets found.");
               }

                var responseDtos = _mapper.Map<List<TargetAssetDto?>>(result);

                foreach (var responseDto in responseDtos)
                {
                    if (responseDto == null)
                    {
                        continue;
                    }
                    responseDto.isStartable = _targetAssetRules.IsStartable(responseDto.status);
                    responseDto.parentTargetAssetCount = _targetAssetRules.CalculateParentTargetAssetCount(responseDtos, responseDto.parentId);
                }
                _logger.LogInformation("The enrichtarget asset : ", responseDtos);

                return responseDtos;
        }      
    }
}

