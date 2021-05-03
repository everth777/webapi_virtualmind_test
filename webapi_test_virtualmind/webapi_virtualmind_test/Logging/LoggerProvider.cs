using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_virtualmind_test.Logging
{
    [ProviderAlias("LoggerProvider")]
    public class LoggerProvider : ILoggerProvider
    {
        public readonly LoggerOptions _options;

        public LoggerProvider(IOptions<LoggerOptions> options) 
        {
            _options = options.Value;

            if (!Directory.Exists(_options.FolderPath)) 
            {
                Directory.CreateDirectory(_options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(this); 
        }

        public void Dispose()
        {            
        }
    }
}
