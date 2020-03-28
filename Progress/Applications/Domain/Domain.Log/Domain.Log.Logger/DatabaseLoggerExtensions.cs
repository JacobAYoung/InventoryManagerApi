using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Log.Logger
{
    public static class DatabaseLoggerExtensions
    {
        public static ILoggerFactory AddDatabaseLogger(this ILoggerFactory loggerFactory, DatabaseLoggerConfiguration config)
        {
            loggerFactory.AddProvider(new DataBaseLoggerProvider(config));
            return loggerFactory;
        }
        public static ILoggerFactory AddDatabaseLogger(this ILoggerFactory loggerFactory)
        {
            var allowedConfigs = new List<LogLevel>{ LogLevel.Information, LogLevel.Error, LogLevel.Warning, LogLevel.Critical };
            allowedConfigs.ForEach(config => loggerFactory.AddDatabaseLogger(new DatabaseLoggerConfiguration() { LogLevel = config }));
            return loggerFactory;
        }
        public static ILoggerFactory AddDatabaseLogger(this ILoggerFactory loggerFactory, Action<DatabaseLoggerConfiguration> configure)
        {
            var config = new DatabaseLoggerConfiguration();
            configure(config);
            return loggerFactory.AddDatabaseLogger(config);
        }
    }
}
