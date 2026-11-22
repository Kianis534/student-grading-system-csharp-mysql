using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Services;
using StudentGradingSystem.Core.Validation;
using StudentGradingSystem.Data.MySql;
using Xunit;

namespace StudentGradingSystem.Core.Tests.Integration;

/// <summary>
/// End-to-end checks of the repositories and services against a real MySQL server,
/// using the same schema and seed scripts that users run.
/// </summary>
public class MySqlIntegrationTests : IClassFixture<MySqlTestDatabase>
{
    // Seeded course IDs (see database/02-seed.sql).
    private const int CS101 = 1;
    private const int CS102 = 2;
    private const int CS202 = 4;
    private const int CS301 = 5;

    private readonly MySqlTestDatabase _db;

    public MySqlIntegrationTests(MySqlTestDatabase db) => _db = db;

    private MySqlConnectionFactory Connections => new MySqlConnectionFactory(_db.ConnectionString);

    private AuthService Auth => new AuthService(new MySqlUserRepository(Connections));

    private CourseService Courses => new CourseService(
        new MySqlCourseRepository(Connections),
        new MySqlActivityRepository(Connections),
        new MySqlMarksRepository(Connections),
        new MySqlGradeRepository(Connections));

    private GradingService Grading => new GradingService(
        new MySqlActivityRepository(Connections),
        new MySqlMarksRepository(Connections),
        new MySqlGradeRepository(Connections));

    private ResultsService Results => new ResultsService(
        new MySqlCourseRepository(Connections),
        new MySqlMarksRepository(Connections));

    [MySqlFact]
    public void SeededAccounts_CanSignIn_AndWrongPasswordsCannot()
    {
        AuthenticatedUser? instructor = Auth.SignIn("instructor", "Instructor@123");
        AuthenticatedUser? student = Auth.SignIn("JSMITH", "Student@123");

        Assert.Equal(UserRole.Instructor, instructor?.Role);
        Assert.Equal(UserRole.Student, student?.Role);
        Assert.Equal("John Smith", student?.DisplayName);
        Assert.Null(Auth.SignIn("instructor", "Student@123"));
        Assert.Null(Auth.SignIn("ghost", "Student@123"));
    }

    [MySqlFact]
    public void CourseSummaries_ReflectSeedData()
    {
        IReadOnlyList<CourseSummary> courses = Courses.GetCourses();
        CourseSummary cs301 = courses.Single(c => c.CourseId == CS301);

        // Other tests in this class may add courses, so only check the seeded ones.
        Assert.Equal(10, courses.Count(c => c.CourseId <= 10));
        Assert.Equal("CS301", cs301.Code);
        Assert.Equal(10, cs301.EnrolledStudents);
        Assert.Equal(4, cs301.ActivityCount);
        Assert.Equal(100m, cs301.AllocatedWeightage);
    }

    [MySqlFact]
    public void RecalculatedGrades_MatchTheGradesComputedBySeedSql()
    {
        Dictionary<int, StudentGrade> seeded = Grading.GetGrades(CS301).ToDictionary(g => g.StudentId);

        IReadOnlyList<StudentGrade> recalculated = Grading.CalculateAndSaveGrades(CS301);

        Assert.Equal(10, recalculated.Count);
        foreach (StudentGrade grade in recalculated)
        {
            Assert.Equal(seeded[grade.StudentId].Percentage, grade.Percentage);
            Assert.Equal(seeded[grade.StudentId].FinalGrade, grade.FinalGrade);
        }

        Assert.Equal("A", recalculated.Single(g => g.StudentId == 1).FinalGrade);
        Assert.Equal("F", recalculated.Single(g => g.StudentId == 10).FinalGrade);
    }

    [MySqlFact]
    public void AddActivity_EnforcesWeightageAndUniqueNames()
    {
        Assert.True(Courses.AddActivity(CS101, "Quiz 1", 10, 60, out Activity? quiz).IsValid);
        Assert.NotNull(quiz);
        Assert.True(quiz!.ActivityId > 0);

        ValidationResult overweight = Courses.AddActivity(CS101, "Final Exam", 100, 41, out _);
        ValidationResult duplicate = Courses.AddActivity(CS101, "quiz 1", 10, 5, out _);

        Assert.False(overweight.IsValid);
        Assert.False(duplicate.IsValid);
        Assert.Single(Courses.GetActivities(CS101));

        // The database enforces uniqueness too, even if the service is bypassed.
        var repository = new MySqlActivityRepository(Connections);
        MySqlException ex = Assert.Throws<MySqlException>(() => repository.Add(CS101, "QUIZ 1", 10, 5));
        Assert.Equal(1062, ex.Number);
    }

    [MySqlFact]
    public void SaveMarks_UpsertsAndClearsInOneTransaction()
    {
        Assert.True(Courses.AddActivity(CS102, "Assignment 1", 20, 20, out Activity? activity).IsValid);

        List<StudentMarkEntry> entries = Courses.GetMarks(activity!.ActivityId).ToList();
        Assert.Equal(10, entries.Count);
        Assert.All(entries, e => Assert.Null(e.ObtainedMarks));

        entries[0].ObtainedMarks = 18;
        entries[1].ObtainedMarks = 12.5m;
        Assert.True(Courses.SaveMarks(activity, entries).IsValid);

        entries = Courses.GetMarks(activity.ActivityId).ToList();
        Assert.Equal(18m, entries[0].ObtainedMarks);
        Assert.Equal(12.5m, entries[1].ObtainedMarks);

        entries[0].ObtainedMarks = 19;
        entries[1].ObtainedMarks = null;
        Assert.True(Courses.SaveMarks(activity, entries).IsValid);

        entries = Courses.GetMarks(activity.ActivityId).ToList();
        Assert.Equal(19m, entries[0].ObtainedMarks);
        Assert.Null(entries[1].ObtainedMarks);

        entries[2].ObtainedMarks = 25; // above the total of 20
        Assert.False(Courses.SaveMarks(activity, entries).IsValid);
        Assert.Null(Courses.GetMarks(activity.ActivityId)[2].ObtainedMarks);
    }

    [MySqlFact]
    public void DeleteActivity_CascadesToItsMarks()
    {
        Assert.True(Courses.AddActivity(CS202, "Lab Work", 10, 10, out Activity? activity).IsValid);
        List<StudentMarkEntry> entries = Courses.GetMarks(activity!.ActivityId).ToList();
        entries.ForEach(e => e.ObtainedMarks = 7);
        Courses.SaveMarks(activity, entries);

        Assert.True(Courses.DeleteActivity(activity));

        Assert.Empty(Courses.GetActivities(CS202));
        Assert.Empty(Courses.GetMarks(activity.ActivityId));
        Assert.False(Courses.DeleteActivity(activity));
    }

    [MySqlFact]
    public void Students_CanOnlyReadCoursesTheyAreEnrolledIn()
    {
        _db.Execute("INSERT INTO Courses (CourseID, CourseCode, CourseTitle, Semester) VALUES (99, 'CS999', 'Private Course', 'Fall 2025');");

        CourseDetails? enrolled = Results.GetCourseDetails(studentId: 1, courseId: CS301);
        CourseDetails? notEnrolled = Results.GetCourseDetails(studentId: 1, courseId: 99);

        Assert.NotNull(enrolled);
        Assert.Equal("A", enrolled!.Course.FinalGrade);
        Assert.Equal(4, enrolled.Activities.Count);
        Assert.Equal(92.75m, enrolled.Activities.Sum(a => a.Contribution));
        Assert.Null(notEnrolled);
        Assert.Empty(new MySqlMarksRepository(Connections).GetActivityResultsForStudent(1, 99));
    }

    [MySqlFact]
    public void DatabaseErrors_ClassifyCommonSetupMistakes()
    {
        var wrongPassword = new MySqlConnectionStringBuilder(_db.ConnectionString) { Password = "definitely-wrong-" + Guid.NewGuid() };
        var unknownDatabase = new MySqlConnectionStringBuilder(_db.ConnectionString) { Database = "sgs_missing_" + Guid.NewGuid().ToString("N").Substring(0, 8) };
        var unreachable = new MySqlConnectionStringBuilder(_db.ConnectionString) { Server = "127.0.0.1", Port = 1, ConnectionTimeout = 2 };

        Assert.Equal(DatabaseErrorKind.AccessDenied, ClassifyFailure(wrongPassword.ConnectionString));
        Assert.Equal(DatabaseErrorKind.SchemaMissing, ClassifyFailure(unknownDatabase.ConnectionString));
        Assert.Equal(DatabaseErrorKind.ServerUnreachable, ClassifyFailure(unreachable.ConnectionString));
        Assert.Equal(DatabaseErrorKind.InvalidConfiguration, ClassifyFailure("Server=localhost;NotARealOption=1"));
    }

    private static DatabaseErrorKind ClassifyFailure(string connectionString)
    {
        try
        {
            MySqlConnectionFactory.TestConnection(connectionString);
        }
        catch (Exception ex)
        {
            return DatabaseErrors.Classify(ex);
        }

        throw new InvalidOperationException("Expected the connection attempt to fail.");
    }
}
