using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_API.Infrastructure.ExternalServices.TargetAsset.Response
{
    /// <summary>
    /// The entities required for the response from the external API
    /// </summary>
    public class TargetAssetApiResponse
    {
        public int id { get; set; }
        public bool? isStartable { get; set; }
        public string location { get; set; }
        public string owner { get; set; }
        public string createdBy { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string[] tags { get; set; }
        public int cpu { get; set; }
        public long ram { get; set; }
        public DateTime createdAt { get; set; }
        public int? parentId { get; set; }
    }
}

