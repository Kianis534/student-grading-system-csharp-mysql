using System;
using System.Drawing;
using System.Windows.Forms;
using StudentGradingSystem.Data.MySql;

namespace StudentGradingSystem.Infrastructure;

internal static class Dialogs
{
    private const string Caption = "Student Grading System";

    public static void Info(IWin32Window? owner, string message) =>
        MessageBox.Show(owner, message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Warning(IWin32Window? owner, string message) =>
        MessageBox.Show(owner, message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public static bool Confirm(IWin32Window? owner, string message, bool destructive = false) =>
        MessageBox.Show(
            owner,
            message,
            Caption,
            MessageBoxButtons.YesNo,
            destructive ? MessageBoxIcon.Warning : MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    /// <summary>
    /// Shows a friendly message for a failed operation. Database setup problems get an
    /// actionable explanation; anything unexpected is logged with its stack trace.
    /// </summary>
    public static void Error(IWin32Window? owner, Exception exception, string whatFailed)
    {
        string message;
        if (DatabaseErrors.Classify(exception) == DatabaseErrorKind.Other)
        {
            AppLog.Error(exception, whatFailed);
            message = $"{whatFailed}{Environment.NewLine}{Environment.NewLine}{exception.Message}" +
                      $"{Environment.NewLine}{Environment.NewLine}Details were written to {AppLog.LogDirectory}";
        }
        else
        {
            message = $"{whatFailed}{Environment.NewLine}{Environment.NewLine}{DatabaseErrors.Describe(exception)}";
        }

        MessageBox.Show(owner, message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

/// <summary>Dialog results with an app-specific meaning.</summary>
internal static class NavigationResults
{
    /// <summary>The user clicked "Sign out": go back to the login screen instead of exiting.</summary>
    public const DialogResult SignOut = DialogResult.Abort;
}

internal static class FormNavigation
{
    /// <summary>
    /// Hides <paramref name="parent"/>, shows <paramref name="child"/> modally in the same spot,
    /// and brings the parent back when the child closes. Keeps the app to a single visible window
    /// and guarantees forms are disposed (no hidden forms piling up in memory).
    /// </summary>
    public static DialogResult ShowChild(this Form parent, Form child)
    {
        if (parent is null)
        {
            throw new ArgumentNullException(nameof(parent));
        }

        if (child is null)
        {
            throw new ArgumentNullException(nameof(child));
        }

        child.StartPosition = FormStartPosition.Manual;
        child.Location = CenterOver(parent, child.Size);
        child.ShowInTaskbar = true;

        parent.Hide();
        try
        {
            return child.ShowDialog();
        }
        finally
        {
            child.Dispose();
            parent.Show();
            parent.Activate();
        }
    }

    private static Point CenterOver(Form parent, Size childSize)
    {
        Rectangle area = Screen.FromControl(parent).WorkingArea;
        int x = parent.Left + ((parent.Width - childSize.Width) / 2);
        int y = parent.Top + ((parent.Height - childSize.Height) / 2);
        x = Math.Max(area.Left, Math.Min(x, area.Right - childSize.Width));
        y = Math.Max(area.Top, Math.Min(y, area.Bottom - childSize.Height));
        return new Point(x, y);
    }
}

/// <summary>Shows a wait cursor for the lifetime of a <c>using</c> block.</summary>
internal sealed class BusyScope : IDisposable
{
    private readonly Form _form;

    public BusyScope(Form form)
    {
        _form = form;
        _form.UseWaitCursor = true;
        Cursor.Current = Cursors.WaitCursor;
    }

    public void Dispose()
    {
        _form.UseWaitCursor = false;
        Cursor.Current = Cursors.Default;
    }
}
