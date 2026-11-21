using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem.Forms;

/// <summary>Student home screen: enrolled courses and released grades.</summary>
public partial class StudentDashboardForm : Form
{
    private readonly AppServices _services;
    private readonly AuthenticatedUser _user;

    internal StudentDashboardForm(AppServices services, AuthenticatedUser user)
    {
        InitializeComponent();
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _user = user ?? throw new ArgumentNullException(nameof(user));

        if (_user.StudentId is null)
        {
            throw new ArgumentException("A student account is required.", nameof(user));
        }

        Theme.Apply(this);
        lblTitle.Text = $"Welcome, {_user.DisplayName}";
    }

    private StudentCourseResult? SelectedCourse => gridCourses.CurrentRow?.DataBoundItem as StudentCourseResult;

    private void StudentDashboardForm_Load(object sender, EventArgs e)
    {
        try
        {
            IReadOnlyList<StudentCourseResult> courses;
            using (new BusyScope(this))
            {
                courses = _services.Results.GetCourses(_user.StudentId!.Value);
            }

            gridCourses.DataSource = courses.ToList();

            int graded = courses.Count(c => c.IsGraded);
            lblSummary.Text = graded == 0
                ? $"{courses.Count} enrolled courses  ·  no grades released yet"
                : $"{courses.Count} enrolled courses  ·  {graded} graded  ·  average {courses.Where(c => c.IsGraded).Average(c => c.Percentage!.Value):0.00}%";
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't load your courses.");
        }

        UpdateButtons();
    }

    private void OpenSelectedCourse()
    {
        StudentCourseResult? course = SelectedCourse;
        if (course is not null)
        {
            this.ShowChild(new StudentCourseDetailsForm(_services, _user, course.CourseId));
        }
    }

    private void UpdateButtons() => btnViewDetails.Enabled = SelectedCourse is not null;

    private void gridCourses_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex == colGrade.Index && e.CellStyle is not null)
        {
            e.CellStyle.ForeColor = e.Value is null ? Theme.TextMuted
                : Equals(e.Value, "F") ? Theme.Danger
                : Theme.TextPrimary;
        }
    }

    private void btnViewDetails_Click(object sender, EventArgs e) => OpenSelectedCourse();

    private void btnSignOut_Click(object sender, EventArgs e) => DialogResult = NavigationResults.SignOut;

    private void gridCourses_SelectionChanged(object sender, EventArgs e) => UpdateButtons();

    private void gridCourses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            OpenSelectedCourse();
        }
    }

    /// <summary>DataGridView consumes Enter itself, so handle it here to open the selected row.</summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Enter && gridCourses.Focused)
        {
            OpenSelectedCourse();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
}
