using System;
using System.Collections.Generic;
using System.Linq;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Core.Grading;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Security;
using StudentGradingSystem.Core.Services;
using StudentGradingSystem.Core.Validation;
using Xunit;

namespace StudentGradingSystem.Core.Tests.Unit;

public class AuthServiceTests
{
    private static readonly string CurrentHash = PasswordHasher.Hash("Student@123", PasswordHasher.DefaultIterations);

    private sealed class FakeUsers : IUserRepository
    {
        public Dictionary<string, UserAccount> Accounts { get; } = new(StringComparer.OrdinalIgnoreCase);

        public List<(int UserId, string Hash)> Updates { get; } = new();

        public UserAccount? FindByUsername(string username) =>
            Accounts.TryGetValue(username, out UserAccount? account) ? account : null;

        public void UpdatePasswordHash(int userId, string passwordHash) => Updates.Add((userId, passwordHash));
    }

    [Fact]
    public void SignIn_WithCorrectPassword_ReturnsStudentWithTheirName()
    {
        var users = new FakeUsers();
        users.Accounts["jsmith"] = new UserAccount(2, "jsmith", CurrentHash, UserRole.Student, 1, "John Smith");

        AuthenticatedUser? user = new AuthService(users).SignIn("  JSmith ", "Student@123");

        Assert.NotNull(user);
        Assert.Equal(UserRole.Student, user!.Role);
        Assert.Equal(1, user.StudentId);
        Assert.Equal("John Smith", user.DisplayName);
        Assert.Empty(users.Updates);
    }

    [Fact]
    public void SignIn_Instructor_UsesTitleCasedUsernameAsDisplayName()
    {
        var users = new FakeUsers();
        users.Accounts["instructor"] = new UserAccount(1, "instructor", CurrentHash, UserRole.Instructor, null, null);

        AuthenticatedUser? user = new AuthService(users).SignIn("instructor", "Student@123");

        Assert.Equal("Instructor", user?.DisplayName);
    }

    [Theory]
    [InlineData("jsmith", "wrong")]
    [InlineData("nobody", "Student@123")]
    [InlineData("", "Student@123")]
    [InlineData("jsmith", "")]
    [InlineData(null, null)]
    public void SignIn_RejectsBadCredentials(string? username, string? password)
    {
        var users = new FakeUsers();
        users.Accounts["jsmith"] = new UserAccount(2, "jsmith", CurrentHash, UserRole.Student, 1, "John Smith");

        Assert.Null(new AuthService(users).SignIn(username, password));
    }

    [Fact]
    public void SignIn_StudentAccountWithoutStudentRecord_Throws()
    {
        var users = new FakeUsers();
        users.Accounts["broken"] = new UserAccount(9, "broken", CurrentHash, UserRole.Student, null, null);

        Assert.Throws<InvalidOperationException>(() => new AuthService(users).SignIn("broken", "Student@123"));
    }

    [Fact]
    public void SignIn_UpgradesHashesCreatedWithAWeakerWorkFactor()
    {
        var users = new FakeUsers();
        string legacyHash = PasswordHasher.Hash("Student@123", PasswordHasher.MinimumIterations);
        users.Accounts["jsmith"] = new UserAccount(2, "jsmith", legacyHash, UserRole.Student, 1, "John Smith");

        new AuthService(users).SignIn("jsmith", "Student@123");

        (int userId, string newHash) = Assert.Single(users.Updates);
        Assert.Equal(2, userId);
        Assert.StartsWith($"{PasswordHasher.Scheme}${PasswordHasher.DefaultIterations}$", newHash);
        Assert.True(PasswordHasher.Verify("Student@123", newHash));
    }
}

public class GradingAndCourseServiceTests
{
    private sealed class FakeActivities : IActivityRepository
    {
        public List<Activity> Items { get; } = new();

        public IReadOnlyList<Activity> GetByCourse(int courseId) => Items.Where(a => a.CourseId == courseId).ToList();

        public Activity Add(int courseId, string name, decimal totalMarks, decimal weightage)
        {
            var activity = new Activity(Items.Count + 1, courseId, name, totalMarks, weightage);
            Items.Add(activity);
            return activity;
        }

        public bool Delete(int activityId) => Items.RemoveAll(a => a.ActivityId == activityId) > 0;
    }

    private sealed class FakeMarks : IMarksRepository
    {
        public List<StudentScores> Scores { get; } = new();

        public List<StudentMarkEntry> Saved { get; } = new();

        public IReadOnlyList<StudentMarkEntry> GetMarksForActivity(int activityId) => Saved;

        public int SaveMarks(int activityId, IEnumerable<StudentMarkEntry> entries)
        {
            Saved.AddRange(entries);
            return Saved.Count;
        }

        public IReadOnlyList<StudentScores> GetScoresForCourse(int courseId) => Scores;

        public IReadOnlyList<ActivityResult> GetActivityResultsForStudent(int studentId, int courseId) =>
            Array.Empty<ActivityResult>();
    }

    private sealed class FakeGrades : IGradeRepository
    {
        public Dictionary<int, List<StudentGrade>> Stored { get; } = new();

        public IReadOnlyList<StudentGrade> GetGradesForCourse(int courseId) =>
            Stored.TryGetValue(courseId, out List<StudentGrade>? grades) ? grades : new List<StudentGrade>();

        public void ReplaceGradesForCourse(int courseId, IEnumerable<StudentGrade> grades) => Stored[courseId] = grades.ToList();

        public StudentGrade? GetGrade(int studentId, int courseId) =>
            GetGradesForCourse(courseId).FirstOrDefault(g => g.StudentId == studentId);
    }

    private sealed class FakeCourses : ICourseRepository
    {
        public IReadOnlyList<CourseSummary> GetCourseSummaries() => Array.Empty<CourseSummary>();

        public CourseSummary? GetCourseSummary(int courseId) => null;

        public IReadOnlyList<StudentCourseResult> GetCoursesForStudent(int studentId) => Array.Empty<StudentCourseResult>();

        public bool IsStudentEnrolled(int studentId, int courseId) => false;
    }

    [Fact]
    public void CalculateAndSaveGrades_GradesEveryStudentAndReplacesStoredGrades()
    {
        var activities = new FakeActivities();
        var marks = new FakeMarks();
        var grades = new FakeGrades();
        marks.Scores.Add(new StudentScores(1, "Top", new[] { new ActivityScore(100, 100, 95) }));
        marks.Scores.Add(new StudentScores(2, "Unmarked", Array.Empty<ActivityScore>()));
        grades.Stored[7] = new List<StudentGrade> { new StudentGrade(99, "Stale", 10, "F") };

        IReadOnlyList<StudentGrade> result = new GradingService(activities, marks, grades).CalculateAndSaveGrades(7);

        Assert.Equal(2, result.Count);
        Assert.Equal("A", result[0].FinalGrade);
        Assert.Equal(0m, result[1].Percentage);
        Assert.Equal("F", result[1].FinalGrade);
        Assert.Equal(new[] { 1, 2 }, grades.Stored[7].Select(g => g.StudentId));
    }

    [Fact]
    public void AddActivity_ValidatesBeforeSaving()
    {
        var activities = new FakeActivities();
        var service = new CourseService(new FakeCourses(), activities, new FakeMarks());

        ValidationResult ok = service.AddActivity(1, "  Mid Term ", 50, 60, out Activity? created);
        ValidationResult tooMuch = service.AddActivity(1, "Final Exam", 100, 50, out Activity? rejected);

        Assert.True(ok.IsValid);
        Assert.Equal("Mid Term", created?.Name);
        Assert.False(tooMuch.IsValid);
        Assert.Null(rejected);
        Assert.Single(activities.Items);
    }

    [Fact]
    public void SaveMarks_RejectsTheWholeBatchIfAnyEntryIsInvalid()
    {
        var marks = new FakeMarks();
        var service = new CourseService(new FakeCourses(), new FakeActivities(), marks);
        var activity = new Activity(1, 1, "Quiz 1", 10, 10);
        var entries = new[]
        {
            new StudentMarkEntry(1, "Valid", 8),
            new StudentMarkEntry(2, "Too High", 11),
        };

        ValidationResult result = service.SaveMarks(activity, entries);

        Assert.False(result.IsValid);
        Assert.StartsWith("Too High:", result.Error);
        Assert.Empty(marks.Saved);
    }
}
