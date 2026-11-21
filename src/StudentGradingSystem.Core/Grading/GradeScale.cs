using System;
using System.Collections.Generic;

namespace StudentGradingSystem.Core.Grading;

/// <summary>A letter grade and the minimum percentage needed to earn it.</summary>
public sealed class GradeBand
{
    public GradeBand(string letter, decimal minimumPercentage)
    {
        Letter = letter;
        MinimumPercentage = minimumPercentage;
    }

    public string Letter { get; }

    public decimal MinimumPercentage { get; }
}

/// <summary>
/// Maps a final percentage (0–100) to a letter grade.
/// </summary>
public static class GradeScale
{
    /// <summary>Lowest letter grade that still counts as a pass.</summary>
    public const string LowestPassingGrade = "D";

    /// <summary>Grade bands ordered from highest to lowest.</summary>
    public static IReadOnlyList<GradeBand> Bands { get; } = new[]
    {
        new GradeBand("A", 90m),
        new GradeBand("A-", 85m),
        new GradeBand("B+", 80m),
        new GradeBand("B", 75m),
        new GradeBand("B-", 70m),
        new GradeBand("C+", 65m),
        new GradeBand("C", 60m),
        new GradeBand("C-", 55m),
        new GradeBand("D", 50m),
        new GradeBand("F", 0m),
    };

    /// <summary>Returns the letter grade for a percentage between 0 and 100 (inclusive).</summary>
    /// <exception cref="ArgumentOutOfRangeException">The percentage is outside 0–100.</exception>
    public static string GetLetterGrade(decimal percentage)
    {
        if (percentage < 0m || percentage > 100m)
        {
            throw new ArgumentOutOfRangeException(nameof(percentage), percentage, "Percentage must be between 0 and 100.");
        }

        foreach (GradeBand band in Bands)
        {
            if (percentage >= band.MinimumPercentage)
            {
                return band.Letter;
            }
        }

        return "F";
    }

    /// <summary>True when <paramref name="letter"/> is a passing grade (anything but F).</summary>
    public static bool IsPassing(string? letter) =>
        !string.IsNullOrEmpty(letter) && !string.Equals(letter, "F", StringComparison.OrdinalIgnoreCase);
}
