using System.Linq;
using StudentGradingSystem.Core.Grading;
using StudentGradingSystem.Core.Models;

namespace StudentGradingSystem.Data.InMemory;

/// <summary>
/// The same fictional data as <c>database/02-seed.sql</c>, ready to use without a database.
/// </summary>
public static class DemoData
{
    public const string InstructorUsername = "instructor";
    public const string InstructorPassword = "Instructor@123";
    public const string StudentUsername = "jsmith";
    public const string StudentPassword = "Student@123";

    /// <summary>
    /// Work factor of the demo password hashes. Lower than the desktop default because the browser
    /// demo runs PBKDF2 in WebAssembly without hardware acceleration.
    /// </summary>
    public const int PasswordIterations = 10_000;

    /// <summary>Creates a fresh store with courses, students, accounts, marks and released grades.</summary>
    public static InMemoryDataStore Create()
    {
        var store = new InMemoryDataStore();

        string[][] courses =
        {
            new[] { "CS101", "Programming Fundamentals", "Fall 2025" },
            new[] { "CS102", "Object Oriented Programming", "Spring 2025" },
            new[] { "CS201", "Data Structures", "Fall 2025" },
            new[] { "CS202", "Database Systems", "Spring 2025" },
            new[] { "CS301", "Software Construction", "Fall 2025" },
            new[] { "CS302", "Design and Analysis of Algorithms", "Spring 2025" },
            new[] { "CS401", "Operating Systems", "Fall 2025" },
            new[] { "CS402", "Computer Networks", "Spring 2025" },
            new[] { "CS501", "Artificial Intelligence", "Fall 2025" },
            new[] { "CS502", "Machine Learning", "Spring 2025" },
        };
        for (int i = 0; i < courses.Length; i++)
        {
            store.AddCourse(i + 1, courses[i][0], courses[i][1], courses[i][2]);
        }

        string[] students =
        {
            "John Smith", "Emily Johnson", "Michael Brown", "Sarah Davis", "David Wilson",
            "Jennifer Lee", "Robert Taylor", "Lisa Anderson", "Thomas Martinez", "Jessica Garcia",
        };
        for (int i = 0; i < students.Length; i++)
        {
            store.AddStudent(i + 1, students[i]);
        }

        for (int course = 1; course <= courses.Length; course++)
        {
            for (int student = 1; student <= students.Length; student++)
            {
                store.Enroll(course, student);
            }
        }

        // PBKDF2-SHA256 hashes (10,000 iterations) of Instructor@123 and Student@123.
        store.AddUser(1, "instructor", "pbkdf2-sha256$10000$grQwhzWCbantigJpTOL+YA==$0/QuXxFXrkTM1ef8xbJHu8YNvuDkbOoJR7JbivnPKng=", UserRole.Instructor, null);
        store.AddUser(2, "jsmith", "pbkdf2-sha256$10000$lur3DSMO6rDrdMbDlIGjrg==$kp3HSrJlc3VMkqq0OZpDnZoZF3LaMoUI52mPPuYCfos=", UserRole.Student, 1);
        store.AddUser(3, "ejohnson", "pbkdf2-sha256$10000$nI06FWniln700SHG4F2hrQ==$ZSeaqrqOmZw5wHNxU8uIhYctAefsI8MDQC5uXrAhruA=", UserRole.Student, 2);
        store.AddUser(4, "mbrown", "pbkdf2-sha256$10000$i5p7pgetHMrNLKUQ3f4aZQ==$E3kOmjc4mrMF2xvzZEszev7ElpCE//PN6gSWOcgBAPs=", UserRole.Student, 3);
        store.AddUser(5, "sdavis", "pbkdf2-sha256$10000$X3oR45XnUy5/HL8Ai0FNzw==$ffhm1pFw0Wiiewi/omf2Z2PHROaKrtaUpBEyi+tFFeY=", UserRole.Student, 4);
        store.AddUser(6, "dwilson", "pbkdf2-sha256$10000$UXO2i+iuJNUpFBNfB14UpQ==$T1EV6zkvq0qp4PBNEbZ00CTLL5qDsdy5meNxk27BY/U=", UserRole.Student, 5);
        store.AddUser(7, "jlee", "pbkdf2-sha256$10000$drhOeMz8/NN+OqmuzJpKkA==$1HPSP8AKWURNbOmA/j3IYcHt9mKtj5SbSU2PnP42LSg=", UserRole.Student, 6);
        store.AddUser(8, "rtaylor", "pbkdf2-sha256$10000$TB5mJpVenAUX3mwhArQjyA==$fVaKh5wHzxs+xqbJHd5Dvt2NT75q063dhDRKJfCcLI0=", UserRole.Student, 7);
        store.AddUser(9, "landerson", "pbkdf2-sha256$10000$G/anJ7i+A9kTxcWO6oZFtw==$hCOwXVQMAJaivJsM/SaqLAL1Ajo/WpF5gVzYINeEhwQ=", UserRole.Student, 8);
        store.AddUser(10, "tmartinez", "pbkdf2-sha256$10000$ZuyPgEWuCHZNg/tJ3ZSwpQ==$8waEr7hD5TgqSMA1dVbKx/7mWVEN0iBszMH51l0Ibz4=", UserRole.Student, 9);
        store.AddUser(11, "jgarcia", "pbkdf2-sha256$10000$1zwilBqqmgm2A6yVRw44wA==$UcQzMMlYWy6h94hQPBgfmmrzzQ+JacSeCCqji98ba2s=", UserRole.Student, 10);

        // CS301 Software Construction: fully assessed (weightages total 100%).
        int quiz = store.AddActivity(5, "Quiz 1", 10, 10);
        int assignment = store.AddActivity(5, "Assignment 1", 20, 15);
        int mid = store.AddActivity(5, "Mid Term", 50, 25);
        int final = store.AddActivity(5, "Final Exam", 100, 50);
        AddMarks(store, quiz, 9.5m, 9, 8, 8.5m, 7, 7.5m, 6, 6.5m, 5, 4);
        AddMarks(store, assignment, 19, 18, 17, 16, 15, 14, 13, 12, 11, 8);
        AddMarks(store, mid, 46, 44, 40, 38, 36, 33, 31, 28, 26, 20);
        AddMarks(store, final, 92, 85, 80, 76, 72, 66, 62, 56, 51, 40);

        // CS201 Data Structures: in progress (60% assessed, no final exam yet).
        int dsQuiz = store.AddActivity(3, "Quiz 1", 10, 10);
        int dsAssignment = store.AddActivity(3, "Assignment 1", 25, 20);
        int dsMid = store.AddActivity(3, "Mid Term", 50, 30);
        AddMarks(store, dsQuiz, 8, 10, 7, 9, 6, 8, 5, 9, 7, 6);
        AddMarks(store, dsAssignment, 22, 24, 18, 20, 15, 21, 14, 23, 17, 12);
        AddMarks(store, dsMid, 41, 47, 35, 39, 30, 43, 27, 45, 33, 25);

        // Release CS301 grades using the same calculator as the app.
        var marks = new InMemoryMarksRepository(store);
        var grades = new InMemoryGradeRepository(store);
        grades.ReplaceGradesForCourse(5, marks.GetScoresForCourse(5).Select(s =>
        {
            GradeResult result = GradeCalculator.Calculate(s.Scores);
            return new StudentGrade(s.StudentId, s.StudentName, result.Percentage, result.LetterGrade);
        }));

        return store;
    }

    private static void AddMarks(InMemoryDataStore store, int activityId, params decimal[] marksByStudent)
    {
        for (int i = 0; i < marksByStudent.Length; i++)
        {
            store.SetMark(activityId, i + 1, marksByStudent[i]);
        }
    }
}
