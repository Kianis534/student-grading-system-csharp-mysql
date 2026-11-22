using System;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Validation;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem.Forms;

/// <summary>Spreadsheet-style marks entry for one assessment, saved in a single transaction.</summary>
public partial class MarksEntryForm : Form
{
    private readonly AppServices _services;
    private readonly CourseSummary _course;
    private readonly Activity _activity;
    private BindingList<StudentMarkEntry> _entries = new BindingList<StudentMarkEntry>();
    private decimal?[] _savedMarks = Array.Empty<decimal?>();

    internal MarksEntryForm(AppServices services, CourseSummary course, Activity activity)
    {
        InitializeComponent();
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _course = course ?? throw new ArgumentNullException(nameof(course));
        _activity = activity ?? throw new ArgumentNullException(nameof(activity));

        Theme.Apply(this);
        gridMarks.ReadOnly = false;
        colMarks.ReadOnly = false;
        colMarks.ValueType = typeof(decimal?);

        Text = $"Student Grading System - {_activity.Name}";
        lblTitle.Text = $"Enter marks  ·  {_activity.Name}";
        lblSubtitle.Text = $"{_course.DisplayName}  ·  Out of {_activity.TotalMarks:0.##}  ·  {_activity.Weightage:0.##}% of the final grade";
        colMarks.HeaderText = $"Obtained marks (/{_activity.TotalMarks:0.##})";
    }

    private void MarksEntryForm_Load(object sender, EventArgs e)
    {
        try
        {
            using (new BusyScope(this))
            {
                _entries = new BindingList<StudentMarkEntry>(_services.Courses.GetMarks(_activity.ActivityId).ToList());
            }

            TakeSnapshot();
            gridMarks.DataSource = _entries;
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't load marks.");
            Close();
            return;
        }

        if (_entries.Count == 0)
        {
            Dialogs.Info(this, "No students are enrolled in this course yet.");
        }
        else
        {
            gridMarks.CurrentCell = gridMarks.Rows[0].Cells[colMarks.Index];
        }

        UpdateStatus();
    }

    /// <summary>True when any mark differs from what was last loaded or saved.</summary>
    private bool IsDirty =>
        _entries.Count != _savedMarks.Length ||
        _entries.Where((entry, i) => entry.ObtainedMarks != _savedMarks[i]).Any();

    private void TakeSnapshot() => _savedMarks = _entries.Select(e => e.ObtainedMarks).ToArray();

    private bool SaveMarks()
    {
        if (gridMarks.IsCurrentCellInEditMode && !gridMarks.EndEdit())
        {
            return false;
        }

        try
        {
            ValidationResult result;
            using (new BusyScope(this))
            {
                result = _services.Courses.SaveMarks(_activity, _entries.ToList());
            }

            if (!result.IsValid)
            {
                Dialogs.Warning(this, result.Error!);
                return false;
            }

            TakeSnapshot();
            UpdateStatus($"Saved at {DateTime.Now:t}." + (HasReleasedGrades() ? " Released grades updated." : string.Empty));
            return true;
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't save marks.");
            return false;
        }
    }

    /// <summary>
    /// True when the course's grades have been released, so saving just recalculated what students see.
    /// Only used for the status line, so a database hiccup here never turns a successful save into an error.
    /// </summary>
    private bool HasReleasedGrades()
    {
        try
        {
            return _services.Grading.GetGrades(_course.CourseId).Count > 0;
        }
        catch (DbException)
        {
            return false;
        }
    }

    private void UpdateStatus(string? suffix = null)
    {
        int marked = _entries.Count(x => x.ObtainedMarks.HasValue);
        string text = $"{marked} of {_entries.Count} students marked";

        if (marked > 0)
        {
            decimal average = _entries.Where(x => x.ObtainedMarks.HasValue).Average(x => x.ObtainedMarks!.Value);
            text += $"  ·  class average {average:0.##} / {_activity.TotalMarks:0.##}";
        }

        bool dirty = IsDirty;
        if (dirty)
        {
            text += "  ·  unsaved changes";
        }
        else if (suffix is not null)
        {
            text += "  ·  " + suffix;
        }

        lblStatus.Text = text;
        lblStatus.ForeColor = dirty ? Theme.Primary : Theme.TextMuted;
        btnSave.Enabled = dirty;
    }

    // Marks never need a thousands separator. Allowing one made "7.5" parse as 75 on Windows set to a
    // comma-decimal language (e.g. German), where "." is the group separator.
    private const NumberStyles MarksStyle = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowDecimalPoint;

    /// <summary>Accepts the Windows decimal separator, and "." everywhere.</summary>
    private static bool TryParseMarks(string text, out decimal value) =>
        decimal.TryParse(text, MarksStyle, CultureInfo.CurrentCulture, out value)
        || decimal.TryParse(text, MarksStyle, CultureInfo.InvariantCulture, out value);

    private void gridMarks_CellEnter(object sender, DataGridViewCellEventArgs e)
    {
        // Only the marks column is editable, so keep the cursor there.
        if (e.ColumnIndex != colMarks.Index && e.RowIndex >= 0)
        {
            int row = e.RowIndex;
            BeginInvoke(new Action(() =>
            {
                if (row < gridMarks.RowCount)
                {
                    gridMarks.CurrentCell = gridMarks.Rows[row].Cells[colMarks.Index];
                }
            }));
        }
    }

    private void gridMarks_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
    {
        if (e.ColumnIndex != colMarks.Index)
        {
            return;
        }

        string text = Convert.ToString(e.Value, CultureInfo.CurrentCulture)?.Trim() ?? string.Empty;
        if (text.Length == 0)
        {
            e.Value = null;
            e.ParsingApplied = true;
        }
        else if (TryParseMarks(text, out decimal marks))
        {
            e.Value = marks;
            e.ParsingApplied = true;
        }
    }

    private void gridMarks_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.ColumnIndex != colMarks.Index || e.RowIndex < 0)
        {
            return;
        }

        DataGridViewRow row = gridMarks.Rows[e.RowIndex];
        string text = Convert.ToString(e.FormattedValue, CultureInfo.CurrentCulture)?.Trim() ?? string.Empty;

        string? error = null;
        if (text.Length > 0)
        {
            if (!TryParseMarks(text, out decimal marks))
            {
                error = "Enter a number, or leave the cell empty.";
            }
            else
            {
                ValidationResult result = ActivityRules.ValidateObtainedMarks(marks, _activity.TotalMarks);
                error = result.IsValid ? null : result.Error;
            }
        }

        row.ErrorText = error ?? string.Empty;
        if (error is not null)
        {
            e.Cancel = true;
            lblStatus.Text = $"{row.Cells[colStudentName.Index].Value}: {error} Press Esc to undo.";
            lblStatus.ForeColor = Theme.ErrorText;
        }
    }

    private void gridMarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        gridMarks.Rows[e.RowIndex].ErrorText = string.Empty;
        UpdateStatus();
    }

    private void gridMarks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == colMarks.Index)
        {
            UpdateStatus();
        }
    }

    private void gridMarks_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
        // CellValueChanged only fires once an edit is committed; enable Save while the first change is still being typed.
        btnSave.Enabled = IsDirty || gridMarks.IsCurrentCellDirty;
    }

    private void gridMarks_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        lblStatus.Text = "That value can't be used for marks. Press Esc to undo.";
        lblStatus.ForeColor = Theme.ErrorText;
    }

    private void btnSave_Click(object sender, EventArgs e) => SaveMarks();

    private void btnBack_Click(object sender, EventArgs e) => Close();

    private void MarksEntryForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.S)
        {
            e.SuppressKeyPress = true;
            SaveMarks();
        }
    }

    private void MarksEntryForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Don't let a half-typed invalid cell block closing; discard it instead.
        e.Cancel = false;
        if (gridMarks.IsCurrentCellInEditMode && !gridMarks.EndEdit())
        {
            gridMarks.CancelEdit();
            gridMarks.EndEdit();
        }

        if (!IsDirty)
        {
            return;
        }

        DialogResult answer = MessageBox.Show(
            this,
            "You have unsaved marks. Save them before leaving?",
            "Student Grading System",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question);

        if (answer == DialogResult.Cancel || (answer == DialogResult.Yes && !SaveMarks()))
        {
            e.Cancel = true;
        }
    }
}
