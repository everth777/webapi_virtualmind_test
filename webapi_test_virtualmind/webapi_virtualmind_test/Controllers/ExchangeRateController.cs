using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi_virtualmind_test.Interfaces;
using webapi_virtualmind_test.Util;

namespace webapi_virtualmind_test.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRate _iExchangeRate;

        public ExchangeRateController(IExchangeRate iExchangeRate)
        {
            _iExchangeRate = iExchangeRate;                 
        }

        [HttpGet("exchangerate/{currency:length(3)}")]
        public async Task<ActionResult<DetailResponse>> GetExchangeRate(string currency)
        {
            try
            {

                if (string.IsNullOrEmpty(currency))
                {
                    return BadRequest();
                }

                DetailResponse detail = await _iExchangeRate.GetExchangeRate(currency);

                if (detail == null)
                {
                    return NotFound();
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
