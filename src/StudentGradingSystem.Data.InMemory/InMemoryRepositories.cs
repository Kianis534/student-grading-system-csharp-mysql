using System;
using System.Collections.Generic;
using System.Linq;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Core.Grading;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Data.InMemory;

// In-memory counterparts of the MySQL repositories. Same interfaces, same ordering and the same
// rules (unique names, enrolment checks, cascading deletes), so services can't tell them apart.

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly InMemoryDataStore _store;

    public InMemoryUserRepository(InMemoryDataStore store) => _store = store ?? throw new ArgumentNullException(nameof(store));

    public UserAccount? FindByUsername(string username)
    {
        lock (_store.Gate)
        {
            InMemoryDataStore.UserRow? user = _store.Users.FirstOrDefault(
                u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            if (user is null)
            {
                return null;
            }

            string? studentName = user.StudentId is int id ? _store.Students.FirstOrDefault(s => s.Id == id)?.Name : null;
            return new UserAccount(user.Id, user.Username, user.PasswordHash, user.Role, user.StudentId, studentName);
        }
    }

    public void UpdatePasswordHash(int userId, string passwordHash)
    {
        lock (_store.Gate)
        {
            InMemoryDataStore.UserRow? user = _store.Users.FirstOrDefault(u => u.Id == userId);
            if (user is not null)
            {
                user.PasswordHash = passwordHash;
            }
        }
    }
}

public sealed class InMemoryCourseRepository : ICourseRepository
{
    private readonly InMemoryDataStore _store;

    public InMemoryCourseRepository(InMemoryDataStore store) => _store = store ?? throw new ArgumentNullException(nameof(store));

    public IReadOnlyList<CourseSummary> GetCourseSummaries()
    {
        lock (_store.Gate)
        {
            return _store.Courses
                .OrderBy(c => c.Code, StringComparer.Ordinal)
                .Select(Summarize)
                .ToList();
        }
    }

    public CourseSummary? GetCourseSummary(int courseId)
    {
        lock (_store.Gate)
        {
            InMemoryDataStore.CourseRow? course = _store.Courses.FirstOrDefault(c => c.Id == courseId);
            return course is null ? null : Summarize(course);
        }
    }

    public IReadOnlyList<StudentCourseResult> GetCoursesForStudent(int studentId)
    {
        lock (_store.Gate)
        {
            return _store.Enrollments
                .Where(e => e.StudentId == studentId)
                .Join(_store.Courses, e => e.CourseId, c => c.Id, (_, c) => c)
                .OrderBy(c => c.Code, StringComparer.Ordinal)
                .Select(c =>
                {
                    InMemoryDataStore.GradeRow? grade = _store.Grades.FirstOrDefault(g => g.CourseId == c.Id && g.StudentId == studentId);
                    return new StudentCourseResult(c.Id, c.Code, c.Title, c.Semester, grade?.Percentage, grade?.FinalGrade);
                })
                .ToList();
        }
    }

    public bool IsStudentEnrolled(int studentId, int courseId)
    {
        lock (_store.Gate)
        {
            return _store.Enrollments.Exists(e => e.StudentId == studentId && e.CourseId == courseId);
        }
    }

    private CourseSummary Summarize(InMemoryDataStore.CourseRow course)
    {
        List<InMemoryDataStore.ActivityRow> activities = _store.Activities.Where(a => a.CourseId == course.Id).ToList();
        return new CourseSummary(
            course.Id,
            course.Code,
            course.Title,
            course.Semester,
            course.Status,
            _store.Enrollments.Count(e => e.CourseId == course.Id),
            activities.Count,
            activities.Sum(a => a.Weightage));
    }
}

public sealed class InMemoryActivityRepository : IActivityRepository
{
    private readonly InMemoryDataStore _store;

    public InMemoryActivityRepository(InMemoryDataStore store) => _store = store ?? throw new ArgumentNullException(nameof(store));

    public IReadOnlyList<Activity> GetByCourse(int courseId)
    {
        lock (_store.Gate)
        {
            return _store.Activities
                .Where(a => a.CourseId == courseId)
                .OrderBy(a => a.Id)
                .Select(ToActivity)
                .ToList();
        }
    }

    public Activity Add(int courseId, string name, decimal totalMarks, decimal weightage)
    {
        int id = _store.AddActivity(courseId, name, totalMarks, weightage);
        return new Activity(id, courseId, name, totalMarks, weightage);
    }

    public bool Delete(int activityId)
    {
        lock (_store.Gate)
        {
            int removed = _store.Activities.RemoveAll(a => a.Id == activityId);
            _store.Marks.RemoveAll(m => m.ActivityId == activityId); // ON DELETE CASCADE
            return removed > 0;
        }
    }

    internal static Activity ToActivity(InMemoryDataStore.ActivityRow a) =>
        new Activity(a.Id, a.CourseId, a.Name, a.TotalMarks, a.Weightage);
}

public sealed class InMemoryMarksRepository : IMarksRepository
{
    private readonly InMemoryDataStore _store;

    public InMemoryMarksRepository(InMemoryDataStore store) => _store = store ?? throw new ArgumentNullException(nameof(store));

    public IReadOnlyList<StudentMarkEntry> GetMarksForActivity(int activityId)
    {
        lock (_store.Gate)
        {
            InMemoryDataStore.ActivityRow? activity = _store.Activities.FirstOrDefault(a => a.Id == activityId);
            if (activity is null)
            {
                return Array.Empty<StudentMarkEntry>();
            }

            return EnrolledStudents(activity.CourseId)
                .Select(s => new StudentMarkEntry(
                    s.Id,
                    s.Name,
                    _store.Marks.FirstOrDefault(m => m.ActivityId == activityId && m.StudentId == s.Id)?.ObtainedMarks))
                .ToList();
        }
    }

    public int SaveMarks(int activityId, IEnumerable<StudentMarkEntry> entries)
    {
        if (entries is null)
        {
            throw new ArgumentNullException(nameof(entries));
        }

        List<StudentMarkEntry> batch = entries.ToList();
        lock (_store.Gate)
        {
            int saved = 0;
            foreach (StudentMarkEntry entry in batch)
            {
                _store.Marks.RemoveAll(m => m.ActivityId == activityId && m.StudentId == entry.StudentId);
                if (entry.ObtainedMarks is decimal marks)
                {
                    _store.Marks.Add(new InMemoryDataStore.MarkRow(activityId, entry.StudentId, marks));
                    saved++;
                }
            }

            return saved;
        }
    }

    public IReadOnlyList<StudentScores> GetScoresForCourse(int courseId)
    {
        lock (_store.Gate)
        {
            List<InMemoryDataStore.ActivityRow> activities = _store.Activities
                .Where(a => a.CourseId == courseId)
                .OrderBy(a => a.Id)
                .ToList();

            return EnrolledStudents(courseId)
                .Select(s => new StudentScores(
                    s.Id,
                    s.Name,
                    activities
                        .Select(a => new ActivityScore(
                            a.TotalMarks,
                            a.Weightage,
                            _store.Marks.FirstOrDefault(m => m.ActivityId == a.Id && m.StudentId == s.Id)?.ObtainedMarks))
                        .ToList()))
                .ToList();
        }
    }

    public IReadOnlyList<ActivityResult> GetActivityResultsForStudent(int studentId, int courseId)
    {
        lock (_store.Gate)
        {
            if (!_store.Enrollments.Exists(e => e.CourseId == courseId && e.StudentId == studentId))
            {
                return Array.Empty<ActivityResult>();
            }

            return _store.Activities
                .Where(a => a.CourseId == courseId)
                .OrderBy(a => a.Id)
                .Select(a => new ActivityResult(
                    a.Name,
                    a.TotalMarks,
                    a.Weightage,
                    _store.Marks.FirstOrDefault(m => m.ActivityId == a.Id && m.StudentId == studentId)?.ObtainedMarks))
                .ToList();
        }
    }

    private IEnumerable<InMemoryDataStore.StudentRow> EnrolledStudents(int courseId) =>
        _store.Enrollments
            .Where(e => e.CourseId == courseId)
            .Join(_store.Students, e => e.StudentId, s => s.Id, (_, s) => s)
            .OrderBy(s => s.Id);
}

public sealed class InMemoryGradeRepository : IGradeRepository
{
    private readonly InMemoryDataStore _store;

    public InMemoryGradeRepository(InMemoryDataStore store) => _store = store ?? throw new ArgumentNullException(nameof(store));

    public IReadOnlyList<StudentGrade> GetGradesForCourse(int courseId)
    {
        lock (_store.Gate)
        {
            return _store.Grades
                .Where(g => g.CourseId == courseId)
                .OrderBy(g => g.StudentId)
                .Select(ToGrade)
                .ToList();
        }
    }

    public void ReplaceGradesForCourse(int courseId, IEnumerable<StudentGrade> grades)
    {
        if (grades is null)
        {
            throw new ArgumentNullException(nameof(grades));
        }

        List<StudentGrade> batch = grades.ToList();
        DateTime now = DateTime.Now;
        lock (_store.Gate)
        {
            _store.Grades.RemoveAll(g => g.CourseId == courseId);
            foreach (StudentGrade grade in batch)
            {
                _store.Grades.Add(new InMemoryDataStore.GradeRow(courseId, grade.StudentId, grade.Percentage, grade.FinalGrade, now));
            }
        }
    }

    public StudentGrade? GetGrade(int studentId, int courseId)
    {
        lock (_store.Gate)
        {
            InMemoryDataStore.GradeRow? row = _store.Grades.FirstOrDefault(g => g.CourseId == courseId && g.StudentId == studentId);
            return row is null ? null : ToGrade(row);
        }
    }

    private StudentGrade ToGrade(InMemoryDataStore.GradeRow g) => new StudentGrade(
        g.StudentId,
        _store.Students.FirstOrDefault(s => s.Id == g.StudentId)?.Name ?? string.Empty,
        g.Percentage,
        g.FinalGrade,
        g.CalculatedAt);
}
