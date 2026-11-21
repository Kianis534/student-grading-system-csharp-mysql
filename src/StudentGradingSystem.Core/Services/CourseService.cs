using System;
using System.Collections.Generic;
using System.Linq;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Validation;

namespace StudentGradingSystem.Core.Services;

/// <summary>Instructor workflows for a course: assessments and marks entry.</summary>
public sealed class CourseService
{
    private readonly ICourseRepository _courses;
    private readonly IActivityRepository _activities;
    private readonly IMarksRepository _marks;

    public CourseService(ICourseRepository courses, IActivityRepository activities, IMarksRepository marks)
    {
        _courses = courses ?? throw new ArgumentNullException(nameof(courses));
        _activities = activities ?? throw new ArgumentNullException(nameof(activities));
        _marks = marks ?? throw new ArgumentNullException(nameof(marks));
    }

    public IReadOnlyList<CourseSummary> GetCourses() => _courses.GetCourseSummaries();

    public CourseSummary? GetCourse(int courseId) => _courses.GetCourseSummary(courseId);

    public IReadOnlyList<Activity> GetActivities(int courseId) => _activities.GetByCourse(courseId);

    /// <summary>Validates and adds an assessment. Returns the failure reason, or the new activity on success.</summary>
    public ValidationResult AddActivity(int courseId, string? name, decimal totalMarks, decimal weightage, out Activity? created)
    {
        created = null;
        IReadOnlyList<Activity> existing = _activities.GetByCourse(courseId);

        ValidationResult result = ActivityRules.ValidateNewActivity(name, totalMarks, weightage, existing.ToList());
        if (!result.IsValid)
        {
            return result;
        }

        created = _activities.Add(courseId, name!.Trim(), totalMarks, weightage);
        return ValidationResult.Success;
    }

    public bool DeleteActivity(int activityId) => _activities.Delete(activityId);

    public IReadOnlyList<StudentMarkEntry> GetMarks(int activityId) => _marks.GetMarksForActivity(activityId);

    /// <summary>
    /// Validates every entry against the assessment's total marks, then saves them in one transaction.
    /// Nothing is written if any entry is invalid.
    /// </summary>
    public ValidationResult SaveMarks(Activity activity, IReadOnlyCollection<StudentMarkEntry> entries)
    {
        if (activity is null)
        {
            throw new ArgumentNullException(nameof(activity));
        }

        if (entries is null)
        {
            throw new ArgumentNullException(nameof(entries));
        }

        foreach (StudentMarkEntry entry in entries)
        {
            ValidationResult check = ActivityRules.ValidateObtainedMarks(entry.ObtainedMarks, activity.TotalMarks);
            if (!check.IsValid)
            {
                return ValidationResult.Fail($"{entry.StudentName}: {check.Error}");
            }
        }

        _marks.SaveMarks(activity.ActivityId, entries);
        return ValidationResult.Success;
    }
}
