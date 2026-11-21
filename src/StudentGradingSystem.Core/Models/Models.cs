using System;

namespace StudentGradingSystem.Core.Models;

/// <summary>What a signed-in user is allowed to do.</summary>
public enum UserRole
{
    Instructor,
    Student,
}

/// <summary>A user account as stored in the database, including its password hash.</summary>
public sealed class UserAccount
{
    public UserAccount(int userId, string username, string passwordHash, UserRole role, int? studentId, string? studentName)
    {
        UserId = userId;
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        StudentId = studentId;
        StudentName = studentName;
    }

    public int UserId { get; }

    public string Username { get; }

    public string PasswordHash { get; }

    public UserRole Role { get; }

    /// <summary>Linked student record; only set for <see cref="UserRole.Student"/> accounts.</summary>
    public int? StudentId { get; }

    public string? StudentName { get; }
}

/// <summary>The person currently signed in. Never carries the password hash.</summary>
public sealed class AuthenticatedUser
{
    public AuthenticatedUser(int userId, string username, UserRole role, int? studentId, string displayName)
    {
        UserId = userId;
        Username = username;
        Role = role;
        StudentId = studentId;
        DisplayName = displayName;
    }

    public int UserId { get; }

    public string Username { get; }

    public UserRole Role { get; }

    public int? StudentId { get; }

    public string DisplayName { get; }
}

/// <summary>A course plus the numbers an instructor needs at a glance.</summary>
public sealed class CourseSummary
{
    public CourseSummary(
        int courseId,
        string code,
        string title,
        string semester,
        string status,
        int enrolledStudents,
        int activityCount,
        decimal allocatedWeightage)
    {
        CourseId = courseId;
        Code = code;
        Title = title;
        Semester = semester;
        Status = status;
        EnrolledStudents = enrolledStudents;
        ActivityCount = activityCount;
        AllocatedWeightage = allocatedWeightage;
    }

    public int CourseId { get; }

    public string Code { get; }

    public string Title { get; }

    public string Semester { get; }

    public string Status { get; }

    public int EnrolledStudents { get; }

    public int ActivityCount { get; }

    /// <summary>Sum of all assessment weightages in this course (0–100).</summary>
    public decimal AllocatedWeightage { get; }

    public string DisplayName => $"{Code} · {Title}";
}

/// <summary>A graded assessment in a course (quiz, assignment, mid term, ...).</summary>
public sealed class Activity
{
    public Activity(int activityId, int courseId, string name, decimal totalMarks, decimal weightage)
    {
        ActivityId = activityId;
        CourseId = courseId;
        Name = name;
        TotalMarks = totalMarks;
        Weightage = weightage;
    }

    public int ActivityId { get; }

    public int CourseId { get; }

    /// <summary>Display name, e.g. "Quiz 1". Stored in the <c>ActivityType</c> column.</summary>
    public string Name { get; }

    public decimal TotalMarks { get; }

    /// <summary>Share of the final grade, in percent.</summary>
    public decimal Weightage { get; }
}

/// <summary>One row on the marks-entry screen: a student and the marks recorded for one assessment.</summary>
public sealed class StudentMarkEntry
{
    public StudentMarkEntry(int studentId, string studentName, decimal? obtainedMarks)
    {
        StudentId = studentId;
        StudentName = studentName;
        ObtainedMarks = obtainedMarks;
    }

    public int StudentId { get; }

    public string StudentName { get; }

    /// <summary>Marks recorded, or <c>null</c> if none have been entered.</summary>
    public decimal? ObtainedMarks { get; set; }
}

/// <summary>A student's calculated final result in a course.</summary>
public sealed class StudentGrade
{
    public StudentGrade(int studentId, string studentName, decimal percentage, string finalGrade, DateTime? calculatedAt = null)
    {
        StudentId = studentId;
        StudentName = studentName;
        Percentage = percentage;
        FinalGrade = finalGrade;
        CalculatedAt = calculatedAt;
    }

    public int StudentId { get; }

    public string StudentName { get; }

    public decimal Percentage { get; }

    public string FinalGrade { get; }

    public DateTime? CalculatedAt { get; }
}

/// <summary>A course on the student dashboard, with the student's grade if it has been released.</summary>
public sealed class StudentCourseResult
{
    public StudentCourseResult(int courseId, string code, string title, string semester, decimal? percentage, string? finalGrade)
    {
        CourseId = courseId;
        Code = code;
        Title = title;
        Semester = semester;
        Percentage = percentage;
        FinalGrade = finalGrade;
    }

    public int CourseId { get; }

    public string Code { get; }

    public string Title { get; }

    public string Semester { get; }

    public decimal? Percentage { get; }

    public string? FinalGrade { get; }

    public bool IsGraded => FinalGrade is not null;
}

/// <summary>A student's marks for one assessment, shown on the course details screen.</summary>
public sealed class ActivityResult
{
    public ActivityResult(string activityName, decimal totalMarks, decimal weightage, decimal? obtainedMarks)
    {
        ActivityName = activityName;
        TotalMarks = totalMarks;
        Weightage = weightage;
        ObtainedMarks = obtainedMarks;
    }

    public string ActivityName { get; }

    public decimal TotalMarks { get; }

    public decimal Weightage { get; }

    public decimal? ObtainedMarks { get; }

    /// <summary>Percentage points earned toward the final grade (0 when no marks are recorded).</summary>
    public decimal Contribution => ObtainedMarks is decimal obtained ? obtained / TotalMarks * Weightage : 0m;
}
