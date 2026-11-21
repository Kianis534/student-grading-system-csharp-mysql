using System;
using System.Collections.Generic;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Data.InMemory;

/// <summary>
/// The whole database held in memory. Mirrors the MySQL schema, including its unique keys and
/// cascading deletes, so the repositories behave the same way as the MySQL ones.
/// </summary>
/// <remarks>Not persisted: everything is lost when the process (or browser tab) ends.</remarks>
public sealed class InMemoryDataStore
{
    internal object Gate { get; } = new object();

    internal List<StudentRow> Students { get; } = new();

    internal List<UserRow> Users { get; } = new();

    internal List<CourseRow> Courses { get; } = new();

    internal List<EnrollmentRow> Enrollments { get; } = new();

    internal List<ActivityRow> Activities { get; } = new();

    internal List<MarkRow> Marks { get; } = new();

    internal List<GradeRow> Grades { get; } = new();

    private int _nextActivityId = 1;

    internal int NextActivityId() => _nextActivityId++;

    public void AddStudent(int studentId, string name)
    {
        lock (Gate)
        {
            Students.Add(new StudentRow(studentId, name));
        }
    }

    public void AddUser(int userId, string username, string passwordHash, UserRole role, int? studentId)
    {
        lock (Gate)
        {
            if (Users.Exists(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Username '{username}' already exists.");
            }

            if ((role == UserRole.Student) != studentId.HasValue)
            {
                throw new InvalidOperationException("Student accounts need a student record; instructor accounts must not have one.");
            }

            Users.Add(new UserRow(userId, username, passwordHash, role, studentId));
        }
    }

    public void AddCourse(int courseId, string code, string title, string semester, string status = "Active")
    {
        lock (Gate)
        {
            Courses.Add(new CourseRow(courseId, code, title, semester, status));
        }
    }

    public void Enroll(int courseId, int studentId)
    {
        lock (Gate)
        {
            if (!Enrollments.Exists(e => e.CourseId == courseId && e.StudentId == studentId))
            {
                Enrollments.Add(new EnrollmentRow(courseId, studentId));
            }
        }
    }

    public int AddActivity(int courseId, string name, decimal totalMarks, decimal weightage)
    {
        lock (Gate)
        {
            if (Activities.Exists(a => a.CourseId == courseId && string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Course {courseId} already has an assessment called '{name}'.");
            }

            int id = NextActivityId();
            Activities.Add(new ActivityRow(id, courseId, name, totalMarks, weightage));
            return id;
        }
    }

    public void SetMark(int activityId, int studentId, decimal obtainedMarks)
    {
        lock (Gate)
        {
            Marks.RemoveAll(m => m.ActivityId == activityId && m.StudentId == studentId);
            Marks.Add(new MarkRow(activityId, studentId, obtainedMarks));
        }
    }

    internal sealed class StudentRow
    {
        public StudentRow(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }

    internal sealed class UserRow
    {
        public UserRow(int id, string username, string passwordHash, UserRole role, int? studentId)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            StudentId = studentId;
        }

        public int Id { get; }

        public string Username { get; }

        public string PasswordHash { get; set; }

        public UserRole Role { get; }

        public int? StudentId { get; }
    }

    internal sealed class CourseRow
    {
        public CourseRow(int id, string code, string title, string semester, string status)
        {
            Id = id;
            Code = code;
            Title = title;
            Semester = semester;
            Status = status;
        }

        public int Id { get; }

        public string Code { get; }

        public string Title { get; }

        public string Semester { get; }

        public string Status { get; }
    }

    internal sealed class EnrollmentRow
    {
        public EnrollmentRow(int courseId, int studentId)
        {
            CourseId = courseId;
            StudentId = studentId;
        }

        public int CourseId { get; }

        public int StudentId { get; }
    }

    internal sealed class ActivityRow
    {
        public ActivityRow(int id, int courseId, string name, decimal totalMarks, decimal weightage)
        {
            Id = id;
            CourseId = courseId;
            Name = name;
            TotalMarks = totalMarks;
            Weightage = weightage;
        }

        public int Id { get; }

        public int CourseId { get; }

        public string Name { get; }

        public decimal TotalMarks { get; }

        public decimal Weightage { get; }
    }

    internal sealed class MarkRow
    {
        public MarkRow(int activityId, int studentId, decimal obtainedMarks)
        {
            ActivityId = activityId;
            StudentId = studentId;
            ObtainedMarks = obtainedMarks;
        }

        public int ActivityId { get; }

        public int StudentId { get; }

        public decimal ObtainedMarks { get; }
    }

    internal sealed class GradeRow
    {
        public GradeRow(int courseId, int studentId, decimal percentage, string finalGrade, DateTime calculatedAt)
        {
            CourseId = courseId;
            StudentId = studentId;
            Percentage = percentage;
            FinalGrade = finalGrade;
            CalculatedAt = calculatedAt;
        }

        public int CourseId { get; }

        public int StudentId { get; }

        public decimal Percentage { get; }

        public string FinalGrade { get; }

        public DateTime CalculatedAt { get; }
    }
}
