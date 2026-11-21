using System;
using System.Collections.Generic;
using System.Linq;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Core.Grading;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Core.Services;

/// <summary>Calculates, stores and reads final course grades.</summary>
public sealed class GradingService
{
    private readonly IActivityRepository _activities;
    private readonly IMarksRepository _marks;
    private readonly IGradeRepository _grades;

    public GradingService(IActivityRepository activities, IMarksRepository marks, IGradeRepository grades)
    {
        _activities = activities ?? throw new ArgumentNullException(nameof(activities));
        _marks = marks ?? throw new ArgumentNullException(nameof(marks));
        _grades = grades ?? throw new ArgumentNullException(nameof(grades));
    }

    /// <summary>Total weightage currently allocated to assessments in the course.</summary>
    public decimal GetAllocatedWeightage(int courseId) => _activities.GetByCourse(courseId).Sum(a => a.Weightage);

    /// <summary>Computes every enrolled student's final grade without saving anything.</summary>
    public IReadOnlyList<StudentGrade> PreviewGrades(int courseId)
    {
        return _marks.GetScoresForCourse(courseId)
            .Select(student =>
            {
                GradeResult result = GradeCalculator.Calculate(student.Scores);
                return new StudentGrade(student.StudentId, student.StudentName, result.Percentage, result.LetterGrade);
            })
            .ToList();
    }

    /// <summary>Recalculates every enrolled student's grade and replaces the stored grades for the course.</summary>
    public IReadOnlyList<StudentGrade> CalculateAndSaveGrades(int courseId)
    {
        IReadOnlyList<StudentGrade> grades = PreviewGrades(courseId);
        _grades.ReplaceGradesForCourse(courseId, grades);
        return grades;
    }

    public IReadOnlyList<StudentGrade> GetGrades(int courseId) => _grades.GetGradesForCourse(courseId);
}
