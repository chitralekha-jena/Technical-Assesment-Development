using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_API.Application.Exceptions
{
    public class TargetAssetServiceException : Exception
    {
        public TargetAssetServiceException(string message) : base(message) { }
    }

}
