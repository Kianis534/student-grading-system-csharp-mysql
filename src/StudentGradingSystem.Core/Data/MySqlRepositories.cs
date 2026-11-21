using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using StudentGradingSystem.Core.Grading;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Core.Data;

// Every query below is parameterized; user input is never concatenated into SQL.

public sealed class MySqlUserRepository : IUserRepository
{
    private readonly IConnectionFactory _connections;

    public MySqlUserRepository(IConnectionFactory connections) =>
        _connections = connections ?? throw new ArgumentNullException(nameof(connections));

    public UserAccount? FindByUsername(string username)
    {
        const string sql = @"
            SELECT u.UserID, u.Username, u.PasswordHash, u.Role, u.StudentID, s.StudentName
            FROM Users u
            LEFT JOIN Students s ON s.StudentID = u.StudentID
            WHERE u.Username = @username
            LIMIT 1;";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(sql).With("@username", username);
        using MySqlDataReader reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        string roleText = reader.ReadString("Role");
        if (!Enum.TryParse(roleText, ignoreCase: true, out UserRole role))
        {
            throw new InvalidOperationException($"User '{username}' has an unknown role '{roleText}'.");
        }

        return new UserAccount(
            reader.ReadInt("UserID"),
            reader.ReadString("Username"),
            reader.ReadString("PasswordHash"),
            role,
            reader.ReadNullableInt("StudentID"),
            reader.ReadNullableString("StudentName"));
    }

    public void UpdatePasswordHash(int userId, string passwordHash)
    {
        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection
            .CreateCommand("UPDATE Users SET PasswordHash = @hash WHERE UserID = @userId;")
            .With("@hash", passwordHash)
            .With("@userId", userId);
        command.ExecuteNonQuery();
    }
}

public sealed class MySqlCourseRepository : ICourseRepository
{
    private const string SummarySelect = @"
        SELECT c.CourseID, c.CourseCode, c.CourseTitle, c.Semester, c.Status,
               (SELECT COUNT(*) FROM CourseEnrollment ce WHERE ce.CourseID = c.CourseID)         AS EnrolledStudents,
               (SELECT COUNT(*) FROM Activities a WHERE a.CourseID = c.CourseID)                 AS ActivityCount,
               (SELECT COALESCE(SUM(a.Weightage), 0) FROM Activities a WHERE a.CourseID = c.CourseID) AS AllocatedWeightage
        FROM Courses c";

    private readonly IConnectionFactory _connections;

    public MySqlCourseRepository(IConnectionFactory connections) =>
        _connections = connections ?? throw new ArgumentNullException(nameof(connections));

    public IReadOnlyList<CourseSummary> GetCourseSummaries()
    {
        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(SummarySelect + " ORDER BY c.CourseCode;");
        using MySqlDataReader reader = command.ExecuteReader();

        var courses = new List<CourseSummary>();
        while (reader.Read())
        {
            courses.Add(ReadSummary(reader));
        }

        return courses;
    }

    public CourseSummary? GetCourseSummary(int courseId)
    {
        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection
            .CreateCommand(SummarySelect + " WHERE c.CourseID = @courseId;")
            .With("@courseId", courseId);
        using MySqlDataReader reader = command.ExecuteReader();

        return reader.Read() ? ReadSummary(reader) : null;
    }

    public IReadOnlyList<StudentCourseResult> GetCoursesForStudent(int studentId)
    {
        const string sql = @"
            SELECT c.CourseID, c.CourseCode, c.CourseTitle, c.Semester, g.Percentage, g.FinalGrade
            FROM CourseEnrollment ce
            JOIN Courses c     ON c.CourseID = ce.CourseID
            LEFT JOIN Grades g ON g.CourseID = ce.CourseID AND g.StudentID = ce.StudentID
            WHERE ce.StudentID = @studentId
            ORDER BY c.CourseCode;";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(sql).With("@studentId", studentId);
        using MySqlDataReader reader = command.ExecuteReader();

        var courses = new List<StudentCourseResult>();
        while (reader.Read())
        {
            courses.Add(new StudentCourseResult(
                reader.ReadInt("CourseID"),
                reader.ReadString("CourseCode"),
                reader.ReadString("CourseTitle"),
                reader.ReadString("Semester"),
                reader.ReadNullableDecimal("Percentage"),
                reader.ReadNullableString("FinalGrade")));
        }

        return courses;
    }

    public bool IsStudentEnrolled(int studentId, int courseId)
    {
        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection
            .CreateCommand("SELECT COUNT(*) FROM CourseEnrollment WHERE StudentID = @studentId AND CourseID = @courseId;")
            .With("@studentId", studentId)
            .With("@courseId", courseId);
        return Convert.ToInt64(command.ExecuteScalar()) > 0;
    }

    private static CourseSummary ReadSummary(MySqlDataReader reader) => new CourseSummary(
        reader.ReadInt("CourseID"),
        reader.ReadString("CourseCode"),
        reader.ReadString("CourseTitle"),
        reader.ReadString("Semester"),
        reader.ReadString("Status"),
        reader.ReadInt("EnrolledStudents"),
        reader.ReadInt("ActivityCount"),
        reader.ReadDecimal("AllocatedWeightage"));
}

public sealed class MySqlActivityRepository : IActivityRepository
{
    private readonly IConnectionFactory _connections;

    public MySqlActivityRepository(IConnectionFactory connections) =>
        _connections = connections ?? throw new ArgumentNullException(nameof(connections));

    public IReadOnlyList<Activity> GetByCourse(int courseId)
    {
        const string sql = @"
            SELECT ActivityID, CourseID, ActivityType, TotalMarks, Weightage
            FROM Activities
            WHERE CourseID = @courseId
            ORDER BY ActivityID;";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(sql).With("@courseId", courseId);
        using MySqlDataReader reader = command.ExecuteReader();

        var activities = new List<Activity>();
        while (reader.Read())
        {
            activities.Add(new Activity(
                reader.ReadInt("ActivityID"),
                reader.ReadInt("CourseID"),
                reader.ReadString("ActivityType"),
                reader.ReadDecimal("TotalMarks"),
                reader.ReadDecimal("Weightage")));
        }

        return activities;
    }

    public Activity Add(int courseId, string name, decimal totalMarks, decimal weightage)
    {
        const string sql = @"
            INSERT INTO Activities (CourseID, ActivityType, TotalMarks, Weightage)
            VALUES (@courseId, @name, @totalMarks, @weightage);";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(sql)
            .With("@courseId", courseId)
            .With("@name", name)
            .With("@totalMarks", totalMarks)
            .With("@weightage", weightage);
        command.ExecuteNonQuery();

        return new Activity(checked((int)command.LastInsertedId), courseId, name, totalMarks, weightage);
    }

    public bool Delete(int activityId)
    {
        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection
            .CreateCommand("DELETE FROM Activities WHERE ActivityID = @activityId;")
            .With("@activityId", activityId);
        return command.ExecuteNonQuery() > 0;
    }
}

public sealed class MySqlMarksRepository : IMarksRepository
{
    private readonly IConnectionFactory _connections;

    public MySqlMarksRepository(IConnectionFactory connections) =>
        _connections = connections ?? throw new ArgumentNullException(nameof(connections));

    public IReadOnlyList<StudentMarkEntry> GetMarksForActivity(int activityId)
    {
        const string sql = @"
            SELECT s.StudentID, s.StudentName, sm.ObtainedMarks
            FROM Activities a
            JOIN CourseEnrollment ce ON ce.CourseID = a.CourseID
            JOIN Students s          ON s.StudentID = ce.StudentID
            LEFT JOIN StudentMarks sm ON sm.ActivityID = a.ActivityID AND sm.StudentID = s.StudentID
            WHERE a.ActivityID = @activityId
            ORDER BY s.StudentID;";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(sql).With("@activityId", activityId);
        using MySqlDataReader reader = command.ExecuteReader();

        var entries = new List<StudentMarkEntry>();
        while (reader.Read())
        {
            entries.Add(new StudentMarkEntry(
                reader.ReadInt("StudentID"),
                reader.ReadString("StudentName"),
                reader.ReadNullableDecimal("ObtainedMarks")));
        }

        return entries;
    }

    public int SaveMarks(int activityId, IEnumerable<StudentMarkEntry> entries)
    {
        if (entries is null)
        {
            throw new ArgumentNullException(nameof(entries));
        }

        const string upsertSql = @"
            INSERT INTO StudentMarks (ActivityID, StudentID, ObtainedMarks)
            VALUES (@activityId, @studentId, @marks)
            ON DUPLICATE KEY UPDATE ObtainedMarks = @marks;";

        const string clearSql = "DELETE FROM StudentMarks WHERE ActivityID = @activityId AND StudentID = @studentId;";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlTransaction transaction = connection.BeginTransaction();

        using MySqlCommand upsert = connection.CreateCommand(upsertSql, transaction);
        MySqlParameter upsertActivity = upsert.Parameters.Add("@activityId", MySqlDbType.Int32);
        MySqlParameter upsertStudent = upsert.Parameters.Add("@studentId", MySqlDbType.Int32);
        MySqlParameter upsertMarks = upsert.Parameters.Add("@marks", MySqlDbType.Decimal);

        using MySqlCommand clear = connection.CreateCommand(clearSql, transaction);
        MySqlParameter clearActivity = clear.Parameters.Add("@activityId", MySqlDbType.Int32);
        MySqlParameter clearStudent = clear.Parameters.Add("@studentId", MySqlDbType.Int32);

        int saved = 0;
        foreach (StudentMarkEntry entry in entries)
        {
            if (entry.ObtainedMarks is decimal marks)
            {
                upsertActivity.Value = activityId;
                upsertStudent.Value = entry.StudentId;
                upsertMarks.Value = marks;
                upsert.ExecuteNonQuery();
                saved++;
            }
            else
            {
                clearActivity.Value = activityId;
                clearStudent.Value = entry.StudentId;
                clear.ExecuteNonQuery();
            }
        }

        transaction.Commit();
        return saved;
    }

    public IReadOnlyList<StudentScores> GetScoresForCourse(int courseId)
    {
        const string sql = @"
            SELECT s.StudentID, s.StudentName, a.ActivityID, a.TotalMarks, a.Weightage, sm.ObtainedMarks
            FROM CourseEnrollment ce
            JOIN Students s           ON s.StudentID = ce.StudentID
            LEFT JOIN Activities a    ON a.CourseID = ce.CourseID
            LEFT JOIN StudentMarks sm ON sm.ActivityID = a.ActivityID AND sm.StudentID = s.StudentID
            WHERE ce.CourseID = @courseId
            ORDER BY s.StudentID, a.ActivityID;";

        var students = new List<(int Id, string Name, List<ActivityScore> Scores)>();

        using (MySqlConnection connection = _connections.OpenConnection())
        using (MySqlCommand command = connection.CreateCommand(sql).With("@courseId", courseId))
        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int studentId = reader.ReadInt("StudentID");
                if (students.Count == 0 || students[students.Count - 1].Id != studentId)
                {
                    students.Add((studentId, reader.ReadString("StudentName"), new List<ActivityScore>()));
                }

                if (reader.ReadNullableInt("ActivityID") is null)
                {
                    continue; // course has no assessments yet
                }

                students[students.Count - 1].Scores.Add(new ActivityScore(
                    reader.ReadDecimal("TotalMarks"),
                    reader.ReadDecimal("Weightage"),
                    reader.ReadNullableDecimal("ObtainedMarks")));
            }
        }

        return students
            .Select(s => new StudentScores(s.Id, s.Name, s.Scores))
            .ToList();
    }

    public IReadOnlyList<ActivityResult> GetActivityResultsForStudent(int studentId, int courseId)
    {
        // The EXISTS guard means a student can only ever read marks for courses they are enrolled in.
        const string sql = @"
            SELECT a.ActivityType, a.TotalMarks, a.Weightage, sm.ObtainedMarks
            FROM Activities a
            LEFT JOIN StudentMarks sm ON sm.ActivityID = a.ActivityID AND sm.StudentID = @studentId
            WHERE a.CourseID = @courseId
              AND EXISTS (SELECT 1 FROM CourseEnrollment ce
                          WHERE ce.CourseID = @courseId AND ce.StudentID = @studentId)
            ORDER BY a.ActivityID;";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(sql)
            .With("@studentId", studentId)
            .With("@courseId", courseId);
        using MySqlDataReader reader = command.ExecuteReader();

        var results = new List<ActivityResult>();
        while (reader.Read())
        {
            results.Add(new ActivityResult(
                reader.ReadString("ActivityType"),
                reader.ReadDecimal("TotalMarks"),
                reader.ReadDecimal("Weightage"),
                reader.ReadNullableDecimal("ObtainedMarks")));
        }

        return results;
    }
}

public sealed class MySqlGradeRepository : IGradeRepository
{
    private readonly IConnectionFactory _connections;

    public MySqlGradeRepository(IConnectionFactory connections) =>
        _connections = connections ?? throw new ArgumentNullException(nameof(connections));

    public IReadOnlyList<StudentGrade> GetGradesForCourse(int courseId)
    {
        const string sql = @"
            SELECT g.StudentID, s.StudentName, g.Percentage, g.FinalGrade, g.CalculatedAt
            FROM Grades g
            JOIN Students s ON s.StudentID = g.StudentID
            WHERE g.CourseID = @courseId
            ORDER BY s.StudentID;";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(sql).With("@courseId", courseId);
        using MySqlDataReader reader = command.ExecuteReader();

        var grades = new List<StudentGrade>();
        while (reader.Read())
        {
            grades.Add(ReadGrade(reader));
        }

        return grades;
    }

    public void ReplaceGradesForCourse(int courseId, IEnumerable<StudentGrade> grades)
    {
        if (grades is null)
        {
            throw new ArgumentNullException(nameof(grades));
        }

        const string insertSql = @"
            INSERT INTO Grades (CourseID, StudentID, Percentage, FinalGrade)
            VALUES (@courseId, @studentId, @percentage, @grade);";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlTransaction transaction = connection.BeginTransaction();

        using (MySqlCommand delete = connection.CreateCommand("DELETE FROM Grades WHERE CourseID = @courseId;", transaction))
        {
            delete.With("@courseId", courseId).ExecuteNonQuery();
        }

        using (MySqlCommand insert = connection.CreateCommand(insertSql, transaction))
        {
            insert.Parameters.AddWithValue("@courseId", courseId);
            MySqlParameter student = insert.Parameters.Add("@studentId", MySqlDbType.Int32);
            MySqlParameter percentage = insert.Parameters.Add("@percentage", MySqlDbType.Decimal);
            MySqlParameter grade = insert.Parameters.Add("@grade", MySqlDbType.VarChar);

            foreach (StudentGrade g in grades)
            {
                student.Value = g.StudentId;
                percentage.Value = g.Percentage;
                grade.Value = g.FinalGrade;
                insert.ExecuteNonQuery();
            }
        }

        transaction.Commit();
    }

    public StudentGrade? GetGrade(int studentId, int courseId)
    {
        const string sql = @"
            SELECT g.StudentID, s.StudentName, g.Percentage, g.FinalGrade, g.CalculatedAt
            FROM Grades g
            JOIN Students s ON s.StudentID = g.StudentID
            WHERE g.StudentID = @studentId AND g.CourseID = @courseId;";

        using MySqlConnection connection = _connections.OpenConnection();
        using MySqlCommand command = connection.CreateCommand(sql)
            .With("@studentId", studentId)
            .With("@courseId", courseId);
        using MySqlDataReader reader = command.ExecuteReader();

        return reader.Read() ? ReadGrade(reader) : null;
    }

    private static StudentGrade ReadGrade(MySqlDataReader reader) => new StudentGrade(
        reader.ReadInt("StudentID"),
        reader.ReadString("StudentName"),
        reader.ReadDecimal("Percentage"),
        reader.ReadString("FinalGrade"),
        reader.ReadNullableDateTime("CalculatedAt"));
}
