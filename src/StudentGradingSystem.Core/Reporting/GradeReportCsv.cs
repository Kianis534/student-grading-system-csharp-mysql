using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Core.Reporting;

/// <summary>Exports a course's final grades as an RFC 4180 CSV file (opens cleanly in Excel).</summary>
public static class GradeReportCsv
{
    public static void Write(TextWriter writer, IEnumerable<StudentGrade> grades)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (grades is null)
        {
            throw new ArgumentNullException(nameof(grades));
        }

        writer.WriteLine("Student ID,Student Name,Percentage,Final Grade");
        foreach (StudentGrade grade in grades)
        {
            writer.WriteLine(string.Join(
                ",",
                grade.StudentId.ToString(CultureInfo.InvariantCulture),
                Escape(grade.StudentName),
                grade.Percentage.ToString("0.00", CultureInfo.InvariantCulture),
                Escape(grade.FinalGrade)));
        }
    }

    public static string ToCsv(IEnumerable<StudentGrade> grades)
    {
        using var writer = new StringWriter(CultureInfo.InvariantCulture);
        Write(writer, grades);
        return writer.ToString();
    }

    /// <summary>
    /// Quotes a field when needed and neutralises spreadsheet formulas
    /// (values starting with = + - @ are prefixed with an apostrophe to prevent CSV injection).
    /// </summary>
    internal static string Escape(string? value)
    {
        string text = value ?? string.Empty;

        if (text.Length > 0 && (text[0] == '=' || text[0] == '+' || text[0] == '-' || text[0] == '@'))
        {
            text = "'" + text;
        }

        bool mustQuote = text.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0
                         || text.StartsWith(" ", StringComparison.Ordinal)
                         || text.EndsWith(" ", StringComparison.Ordinal);

        if (!mustQuote)
        {
            return text;
        }

        var builder = new StringBuilder(text.Length + 2);
        builder.Append('"').Append(text.Replace("\"", "\"\"")).Append('"');
        return builder.ToString();
    }
}
