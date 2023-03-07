using Demo_API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_Api.Application.Interfaces
{
    /// <summary>
    /// Interface for TargetAssetRules
    /// </summary>
    public interface ITargetAssetRules
    {
        bool IsStartable(string status);
        int CalculateParentTargetAssetCount(List<TargetAssetDto?> targetAssets, int? targetAssetId);
    }
}
