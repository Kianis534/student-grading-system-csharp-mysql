using System;
using System.Data;
using MySqlConnector;

namespace StudentGradingSystem.Core.Data;

/// <summary>Creates open connections to the application database.</summary>
public interface IConnectionFactory
{
    MySqlConnection OpenConnection();
}

/// <summary>Opens MySQL connections from a single connection string.</summary>
public sealed class MySqlConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public MySqlConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("A connection string is required.", nameof(connectionString));
        }

        // Fail fast on malformed strings instead of on the first query.
        try
        {
            _connectionString = new MySqlConnectionStringBuilder(connectionString).ConnectionString;
        }
        catch (ArgumentException ex)
        {
            throw new InvalidConnectionStringException($"The connection string is not valid: {ex.Message}", ex);
        }
    }

    public MySqlConnection OpenConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Opens a connection and checks that the schema is installed.
    /// Throws a <see cref="MySqlException"/> if the server, credentials or schema are wrong.
    /// </summary>
    public static void TestConnection(string connectionString)
    {
        var factory = new MySqlConnectionFactory(connectionString);
        using MySqlConnection connection = factory.OpenConnection();
        using var command = new MySqlCommand("SELECT COUNT(*) FROM Users;", connection);
        command.ExecuteScalar();
    }
}

/// <summary>Small helpers that keep the repositories readable.</summary>
internal static class DbExtensions
{
    public static MySqlCommand CreateCommand(this MySqlConnection connection, string sql, MySqlTransaction? transaction = null)
    {
        return new MySqlCommand(sql, connection, transaction);
    }

    public static MySqlCommand With(this MySqlCommand command, string name, object? value)
    {
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        return command;
    }

    public static decimal? ReadNullableDecimal(this IDataRecord record, string column)
    {
        int ordinal = record.GetOrdinal(column);
        return record.IsDBNull(ordinal) ? null : Convert.ToDecimal(record.GetValue(ordinal));
    }

    public static int? ReadNullableInt(this IDataRecord record, string column)
    {
        int ordinal = record.GetOrdinal(column);
        return record.IsDBNull(ordinal) ? null : Convert.ToInt32(record.GetValue(ordinal));
    }

    public static string? ReadNullableString(this IDataRecord record, string column)
    {
        int ordinal = record.GetOrdinal(column);
        return record.IsDBNull(ordinal) ? null : record.GetString(ordinal);
    }

    public static DateTime? ReadNullableDateTime(this IDataRecord record, string column)
    {
        int ordinal = record.GetOrdinal(column);
        return record.IsDBNull(ordinal) ? null : record.GetDateTime(ordinal);
    }

    public static int ReadInt(this IDataRecord record, string column) =>
        Convert.ToInt32(record.GetValue(record.GetOrdinal(column)));

    public static decimal ReadDecimal(this IDataRecord record, string column) =>
        Convert.ToDecimal(record.GetValue(record.GetOrdinal(column)));

    public static string ReadString(this IDataRecord record, string column) =>
        Convert.ToString(record.GetValue(record.GetOrdinal(column)), System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty;
}
