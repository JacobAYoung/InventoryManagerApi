using Microsoft.Extensions.Logging;
using System;

namespace Domain.Log.Logger
{
    public class Logger : ILogger
    {
        private readonly string _name;
        private readonly DatabaseLoggerConfiguration _config;

        public Logger(string name)
        {
            _name = name;
            _config = new DatabaseLoggerConfiguration();
        }

        public Logger(string name, DatabaseLoggerConfiguration config)
        {
            _name = name;
            _config = config;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                IDatabaseLogger databaseLogger = new DatabaseLogger();
                databaseLogger.DatabaseLog(_name, logLevel, eventId, state.ToString(), exception);
                Console.WriteLine($"{logLevel.ToString()} - {eventId.Id} - {_name} - {formatter(state, exception)}");
            }
        }
    }
}
