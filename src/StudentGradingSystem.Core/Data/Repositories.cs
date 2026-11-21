using System.Collections.Generic;
using StudentGradingSystem.Core.Grading;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Core.Data;

public interface IUserRepository
{
    /// <summary>Looks up an account by username (case-insensitive). Returns <c>null</c> if none exists.</summary>
    UserAccount? FindByUsername(string username);

    void UpdatePasswordHash(int userId, string passwordHash);
}

public interface ICourseRepository
{
    IReadOnlyList<CourseSummary> GetCourseSummaries();

    CourseSummary? GetCourseSummary(int courseId);

    IReadOnlyList<StudentCourseResult> GetCoursesForStudent(int studentId);

    bool IsStudentEnrolled(int studentId, int courseId);
}

public interface IActivityRepository
{
    IReadOnlyList<Activity> GetByCourse(int courseId);

    Activity Add(int courseId, string name, decimal totalMarks, decimal weightage);

    /// <summary>Deletes an assessment and (via cascade) every mark recorded for it.</summary>
    bool Delete(int activityId);
}

/// <summary>All of one student's assessment scores in a course, ready for grading.</summary>
public sealed class StudentScores
{
    public StudentScores(int studentId, string studentName, IReadOnlyList<ActivityScore> scores)
    {
        StudentId = studentId;
        StudentName = studentName;
        Scores = scores;
    }

    public int StudentId { get; }

    public string StudentName { get; }

    public IReadOnlyList<ActivityScore> Scores { get; }
}

public interface IMarksRepository
{
    /// <summary>Every student enrolled in the assessment's course, with any marks already recorded.</summary>
    IReadOnlyList<StudentMarkEntry> GetMarksForActivity(int activityId);

    /// <summary>Saves marks in one transaction. Entries with <c>null</c> marks are cleared.</summary>
    int SaveMarks(int activityId, IEnumerable<StudentMarkEntry> entries);

    IReadOnlyList<StudentScores> GetScoresForCourse(int courseId);

    IReadOnlyList<ActivityResult> GetActivityResultsForStudent(int studentId, int courseId);
}

public interface IGradeRepository
{
    IReadOnlyList<StudentGrade> GetGradesForCourse(int courseId);

    /// <summary>Replaces every stored grade for the course in one transaction.</summary>
    void ReplaceGradesForCourse(int courseId, IEnumerable<StudentGrade> grades);

    StudentGrade? GetGrade(int studentId, int courseId);
}
