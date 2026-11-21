using System;
using System.Globalization;
using StudentGradingSystem.Core.Data;
using StudentGradingSystem.Core.Models;
using StudentGradingSystem.Core.Security;

namespace StudentGradingSystem.Core.Services;

/// <summary>Signs users in by checking their password against the stored PBKDF2 hash.</summary>
public sealed class AuthService
{
    // Verified when the username doesn't exist, so a missing account takes as long to reject as a wrong password.
    // It is the hash of a random, discarded value and uses the same work factor as real accounts.
    private const string DummyHash =
        "pbkdf2-sha256$210000$+SWrG5aEv+vU/wQm/YdeMw==$7fvX2uJBz+vukpf8C6MmjzT3f1qtM6ZAkuQUtEZWd0w=";

    private readonly IUserRepository _users;

    public AuthService(IUserRepository users) =>
        _users = users ?? throw new ArgumentNullException(nameof(users));

    /// <summary>
    /// Returns the signed-in user, or <c>null</c> when the username or password is wrong.
    /// The caller can't tell which one was wrong, which is intentional.
    /// </summary>
    public AuthenticatedUser? SignIn(string? username, string? password)
    {
        string name = (username ?? string.Empty).Trim();
        if (name.Length == 0 || string.IsNullOrEmpty(password))
        {
            return null;
        }

        UserAccount? account = _users.FindByUsername(name);
        if (account is null)
        {
            PasswordHasher.Verify(password!, DummyHash);
            return null;
        }

        if (!PasswordHasher.Verify(password!, account.PasswordHash))
        {
            return null;
        }

        if (account.Role == UserRole.Student && account.StudentId is null)
        {
            throw new InvalidOperationException(
                $"Student account '{account.Username}' is not linked to a student record.");
        }

        UpgradeHashIfNeeded(account, password!);

        string displayName = account.Role == UserRole.Student && !string.IsNullOrWhiteSpace(account.StudentName)
            ? account.StudentName!
            : ToTitleCase(account.Username);

        return new AuthenticatedUser(account.UserId, account.Username, account.Role, account.StudentId, displayName);
    }

    /// <summary>Re-hashes with the current work factor when an older, weaker hash is found.</summary>
    private void UpgradeHashIfNeeded(UserAccount account, string password)
    {
        string[] parts = account.PasswordHash.Split('$');
        if (parts.Length == 4
            && int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int iterations)
            && iterations < PasswordHasher.DefaultIterations)
        {
            _users.UpdatePasswordHash(account.UserId, PasswordHasher.Hash(password));
        }
    }

    private static string ToTitleCase(string value) =>
        CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());
}
