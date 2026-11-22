using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Services;
using StudentGradingSystem.Data.InMemory;

namespace StudentGradingSystem.Web.Services;

/// <summary>
/// One visitor's demo: a private in-memory copy of the sample database plus the same
/// Core services the desktop app uses. Nothing leaves the browser; reloading starts over.
/// </summary>
public sealed class DemoSession
{
    public DemoSession() => Reset();

    public event Action? Changed;

    public AuthService Auth { get; private set; } = null!;

    public CourseService Courses { get; private set; } = null!;

    public GradingService Grading { get; private set; } = null!;

    public ResultsService Results { get; private set; } = null!;

    public AuthenticatedUser? User { get; private set; }

    public bool IsSignedIn => User is not null;

    /// <summary>Throws away every change and reloads the sample data.</summary>
    public void Reset()
    {
        InMemoryDataStore store = DemoData.Create();
        var courses = new InMemoryCourseRepository(store);
        var activities = new InMemoryActivityRepository(store);
        var marks = new InMemoryMarksRepository(store);
        var grades = new InMemoryGradeRepository(store);

        Auth = new AuthService(new InMemoryUserRepository(store), DemoData.PasswordIterations);
        Courses = new CourseService(courses, activities, marks, grades);
        Grading = new GradingService(activities, marks, grades);
        Results = new ResultsService(courses, marks);
        User = null;
        Changed?.Invoke();
    }

    public async Task<AuthenticatedUser?> SignInAsync(string username, string password)
    {
        // Give the UI a chance to show "Signing in..." before PBKDF2 runs on this (single) thread.
        await Task.Delay(30);
        User = Auth.SignIn(username, password);
        Changed?.Invoke();
        return User;
    }

    public void SignOut()
    {
        User = null;
        Changed?.Invoke();
    }

    /// <summary>Relative URL of the signed-in user's home screen.</summary>
    public string HomeUrl => User?.Role switch
    {
        UserRole.Instructor => "instructor",
        UserRole.Student => "student",
        _ => string.Empty,
    };
}
