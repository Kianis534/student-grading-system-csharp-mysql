using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace StudentGradingSystem.Infrastructure;

/// <summary>Where the active connection string came from.</summary>
internal enum ConnectionStringSource
{
    None,
    EnvironmentVariable,
    LocalConfigFile,
}

/// <summary>
/// Resolves the database connection string without ever storing it in source control.
/// </summary>
/// <remarks>
/// Lookup order:
/// <list type="number">
///   <item><c>SGS_CONNECTION_STRING</c> environment variable</item>
///   <item><c>Database.config</c> next to the executable (git-ignored)</item>
/// </list>
/// </remarks>
internal static class AppConfiguration
{
    public const string EnvironmentVariableName = "SGS_CONNECTION_STRING";
    public const string LocalConfigFileName = "Database.config";
    public const string ConnectionStringKey = "SGS.ConnectionString";

    /// <summary>Marker used in the template; a connection string containing it hasn't been configured yet.</summary>
    public const string PasswordPlaceholder = "YOUR_MYSQL_PASSWORD";

    public static string LocalConfigPath =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LocalConfigFileName);

    /// <summary>Returns the connection string to use, or <c>null</c> if none is configured.</summary>
    public static string? GetConnectionString(out ConnectionStringSource source)
    {
        string? fromEnvironment = Environment.GetEnvironmentVariable(EnvironmentVariableName);
        if (!string.IsNullOrWhiteSpace(fromEnvironment))
        {
            source = ConnectionStringSource.EnvironmentVariable;
            return fromEnvironment!.Trim();
        }

        string? fromFile = ReadLocalConfig();
        if (!string.IsNullOrWhiteSpace(fromFile))
        {
            source = ConnectionStringSource.LocalConfigFile;
            return fromFile;
        }

        source = ConnectionStringSource.None;
        return null;
    }

    /// <summary>True when the value is missing or still contains the template placeholder.</summary>
    public static bool IsUnconfigured(string? connectionString) =>
        string.IsNullOrWhiteSpace(connectionString)
        || connectionString!.IndexOf(PasswordPlaceholder, StringComparison.OrdinalIgnoreCase) >= 0;

    public static bool IsEnvironmentOverrideActive =>
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(EnvironmentVariableName));

    /// <summary>Reads the connection string from Database.config, or returns <c>null</c>.</summary>
    public static string? ReadLocalConfig()
    {
        string path = LocalConfigPath;
        if (!File.Exists(path))
        {
            return null;
        }

        XDocument document = XDocument.Load(path);
        XElement? entry = document.Root?
            .Elements("add")
            .FirstOrDefault(e => string.Equals((string?)e.Attribute("key"), ConnectionStringKey, StringComparison.Ordinal));

        string? value = (string?)entry?.Attribute("value");
        return string.IsNullOrWhiteSpace(value) ? null : value!.Trim();
    }

    /// <summary>Writes (or overwrites) Database.config next to the executable.</summary>
    /// <exception cref="UnauthorizedAccessException">The application folder is read-only.</exception>
    public static void SaveLocalConfig(string connectionString)
    {
        var document = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XComment(" Student Grading System - local database settings. Git-ignored: never commit this file. "),
            new XElement(
                "appSettings",
                new XElement(
                    "add",
                    new XAttribute("key", ConnectionStringKey),
                    new XAttribute("value", connectionString))));

        document.Save(LocalConfigPath);
    }
}
