using System;
using Microsoft.Data.Sqlite;

namespace SqLiteConsoleTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var connection = new SqliteConnection("Data Source = :memory:"))
            { 
                connection.Open();

            var command = new SqliteCommand("SELECT 1;", connection);

                long result = (long)command.ExecuteScalar();
                Console.WriteLine($"command output: {result}");

            }    
        }
    }
}
