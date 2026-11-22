using System;
using System.Collections.Generic;
using System.Globalization;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Reporting;
using StudentGradingSystem.Core.Validation;
using Xunit;

namespace StudentGradingSystem.Core.Tests.Unit;

public class ActivityRulesTests
{
    private static readonly IReadOnlyCollection<Activity> NoActivities = Array.Empty<Activity>();

    private static IReadOnlyCollection<Activity> Existing(params (string Name, decimal Weight)[] items)
    {
        var list = new List<Activity>();
        int id = 1;
        foreach ((string name, decimal weight) in items)
        {
            list.Add(new Activity(id++, 1, name, 10, weight));
        }

        return list;
    }

    [Fact]
    public void ValidateNewActivity_AcceptsAValidAssessment()
    {
        ValidationResult result = ActivityRules.ValidateNewActivity("Quiz 1", 10, 10, NoActivities);

        Assert.True(result.IsValid);
        Assert.Null(result.Error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateNewActivity_RequiresAName(string? name)
    {
        Assert.False(ActivityRules.ValidateNewActivity(name, 10, 10, NoActivities).IsValid);
    }

    [Fact]
    public void ValidateNewActivity_RejectsNamesLongerThanTheColumn()
    {
        string name = new string('x', ActivityRules.MaxNameLength + 1);

        Assert.False(ActivityRules.ValidateNewActivity(name, 10, 10, NoActivities).IsValid);
    }

    [Fact]
    public void ValidateNewActivity_RejectsDuplicateNamesIgnoringCase()
    {
        ValidationResult result = ActivityRules.ValidateNewActivity("  quiz 1 ", 10, 10, Existing(("Quiz 1", 10)));

        Assert.False(result.IsValid);
        Assert.Contains("already has", result.Error);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(1000.01)]
    public void ValidateNewActivity_RejectsOutOfRangeTotalMarks(double totalMarks)
    {
        Assert.False(ActivityRules.ValidateNewActivity("Quiz", (decimal)totalMarks, 10, NoActivities).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100.5)]
    public void ValidateNewActivity_RejectsOutOfRangeWeightage(double weightage)
    {
        Assert.False(ActivityRules.ValidateNewActivity("Quiz", 10, (decimal)weightage, NoActivities).IsValid);
    }

    [Theory]
    [InlineData("0.001", "10")]
    [InlineData("10.555", "10")]
    [InlineData("10", "0.004")]
    [InlineData("10", "12.345")]
    public void ValidateNewActivity_RejectsMoreThanTwoDecimalPlaces(string totalMarks, string weightage)
    {
        ValidationResult result = ActivityRules.ValidateNewActivity(
            "Quiz", decimal.Parse(totalMarks, CultureInfo.InvariantCulture), decimal.Parse(weightage, CultureInfo.InvariantCulture), NoActivities);

        Assert.False(result.IsValid);
        Assert.Contains("at most 2 decimal places", result.Error);
    }

    [Fact]
    public void ValidateNewActivity_AcceptsTwoDecimalPlacesAndTrailingZeros()
    {
        Assert.True(ActivityRules.ValidateNewActivity("Quiz", 12.75m, 12.50m, NoActivities).IsValid);
        Assert.True(ActivityRules.ValidateNewActivity("Quiz", 12.7500m, 12.5000m, NoActivities).IsValid);
    }

    [Fact]
    public void ValidateNewActivity_KeepsCourseWeightageAtOrBelow100()
    {
        IReadOnlyCollection<Activity> existing = Existing(("Mid Term", 30), ("Quiz 1", 10), ("Assignment 1", 20));

        ValidationResult tooMuch = ActivityRules.ValidateNewActivity("Final Exam", 100, 41, existing);
        ValidationResult exactlyEnough = ActivityRules.ValidateNewActivity("Final Exam", 100, 40, existing);

        Assert.False(tooMuch.IsValid);
        Assert.Contains("at most 40% is left", tooMuch.Error);
        Assert.True(exactlyEnough.IsValid);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData(0.0, true)]
    [InlineData(25.5, true)]
    [InlineData(50.0, true)]
    [InlineData(50.01, false)]
    [InlineData(-1.0, false)]
    public void ValidateObtainedMarks_AllowsEmptyOrZeroToTotal(double? marks, bool expected)
    {
        decimal? value = marks.HasValue ? (decimal)marks.Value : null;

        Assert.Equal(expected, ActivityRules.ValidateObtainedMarks(value, totalMarks: 50).IsValid);
    }

    [Theory]
    [InlineData("9.999")]
    [InlineData("0.001")]
    public void ValidateObtainedMarks_RejectsMoreThanTwoDecimalPlaces(string marks)
    {
        ValidationResult result = ActivityRules.ValidateObtainedMarks(decimal.Parse(marks, CultureInfo.InvariantCulture), totalMarks: 10);

        Assert.False(result.IsValid);
        Assert.Contains("at most 2 decimal places", result.Error);
    }
}

public class GradeReportCsvTests
{
    [Fact]
    public void ToCsv_WritesHeaderAndOneRowPerStudent()
    {
        var grades = new[]
        {
            new StudentGrade(1, "John Smith", 92.75m, "A"),
            new StudentGrade(2, "Emily Johnson", 87m, "A-"),
        };

        string[] lines = GradeReportCsv.ToCsv(grades).TrimEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        Assert.Equal(3, lines.Length);
        Assert.Equal("Student ID,Student Name,Percentage,Final Grade", lines[0]);
        Assert.Equal("1,John Smith,92.75,A", lines[1]);
        Assert.Equal("2,Emily Johnson,87.00,A-", lines[2]);
    }

    [Theory]
    [InlineData("Plain", "Plain")]
    [InlineData("Smith, John", "\"Smith, John\"")]
    [InlineData("The \"Rock\"", "\"The \"\"Rock\"\"\"")]
    [InlineData("=HYPERLINK(\"x\")", "\"'=HYPERLINK(\"\"x\"\")\"")]
    [InlineData("+1", "'+1")]
    [InlineData("@cmd", "'@cmd")]
    [InlineData("", "")]
    public void Escape_QuotesSpecialCharactersAndBlocksFormulaInjection(string input, string expected)
    {
        Assert.Equal(expected, GradeReportCsv.Escape(input));
    }
}
