using Microsoft.Extensions.Logging;

namespace Domain.Log.Logger
{
    public class DatabaseLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public string Category { get; set; }

        public string Message { get; set; }

        public int EventId { get; set; } = 0;
    }
}
