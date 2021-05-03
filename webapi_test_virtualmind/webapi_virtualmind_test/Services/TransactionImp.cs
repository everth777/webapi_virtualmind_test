using Microsoft.AspNetCore.Mvc;
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
    public class TransactionImp : ITransaction
    {

        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;

        public TransactionImp(AppDbContext context, IHttpClientFactory clientFactory, ILogger<TransactionImp> logger)
        {
            _context = context;
            _clientFactory = clientFactory;
            _logger = logger;
        }
        
        public async Task<DetailResponse> purchase(Transaction transaction)
        {

                _logger.LogInformation("TransactionImp-purchase-Starting on purchase function with this transaction object-->"+ transaction);
                //NOTE: parameter transactions contains the field UserId, this one is coming from frontend
                ////
                ///UserId MUST be red from the database after user has logged in. This can be achieved through User authentication mechanism 
                /////                                

                ExchangeRateImp exchangeRageImp = new ExchangeRateImp(_context,_clientFactory);

                _logger.LogInformation("TransactionImp-purchase-Starting to get the exchange rate");
                DetailResponse details = await exchangeRageImp.GetExchangeRateFromURL(transaction.Currency);
                _logger.LogInformation("TransactionImp-purchase-Result after getting exchange rate-->"+ JsonSerializer.Serialize(details));

                if (details == null) 
                {
                    return new DetailResponse
                    {
                        Message = "Error|Something went wrong while getting the exchange rate"
                    };
                }

                if (details.Message.Contains("Success"))
                {
                    ExchangeRate exchangeRate = (ExchangeRate)details.Data;

                    if (transaction.Currency.ToUpper().Equals("USD") && (transaction.Amount / exchangeRate.Buying) > Currency.MAX_USD_AMOUNT) 
                    {
                        return new DetailResponse
                        {
                            Message = $"Warning|The amount has exceeded the limint of {transaction.Currency}{Currency.MAX_USD_AMOUNT}"
                        };
                    }

                    if (transaction.Currency.ToUpper().Equals("BRL") && (transaction.Amount / exchangeRate.Buying) > Currency.MAX_BRL_AMOUNT)
                    {
                        return new DetailResponse
                        {
                            Message = $"Warning|The amount has exceeded the limit of {transaction.Currency}{Currency.MAX_BRL_AMOUNT}"
                        };
                    }

                transaction.Date = DateTime.UtcNow;
                _logger.LogInformation("TransactionImp-purchase-Preparing the transaction object to be saved in database");
                _context.Add(transaction);

                _logger.LogInformation("TransactionImp-purchase-Before saving the transaction in database");
                int id = _context.SaveChanges();
                _logger.LogInformation("TransactionImp-purchase-After executing _context.SaveChanges() had this result-->int id = "+ id);

                if (id == 0) 
                    {                        
                        return new DetailResponse
                        {                            
                            Message = "Error|Something went wrong while processing the transaction, please try again"
                        };
                    }
                    
                    return new DetailResponse
                    {
                        Id = id,
                        Message = "Success|Transaction completed successfully",
                        Data = Math.Round((transaction.Amount / exchangeRate.Buying),4)
                    };
                }
                else 
                {
                    return new DetailResponse
                    {                        
                        Message = details.Message,                        
                    };
                }
            
        }
    }
}
