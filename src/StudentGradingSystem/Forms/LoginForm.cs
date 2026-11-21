using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudentGradingSystem.Data.MySql;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem.Forms;

/// <summary>Entry screen. Authenticates the user and opens the dashboard for their role.</summary>
public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
        Theme.Apply(this);

        Version? version = Assembly.GetExecutingAssembly().GetName().Version;
        lblVersion.Text = version is null ? string.Empty : $"v{version.Major}.{version.Minor}.{version.Build}";
    }

    private void LoginForm_Load(object sender, EventArgs e)
    {
        string remembered = UserPreferences.Default.RememberedUsername;
        if (!string.IsNullOrWhiteSpace(remembered))
        {
            txtUsername.Text = remembered;
            chkRemember.Checked = true;
            ActiveControl = txtPassword;
        }
        else
        {
            ActiveControl = txtUsername;
        }
    }

    private async void btnSignIn_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;

        if (username.Length == 0 || password.Length == 0)
        {
            ShowError("Enter your username and password.");
            return;
        }

        string? connectionString = AppConfiguration.GetConnectionString(out ConnectionStringSource source);
        if (AppConfiguration.IsUnconfigured(connectionString))
        {
            ShowError("The database connection hasn't been set up yet.");
            if (OpenDatabaseSettings())
            {
                ShowError("Database settings saved. Click \"Sign in\" to continue.");
            }

            return;
        }

        AppServices? services = null;
        AuthenticatedUser? user = null;

        SetBusy(true);
        try
        {
            services = new AppServices(connectionString!);
            user = await Task.Run(() => services.Auth.SignIn(username, password));
        }
        catch (Exception ex) when (DatabaseErrors.IsConfigurationProblem(ex))
        {
            string reason = DatabaseErrors.Describe(ex);
            ShowError(reason);

            if (source == ConnectionStringSource.LocalConfigFile
                && Dialogs.Confirm(this, $"{reason}{Environment.NewLine}{Environment.NewLine}Open database settings now?"))
            {
                OpenDatabaseSettings();
            }

            return;
        }
        catch (Exception ex)
        {
            ShowError(null);
            Dialogs.Error(this, ex, "Sign-in failed.");
            return;
        }
        finally
        {
            SetBusy(false);
        }

        if (user is null)
        {
            ShowError("Incorrect username or password.");
            txtPassword.SelectAll();
            txtPassword.Focus();
            return;
        }

        RememberUsername(username);
        ShowError(null);
        OpenDashboard(services!, user);
    }

    private void OpenDashboard(AppServices services, AuthenticatedUser user)
    {
        Form dashboard = user.Role == UserRole.Instructor
            ? new InstructorDashboardForm(services, user)
            : new StudentDashboardForm(services, user);

        txtPassword.Clear();
        Hide();

        DialogResult result;
        using (dashboard)
        {
            result = dashboard.ShowDialog();
        }

        if (result == NavigationResults.SignOut)
        {
            Show();
            Activate();
            ActiveControl = txtUsername.TextLength > 0 ? txtPassword : txtUsername;
        }
        else
        {
            // The user closed the dashboard window: exit the application.
            Close();
        }
    }

    private bool OpenDatabaseSettings()
    {
        using var settings = new DatabaseSettingsForm();
        return settings.ShowDialog(this) == DialogResult.OK;
    }

    private void RememberUsername(string username)
    {
        UserPreferences.Default.RememberedUsername = chkRemember.Checked ? username : string.Empty;
        try
        {
            UserPreferences.Default.Save();
        }
        catch (System.Configuration.ConfigurationException ex)
        {
            AppLog.Error(ex, "Could not save user preferences");
        }
    }

    private void SetBusy(bool busy)
    {
        UseWaitCursor = busy;
        btnSignIn.Enabled = !busy;
        btnSignIn.Text = busy ? "Signing in..." : "Sign in";
        txtUsername.Enabled = !busy;
        txtPassword.Enabled = !busy;
    }

    private void ShowError(string? message)
    {
        lblError.Text = message ?? string.Empty;
    }

    private void Credentials_TextChanged(object sender, EventArgs e)
    {
        if (lblError.Text.Length > 0)
        {
            ShowError(null);
        }
    }

    private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
    {
        txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
    }

    private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Dialogs.Info(
            this,
            "Passwords are managed by your instructor or system administrator." + Environment.NewLine +
            "Ask them to reset yours.");
    }

    private void lnkDatabaseSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        if (OpenDatabaseSettings())
        {
            ShowError(null);
        }
    }

    private void lnkDemoAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Dialogs.Info(
            this,
            "The sample database (database/02-seed.sql) includes these accounts:" + Environment.NewLine + Environment.NewLine +
            "Instructor:   instructor  /  Instructor@123" + Environment.NewLine +
            "Student:       jsmith  /  Student@123" + Environment.NewLine +
            "                    (also ejohnson, mbrown, sdavis, dwilson, jlee, ...)" + Environment.NewLine + Environment.NewLine +
            "These are for local demos only - change them before real use.");
    }
}
