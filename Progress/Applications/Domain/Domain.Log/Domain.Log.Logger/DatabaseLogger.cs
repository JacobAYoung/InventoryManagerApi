using System;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Domain.Log.Logger
{
    public class DatabaseLogger : IDatabaseLogger
    {
        private readonly string ConnectionString = "Server=(localdb)\\localdb;Database=Product;Trusted_Connection=True;";
        public void DatabaseLog(string category, LogLevel logLevel, EventId eventId, string Message, Exception exception)
        {
			try
			{
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    var parameters = new { EventID = eventId.Id, EventName = "Name", LogLevel = logLevel.ToString(), Category = category, Message = Message, Exception = exception?.Message };
                    connection.Execute("[dbo].[LogRecordInsert]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
			}
			catch (Exception ex)
			{
                Console.WriteLine($"Error logging to Database. {ex}");
			}
        }
    }
}
