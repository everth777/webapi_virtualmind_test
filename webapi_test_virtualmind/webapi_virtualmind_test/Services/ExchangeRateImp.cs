using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using webapi_virtualmind_test.Context;
using webapi_virtualmind_test.Interfaces;
using webapi_virtualmind_test.Model;
using webapi_virtualmind_test.Structs;
using webapi_virtualmind_test.Util;

namespace webapi_virtualmind_test.Services
{
    public class ExchangeRateImp : IExchangeRate
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public ExchangeRateImp(AppDbContext context, IHttpClientFactory clientFactory, ILogger<ExchangeRateImp> logger)
        {
            _context = context;
            _clientFactory = clientFactory;
            _logger = logger;
        }
        public ExchangeRateImp(AppDbContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }
        public async Task<DetailResponse> GetExchangeRate(string currency)
        {

            _logger.LogInformation($"ExchangeRateImp-GetExchangeRate-Starting to get exchange rate of the day with currency: ({currency})");            

            if (!(currency.Equals("USD") || currency.Equals("BRL")))
                return new DetailResponse { Message = "Warning|Currency not valid" };

            Task<DetailResponse> detail = GetExchangeRateFromURL(currency);

            if (detail.Result.Message.Contains("Success")) 
            {
                _logger.LogInformation("ExchangeRateImp-GetExchangeRate-Exchange rate retrieved successfully.");
                return await detail;
            }
                
            else
            {
                string message = "Error|Getting exchange rate has failed";
                _logger.LogError("ExchangeRateImp-GetExchangeRate-->" + message);
                return new DetailResponse { Message = message };
            }


        }

        public async Task<DetailResponse> GetExchangeRateFromURL(string currency)
        {
            string url = string.Empty;

            if (currency.ToUpper().Equals("USD")) 
            {
                url = URL.URL_DOLLAR_EXCHANGERATE;
            }
            else if (currency.ToUpper().Equals("BRL"))
            {
                //Once the brazilian exchange rate is available from a different endpoing the URL should update 
                //Use the same url for dollar in this time
                url = URL.URL_DOLLAR_EXCHANGERATE;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");

            ExchangeRate exchangeRate = new ExchangeRate();

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                string[] result = await JsonSerializer.DeserializeAsync<string[]>(responseStream);

                //USA currency result
                exchangeRate.Buying = Double.Parse(result[0]);
                exchangeRate.Selling = Double.Parse(result[1]);
                exchangeRate.Date = result[2];

                //Brazilian currency result
                if (currency.ToUpper().Equals("BRL"))
                {
                    exchangeRate.Buying = exchangeRate.Buying / 4;
                    exchangeRate.Selling = exchangeRate.Selling / 4;
                }

                //Any other future currecy(canadian) convertion based in USD
                if (currency.ToUpper().Equals("FUTURCURENCY")) {
                    //logic goes here
                }

               
                return new DetailResponse
                {
                    Message = $"Success|({currency}) Retrieved Successfully",
                    Data = exchangeRate
                };
            }

            return new DetailResponse
            {
                Message = "Warning|Something went wrong while getting the exchange rate",
            };

        }
    }
}
