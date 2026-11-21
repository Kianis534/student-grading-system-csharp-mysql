using System;
using System.Collections.Generic;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Core.Services;

/// <summary>Everything a student can see about their own courses.</summary>
public sealed class CourseDetails
{
    public CourseDetails(StudentCourseResult course, IReadOnlyList<ActivityResult> activities)
    {
        Course = course;
        Activities = activities;
    }

    public StudentCourseResult Course { get; }

    public IReadOnlyList<ActivityResult> Activities { get; }
}

/// <summary>Read-only, student-scoped access to courses and results.</summary>
public sealed class ResultsService
{
    private readonly ICourseRepository _courses;
    private readonly IMarksRepository _marks;

    public ResultsService(ICourseRepository courses, IMarksRepository marks)
    {
        _courses = courses ?? throw new ArgumentNullException(nameof(courses));
        _marks = marks ?? throw new ArgumentNullException(nameof(marks));
    }

    public IReadOnlyList<StudentCourseResult> GetCourses(int studentId) => _courses.GetCoursesForStudent(studentId);

    /// <summary>
    /// Marks breakdown for one course. Returns <c>null</c> if the student is not enrolled,
    /// so a student can never read another course's data.
    /// </summary>
    public CourseDetails? GetCourseDetails(int studentId, int courseId)
    {
        // GetCoursesForStudent only returns enrolled courses, so this doubles as the access check.
        StudentCourseResult? course = null;
        foreach (StudentCourseResult c in _courses.GetCoursesForStudent(studentId))
        {
            if (c.CourseId == courseId)
            {
                course = c;
                break;
            }
        }

        if (course is null)
        {
            return null;
        }

        return new CourseDetails(course, _marks.GetActivityResultsForStudent(studentId, courseId));
    }
}
