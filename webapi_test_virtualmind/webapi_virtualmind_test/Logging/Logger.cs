using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace webapi_virtualmind_test.Logging
{
    public class Logger : ILogger
    {
        private readonly LoggerProvider _loggerProvider;        
        public Logger(LoggerProvider loggerProvider) 
        {
            _loggerProvider = loggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string fullPath = string.Format("{0}/{1}", _loggerProvider._options.FolderPath, _loggerProvider._options.FilePath.Replace("{date}",DateTime.UtcNow.ToString("yyyy-MM-dd")));
            string logRecord = string.Format("{0} [{1}] {2} {3}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), logLevel.ToString(), formatter(state, exception), (exception != null) ? exception.StackTrace : "");

            using (StreamWriter streamWriter = new StreamWriter(fullPath, true)) 
            {
                streamWriter.WriteLine(logRecord);
            }
        }
    }
}
