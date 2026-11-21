using System;
using System.Threading;
using System.Windows.Forms;
using StudentGradingSystem.Forms;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem;

internal static class Program
{
    // Per Windows session, so two different users on one machine can each run the app.
    private const string SingleInstanceMutexName = @"Local\StudentGradingSystem.SingleInstance";

    [STAThread]
    private static void Main()
    {
        using var mutex = new Mutex(initiallyOwned: true, SingleInstanceMutexName, out bool isFirstInstance);
        if (!isFirstInstance)
        {
            MessageBox.Show(
                "Student Grading System is already running.",
                "Student Grading System",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => Dialogs.Error(null, e.Exception, "Something went wrong.");
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception exception)
            {
                AppLog.Error(exception, "Unhandled exception");
            }
        };

        Application.Run(new LoginForm());
        GC.KeepAlive(mutex);
    }
}
