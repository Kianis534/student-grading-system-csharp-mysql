using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Core.Validation;

/// <summary>
/// Business rules for course assessments (quizzes, assignments, exams…) and the marks recorded against them.
/// </summary>
public static class ActivityRules
{
    /// <summary>Longest allowed assessment name (matches the database column).</summary>
    public const int MaxNameLength = 50;

    /// <summary>Highest total marks an assessment can have (DECIMAL(6,2) column).</summary>
    public const decimal MaxTotalMarks = 1000m;

    /// <summary>All assessments in a course together may not exceed this weightage.</summary>
    public const decimal MaxCourseWeightage = 100m;

    /// <summary>
    /// Total marks, weightages and obtained marks are stored in DECIMAL(x,2) columns. Anything finer would be
    /// rounded silently by the database (or turn into 0.00 and break a CHECK constraint), so it's rejected up front.
    /// </summary>
    public const int MaxDecimalPlaces = 2;

    /// <summary>Suggested names offered in the UI. Instructors may type their own.</summary>
    public static IReadOnlyList<string> SuggestedNames { get; } = new[]
    {
        "Quiz 1", "Quiz 2", "Assignment 1", "Assignment 2", "Lab Work",
        "Project", "Class Participation", "Mid Term", "Final Exam",
    };

    /// <summary>Validates a new assessment against the ones already in the course.</summary>
    public static ValidationResult ValidateNewActivity(
        string? name,
        decimal totalMarks,
        decimal weightage,
        IReadOnlyCollection<Activity> existingActivities)
    {
        if (existingActivities is null)
        {
            throw new ArgumentNullException(nameof(existingActivities));
        }

        string trimmed = (name ?? string.Empty).Trim();
        if (trimmed.Length == 0)
        {
            return ValidationResult.Fail("Enter a name for the assessment, for example \"Quiz 1\".");
        }

        if (trimmed.Length > MaxNameLength)
        {
            return ValidationResult.Fail($"Assessment names can be at most {MaxNameLength} characters.");
        }

        if (existingActivities.Any(a => string.Equals(a.Name, trimmed, StringComparison.OrdinalIgnoreCase)))
        {
            return ValidationResult.Fail($"This course already has an assessment called \"{trimmed}\".");
        }

        if (totalMarks <= 0m || totalMarks > MaxTotalMarks)
        {
            return ValidationResult.Fail($"Total marks must be greater than 0 and at most {MaxTotalMarks:0}.");
        }

        if (!HasValidPrecision(totalMarks))
        {
            return ValidationResult.Fail($"Total marks can have at most {MaxDecimalPlaces} decimal places.");
        }

        if (weightage <= 0m || weightage > MaxCourseWeightage)
        {
            return ValidationResult.Fail("Weightage must be greater than 0% and at most 100%.");
        }

        if (!HasValidPrecision(weightage))
        {
            return ValidationResult.Fail($"Weightage can have at most {MaxDecimalPlaces} decimal places.");
        }

        decimal allocated = existingActivities.Sum(a => a.Weightage);
        if (allocated + weightage > MaxCourseWeightage)
        {
            decimal remaining = MaxCourseWeightage - allocated;
            return ValidationResult.Fail(
                $"Weightage would exceed 100%. {Format(allocated)}% is already allocated, so at most {Format(remaining)}% is left.");
        }

        return ValidationResult.Success;
    }

    /// <summary>Validates marks entered for one student. <c>null</c> means "not entered yet" and is allowed.</summary>
    public static ValidationResult ValidateObtainedMarks(decimal? obtainedMarks, decimal totalMarks)
    {
        if (obtainedMarks is null)
        {
            return ValidationResult.Success;
        }

        if (obtainedMarks < 0m || obtainedMarks > totalMarks)
        {
            return ValidationResult.Fail($"Marks must be between 0 and {Format(totalMarks)}.");
        }

        if (!HasValidPrecision(obtainedMarks.Value))
        {
            return ValidationResult.Fail($"Marks can have at most {MaxDecimalPlaces} decimal places.");
        }

        return ValidationResult.Success;
    }

    private static bool HasValidPrecision(decimal value) => decimal.Round(value, MaxDecimalPlaces) == value;

    private static string Format(decimal value) => value.ToString("0.##", CultureInfo.InvariantCulture);
}
