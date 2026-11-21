using System;
using MySqlConnector;

namespace StudentGradingSystem.Core.Data;

/// <summary>How a database failure should be presented to the user.</summary>
public enum DatabaseErrorKind
{
    /// <summary>Not a recognised connectivity or setup problem.</summary>
    Other,

    /// <summary>The MySQL server can't be reached.</summary>
    ServerUnreachable,

    /// <summary>Wrong MySQL username or password.</summary>
    AccessDenied,

    /// <summary>The database or its tables haven't been created yet.</summary>
    SchemaMissing,

    /// <summary>The connection string itself is malformed.</summary>
    InvalidConfiguration,
}

/// <summary>Thrown when a connection string can't be parsed.</summary>
public sealed class InvalidConnectionStringException : Exception
{
    public InvalidConnectionStringException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>Turns low-level MySQL exceptions into messages a user can act on.</summary>
public static class DatabaseErrors
{
    private const string SetupHint = "See the \"Database setup\" section of README.md.";

    public static DatabaseErrorKind Classify(Exception exception)
    {
        Exception? current = exception;
        while (current is not null)
        {
            if (current is MySqlException mySql)
            {
                switch (mySql.Number)
                {
                    case 1042: // Unable to connect to any of the specified MySQL hosts
                    case 2002: // Can't connect to local server
                    case 2003: // Can't connect to MySQL server
                    case 2005: // Unknown host
                        return DatabaseErrorKind.ServerUnreachable;
                    case 1044: // Access denied to database
                    case 1045: // Access denied for user
                        return DatabaseErrorKind.AccessDenied;
                    case 1049: // Unknown database
                    case 1146: // Table doesn't exist
                    case 1054: // Unknown column (schema from an older version)
                        return DatabaseErrorKind.SchemaMissing;
                }
            }

            if (current is InvalidConnectionStringException)
            {
                return DatabaseErrorKind.InvalidConfiguration;
            }

            current = current.InnerException;
        }

        return DatabaseErrorKind.Other;
    }

    /// <summary>A short, friendly explanation with a next step.</summary>
    public static string Describe(Exception exception)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return Classify(exception) switch
        {
            DatabaseErrorKind.ServerUnreachable =>
                "Can't reach the MySQL server. Make sure MySQL is running and that the server name and port in your database settings are correct.",
            DatabaseErrorKind.AccessDenied =>
                "MySQL rejected the username or password. Update your database settings with valid MySQL credentials.",
            DatabaseErrorKind.SchemaMissing =>
                "The StudentGradingSystem database or its tables were not found. Run database/01-schema.sql and database/02-seed.sql, then try again. " + SetupHint,
            DatabaseErrorKind.InvalidConfiguration =>
                "The database connection string is not valid. " + SetupHint,
            _ => exception.Message,
        };
    }

    /// <summary>True for problems that are fixed by changing database settings rather than retrying.</summary>
    public static bool IsConfigurationProblem(Exception exception) =>
        Classify(exception) is DatabaseErrorKind.ServerUnreachable
            or DatabaseErrorKind.AccessDenied
            or DatabaseErrorKind.SchemaMissing
            or DatabaseErrorKind.InvalidConfiguration;
}
