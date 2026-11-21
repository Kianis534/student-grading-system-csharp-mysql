using System;
using System.Collections.Generic;
using System.Globalization;
using StudentGradingSystem.Core.Grading;
using StudentGradingSystem.Core.Models;
using Xunit;

namespace StudentGradingSystem.Core.Tests.Unit;

public class GradeScaleTests
{
    [Theory]
    [InlineData("100", "A")]
    [InlineData("90", "A")]
    [InlineData("89.99", "A-")]
    [InlineData("85", "A-")]
    [InlineData("84.99", "B+")]
    [InlineData("80", "B+")]
    [InlineData("75", "B")]
    [InlineData("70", "B-")]
    [InlineData("65", "C+")]
    [InlineData("60", "C")]
    [InlineData("55", "C-")]
    [InlineData("50", "D")]
    [InlineData("49.99", "F")]
    [InlineData("0", "F")]
    public void GetLetterGrade_UsesInclusiveLowerBounds(string percentage, string expected)
    {
        decimal value = decimal.Parse(percentage, CultureInfo.InvariantCulture);

        Assert.Equal(expected, GradeScale.GetLetterGrade(value));
    }

    [Theory]
    [InlineData("-0.01")]
    [InlineData("100.01")]
    public void GetLetterGrade_RejectsOutOfRangePercentages(string percentage)
    {
        decimal value = decimal.Parse(percentage, CultureInfo.InvariantCulture);

        Assert.Throws<ArgumentOutOfRangeException>(() => GradeScale.GetLetterGrade(value));
    }

    [Fact]
    public void Bands_AreOrderedFromHighestToLowest()
    {
        for (int i = 1; i < GradeScale.Bands.Count; i++)
        {
            Assert.True(GradeScale.Bands[i - 1].MinimumPercentage > GradeScale.Bands[i].MinimumPercentage);
        }
    }

    [Theory]
    [InlineData("A", true)]
    [InlineData("D", true)]
    [InlineData("F", false)]
    [InlineData("f", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsPassing_TreatsEverythingButFAsPassing(string? grade, bool expected)
    {
        Assert.Equal(expected, GradeScale.IsPassing(grade));
    }
}

public class GradeCalculatorTests
{
    [Fact]
    public void Calculate_WeightsEachAssessment()
    {
        // John Smith in the seeded CS301 course.
        var scores = new[]
        {
            new ActivityScore(totalMarks: 10, weightage: 10, obtainedMarks: 9.5m),
            new ActivityScore(totalMarks: 20, weightage: 15, obtainedMarks: 19),
            new ActivityScore(totalMarks: 50, weightage: 25, obtainedMarks: 46),
            new ActivityScore(totalMarks: 100, weightage: 50, obtainedMarks: 92),
        };

        GradeResult result = GradeCalculator.Calculate(scores);

        Assert.Equal(92.75m, result.Percentage);
        Assert.Equal("A", result.LetterGrade);
    }

    [Fact]
    public void Calculate_TreatsMissingMarksAsZero()
    {
        var scores = new[]
        {
            new ActivityScore(totalMarks: 50, weightage: 50, obtainedMarks: 50),
            new ActivityScore(totalMarks: 50, weightage: 50, obtainedMarks: null),
        };

        GradeResult result = GradeCalculator.Calculate(scores);

        Assert.Equal(50m, result.Percentage);
        Assert.Equal("D", result.LetterGrade);
    }

    [Fact]
    public void Calculate_RoundsToTwoDecimalsAwayFromZeroBeforeGrading()
    {
        // 89.995% rounds up to 90.00% and earns an A, not an A-.
        var scores = new[] { new ActivityScore(totalMarks: 1000, weightage: 100, obtainedMarks: 899.95m) };

        GradeResult result = GradeCalculator.Calculate(scores);

        Assert.Equal(90.00m, result.Percentage);
        Assert.Equal("A", result.LetterGrade);
    }

    [Fact]
    public void Calculate_WithNoAssessments_ReturnsZeroAndF()
    {
        GradeResult result = GradeCalculator.Calculate(Array.Empty<ActivityScore>());

        Assert.Equal(0m, result.Percentage);
        Assert.Equal("F", result.LetterGrade);
    }

    [Fact]
    public void Calculate_ClampsToOneHundred()
    {
        var scores = new[] { new ActivityScore(totalMarks: 10, weightage: 100, obtainedMarks: 12) };

        Assert.Equal(100m, GradeCalculator.Calculate(scores).Percentage);
    }

    [Fact]
    public void ActivityScore_RejectsInvalidInput()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ActivityScore(0, 10, 5));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ActivityScore(10, -1, 5));
    }

    [Fact]
    public void CalculateWeightedPercentage_RejectsNull()
    {
        Assert.Throws<ArgumentNullException>(() => GradeCalculator.CalculateWeightedPercentage(null!));
    }
}

public class GradeStatisticsTests
{
    [Fact]
    public void From_SummarisesTheClass()
    {
        var grades = new List<StudentGrade>
        {
            new StudentGrade(1, "A", 92.75m, "A"),
            new StudentGrade(2, "B", 77.50m, "B"),
            new StudentGrade(3, "C", 40.00m, "F"),
        };

        GradeStatistics stats = GradeStatistics.From(grades);

        Assert.Equal(3, stats.Count);
        Assert.Equal(70.08m, stats.Average);
        Assert.Equal(92.75m, stats.Highest);
        Assert.Equal(40.00m, stats.Lowest);
        Assert.Equal(2, stats.Passed);
        Assert.Equal(1, stats.Failed);
        Assert.Equal(1, stats.Distribution["A"]);
        Assert.Equal(1, stats.Distribution["B"]);
        Assert.Equal(1, stats.Distribution["F"]);
        Assert.Equal(0, stats.Distribution["C"]);
    }

    [Fact]
    public void From_EmptyList_ReturnsZeros()
    {
        GradeStatistics stats = GradeStatistics.From(new List<StudentGrade>());

        Assert.Equal(0, stats.Count);
        Assert.Equal(0m, stats.Average);
        Assert.Equal(GradeScale.Bands.Count, stats.Distribution.Count);
    }
}
