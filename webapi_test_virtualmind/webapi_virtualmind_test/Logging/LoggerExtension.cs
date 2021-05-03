
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_virtualmind_test.Logging
{
    public static class LoggerExtension
    {      
        public static ILoggingBuilder AddLogger(this ILoggingBuilder builder, Action<LoggerOptions> configure)
		{              
            builder.Services.AddSingleton<ILoggerProvider, LoggerProvider>();
			builder.Services.Configure(configure);
			return builder;
		}
	}
}
