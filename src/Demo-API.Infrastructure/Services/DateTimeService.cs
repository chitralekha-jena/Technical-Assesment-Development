using Demo_Api.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_API.Infrastructure.Services
{
    /// <summary>
    /// Datetime service class
    /// </summary>
    public class DateTimeService:IDateTimeService
    {
        public DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }
    }
}
