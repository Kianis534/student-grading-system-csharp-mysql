using System.Configuration;

namespace StudentGradingSystem.Infrastructure;

/// <summary>Per-Windows-user preferences, stored in the user's local app data folder.</summary>
internal sealed class UserPreferences : ApplicationSettingsBase
{
    public static UserPreferences Default { get; } = (UserPreferences)Synchronized(new UserPreferences());

    /// <summary>Username pre-filled on the login screen when "Remember my username" is ticked.</summary>
    [UserScopedSetting]
    [DefaultSettingValue("")]
    public string RememberedUsername
    {
        get => (string)this[nameof(RememberedUsername)];
        set => this[nameof(RememberedUsername)] = value;
    }
}
