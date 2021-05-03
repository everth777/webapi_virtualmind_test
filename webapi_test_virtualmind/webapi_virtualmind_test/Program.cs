using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi_virtualmind_test.Logging;

namespace webapi_virtualmind_test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)            
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
              .ConfigureLogging((context, logging) =>
              {
                  logging.AddLogger(options => {
                      context.Configuration.GetSection("Logging").GetSection("LoggerProvider").GetSection("Options").Bind(options);
                  });
              })                            
            ;
    }
}
