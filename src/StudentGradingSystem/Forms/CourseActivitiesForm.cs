using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Validation;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem.Forms;

/// <summary>One course: its assessments, the weightage split, and the path to marks and grades.</summary>
public partial class CourseActivitiesForm : Form
{
    private readonly AppServices _services;
    private readonly int _courseId;
    private CourseSummary? _course;
    private IReadOnlyList<Activity> _activities = Array.Empty<Activity>();

    internal CourseActivitiesForm(AppServices services, int courseId)
    {
        InitializeComponent();
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _courseId = courseId;

        Theme.Apply(this);
        cboName.Items.AddRange(ActivityRules.SuggestedNames.Cast<object>().ToArray());
    }

    private Activity? SelectedActivity => gridActivities.CurrentRow?.DataBoundItem as Activity;

    private decimal AllocatedWeightage => _activities.Sum(a => a.Weightage);

    private void CourseActivitiesForm_Load(object sender, EventArgs e)
    {
        if (!LoadData())
        {
            Close();
        }
    }

    private bool LoadData(int? selectActivityId = null)
    {
        try
        {
            using (new BusyScope(this))
            {
                _course = _services.Courses.GetCourse(_courseId);
                if (_course is null)
                {
                    Dialogs.Warning(this, "This course no longer exists.");
                    return false;
                }

                _activities = _services.Courses.GetActivities(_courseId);
            }
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't load the course.");
            return false;
        }

        Text = $"Student Grading System - {_course.Code}";
        lblTitle.Text = _course.DisplayName;
        lblSubtitle.Text = $"{_course.Semester}  ·  {_course.EnrolledStudents} students enrolled  ·  {_activities.Count} assessments";

        gridActivities.DataSource = _activities.ToList();
        if (selectActivityId is int id)
        {
            foreach (DataGridViewRow row in gridActivities.Rows)
            {
                if (row.DataBoundItem is Activity a && a.ActivityId == id)
                {
                    gridActivities.CurrentCell = row.Cells[colName.Index];
                    break;
                }
            }
        }

        UpdateWeightageSummary();
        UpdateButtons();
        return true;
    }

    private void UpdateWeightageSummary()
    {
        decimal allocated = AllocatedWeightage;
        decimal remaining = Math.Max(0m, ActivityRules.MaxCourseWeightage - allocated);

        progressWeight.Value = (int)Math.Min(100m, Math.Round(allocated, MidpointRounding.AwayFromZero));
        lblWeightValue.Text = remaining == 0m
            ? $"{allocated:0.##}% allocated - ready to grade"
            : $"{allocated:0.##}% allocated  ·  {remaining:0.##}% remaining";
        lblWeightValue.ForeColor = remaining == 0m ? Theme.Success : Theme.TextMuted;

        btnAdd.Enabled = remaining > 0m;
        if (remaining > 0m)
        {
            numWeight.Value = Math.Max(numWeight.Minimum, Math.Min(numWeight.Value, remaining));
        }
    }

    private void UpdateButtons()
    {
        bool hasSelection = SelectedActivity is not null;
        btnEnterMarks.Enabled = hasSelection;
        btnDelete.Enabled = hasSelection;
        btnCalculate.Enabled = _activities.Count > 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            ValidationResult result = _services.Courses.AddActivity(
                _courseId, cboName.Text, numTotal.Value, numWeight.Value, out Activity? created);

            if (!result.IsValid)
            {
                Dialogs.Warning(this, result.Error!);
                cboName.Focus();
                return;
            }

            cboName.Text = string.Empty;
            LoadData(created?.ActivityId);
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't add the assessment.");
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        Activity? activity = SelectedActivity;
        if (activity is null)
        {
            return;
        }

        bool confirmed = Dialogs.Confirm(
            this,
            $"Delete \"{activity.Name}\" and every mark recorded for it?" + Environment.NewLine + Environment.NewLine +
            "This can't be undone. If grades have been released, they're recalculated straight away.",
            destructive: true);

        if (!confirmed)
        {
            return;
        }

        try
        {
            _services.Courses.DeleteActivity(activity);
            LoadData();
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't delete the assessment.");
        }
    }

    private void OpenMarksEntry()
    {
        Activity? activity = SelectedActivity;
        if (activity is null || _course is null)
        {
            return;
        }

        this.ShowChild(new MarksEntryForm(_services, _course, activity));
    }

    private void btnEnterMarks_Click(object sender, EventArgs e) => OpenMarksEntry();

    private void btnCalculate_Click(object sender, EventArgs e)
    {
        if (_course is null)
        {
            return;
        }

        if (_activities.Count == 0)
        {
            Dialogs.Warning(this, "Add at least one assessment before calculating grades.");
            return;
        }

        decimal allocated = AllocatedWeightage;
        if (allocated < ActivityRules.MaxCourseWeightage &&
            !Dialogs.Confirm(
                this,
                $"Only {allocated:0.##}% of the final grade has been allocated to assessments." + Environment.NewLine +
                "Students will be graded out of 100%, so missing weightage counts as zero." + Environment.NewLine + Environment.NewLine +
                "Calculate grades anyway?"))
        {
            return;
        }

        try
        {
            int count;
            using (new BusyScope(this))
            {
                count = _services.Grading.CalculateAndSaveGrades(_courseId).Count;
            }

            AppLog.Info($"Calculated grades for {count} students in {_course.Code}.");
            this.ShowChild(new CourseGradesForm(_services, _course, justCalculated: true));
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't calculate grades.");
        }
    }

    private void btnViewGrades_Click(object sender, EventArgs e)
    {
        if (_course is not null)
        {
            this.ShowChild(new CourseGradesForm(_services, _course, justCalculated: false));
        }
    }

    private void btnBack_Click(object sender, EventArgs e) => Close();

    private void gridActivities_SelectionChanged(object sender, EventArgs e) => UpdateButtons();

    private void gridActivities_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            OpenMarksEntry();
        }
    }

    private void gridActivities_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete)
        {
            e.Handled = true;
            btnDelete.PerformClick();
        }
    }

    /// <summary>DataGridView consumes Enter itself, so handle it here to open the selected row.</summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Enter && gridActivities.Focused)
        {
            OpenMarksEntry();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }
}
