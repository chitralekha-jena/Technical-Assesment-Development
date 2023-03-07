using Demo_API.Infrastructure.ExternalServices.TargetAsset.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Api.Application.Interfaces
{
    public interface ITargetAssetService
    {
        Task<List<TargetAssetApiResponse>> GetTargetAssets();
    }
}
