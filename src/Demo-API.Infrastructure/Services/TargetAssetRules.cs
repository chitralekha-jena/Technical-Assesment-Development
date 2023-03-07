using Demo_Api.Application.Interfaces;
using Demo_API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_API.Infrastructure.Services
{
    /// <summary>
    /// Class to evaluate the business rule for TargetAsset extra fields
    /// </summary>
    public class TargetAssetRules : ITargetAssetRules
    {
        private readonly IDateTimeService _dateTimeService;

        public TargetAssetRules(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        /// <summary>
        /// Get The TargetAssetCount
        /// </summary>
        /// <param name="targetAssets"></param>
        /// <param name="targetAssetParentId"></param>
        /// <returns></returns>
        public int CalculateParentTargetAssetCount(List<TargetAssetDto?> targetAssets, int? targetAssetParentId)
        {
            if (targetAssets == null || !targetAssets.Any())
            {
                return 0;
            }

            if (targetAssetParentId == null)
            {
                return 1;
            }

            int parentTargetAssetCount = 0;

            while (targetAssetParentId != null)
            {
                var parentTargetAsset = targetAssets.FirstOrDefault(t => t?.id == targetAssetParentId);

                if (parentTargetAsset == null)
                {
                    break;
                }

                parentTargetAssetCount++;
                targetAssetParentId = parentTargetAsset.parentId;

                if (parentTargetAssetCount >= targetAssets.Count) // Infinite loop
                {
                    return 0;
                }
            }

            return parentTargetAssetCount + 1;
        }

        /// <summary>
        /// rule to set the IsStartable field
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsStartable(string status)
        {
            var currentDate = _dateTimeService.GetCurrentDate();
            return currentDate.Day == 3 && status.Equals("Running", StringComparison.OrdinalIgnoreCase);
        }
    }
}
