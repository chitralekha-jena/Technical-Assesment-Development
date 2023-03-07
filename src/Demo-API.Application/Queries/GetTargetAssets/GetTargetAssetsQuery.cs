using Demo_API.Core.Entities;
using MediatR;

namespace Demo_Api.Application.Queries.GetTargetAssets
{
    /// <summary>
    /// Request object
    /// </summary>
    public class GetTargetAssetsQuery : IRequest<List<TargetAssetDto?>>
    {
    }
}
