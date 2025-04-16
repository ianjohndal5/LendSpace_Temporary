using System;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

class TestMySQL
{
    static void Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();

        string connectionString = configuration.GetConnectionString("MySqlConnection");

        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connected to MySQL successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MySQL: {ex.Message}");
            }
        }
    }
}
