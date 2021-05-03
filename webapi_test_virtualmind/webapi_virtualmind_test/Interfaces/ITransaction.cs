using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi_virtualmind_test.Model;
using webapi_virtualmind_test.Util;

namespace webapi_virtualmind_test.Interfaces
{
    public interface ITransaction
    {
        Task<DetailResponse> purchase(Transaction transaction);
    }
}
