using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.IO;

public class DatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();
        _connectionString = configuration.GetConnectionString("MySqlConnection");
    }

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    public void TestConnection()
    {
        using (MySqlConnection conn = new MySqlConnection(_connectionString))
        {
            try
            {
                conn.Open();
                Console.WriteLine("Connected to MySQL!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MySQL: {ex.Message}");
            }
        }
    }
}