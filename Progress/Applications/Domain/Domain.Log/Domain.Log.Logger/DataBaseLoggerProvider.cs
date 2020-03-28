using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Domain.Log.Logger
{
    public class DataBaseLoggerProvider : ILoggerProvider
    {
        private readonly DatabaseLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, Logger> _loggers = new ConcurrentDictionary<string, Logger>();
        public DataBaseLoggerProvider(DatabaseLoggerConfiguration config)
        {
            _config = config;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new Logger(name, _config));
        }
        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
