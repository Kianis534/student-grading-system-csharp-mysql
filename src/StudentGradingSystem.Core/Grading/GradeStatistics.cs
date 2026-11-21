using System;
using System.Collections.Generic;
using System.Linq;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Core.Grading;

/// <summary>Class-level summary of a set of final grades.</summary>
public sealed class GradeStatistics
{
    private GradeStatistics(
        int count,
        decimal average,
        decimal highest,
        decimal lowest,
        int passed,
        IReadOnlyDictionary<string, int> distribution)
    {
        Count = count;
        Average = average;
        Highest = highest;
        Lowest = lowest;
        Passed = passed;
        Distribution = distribution;
    }

    public int Count { get; }

    public decimal Average { get; }

    public decimal Highest { get; }

    public decimal Lowest { get; }

    public int Passed { get; }

    public int Failed => Count - Passed;

    /// <summary>Number of students per letter grade, in <see cref="GradeScale.Bands"/> order.</summary>
    public IReadOnlyDictionary<string, int> Distribution { get; }

    public static GradeStatistics From(IReadOnlyCollection<StudentGrade> grades)
    {
        if (grades is null)
        {
            throw new ArgumentNullException(nameof(grades));
        }

        var distribution = GradeScale.Bands.ToDictionary(b => b.Letter, _ => 0, StringComparer.OrdinalIgnoreCase);
        foreach (StudentGrade grade in grades)
        {
            if (distribution.ContainsKey(grade.FinalGrade))
            {
                distribution[grade.FinalGrade]++;
            }
        }

        if (grades.Count == 0)
        {
            return new GradeStatistics(0, 0m, 0m, 0m, 0, distribution);
        }

        return new GradeStatistics(
            grades.Count,
            Math.Round(grades.Average(g => g.Percentage), 2, MidpointRounding.AwayFromZero),
            grades.Max(g => g.Percentage),
            grades.Min(g => g.Percentage),
            grades.Count(g => GradeScale.IsPassing(g.FinalGrade)),
            distribution);
    }
}
