using System;
using System.Linq;
using System.Windows.Forms;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Services;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem.Forms;

/// <summary>A student's marks in one course and how each assessment counts toward the final grade.</summary>
public partial class StudentCourseDetailsForm : Form
{
    private readonly AppServices _services;
    private readonly AuthenticatedUser _user;
    private readonly int _courseId;

    internal StudentCourseDetailsForm(AppServices services, AuthenticatedUser user, int courseId)
    {
        InitializeComponent();
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _courseId = courseId;

        Theme.Apply(this);
        Theme.UseAsReport(gridActivities);
    }

    private void StudentCourseDetailsForm_Load(object sender, EventArgs e)
    {
        CourseDetails? details;
        try
        {
            using (new BusyScope(this))
            {
                details = _services.Results.GetCourseDetails(_user.StudentId!.Value, _courseId);
            }
        }
        catch (Exception ex)
        {
            Dialogs.Error(this, ex, "Couldn't load your marks.");
            Close();
            return;
        }

        if (details is null)
        {
            Dialogs.Warning(this, "You are not enrolled in this course.");
            Close();
            return;
        }

        StudentCourseResult course = details.Course;
        Text = $"Student Grading System - {course.Code}";
        lblTitle.Text = $"{course.Code} · {course.Title}";
        lblSubtitle.Text = $"{_user.DisplayName}  ·  {course.Semester}";
        gridActivities.DataSource = details.Activities.ToList();

        decimal assessed = details.Activities.Sum(a => a.Weightage);
        decimal earned = details.Activities.Sum(a => a.Contribution);
        int marked = details.Activities.Count(a => a.ObtainedMarks.HasValue);

        if (course.IsGraded)
        {
            lblGrade.Text = course.FinalGrade;
            lblResultHeadline.Text = $"{course.Percentage:0.00}% overall";
            lblResultDetail.Text = $"Based on {details.Activities.Count} assessments worth {assessed:0.##}% of the course.";
        }
        else
        {
            lblGrade.Text = "-";
            lblGradeCaption.Text = "NOT GRADED";
            lblResultHeadline.Text = details.Activities.Count == 0
                ? "No assessments have been set up yet"
                : $"{earned:0.##}% earned so far";
            lblResultDetail.Text = details.Activities.Count == 0
                ? "Your instructor hasn't added any assessments to this course."
                : $"{marked} of {details.Activities.Count} assessments marked, covering {assessed:0.##}% of the final grade. " +
                  "The final grade appears once your instructor releases it.";
        }
    }

    private void btnBack_Click(object sender, EventArgs e) => Close();
}
