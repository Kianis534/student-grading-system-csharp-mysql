using System;
using System.Collections.Generic;

namespace StudentGradingSystem.Core.Grading;

/// <summary>One assessment's marks as they feed into the final grade.</summary>
public sealed class ActivityScore
{
    public ActivityScore(decimal totalMarks, decimal weightage, decimal? obtainedMarks)
    {
        if (totalMarks <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(totalMarks), "Total marks must be greater than zero.");
        }

        if (weightage < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(weightage), "Weightage cannot be negative.");
        }

        TotalMarks = totalMarks;
        Weightage = weightage;
        ObtainedMarks = obtainedMarks;
    }

    public decimal TotalMarks { get; }

    /// <summary>Share of the final grade, in percent (e.g. 25 for 25%).</summary>
    public decimal Weightage { get; }

    /// <summary>Marks earned, or <c>null</c> when nothing has been recorded yet.</summary>
    public decimal? ObtainedMarks { get; }

    /// <summary>Percentage points this assessment adds to the final result.</summary>
    public decimal Contribution =>
        ObtainedMarks is decimal obtained ? obtained / TotalMarks * Weightage : 0m;
}

/// <summary>The outcome of a grade calculation.</summary>
public sealed class GradeResult
{
    public GradeResult(decimal percentage, string letterGrade)
    {
        Percentage = percentage;
        LetterGrade = letterGrade;
    }

    /// <summary>Weighted percentage rounded to two decimals.</summary>
    public decimal Percentage { get; }

    public string LetterGrade { get; }
}

/// <summary>
/// Turns assessment marks into a weighted percentage and letter grade.
/// </summary>
/// <remarks>
/// <para>final % = Σ (obtained ÷ total × weightage)</para>
/// <para>Assessments without recorded marks contribute zero. The result is rounded to two
/// decimals (midpoint away from zero) and clamped to 0–100 before the letter is looked up.</para>
/// </remarks>
public static class GradeCalculator
{
    /// <summary>Unrounded weighted percentage for the given scores.</summary>
    public static decimal CalculateWeightedPercentage(IEnumerable<ActivityScore> scores)
    {
        if (scores is null)
        {
            throw new ArgumentNullException(nameof(scores));
        }

        decimal total = 0m;
        foreach (ActivityScore score in scores)
        {
            total += score.Contribution;
        }

        return total;
    }

    /// <summary>Rounded percentage plus the matching letter grade.</summary>
    public static GradeResult Calculate(IEnumerable<ActivityScore> scores)
    {
        decimal raw = CalculateWeightedPercentage(scores);
        decimal percentage = Math.Round(raw, 2, MidpointRounding.AwayFromZero);
        percentage = Math.Min(100m, Math.Max(0m, percentage));

        return new GradeResult(percentage, GradeScale.GetLetterGrade(percentage));
    }
}
