using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace CS2_LegHider;

public class DatabaseService(string connectionString, ILogger logger)
{
    private readonly string _connectionString = connectionString;
    private readonly ILogger _logger = logger;
    
    public MySqlConnection GetConnection()
    {
        try
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            _logger?.LogCritical("Unable to connect to database: {message}", ex.Message);
            throw;
        }
    }

    public async Task<MySqlConnection> GetConnectionAsync()
    {
        try
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
        catch (Exception ex)
        {
            _logger?.LogCritical("Unable to connect to database: {message}", ex.Message);
            throw;
        }
    }

    public bool CheckDatabaseConnection()
    {
        using var connection = GetConnection();

        try
        {
            return connection.Ping();
        }
        catch
        {
            return false;
        }
    }

    public void CreateTable()
    {
        using var connection = GetConnection();

        // Create a table with steamid and a boolean for leghider
        var command = new MySqlCommand(@"
            CREATE TABLE IF NOT EXISTS `leghider_status` (
                `steamid` VARCHAR(64) NOT NULL PRIMARY KEY,
                `is_enabled` BOOLEAN NOT NULL DEFAULT FALSE
            )", connection);
        command.ExecuteNonQuery();
    }
}