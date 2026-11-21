using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StudentGradingSystem.Core.Grading;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Reporting;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem.Forms;

/// <summary>Final grades for a course with class statistics and CSV export.</summary>
public partial class CourseGradesForm : Form
{
    private readonly AppServices _services;
    private readonly CourseSummary _course;
    private readonly bool _justCalculated;
    private IReadOnlyList<StudentGrade> _grades = Array.Empty<StudentGrade>();

    internal CourseGradesForm(AppServices services, CourseSummary course, bool justCalculated)
    {
        InitializeComponent();
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _course = course ?? throw new ArgumentNullException(nameof(course));
        _justCalculated = justCalculated;

        Theme.Apply(this);
        Theme.UseAsReport(gridGrades);
        Text = $"Student Grading System - {_course.Code} grades";
        lblSubtitle.Text = $"{_course.DisplayName}  ·  {_course.Semester}";
    }

    private void CourseGradesForm_Load(object sender, EventArgs e)
    {
        try
        {
            using (new BusyScope(this))
            {
                _grades = _services.Grading.GetGrades(_course.CourseId);
            }
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't load grades.");
            Close();
            return;
        }

        gridGrades.DataSource = _grades.ToList();
        ShowStatistics();
    }

    private void ShowStatistics()
    {
        btnExport.Enabled = _grades.Count > 0;

        if (_grades.Count == 0)
        {
            lblStats.Text = "No grades yet";
            lblDistribution.Text = "Open the course and click \"Calculate grades\" to generate final grades.";
            lblCalculated.Text = string.Empty;
            return;
        }

        GradeStatistics stats = GradeStatistics.From(_grades.ToList());
        lblStats.Text =
            $"Average {stats.Average:0.00}%     Highest {stats.Highest:0.00}%     Lowest {stats.Lowest:0.00}%     Passed {stats.Passed} of {stats.Count}";

        lblDistribution.Text = "Distribution:  " + string.Join(
            "   ",
            stats.Distribution.Where(d => d.Value > 0).Select(d => $"{d.Key} × {d.Value}"));

        DateTime? calculatedAt = _grades.Max(g => g.CalculatedAt);
        lblCalculated.Text = _justCalculated
            ? "Grades recalculated just now."
            : calculatedAt is DateTime when ? $"Last calculated {when:g}." : string.Empty;
        lblCalculated.ForeColor = _justCalculated ? Theme.Success : Theme.TextMuted;
    }

    private void gridGrades_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex != colGrade.Index || e.Value is not string grade || e.CellStyle is null)
        {
            return;
        }

        e.CellStyle.ForeColor = grade switch
        {
            "F" => Theme.Danger,
            "D" => Color.FromArgb(180, 83, 9),
            _ when grade.StartsWith("A", StringComparison.Ordinal) => Theme.Success,
            _ => Theme.TextPrimary,
        };
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Export grades",
            Filter = "CSV files (*.csv)|*.csv",
            FileName = $"{_course.Code}-final-grades.csv",
            OverwritePrompt = true,
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            // UTF-8 with BOM so Excel detects the encoding of non-ASCII names correctly.
            File.WriteAllText(dialog.FileName, GradeReportCsv.ToCsv(_grades), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
            lblCalculated.Text = $"Exported {_grades.Count} grades to {Path.GetFileName(dialog.FileName)}.";
            lblCalculated.ForeColor = Theme.Success;
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            Dialogs.Error(this, ex, "Couldn't export grades.");
        }
    }

    private void btnBack_Click(object sender, EventArgs e) => Close();
}
