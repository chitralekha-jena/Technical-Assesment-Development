using AutoMapper;
using Demo_API.Core.Entities;
using Demo_API.Infrastructure.ExternalServices.TargetAsset.Response;

namespace Demo_Api.Application.Mapping
{
    /// <summary>
    /// Mapping profile using automapper
    /// </summary>
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<TargetAssetApiResponse, TargetAssetDto>()
            .ForMember(dest => dest.parentTargetAssetCount, opt => opt.Ignore());
        }      
    }
}
