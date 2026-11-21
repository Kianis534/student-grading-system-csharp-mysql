using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem.Forms;

/// <summary>Instructor home screen: every course with its enrolment and assessment setup.</summary>
public partial class InstructorDashboardForm : Form
{
    private readonly AppServices _services;

    internal InstructorDashboardForm(AppServices services, AuthenticatedUser user)
    {
        InitializeComponent();
        _services = services ?? throw new ArgumentNullException(nameof(services));
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        Theme.Apply(this);
        lblSubtitle.Text = $"Signed in as {user.DisplayName}  ·  Open a course to manage its assessments, marks and grades.";
    }

    private CourseSummary? SelectedCourse => gridCourses.CurrentRow?.DataBoundItem as CourseSummary;

    private void InstructorDashboardForm_Load(object sender, EventArgs e) => LoadCourses();

    private void LoadCourses(int? selectCourseId = null)
    {
        try
        {
            IReadOnlyList<CourseSummary> courses;
            using (new BusyScope(this))
            {
                courses = _services.Courses.GetCourses();
            }

            gridCourses.DataSource = courses.ToList();
            int fullyAssessed = courses.Count(c => c.AllocatedWeightage >= 100m);
            lblStatus.Text = $"{courses.Count} courses  ·  {fullyAssessed} with assessments totalling 100%";

            if (selectCourseId is int id)
            {
                SelectRow(id);
            }
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't load courses.");
        }

        UpdateButtons();
    }

    private void SelectRow(int courseId)
    {
        foreach (DataGridViewRow row in gridCourses.Rows)
        {
            if (row.DataBoundItem is CourseSummary course && course.CourseId == courseId)
            {
                gridCourses.CurrentCell = row.Cells[colCode.Index];
                return;
            }
        }
    }

    private void OpenSelectedCourse()
    {
        CourseSummary? course = SelectedCourse;
        if (course is null)
        {
            return;
        }

        this.ShowChild(new CourseActivitiesForm(_services, course.CourseId));
        LoadCourses(course.CourseId);
    }

    private void UpdateButtons() => btnOpenCourse.Enabled = SelectedCourse is not null;

    private void btnOpenCourse_Click(object sender, EventArgs e) => OpenSelectedCourse();

    private void btnRefresh_Click(object sender, EventArgs e) => LoadCourses(SelectedCourse?.CourseId);

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
