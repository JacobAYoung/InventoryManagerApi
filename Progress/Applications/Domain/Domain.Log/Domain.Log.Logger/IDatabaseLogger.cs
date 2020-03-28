using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Log.Logger
{
    public interface IDatabaseLogger
    {
        void DatabaseLog(string category, LogLevel logLevel, EventId eventId, string Message, Exception exception);
    }
}
