using System;
using System.Collections.Generic;
using System.Linq;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Security;
using StudentGradingSystem.Core.Services;
using StudentGradingSystem.Core.Validation;
using StudentGradingSystem.Data.InMemory;
using Xunit;

namespace StudentGradingSystem.Core.Tests.InMemory;

/// <summary>
/// The browser demo runs on the in-memory repositories, so they must behave like the MySQL ones.
/// These tests mirror <c>MySqlIntegrationTests</c> scenario for scenario.
/// </summary>
public class InMemoryRepositoryTests
{
    private const int CS101 = 1;
    private const int CS102 = 2;
    private const int CS202 = 4;
    private const int CS301 = 5;

    private readonly InMemoryDataStore _store = DemoData.Create();

    private AuthService Auth => new AuthService(new InMemoryUserRepository(_store), DemoData.PasswordIterations);

    private CourseService Courses => new CourseService(
        new InMemoryCourseRepository(_store),
        new InMemoryActivityRepository(_store),
        new InMemoryMarksRepository(_store),
        new InMemoryGradeRepository(_store));

    private GradingService Grading => new GradingService(
        new InMemoryActivityRepository(_store),
        new InMemoryMarksRepository(_store),
        new InMemoryGradeRepository(_store));

    private ResultsService Results => new ResultsService(
        new InMemoryCourseRepository(_store),
        new InMemoryMarksRepository(_store));

    [Fact]
    public void DemoAccounts_CanSignIn_AndWrongPasswordsCannot()
    {
        Assert.Equal(UserRole.Instructor, Auth.SignIn(DemoData.InstructorUsername, DemoData.InstructorPassword)?.Role);
        Assert.Equal("John Smith", Auth.SignIn("JSMITH", DemoData.StudentPassword)?.DisplayName);
        Assert.Null(Auth.SignIn(DemoData.InstructorUsername, DemoData.StudentPassword));
        Assert.Null(Auth.SignIn("ghost", DemoData.StudentPassword));
    }

    [Fact]
    public void DemoSignIn_DoesNotRehashWithTheDesktopWorkFactor()
    {
        Auth.SignIn(DemoData.StudentUsername, DemoData.StudentPassword);

        string hash = new InMemoryUserRepository(_store).FindByUsername(DemoData.StudentUsername)!.PasswordHash;
        Assert.StartsWith($"{PasswordHasher.Scheme}${DemoData.PasswordIterations}$", hash);
    }

    [Fact]
    public void CourseSummaries_MatchTheSqlSeed()
    {
        IReadOnlyList<CourseSummary> courses = Courses.GetCourses();
        CourseSummary cs301 = courses.Single(c => c.CourseId == CS301);
        CourseSummary cs201 = courses.Single(c => c.Code == "CS201");

        Assert.Equal(10, courses.Count);
        Assert.Equal(new[] { "CS101", "CS102", "CS201" }, courses.Take(3).Select(c => c.Code));
        Assert.Equal(10, cs301.EnrolledStudents);
        Assert.Equal(4, cs301.ActivityCount);
        Assert.Equal(100m, cs301.AllocatedWeightage);
        Assert.Equal(60m, cs201.AllocatedWeightage);
    }

    [Fact]
    public void SeededGrades_MatchTheSqlSeedResults()
    {
        var expected = new Dictionary<int, (decimal, string)>
        {
            [1] = (92.75m, "A"), [2] = (87.00m, "A-"), [3] = (80.75m, "B+"), [4] = (77.50m, "B"), [5] = (72.25m, "B-"),
            [6] = (67.50m, "C+"), [7] = (62.25m, "C"), [8] = (57.50m, "C-"), [9] = (51.75m, "D"), [10] = (40.00m, "F"),
        };

        IReadOnlyList<StudentGrade> grades = Grading.GetGrades(CS301);

        Assert.Equal(10, grades.Count);
        foreach (StudentGrade grade in grades)
        {
            Assert.Equal(expected[grade.StudentId], (grade.Percentage, grade.FinalGrade));
        }
    }

    [Fact]
    public void AddActivity_EnforcesWeightageAndUniqueNames()
    {
        Assert.True(Courses.AddActivity(CS101, "Quiz 1", 10, 60, out Activity? quiz).IsValid);
        Assert.True(quiz!.ActivityId > 7);

        Assert.False(Courses.AddActivity(CS101, "Final Exam", 100, 41, out _).IsValid);
        Assert.False(Courses.AddActivity(CS101, "quiz 1", 10, 5, out _).IsValid);
        Assert.Single(Courses.GetActivities(CS101));

        var repository = new InMemoryActivityRepository(_store);
        Assert.Throws<InvalidOperationException>(() => repository.Add(CS101, "QUIZ 1", 10, 5));
    }

    [Fact]
    public void SaveMarks_UpsertsAndClears()
    {
        Assert.True(Courses.AddActivity(CS102, "Assignment 1", 20, 20, out Activity? activity).IsValid);

        List<StudentMarkEntry> entries = Courses.GetMarks(activity!.ActivityId).ToList();
        Assert.Equal(10, entries.Count);
        Assert.All(entries, e => Assert.Null(e.ObtainedMarks));

        entries[0].ObtainedMarks = 18;
        entries[1].ObtainedMarks = 12.5m;
        Assert.True(Courses.SaveMarks(activity, entries).IsValid);

        entries = Courses.GetMarks(activity.ActivityId).ToList();
        entries[0].ObtainedMarks = 19;
        entries[1].ObtainedMarks = null;
        Assert.True(Courses.SaveMarks(activity, entries).IsValid);

        entries = Courses.GetMarks(activity.ActivityId).ToList();
        Assert.Equal(19m, entries[0].ObtainedMarks);
        Assert.Null(entries[1].ObtainedMarks);

        entries[2].ObtainedMarks = 25;
        Assert.False(Courses.SaveMarks(activity, entries).IsValid);
        Assert.Null(Courses.GetMarks(activity.ActivityId)[2].ObtainedMarks);
    }

    [Fact]
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

    [Fact]
    public void ChangingMarks_UpdatesReleasedGradesStudentsSee()
    {
        Activity finalExam = Courses.GetActivities(CS301).Single(a => a.Name == "Final Exam");
        List<StudentMarkEntry> entries = Courses.GetMarks(finalExam.ActivityId).ToList();
        entries.Single(e => e.StudentId == 10).ObtainedMarks = 60; // Jessica: 40 -> 60 on the final

        Assert.True(Courses.SaveMarks(finalExam, entries).IsValid);

        StudentCourseResult jessica = Results.GetCourses(10).Single(c => c.CourseId == CS301);
        Assert.Equal(50.00m, jessica.Percentage);
        Assert.Equal("D", jessica.FinalGrade);
    }

    [Fact]
    public void ChangingMarks_DoesNotGradeACourseBeforeGradesAreReleased()
    {
        Assert.True(Courses.AddActivity(CS202, "Lab Work", 10, 10, out Activity? activity).IsValid);
        List<StudentMarkEntry> entries = Courses.GetMarks(activity!.ActivityId).ToList();
        entries.ForEach(e => e.ObtainedMarks = 7);

        Assert.True(Courses.SaveMarks(activity, entries).IsValid);

        Assert.Empty(Grading.GetGrades(CS202));
    }

    [Fact]
    public void Recalculating_PicksUpChangedMarks()
    {
        Activity finalExam = Courses.GetActivities(CS301).Single(a => a.Name == "Final Exam");
        List<StudentMarkEntry> entries = Courses.GetMarks(finalExam.ActivityId).ToList();
        entries.Single(e => e.StudentId == 10).ObtainedMarks = 60; // Jessica: 40 -> 60 on the final

        Courses.SaveMarks(finalExam, entries);
        StudentGrade jessica = Grading.CalculateAndSaveGrades(CS301).Single(g => g.StudentId == 10);

        Assert.Equal(50.00m, jessica.Percentage);
        Assert.Equal("D", jessica.FinalGrade);
        Assert.Equal("D", Results.GetCourses(10).Single(c => c.CourseId == CS301).FinalGrade);
    }

    [Fact]
    public void Students_CanOnlyReadCoursesTheyAreEnrolledIn()
    {
        _store.AddCourse(99, "CS999", "Private Course", "Fall 2025");

        CourseDetails? enrolled = Results.GetCourseDetails(studentId: 1, courseId: CS301);

        Assert.NotNull(enrolled);
        Assert.Equal(92.75m, enrolled!.Activities.Sum(a => a.Contribution));
        Assert.Null(Results.GetCourseDetails(studentId: 1, courseId: 99));
        Assert.Empty(new InMemoryMarksRepository(_store).GetActivityResultsForStudent(1, 99));
    }

    [Fact]
    public void Store_EnforcesAccountRules()
    {
        Assert.Throws<InvalidOperationException>(() => _store.AddUser(50, "JSmith", "x", UserRole.Student, 2));
        Assert.Throws<InvalidOperationException>(() => _store.AddUser(51, "newteacher", "x", UserRole.Instructor, 3));
        Assert.Throws<InvalidOperationException>(() => _store.AddUser(52, "newstudent", "x", UserRole.Student, null));
    }

    [Fact]
    public void EachDemoStoreIsIndependent()
    {
        InMemoryDataStore other = DemoData.Create();
        new InMemoryActivityRepository(other).Delete(1);

        Assert.Equal(4, Courses.GetActivities(CS301).Count);
        Assert.Equal(3, new InMemoryActivityRepository(other).GetByCourse(CS301).Count);
    }
}
