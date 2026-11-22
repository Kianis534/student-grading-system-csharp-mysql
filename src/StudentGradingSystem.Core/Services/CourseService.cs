using System;
using System.Collections.Generic;
using System.Linq;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Validation;

namespace StudentGradingSystem.Core.Services;

/// <summary>
/// Instructor workflows for a course: assessments and marks entry. Once a course's grades have been released,
/// every change to its assessments or marks recalculates them, so students never see a grade that no longer
/// matches their marks.
/// </summary>
public sealed class CourseService
{
    private readonly ICourseRepository _courses;
    private readonly IActivityRepository _activities;
    private readonly IMarksRepository _marks;
    private readonly IGradeRepository _grades;

    public CourseService(ICourseRepository courses, IActivityRepository activities, IMarksRepository marks, IGradeRepository grades)
    {
        _courses = courses ?? throw new ArgumentNullException(nameof(courses));
        _activities = activities ?? throw new ArgumentNullException(nameof(activities));
        _marks = marks ?? throw new ArgumentNullException(nameof(marks));
        _grades = grades ?? throw new ArgumentNullException(nameof(grades));
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
        RefreshReleasedGrades(courseId);
        return ValidationResult.Success;
    }

    /// <summary>Deletes an assessment and every mark recorded for it. Returns <c>false</c> if it was already gone.</summary>
    public bool DeleteActivity(Activity activity)
    {
        if (activity is null)
        {
            throw new ArgumentNullException(nameof(activity));
        }

        bool deleted = _activities.Delete(activity.ActivityId);
        if (deleted)
        {
            RefreshReleasedGrades(activity.CourseId);
        }

        return deleted;
    }

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
        RefreshReleasedGrades(activity.CourseId);
        return ValidationResult.Success;
    }

    /// <summary>Recalculates the course's stored grades if they've been released; otherwise leaves it ungraded.</summary>
    private void RefreshReleasedGrades(int courseId)
    {
        if (_grades.GetGradesForCourse(courseId).Count > 0)
        {
            _grades.ReplaceGradesForCourse(courseId, GradingService.Calculate(_marks, courseId));
        }
    }
}
