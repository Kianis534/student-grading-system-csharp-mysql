using System;
using System.IO;
using MySqlConnector;
using Xunit;

namespace StudentGradingSystem.Core.Tests.Integration;

/// <summary>
/// Runs a test only when a MySQL server is available.
/// Set SGS_TEST_CONNECTION_STRING to a server-level connection string, for example:
/// <c>Server=127.0.0.1;Port=3306;User ID=root;Password=&lt;your password&gt;</c>
/// </summary>
public sealed class MySqlFactAttribute : FactAttribute
{
    public MySqlFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(MySqlTestDatabase.ServerConnectionString))
        {
            Skip = $"Set {MySqlTestDatabase.EnvironmentVariable} to run MySQL integration tests.";
        }
    }
}

/// <summary>
/// Creates a uniquely named database from database/01-schema.sql and 02-seed.sql,
/// and drops it again when the test class finishes. Tests never touch a real database.
/// </summary>
public sealed class MySqlTestDatabase : IDisposable
{
    public const string EnvironmentVariable = "SGS_TEST_CONNECTION_STRING";
    private const string ProductionDatabaseName = "StudentGradingSystem";

    public MySqlTestDatabase()
    {
        if (string.IsNullOrWhiteSpace(ServerConnectionString))
        {
            return;
        }

        DatabaseName = "sgs_test_" + Guid.NewGuid().ToString("N").Substring(0, 12);

        var builder = new MySqlConnectionStringBuilder(ServerConnectionString!) { Database = string.Empty };
        string serverOnly = builder.ConnectionString;

        builder.Database = DatabaseName;
        ConnectionString = builder.ConnectionString;

        RunScript(serverOnly, "01-schema.sql");
        RunScript(serverOnly, "02-seed.sql");
    }

    public static string? ServerConnectionString => Environment.GetEnvironmentVariable(EnvironmentVariable);

    public string DatabaseName { get; } = string.Empty;

    /// <summary>Connection string for the throwaway database.</summary>
    public string ConnectionString { get; } = string.Empty;

    public void Execute(string sql)
    {
        using var connection = new MySqlConnection(ConnectionString);
        connection.Open();
        using var command = new MySqlCommand(sql, connection);
        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        if (DatabaseName.Length == 0)
        {
            return;
        }

        var builder = new MySqlConnectionStringBuilder(ConnectionString) { Database = string.Empty };
        using var connection = new MySqlConnection(builder.ConnectionString);
        connection.Open();
        using var command = new MySqlCommand($"DROP DATABASE IF EXISTS `{DatabaseName}`;", connection);
        command.ExecuteNonQuery();
    }

    private void RunScript(string serverConnectionString, string fileName)
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", fileName);
        string sql = File.ReadAllText(path).Replace(ProductionDatabaseName, DatabaseName);

        using var connection = new MySqlConnection(serverConnectionString);
        connection.Open();
        using var command = new MySqlCommand(sql, connection) { CommandTimeout = 120 };
        command.ExecuteNonQuery();
    }
}
