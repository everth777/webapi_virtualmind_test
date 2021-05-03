using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi_virtualmind_test.Util;

namespace webapi_virtualmind_test.Interfaces
{
    public interface IExchangeRate
    {
        Task<DetailResponse> GetExchangeRate(string currency);
    }
}
