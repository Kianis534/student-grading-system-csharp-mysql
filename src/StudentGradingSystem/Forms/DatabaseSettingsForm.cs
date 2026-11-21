using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Infrastructure;

namespace StudentGradingSystem.Forms;

/// <summary>Lets users point the app at their MySQL server without editing config files by hand.</summary>
public partial class DatabaseSettingsForm : Form
{
    private const string DefaultDatabase = "StudentGradingSystem";

    public DatabaseSettingsForm()
    {
        InitializeComponent();
        Theme.Apply(this);
    }

    private void DatabaseSettingsForm_Load(object sender, EventArgs e)
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Port = 3306,
            Database = DefaultDatabase,
            UserID = "root",
        };

        try
        {
            string? existing = AppConfiguration.GetConnectionString(out _);
            if (!string.IsNullOrWhiteSpace(existing))
            {
                builder = new MySqlConnectionStringBuilder(existing!);
            }
        }
        catch (Exception ex) when (ex is ArgumentException || ex is System.Xml.XmlException)
        {
            SetStatus("The saved settings could not be read, so defaults are shown.", success: false);
        }

        txtServer.Text = builder.Server;
        numPort.Value = Math.Min(numPort.Maximum, Math.Max(numPort.Minimum, builder.Port));
        txtDatabase.Text = string.IsNullOrWhiteSpace(builder.Database) ? DefaultDatabase : builder.Database;
        txtUser.Text = builder.UserID;
        txtPassword.Text = string.Equals(builder.Password, AppConfiguration.PasswordPlaceholder, StringComparison.Ordinal)
            ? string.Empty
            : builder.Password;

        if (AppConfiguration.IsEnvironmentOverrideActive)
        {
            lblNotice.Text =
                $"Note: the {AppConfiguration.EnvironmentVariableName} environment variable is set and takes precedence over " +
                "the settings saved here.";
        }

        ActiveControl = txtPassword.TextLength == 0 ? txtPassword : btnTest;
    }

    private async void btnTest_Click(object sender, EventArgs e)
    {
        if (!TryBuildConnectionString(out string connectionString))
        {
            return;
        }

        SetBusy(true);
        SetStatus("Connecting...", success: null);
        try
        {
            await Task.Run(() => MySqlConnectionFactory.TestConnection(connectionString));
            SetStatus("Connected successfully. The StudentGradingSystem tables were found.", success: true);
        }
        catch (Exception ex) when (ex is MySqlException || ex is InvalidConnectionStringException)
        {
            SetStatus(DatabaseErrors.Describe(ex), success: false);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!TryBuildConnectionString(out string connectionString))
        {
            return;
        }

        try
        {
            AppConfiguration.SaveLocalConfig(connectionString);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException || ex is IOException)
        {
            SetStatus(
                $"Couldn't save {AppConfiguration.LocalConfigPath}: {ex.Message} " +
                $"Move the app to a folder you can write to, or set {AppConfiguration.EnvironmentVariableName}.",
                success: false);
        }
    }

    private bool TryBuildConnectionString(out string connectionString)
    {
        connectionString = string.Empty;

        if (string.IsNullOrWhiteSpace(txtServer.Text) ||
            string.IsNullOrWhiteSpace(txtDatabase.Text) ||
            string.IsNullOrWhiteSpace(txtUser.Text))
        {
            SetStatus("Server, database and username are required.", success: false);
            return false;
        }

        var builder = new MySqlConnectionStringBuilder
        {
            Server = txtServer.Text.Trim(),
            Port = Convert.ToUInt32(numPort.Value, CultureInfo.InvariantCulture),
            Database = txtDatabase.Text.Trim(),
            UserID = txtUser.Text.Trim(),
            Password = txtPassword.Text,
            SslMode = MySqlSslMode.Preferred,
            AllowPublicKeyRetrieval = true,
        };

        connectionString = builder.ConnectionString;
        return true;
    }

    private void SetBusy(bool busy)
    {
        UseWaitCursor = busy;
        btnTest.Enabled = !busy;
        btnSave.Enabled = !busy;
    }

    private void SetStatus(string message, bool? success)
    {
        lblStatus.Text = message;
        lblStatus.ForeColor = success switch
        {
            true => Theme.Success,
            false => Theme.ErrorText,
            null => Theme.TextMuted,
        };
    }

    private void Field_Changed(object? sender, EventArgs e)
    {
        lblStatus.Text = string.Empty;
    }
}
