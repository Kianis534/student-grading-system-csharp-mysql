using StudentGradingSystem.Data.MySql;
using StudentGradingSystem.Core.Services;

namespace StudentGradingSystem.Infrastructure;

/// <summary>
/// Composition root: wires repositories and services together for one connection string.
/// Forms receive this object instead of creating database objects themselves.
/// </summary>
internal sealed class AppServices
{
    public AppServices(string connectionString)
    {
        var connections = new MySqlConnectionFactory(connectionString);

        var users = new MySqlUserRepository(connections);
        var courses = new MySqlCourseRepository(connections);
        var activities = new MySqlActivityRepository(connections);
        var marks = new MySqlMarksRepository(connections);
        var grades = new MySqlGradeRepository(connections);

        Auth = new AuthService(users);
        Courses = new CourseService(courses, activities, marks, grades);
        Grading = new GradingService(activities, marks, grades);
        Results = new ResultsService(courses, marks);
    }

    public AuthService Auth { get; }

    public CourseService Courses { get; }

    public GradingService Grading { get; }

    public ResultsService Results { get; }
}
