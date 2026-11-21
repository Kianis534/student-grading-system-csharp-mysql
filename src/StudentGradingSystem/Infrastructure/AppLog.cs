using System;
using System.Globalization;
using System.IO;

namespace StudentGradingSystem.Infrastructure;

/// <summary>
/// Minimal file logger for unexpected errors: %LOCALAPPDATA%\StudentGradingSystem\logs\app-yyyyMMdd.log.
/// Logging must never crash the app, so every failure here is swallowed.
/// </summary>
internal static class AppLog
{
    private static readonly object Gate = new object();

    public static string LogDirectory { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "StudentGradingSystem",
        "logs");

    public static void Error(Exception exception, string context)
    {
        Write("ERROR", $"{context}{Environment.NewLine}{exception}");
    }

    public static void Info(string message)
    {
        Write("INFO", message);
    }

    private static void Write(string level, string message)
    {
        try
        {
            lock (Gate)
            {
                Directory.CreateDirectory(LogDirectory);
                string file = Path.Combine(
                    LogDirectory,
                    $"app-{DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.log");
                string line = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0:yyyy-MM-dd HH:mm:ss.fff} [{1}] {2}{3}",
                    DateTime.Now,
                    level,
                    message,
                    Environment.NewLine);
                File.AppendAllText(file, line);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
