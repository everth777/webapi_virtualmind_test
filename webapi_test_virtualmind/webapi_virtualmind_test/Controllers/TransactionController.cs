using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using webapi_virtualmind_test.Interfaces;
using webapi_virtualmind_test.Model;
using webapi_virtualmind_test.Util;

namespace webapi_virtualmind_test.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _iTransaction;
        public TransactionController(ITransaction iTransaction)
        {
            _iTransaction = iTransaction;            
        }

        [HttpPost("transaction")]
        public async Task<ActionResult<DetailResponse>> purchase([FromBody]Transaction transaction)
        {
            try
            {
                if (transaction == null) 
                {
                    return BadRequest();
                }
                DetailResponse detail =  await _iTransaction.purchase(transaction);

                if (detail == null) 
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Not able to process the transaction");
                }

                return detail;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }
    }
}
